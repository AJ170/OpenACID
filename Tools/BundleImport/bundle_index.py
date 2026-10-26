"""Index the game's original (Unity 5.2, Android) asset bundles.

For every bundle: the explicit asset names the game loads (m_Container), the main asset, whether it is a streamed
scene bundle, the bundles it depends on (externals), and every object in it (path ID, type, name). The import tool
uses this to put each exported asset back into the bundle it came from (the AssetRipper export is not split by
bundle).

Needs UnityPy (pip install --target <dir> UnityPy; add <dir> to PYTHONPATH).

    python bundle_index.py <folder with .unity3d files (recursive)> <out.json>
"""
import json
import os
import sys

import UnityPy


def index_bundle(path):
    env = UnityPy.load(path)
    bundle = {'file': os.path.basename(path), 'serialized_files': {}, 'container': [], 'main_asset': None,
              'scene_bundle': False, 'objects': []}
    for bundle_file in env.files.values():
        for name, sf in getattr(bundle_file, 'files', {}).items():
            if hasattr(sf, 'externals'):
                bundle['serialized_files'][name] = [e.path for e in sf.externals]
    for obj in env.objects:
        type_name = obj.type.name
        entry = {'file': obj.assets_file.name, 'path_id': obj.path_id, 'type': type_name}
        if type_name == 'AssetBundle':
            tree = obj.read_typetree()
            bundle['asset_bundle_name'] = tree.get('m_AssetBundleName', '')
            bundle['scene_bundle'] = bool(tree.get('m_IsStreamedSceneAssetBundle'))
            for name, info in tree['m_Container']:
                bundle['container'].append({'name': name, 'file_id': info['asset']['m_FileID'],
                                            'path_id': info['asset']['m_PathID']})
            main = tree['m_MainAsset']['asset']
            bundle['main_asset'] = {'file_id': main['m_FileID'], 'path_id': main['m_PathID']}
        else:
            try:
                entry['name'] = obj.peek_name()
            except Exception:
                entry['name'] = None
        bundle['objects'].append(entry)
    return bundle


def main():
    root, out = sys.argv[1], sys.argv[2]
    result = []
    for dirpath, _, files in os.walk(root):
        for f in sorted(files):
            if f.endswith('.unity3d'):
                print('indexing', f, flush=True)
                result.append(index_bundle(os.path.join(dirpath, f)))
    with open(out, 'w', encoding='utf-8', newline='\n') as fh:
        json.dump(result, fh, indent=1)
    print('wrote', out, len(result), 'bundles')


if __name__ == '__main__':
    main()
