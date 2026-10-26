"""Bring the game's downloaded asset bundles into the project, so they can be rebuilt for PS Vita under their
original names (Assets/Editor/ACIDBundleBuild.cs).

Input: a per-bundle AssetRipper export (Tools/BundleRip: <rip>/<bundle>/Assets/...) and the bundle index
(bundle_index.py). Each export holds exactly one bundle, and the bundles are self-contained, so a bundle's export is
copied as a whole to Assets/BundleAssets/<bundle>/ with its .meta files (AssetRipper's GUIDs keep every reference
inside the export intact; script references already point at the project's scripts).

Shaders are not copied: the exports only hold disassembled programs. Every shader reference is pointed at the
project shader of the same name, or at Unity's built-in shader of that name (Logs/builtin_shaders.txt, from
ACIDBuiltinShaderMap). References to shaders the project does not have yet keep their export GUID and are listed in
Assets/BundleAssets/_import/shaders.json, so shader_remap.py can resolve them once those shaders are ported; their
disassembly is copied to Tools/ShaderPort/bundle_originals/ for porting. Materials whose shader reference the rip
lost (AssetRipper's 0000000deadbeef placeholder) get it back from the original bundle (--bundles, needs UnityPy).

Assets/BundleAssets/_import/bundles.json lists, per bundle file: whether it is a scene bundle and every explicit
asset name the game loads with the project path of that asset.

    python import_bundles.py --project <ACIdentity2017> --rip <cache_rip_bundles> --index <bundle_index.json>
        [--bundles <dir with the original .unity3d files>] [--pylib <UnityPy dir>] [--replace] <bundle name or glob>...

A bundle already imported is left alone unless --replace is given (then only that bundle's own folder is replaced).
"""
import argparse
import fnmatch
import json
import os
import re
import shutil
import sys

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))  # python -I leaves the script folder out
import animator_hierarchy  # noqa: E402
import clip_params  # noqa: E402

GUID_RE = re.compile(r'^guid: ([0-9a-f]{32})', re.M)
SHADER_NAME_RE = re.compile(r'^\s*Shader\s+"([^"]+)"', re.M)
DEAD_SHADER = '0000000deadbeef15deadf00d0000000'
SKIP_DIRS = {'Scripts', 'Plugins', 'Shader'}
# Never loaded from a bundle by name: scripts, shaders (remapped), and the components a model's container entry also
# lists (they come with its GameObject).
NOT_LOADABLE = {'MonoScript', 'Shader', 'AssetBundle', 'Transform', 'RectTransform', 'MeshFilter', 'MeshRenderer',
                'SkinnedMeshRenderer', 'Animator', 'Animation', 'MeshCollider', 'BoxCollider', 'SphereCollider',
                'CapsuleCollider', 'Rigidbody', 'ParticleSystem', 'ParticleSystemRenderer', 'LODGroup', 'Light',
                'AudioSource', 'MonoBehaviour', 'Camera', 'TrailRenderer', 'LineRenderer'}
TYPE_EXTENSIONS = {
    'Material': {'.mat'},
    'Texture2D': {'.png', '.asset', '.exr', '.tga'},
    'Cubemap': {'.png', '.asset', '.cubemap'},
    'Mesh': {'.asset'},
    'AnimationClip': {'.anim'},
    'GameObject': {'.prefab'},
    'TextAsset': {'.bytes', '.txt', '.json', '.xml', '.csv', '.html', '.htm', '.yaml'},
    'AudioClip': {'.wav', '.ogg', '.mp3', '.aif', '.aiff', '.asset'},
    'AnimatorController': {'.controller'},
    'AnimatorOverrideController': {'.overrideController', '.overridecontroller'},
    'Avatar': {'.asset'},
    'Font': {'.ttf', '.otf', '.fontsettings', '.asset'},
    'Sprite': {'.asset'},
    'PhysicMaterial': {'.physicmaterial', '.physicMaterial'},
}


def read_guid(meta_path):
    with open(meta_path, encoding='utf-8', errors='replace') as fh:
        m = GUID_RE.search(fh.read())
    return m.group(1) if m else None


