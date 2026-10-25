"""Rebuild the ripped AC Identity shaders as Cg source for Unity 2017.4 (PS Vita).

The AssetRipper export only contains Unity 5.2 *compiled* shaders (GLES/GLES3
programs, 2-3 junk bytes in front of `Shader "..."`). This tool keeps the
original ShaderLab shell of every shader (Properties, SubShader/Pass tags,
render states, Fallback) and swaps each compiled `Program` block for a
hand-written CGPROGRAM body taken from a spec file.

Layout
  _RipReference/OriginalShaders/<path>.shader   untouched originals (junk bytes stripped)
  Tools/ShaderPort/specs/<path>.shader.cg       hand-ported Cg bodies, one per pass
  Assets/<path>.shader                          generated output (keeps its .meta / GUID)

Spec format
  //@@KEEP_SUBSHADERS 0 2    optional: indices of SubShaders to keep (default: every
                             SubShader that had compiled programs)
  //@@DROP_PASSES 1          optional: indices of Pass blocks (after the SubShader filter)
                             to remove, e.g. the obsolete SHADOWCOLLECTOR pass
  //@@VERBATIM               optional: the spec is a complete .shader file, copy as-is
  text before the first //@@PASS is prepended to every pass body
  //@@PASS                   starts the body of the next programmable pass (in order)

`#include "ACID/<file>"` is rewritten to the relative path of Assets/Shader/Include/<file>.

usage: python -I Tools/ShaderPort/port_shaders.py [--snapshot] [--check]
  --snapshot  copy Assets shaders that still hold compiled programs into _RipReference first
  --check     list shaders that still have no spec
"""
import os
import re
import sys

ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
ASSETS = os.path.join(ROOT, 'Assets')
ORIGINALS = os.path.join(ROOT, '_RipReference', 'OriginalShaders')
SPECS = os.path.join(ROOT, 'Tools', 'ShaderPort', 'specs')
INCLUDE_DIR = os.path.join(ASSETS, 'Shader', 'Include')

TAG_KEYS = {
    'LIGHTMODE': 'LightMode', 'QUEUE': 'Queue', 'IGNOREPROJECTOR': 'IgnoreProjector',
    'RENDERTYPE': 'RenderType', 'FORCENOSHADOWCASTING': 'ForceNoShadowCasting',
    'PREVIEWTYPE': 'PreviewType', 'CANUSESPRITEATLAS': 'CanUseSpriteAtlas',
    'DISABLEBATCHING': 'DisableBatching',
}
LIGHTMODES = {
    'FORWARDBASE': 'ForwardBase', 'FORWARDADD': 'ForwardAdd', 'SHADOWCASTER': 'ShadowCaster',
    'ALWAYS': 'Always', 'VERTEX': 'Vertex', 'VERTEXLM': 'VertexLM', 'DEFERRED': 'Deferred',
    'META': 'Meta', 'PREPASSBASE': 'PrepassBase', 'PREPASSFINAL': 'PrepassFinal',
}
# Compiled-only tags that have no meaning in source form.
DROP_TAGS = {'SHADOWSUPPORT'}


def read_original_text(path):
    raw = open(path, 'rb').read()
    i = raw.find(b'Shader "')
    if i < 0:
        raise ValueError(f'{path}: no Shader keyword')
    return raw[i:].decode('utf8')


def is_compiled(text):
    return re.search(r'^\s*Program "(vp|fp)" \{', text, re.M) is not None


def find_block_end(text, open_brace):
    """Index just past the brace matching text[open_brace]; quotes are skipped."""
    depth, i, in_str = 0, open_brace, False
    while i < len(text):
        ch = text[i]
        if ch == '"':
            in_str = not in_str
        elif not in_str:
            if ch == '{':
                depth += 1
            elif ch == '}':
                depth -= 1
                if depth == 0:
                    return i + 1
        i += 1
    raise ValueError('unbalanced braces')


def strip_programs(text):
    """Replace each pass's compiled programs with one @@PROGRAM@@ marker."""
    out, pos = [], 0
    pat = re.compile(r'^([ \t]*)Program "(vp|fp)" \{', re.M)
    while True:
        m = pat.search(text, pos)
        if not m:
            out.append(text[pos:])
            break
        out.append(text[pos:m.start()])
        end = find_block_end(text, m.end() - 1)
        if m.group(2) == 'vp':
            out.append(m.group(1) + '@@PROGRAM@@\n')
        # drop the newline that followed the removed block
        pos = end + 1 if end < len(text) and text[end] == '\n' else end
    text = ''.join(out)
    text = re.sub(r'^[ \t]*GpuProgramID \d+\n', '', text, flags=re.M)
    return text


def normalize_tags(text):
    def fix_tags(m):
        body = m.group(1)
        pairs = re.findall(r'"([^"]*)"\s*=\s*"([^"]*)"', body)
        res = []
        for k, v in pairs:
            ku = k.upper()
            if ku in DROP_TAGS:
                continue
            k = TAG_KEYS.get(ku, k)
            if k == 'LightMode':
                v = LIGHTMODES.get(v.upper(), v)
            res.append(f'"{k}"="{v}"')
        return 'Tags { ' + ' '.join(res) + ' }'
    text = re.sub(r'Tags \{([^}]*)\}', fix_tags, text)
    # Unity 5.2 serialised `ZTest Off` as `ZTest False`, which ShaderLab does not parse.
    return re.sub(r'\bZTest False\b', 'ZTest Always', text)


