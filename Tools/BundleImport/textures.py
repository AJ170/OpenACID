"""Texture repairs for the rebuilt (PS Vita) bundles.

The bundles are Android ETC2 builds, and the rip carries that over:
- Every PNG's import settings force the Android format (legacy TextureImporter textureFormat 45 = ETC2 RGB,
  47 = ETC2 RGBA8, ...). The Vita cannot sample ETC, so Unity would decompress them to RGBA32 on PSP2. The format
  is set to -1 (automatic compressed): each platform picks its own compression, with alpha where the texture has it.
- Textures the rip could not turn into images stay raw Texture2D .asset files with the ETC data inside (the
  levels' lightmaps, 4096x4096 ETC2). Those are decoded from the original bundle (UnityPy) into a PNG under the
  same GUID, so everything that references them (LightingData.asset) keeps working.

    fix_texture_meta(text) -> text
    convert_raw_textures(<imported bundle folder>, <original .unity3d>) -> [converted names]
"""
import os
import re
import time

FORCED_FORMAT_RE = re.compile(r'^(  textureFormat: )(?!-1$)-?\d+$', re.M)
# TextureFormat values the Vita GPU cannot sample: ETC_RGB4, EAC/ETC2 variants
ETC_FORMATS = {34, 41, 42, 43, 44, 45, 46, 47}

META = '''fileFormatVersion: 2
guid: {guid}
timeCreated: {time}
licenseType: Free
TextureImporter:
  serializedVersion: 2
  mipmaps:
    mipMapMode: 0
    enableMipMap: {mips}
    linearTexture: 0
    correctGamma: 0
  isReadable: 0
  textureFormat: -1
  maxTextureSize: {size}
  textureSettings:
    filterMode: {filter}
    aniso: {aniso}
    mipBias: 0
    wrapMode: {wrap}
  nPOTScale: 0
  lightmap: 0
  alphaIsTransparency: 0
  textureType: 0
  buildTargetSettings:
  - buildTarget: DefaultTexturePlatform
    maxTextureSize: {size}
    textureFormat: -1
    compressionQuality: 50
    allowsAlphaSplitting: 0
  userData:
  assetBundleName:
  assetBundleVariant:
'''


def fix_texture_meta(text):
    return FORCED_FORMAT_RE.sub(r'\g<1>-1', text)


def _field(text, name):
    m = re.search(r'^  +%s: (-?\d+)' % name, text, re.M)
    return int(m.group(1)) if m else None


def convert_raw_textures(folder, bundle_path):
    raw = {}
    for d, _, files in os.walk(folder):
        for f in files:
            if not f.endswith('.asset'):
                continue
            p = os.path.join(d, f)
            with open(p, 'rb') as fh:
                head = fh.read(1500).decode('utf-8', 'replace')
            if '--- !u!28 ' not in head or _field(head, 'm_TextureFormat') not in ETC_FORMATS:
                continue
            m = re.search(r'^  m_Name: (.*)$', head, re.M)
            raw[m.group(1).strip()] = (p, head)
    if not raw:
        return []
    import UnityPy
    env = UnityPy.load(bundle_path)
    done = []
    for obj in env.objects:
        if obj.type.name != 'Texture2D':
            continue
        tex = obj.read()
        entry = raw.get(tex.m_Name)
        if entry is None:
            continue
        path, head = entry
        png = os.path.splitext(path)[0] + '.png'
        tex.image.save(png)
        meta = open(path + '.meta', encoding='utf-8').read()
        guid = re.search(r'^guid: (\w+)', meta, re.M).group(1)
        with open(png + '.meta', 'w', encoding='utf-8', newline='\n') as fh:
            fh.write(META.format(guid=guid, time=int(time.time()), mips=1 if (_field(head, 'm_MipCount') or 1) > 1 else 0,
                                 size=max(tex.m_Width, tex.m_Height, 32), filter=_field(head, 'm_FilterMode') or 1,
                                 aniso=_field(head, 'm_Aniso') or 1, wrap=_field(head, 'm_WrapMode') or 0))
        os.remove(path)
        os.remove(path + '.meta')
        done.append(tex.m_Name)
        del raw[tex.m_Name]
    return done