def project_shaders(project):
    """name -> GUID of every shader under Assets/ (outside BundleAssets)."""
    result = {}
    assets = os.path.join(project, 'Assets')
    for d, dirs, files in os.walk(assets):
        dirs[:] = [x for x in dirs if x != 'BundleAssets']
        for f in files:
            if f.endswith('.shader'):
                p = os.path.join(d, f)
                with open(p, encoding='utf-8', errors='replace') as fh:
                    m = SHADER_NAME_RE.search(fh.read())
                guid = read_guid(p + '.meta') if os.path.exists(p + '.meta') else None
                if m and guid:
                    result[m.group(1)] = guid
    return result


def builtin_shaders(project):
    """name -> (fileID, file GUID) from Logs/builtin_shaders.txt."""
    path = os.path.join(project, 'Logs', 'builtin_shaders.txt')
    result = {}
    if os.path.exists(path):
        for line in open(path, encoding='utf-8'):
            parts = line.rstrip('\n').split('\t')
            if len(parts) == 3:
                result[parts[0]] = (int(parts[1]), parts[2])
    return result


def shader_ref(name, proj, builtin):
    """The m_Shader value for a shader name, or None when the project has no such shader yet."""
    if name in proj:
        return '{fileID: 4800000, guid: %s, type: 3}' % proj[name]
    if name in builtin:
        file_id, guid = builtin[name]
        return '{fileID: %d, guid: %s, type: 0}' % (file_id, guid)
    return None


def lost_shader_names(bundle_path):
    """material name -> shader name, read from the original bundle (for materials whose reference the rip lost)."""
    try:
        import UnityPy
    except ImportError:
        return {}
    env = UnityPy.load(bundle_path)
    shader_names = {}
    for obj in env.objects:
        if obj.type.name == 'Shader':
            try:
                tree = obj.read_typetree()
                shader_names[(obj.assets_file.name, obj.path_id)] = tree.get('m_ParsedForm', {}).get('m_Name') or tree.get('m_Name')
            except Exception:
                pass
    result = {}
    for obj in env.objects:
        if obj.type.name != 'Material':
            continue
        try:
            tree = obj.read_typetree()
        except Exception:
            continue
        ref = tree.get('m_Shader', {})
        name = None
        if ref.get('m_FileID') == 0:
            name = shader_names.get((obj.assets_file.name, ref.get('m_PathID')))
        else:
            # external: a built-in file; resolve through the serialized file's externals
            try:
                ext = obj.assets_file.externals[ref['m_FileID'] - 1].path
                name = 'builtin:%s:%d' % (ext, ref['m_PathID'])
            except Exception:
                name = None
        if name:
            result[tree.get('m_Name')] = name
    return result


BUILTIN_FILES = {'unity default resources': '0000000000000000e000000000000000',
                 'unity_builtin_extra': '0000000000000000f000000000000000'}
# YAML class ID, field, UnityPy type and typetree field of the component references the rip can lose
LOST_REF_FIELDS = {33: ('m_Mesh', 'MeshFilter', 'm_Mesh'), 64: ('m_Mesh', 'MeshCollider', 'm_Mesh'),
                   199: ('m_Mesh', 'ParticleSystemRenderer', 'm_Mesh'), 108: ('m_SpotCookie', 'Light', 'm_Cookie')}


