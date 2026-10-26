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

    # 1. shaders of this export: GUID -> name
    export_shader = {}
    shader_dir = os.path.join(src_assets, 'Shader')
    if os.path.isdir(shader_dir):
        for f in os.listdir(shader_dir):
            if f.endswith('.shader'):
                p = os.path.join(shader_dir, f)
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

    # 2. copy, rewriting shader references
    copied = 0
    for d, dirs, files in os.walk(src_assets):
        rel_dir = os.path.relpath(d, src_assets)
        if rel_dir == '.':
            dirs[:] = [x for x in dirs if x not in SKIP_DIRS]
        out_dir = os.path.join(dst_root, rel_dir) if rel_dir != '.' else dst_root
        os.makedirs(out_dir, exist_ok=True)
        for f in files:
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
    with open(shaders_path, 'w', encoding='utf-8', newline='\n') as fh:
        json.dump(shader_log, fh, indent=1, sort_keys=True)
    write_tsv(manifest, os.path.join(import_dir, 'bundles.tsv'))
    unresolved = sorted(set(shader_log.values()))
    print('shaders still to port (%d): %s' % (len(unresolved), ', '.join(unresolved)))


if __name__ == '__main__':
    main()