def subshader_spans(text):
    spans = []
    for m in re.finditer(r'^[ \t]*SubShader \{', text, re.M):
        spans.append((m.start(), find_block_end(text, m.end() - 1)))
    return spans


def parse_spec(path):
    src = open(path, encoding='utf8').read()
    directives = dict(re.findall(r'^//@@(KEEP_SUBSHADERS|DROP_PASSES|VERBATIM)\b[ \t]*(.*)$', src, re.M))
    if 'VERBATIM' in directives:
        return directives, None, None
    parts = re.split(r'^//@@PASS[^\n]*\n', src, flags=re.M)
    shared = re.sub(r'^//@@(KEEP_SUBSHADERS|DROP_PASSES)[^\n]*\n', '', parts[0], flags=re.M)
    return directives, shared, parts[1:]


def fix_includes(body, out_path):
    rel = os.path.relpath(INCLUDE_DIR, os.path.dirname(out_path)).replace(os.sep, '/')
    return body.replace('#include "ACID/', f'#include "{rel}/')


def build(rel):
    orig_path = os.path.join(ORIGINALS, rel)
    spec_path = os.path.join(SPECS, rel + '.cg')
    out_path = os.path.join(ASSETS, rel)
    directives, shared, bodies = parse_spec(spec_path)
    if 'VERBATIM' in directives:
        text = open(spec_path, encoding='utf8').read()
        text = re.sub(r'^//@@VERBATIM[^\n]*\n', '', text, flags=re.M)
        return fix_includes(text, out_path)

    text = normalize_tags(strip_programs(read_original_text(orig_path)))
    spans = subshader_spans(text)
    if 'KEEP_SUBSHADERS' in directives:
        keep = {int(x) for x in directives['KEEP_SUBSHADERS'].split()}
    else:
        keep = {i for i, (a, b) in enumerate(spans) if '@@PROGRAM@@' in text[a:b]}
    for i in reversed(range(len(spans))):
        if i not in keep:
            a, b = spans[i]
            if b < len(text) and text[b] == '\n':
                b += 1
            text = text[:a] + text[b:]

    if 'DROP_PASSES' in directives:
        drop = {int(x) for x in directives['DROP_PASSES'].split()}
        passes = [(m.start(), find_block_end(text, m.end() - 1))
                  for m in re.finditer(r'^[ \t]*Pass \{', text, re.M)]
        for i in reversed(range(len(passes))):
            if i in drop:
                a, b = passes[i]
                if b < len(text) and text[b] == '\n':
                    b += 1
                text = text[:a] + text[b:]

    markers = text.count('@@PROGRAM@@')
    if markers != len(bodies):
        raise ValueError(f'{rel}: {markers} programmable passes but spec has {len(bodies)} //@@PASS bodies')
    for body in bodies:
        block = '  CGPROGRAM\n' + (shared + body).strip('\n') + '\nENDCG'
        text = text.replace('@@PROGRAM@@', block.replace('\n', '\n  '), 1)
    header = ('// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.\n'
              '// Edit Tools/ShaderPort/specs/' + rel.replace(os.sep, '/') + '.cg, not this file.\n')
    return fix_includes(header + text.rstrip() + '\n', out_path)


def all_shaders(base):
    for dp, _, fns in os.walk(base):
        for fn in fns:
            if fn.endswith('.shader'):
                yield os.path.relpath(os.path.join(dp, fn), base)


def snapshot():
    n = 0
    for rel in all_shaders(ASSETS):
        src = os.path.join(ASSETS, rel)
        raw = open(src, 'rb').read()
        if b'Program "vp"' not in raw:
            continue
        dst = os.path.join(ORIGINALS, rel)
        if os.path.exists(dst):
            continue
        os.makedirs(os.path.dirname(dst), exist_ok=True)
        open(dst, 'w', encoding='utf8', newline='\n').write(read_original_text(src))
        n += 1
    print(f'snapshot: {n} original shaders copied to {ORIGINALS}')


def main():
    if '--snapshot' in sys.argv:
        snapshot()
    built, missing, errors = 0, [], []
    for rel in sorted(all_shaders(ORIGINALS)):
        if not os.path.exists(os.path.join(SPECS, rel + '.cg')):
            missing.append(rel)
            continue
        try:
            out = build(rel)
        except Exception as e:  # report every broken spec in one run
            errors.append(f'{rel}: {e}')
            continue
        out_path = os.path.join(ASSETS, rel)
        old = open(out_path, encoding='utf8', errors='replace').read() if os.path.exists(out_path) else None
        if old != out:
            open(out_path, 'w', encoding='utf8', newline='\n').write(out)
        built += 1
    print(f'built {built} shaders, {len(missing)} without spec, {len(errors)} errors')
    for e in errors:
        print('  ERROR', e)
    if '--check' in sys.argv:
        for m in missing:
            print('  no spec:', m)
    return 1 if errors else 0


if __name__ == '__main__':
    sys.exit(main())