def lost_builtin_refs(bundle_path):
    """(class ID, GameObject name) -> '{fileID: n, guid: g, type: 0}' for components of the original bundle that
    reference a Unity built-in (Cube, Quad, default cookie, ...). With engine assets ignored the rip writes those
    references as the 0000000deadbeef placeholder."""
    try:
        import UnityPy
    except ImportError:
        return {}
    env = UnityPy.load(bundle_path)
    names = {}
    for obj in env.objects:
        if obj.type.name == 'GameObject':
            try:
                names[(obj.assets_file.name, obj.path_id)] = obj.read_typetree().get('m_Name')
            except Exception:
                pass
    result = {}
    for class_id, (_, type_name, tree_field) in LOST_REF_FIELDS.items():
        for obj in env.objects:
            if obj.type.name != type_name:
                continue
            try:
                tree = obj.read_typetree()
                ref = tree[tree_field]
                if ref['m_FileID'] == 0:
                    continue
                ext = obj.assets_file.externals[ref['m_FileID'] - 1].path.split('/')[-1]
            except Exception:
                continue
            if ext in BUILTIN_FILES:
                go = names.get((obj.assets_file.name, tree['m_GameObject']['m_PathID']))
                value = '{fileID: %d, guid: %s, type: 0}' % (ref['m_PathID'], BUILTIN_FILES[ext])
                # a name used with two different built-ins cannot be resolved by name
                result[(class_id, go)] = value if result.get((class_id, go), value) == value else None
    return result


def restore_builtin_refs(text, refs):
    """Put lost built-in references back into the components of one exported YAML file."""
    docs = re.split(r'(?=^--- !u!)', text, flags=re.M)
    go_names = {}
    for d in docs:
        m = re.match(r'--- !u!1 &(-?\d+)', d)
        if m:
            n = re.search(r'^  m_Name: ([^\n]*)$', d, re.M)
            go_names[m.group(1)] = n.group(1) if n else None
    fixed = 0
    for i, d in enumerate(docs):
        m = re.match(r'--- !u!(\d+) ', d)
        if not m or int(m.group(1)) not in LOST_REF_FIELDS or DEAD_SHADER not in d:
            continue
        yaml_field = LOST_REF_FIELDS[int(m.group(1))][0]
        go = re.search(r'^  m_GameObject: \{fileID: (-?\d+)\}', d, re.M)
        value = refs.get((int(m.group(1)), go_names.get(go.group(1)) if go else None))
        if value:
            docs[i] = re.sub(r'(^  %s: )\{fileID: -?\d+, guid: %s, type: \d\}' % (yaml_field, DEAD_SHADER),
                             lambda mm: mm.group(1) + value, d, flags=re.M)
            fixed += docs[i] != d
    return ''.join(docs), fixed


KEY = re.compile(r'^( *)(- )?([^\s:#-][^:\n]*):$')


def fix_array_wrappers(text):
    """Serialized lists that the ripper read from a bundle's type tree come out with an extra 'Array:' level
    (key: / Array: / - item). Unity cannot read that and leaves the list at its default; put the items (or [])
    directly under the key."""
    lines = text.split('\n')
    out = []
    i = 0
    while i < len(lines):
        line = lines[i]
        m = KEY.match(line)
        if m and i + 1 < len(lines):
            col = len(m.group(1)) + (2 if m.group(2) else 0)
            nxt = lines[i + 1]
            if nxt == ' ' * (col + 2) + 'Array: []':
                out.append(line + ' []')
                i += 2
                continue
            if nxt == ' ' * (col + 2) + 'Array:':
                out.append(line)
                i += 2
                continue
        out.append(line)
        i += 1
    return '\n'.join(out)


def fix_particle_modules(text):
    """Unity 5.3 added CollisionModule.radiusScale (1 = the particle radius as before). The 5.2 data has none and it
    loads as 0. World collisions at High quality (raycasts against the level's colliders) still turn particle positions
    into NaN under 2017 (the butterflies of the city levels: Invalid AABB / IsFinite(outDistanceForSort) every frame);
    Medium quality (voxel cache) does not, and is cheaper on the Vita."""
    text = re.sub(r'(^  CollisionModule:\n(?:    (?!radiusScale).*\n)*?    particleRadius: [^\n]*\n)(?!    radiusScale)',
                  lambda m: m.group(1) + '    radiusScale: 1\n', text, flags=re.M)
    return re.sub(r'(^  CollisionModule:\n(?:    serializedVersion: \d+\n)?    enabled: \d\n    type: 1\n(?:    (?!quality:).*\n)*?)    quality: 0\n',
                  lambda m: m.group(1) + '    quality: 1\n', text, flags=re.M)


