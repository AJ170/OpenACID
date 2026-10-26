"""Point imported bundle assets at shaders ported after their import.

import_bundles.py leaves references to shaders the project did not have yet on their export GUID and lists them in
Assets/BundleAssets/_import/shaders.json (export GUID -> shader name). Once such a shader is ported, this rewrites
every reference to it under Assets/BundleAssets (materials, prefabs, scenes, assets) the same way the importer would
have, and drops it from shaders.json. No re-import needed.

    python -I Tools/BundleImport/shader_remap.py [--project <ACIdentity2017>]
"""
import argparse
import json
import os
import re
import sys

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))  # python -I leaves the script folder out
from import_bundles import builtin_shaders, project_shaders, shader_ref  # noqa: E402

TEXT_EXTENSIONS = ('.mat', '.prefab', '.unity', '.asset')


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument('--project', default=os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))))
    args = ap.parse_args()
    root = os.path.join(args.project, 'Assets', 'BundleAssets')
    log_path = os.path.join(root, '_import', 'shaders.json')
    pending = json.load(open(log_path, encoding='utf-8'))
    proj, builtin = project_shaders(args.project), builtin_shaders(args.project)
    resolved = {g: shader_ref(n, proj, builtin) for g, n in pending.items()}
    resolved = {g: r for g, r in resolved.items() if r is not None}
    if not resolved:
        print('nothing to remap; still missing: %s' % ', '.join(sorted(set(pending.values()))))
        return
    pattern = re.compile(r'\{fileID: 4800000, guid: (%s), type: \d\}' % '|'.join(resolved))
    files = refs = 0
    for d, _, names in os.walk(root):
        for f in names:
            if not f.endswith(TEXT_EXTENSIONS):
                continue
            p = os.path.join(d, f)
            with open(p, 'rb') as fh:
                text = fh.read().decode('utf-8', errors='surrogateescape')
            new, n = pattern.subn(lambda m: resolved[m.group(1)], text)
            if n:
                with open(p, 'wb') as fh:
                    fh.write(new.encode('utf-8', errors='surrogateescape'))
                files += 1
                refs += n
    for g in resolved:
        pending.pop(g)
    with open(log_path, 'w', encoding='utf-8', newline='\n') as fh:
        json.dump(pending, fh, indent=1, sort_keys=True)
    print('remapped %d references in %d files to %d shaders' % (refs, files, len(resolved)))
    print('still missing (%d): %s' % (len(set(pending.values())), ', '.join(sorted(set(pending.values())))))


if __name__ == '__main__':
    main()