def fix_lod_groups(text):
    """The rip writes LOD fadeTransitionWidth as NaN (5.2 data has none); 0 is Unity's default."""
    return text.replace('    fadeTransitionWidth: NaN\n', '    fadeTransitionWidth: 0\n')


def import_bundle(args, bundle, index_entry, proj_shaders, builtin, shader_log, manifest):
    src_root = os.path.join(args.rip, bundle)
    src_assets = os.path.join(src_root, 'Assets')
    if not os.path.isfile(os.path.join(src_root, '.done')):
        print('  not ripped (no finished export):', bundle)
        return False
    dst_root = os.path.join(args.project, 'Assets', 'BundleAssets', bundle)
    if os.path.exists(dst_root):
        if not args.replace:
            print('  already imported:', bundle)
            return False
        # only ever the folder named after this bundle, inside Assets/BundleAssets
        shutil.rmtree(dst_root)
        meta = dst_root + '.meta'
        if os.path.exists(meta):
            os.remove(meta)

    # 1. shaders of this export, wherever they sit (Shader/ or a container path such as
    # Asset_Bundles/shared/shaders/...): GUID -> name. None of them is copied.
    export_shader = {}
    shader_files = [os.path.join(d, f) for d, _, fs in os.walk(src_assets) for f in fs if f.endswith('.shader')]
    for p in shader_files:
        with open(p, encoding='utf-8', errors='replace') as fh:
            m = SHADER_NAME_RE.search(fh.read())
        guid = read_guid(p + '.meta')
        if m and guid:
            export_shader[guid] = m.group(1)
            if shader_ref(m.group(1), proj_shaders, builtin) is None:
                keep = os.path.join(args.project, 'Tools', 'ShaderPort', 'bundle_originals',
                                    m.group(1).replace('/', '_').replace(' ', '_') + '.shader')
                if not os.path.exists(keep):
                    os.makedirs(os.path.dirname(keep), exist_ok=True)
                    shutil.copyfile(p, keep)
    lost = None
    builtin_refs = None

    # 2. copy, rewriting shader references
    copied = 0
    for d, dirs, files in os.walk(src_assets):
        rel_dir = os.path.relpath(d, src_assets)
        if rel_dir == '.':
            dirs[:] = [x for x in dirs if x not in SKIP_DIRS]
        out_dir = os.path.join(dst_root, rel_dir) if rel_dir != '.' else dst_root
        os.makedirs(out_dir, exist_ok=True)
        for f in files:
            if f.endswith(('.shader', '.shader.meta')):
                continue
            src = os.path.join(d, f)
            dst = os.path.join(out_dir, f)
            if f.endswith(('.mat', '.asset', '.prefab', '.unity', '.controller', '.overrideController')):
                with open(src, 'rb') as fh:
                    data = fh.read()
                text = data.decode('utf-8', errors='surrogateescape')
                changed = False
                for guid, name in export_shader.items():
                    if guid in text:
                        ref = shader_ref(name, proj_shaders, builtin)
                        if ref is not None:
                            text = re.sub(r'\{fileID: 4800000, guid: %s, type: \d\}' % guid, ref, text)
                        else:
                            shader_log.setdefault(guid, name)
                        changed = True
                if f.endswith(('.unity', '.prefab')) and DEAD_SHADER in text:
                    if builtin_refs is None:
                        builtin_refs = lost_builtin_refs(os.path.join(args.bundles, index_entry['rel'])) if args.bundles else {}
                    text, n = restore_builtin_refs(text, builtin_refs)
                    changed = changed or n > 0
                if 'Array:' in text:
                    fixed_text = fix_array_wrappers(text)
                    changed = changed or fixed_text != text
                    text = fixed_text
                if 'CollisionModule:' in text:
                    fixed_text = fix_particle_modules(text)
                    changed = changed or fixed_text != text
                    text = fixed_text
                if 'fadeTransitionWidth: NaN' in text:
                    fixed_text = fix_lod_groups(text)
                    changed = changed or fixed_text != text
                    text = fixed_text
                if f.endswith('.mat') and DEAD_SHADER in text:
                    if lost is None:
                        lost = lost_shader_names(os.path.join(args.bundles, index_entry['rel'])) if args.bundles else {}
                    m = re.search(r'^  m_Name: (.*)$', text, re.M)
                    name = lost.get(m.group(1).strip()) if m else None
                    ref = None
                    if name and name.startswith('builtin:'):
                        # Built-in shader IDs are stable across Unity versions (7 = Legacy Shaders/Diffuse, ...).
                        _, file_name, path_id = name.split(':')
                        file_guid = '0000000000000000f000000000000000' if 'builtin_extra' in file_name else '0000000000000000e000000000000000'
                        ref = '{fileID: %s, guid: %s, type: 0}' % (path_id, file_guid)
                    elif name:
                        ref = shader_ref(name, proj_shaders, builtin)
                    if ref:
                        text = re.sub(r'\{fileID: 4800000, guid: %s, type: \d\}' % DEAD_SHADER, ref, text)
                        changed = True
                    else:
                        print('    lost shader on', os.path.relpath(src, src_assets), '->', name)
                if changed:
                    with open(dst, 'wb') as fh:
                        fh.write(text.encode('utf-8', errors='surrogateescape'))
                else:
                    shutil.copyfile(src, dst)
            else:
                shutil.copyfile(src, dst)
            copied += 1

    # 2b. ripped AnimatorControllers are flat; put their states back into their sub-state machines
    for path, result in animator_hierarchy.restore_folder(dst_root):
        print('    %s: %s' % (os.path.basename(path), result))

    # 2c. animator parameter curves (Foot_R_front, IK_footl, ...) the rip drops from AnimationClips; smooth tangents
    if args.bundles:
        names = clip_params.parameter_names(os.path.join(args.project, 'Assets', 'BundleAssets'))
        clips, curves, smoothed = clip_params.restore_folder(dst_root, os.path.join(args.bundles, index_entry['rel']), names)
        if clips or smoothed:
            print('    clips: %d parameter curves added to %d clips, tangents smoothed in %d' % (curves, clips, smoothed))

    # 3. explicit asset names -> project paths. A container entry names an object of the bundle; the export names
    # each file after its object (m_Name + extension), so the object's name and type find it. Several entries can
    # name the same object, and an .fbx entry names a clip or mesh inside the model.
    exported = {}
    for d, _, files in os.walk(dst_root):
        for f in files:
            if not f.endswith('.meta'):
                p = os.path.join(d, f)
                exported.setdefault(os.path.splitext(f)[0].lower(), []).append(p)
    objects = {(o['file'], o['path_id']): o for o in index_entry['objects']}
    bundle_file = next((o['file'] for o in index_entry['objects'] if o['type'] == 'AssetBundle'), None)
    entries = []
    skipped = 0
    for c in index_entry['container']:
        key = c['name']
        obj = objects.get((bundle_file, c['path_id'])) if c['file_id'] == 0 else None
        if index_entry['scene_bundle']:
            # a scene bundle's entries name the scene (path ID 0, no object)
            obj = {'type': 'SceneAsset', 'name': os.path.splitext(os.path.basename(key))[0]}
        elif obj is None or obj['type'] in NOT_LOADABLE:
            # scripts and built-in resources are listed in containers but never loaded from the bundle
            skipped += 1
            continue
        if index_entry['scene_bundle']:
            found = next((p for ps in exported.values() for p in ps if p.endswith('.unity')
                          and os.path.splitext(os.path.basename(p))[0].lower() == os.path.splitext(os.path.basename(key))[0].lower()), None)
        else:
            exts = TYPE_EXTENSIONS.get(obj['type'])
            cands = [p for p in exported.get((obj['name'] or '').lower(), [])
                     if exts is None or os.path.splitext(p)[1].lower() in exts]
            rel_dir = os.path.dirname(key[len('assets/'):] if key.lower().startswith('assets/') else key).replace('\\', '/').lower()
            cands.sort(key=lambda p: (rel_dir not in os.path.relpath(p, dst_root).replace('\\', '/').lower(), len(p)))
            found = cands[0] if cands else None
        entries.append({'name': key, 'type': obj['type'],
                        'path': os.path.relpath(found, args.project).replace('\\', '/') if found else None})
    # A model's entry also lists its child GameObjects (bones, LODs): they come with the root's prefab. Keep only
    # what resolved, and report names that resolved to nothing at all.
    resolved_names = {e['name'] for e in entries if e['path'] is not None}
    missing = sorted({(e['name'], e['type']) for e in entries if e['name'] not in resolved_names})
    entries = [e for e in entries if e['path'] is not None]
    if missing:
        print('    %d explicit assets not found in the export, e.g. %s' % (len(missing), missing[:3]))
    manifest[index_entry['file']] = {'bundle': bundle, 'scene': index_entry['scene_bundle'], 'assets': entries}
    print('  imported %s: %d files, %d explicit assets' % (bundle, copied, len(entries)))
    return True


def write_tsv(manifest, path):
    """bundles.json flattened for the game code (JsonUtility cannot read it): one line per explicit asset,
    bundle file <TAB> scene (0/1) <TAB> explicit name <TAB> type <TAB> project path."""
    with open(path, 'w', encoding='utf-8', newline='\n') as fh:
        for bundle_file in sorted(manifest):
            entry = manifest[bundle_file]
            for asset in entry['assets']:
                fh.write('%s\t%d\t%s\t%s\t%s\n' % (bundle_file, 1 if entry['scene'] else 0, asset['name'],
                                                   asset.get('type', ''), asset['path']))


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument('--project', required=True)
    ap.add_argument('--rip', required=True)
    ap.add_argument('--index', required=True)
    ap.add_argument('--bundles')
    ap.add_argument('--replace', action='store_true')
    ap.add_argument('--pylib', help='extra module folder (UnityPy), for python -I')
    ap.add_argument('names', nargs='+')
    args = ap.parse_args()
    if args.pylib:
        sys.path.insert(0, args.pylib)

    index = {os.path.splitext(b['file'])[0]: b for b in json.load(open(args.index, encoding='utf-8'))}
    if args.bundles:
        for b in index.values():
            for d, _, files in os.walk(args.bundles):
                if b['file'] in files:
                    b['rel'] = os.path.relpath(os.path.join(d, b['file']), args.bundles)
    selected = sorted({n for pat in args.names for n in index if fnmatch.fnmatch(n, pat)})
    if not selected:
        sys.exit('no bundle matches ' + ' '.join(args.names))

    import_dir = os.path.join(args.project, 'Assets', 'BundleAssets', '_import')
    os.makedirs(import_dir, exist_ok=True)
    manifest_path = os.path.join(import_dir, 'bundles.json')
    shaders_path = os.path.join(import_dir, 'shaders.json')
    manifest = json.load(open(manifest_path, encoding='utf-8')) if os.path.exists(manifest_path) else {}
    shader_log = json.load(open(shaders_path, encoding='utf-8')) if os.path.exists(shaders_path) else {}

    proj = project_shaders(args.project)
    builtin = builtin_shaders(args.project)
    print('project shaders: %d, built-in: %d' % (len(proj), len(builtin)))
    for name in selected:
        print(name)
        import_bundle(args, name, index[name], proj, builtin, shader_log, manifest)

    with open(manifest_path, 'w', encoding='utf-8', newline='\n') as fh:
        json.dump(manifest, fh, indent=1, sort_keys=True)
    # drop shaders that have been ported since an earlier import
    shader_log = {g: n for g, n in shader_log.items() if shader_ref(n, proj, builtin) is None}
    with open(shaders_path, 'w', encoding='utf-8', newline='\n') as fh:
        json.dump(shader_log, fh, indent=1, sort_keys=True)
    write_tsv(manifest, os.path.join(import_dir, 'bundles.tsv'))
    unresolved = sorted(set(shader_log.values()))
    print('shaders still to port (%d): %s' % (len(unresolved), ', '.join(unresolved)))


if __name__ == '__main__':
    main()
