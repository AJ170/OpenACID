"""Put back the animator parameter curves the rip drops from AnimationClips.

A clip can drive Animator float parameters (generic bindings with class ID 95, custom type 0, attribute = CRC32 of the
parameter name). The legacy AssetRipper only knows the hash, not the name, so it leaves those curves out. The game's
controllers depend on them: Foot_R_front picks every left/right-foot transition (attacks from a right-foot combat
stance need Foot_R_front > 0.5, so without it the player's attack never starts and combat locks up), IK_footl /
IK_footr weight the foot IK, MatchStart / MatchEnd, JumpFootr, Foot_R_on_ground, ConstWeaponToHand.

The values come from the original bundle's muscle clip (streamed, dense or constant curves; curve i belongs to
generic binding i) and are written into the ripped .anim as m_FloatCurves sampled at the clip's sample rate, the same
way the rip writes the other curves.

    restore_folder(<imported bundle folder>, <original .unity3d>)
"""
import os
import re
import struct
import zlib

FLOAT_CURVES_RE = re.compile(r'^  m_FloatCurves:( \[\])?\n', re.M)
PPTR_CURVES = '  m_PPtrCurves:'
UNSAFE_NAME_RE = re.compile(r'[<>:"/\\|?*]')


def parameter_names(folder):
    """CRC32 -> name of every parameter of the controllers in an imported bundle folder tree."""
    names = {}
    for d, _, files in os.walk(folder):
        for f in files:
            if f.endswith('.controller'):
                with open(os.path.join(d, f), encoding='utf-8', errors='replace') as fh:
                    for n in re.findall(r'^  - m_Name: (\S+)\n    m_Type: \d', fh.read(), re.M):
                        names[zlib.crc32(n.encode())] = n
    return names


def _streamed_keys(words):
    """Streamed clip data (uint32 words) -> {curve index: [(time, (c0, c1, c2, c3))]}."""
    raw = struct.pack('<%dI' % len(words), *words)
    keys = {}
    pos = 0
    while pos + 8 <= len(raw):
        time, count = struct.unpack_from('<fI', raw, pos)
        pos += 8
        for _ in range(count):
            index, c0, c1, c2, c3 = struct.unpack_from('<I4f', raw, pos)
            pos += 20
            keys.setdefault(index, []).append((time, (c0, c1, c2, c3)))
    return keys


def _eval_streamed(keys, t):
    """Value at t: the cubic of the last key at or before t, in the time since that key."""
    best = keys[0]
    for k in keys:
        if k[0] <= t:
            best = k
        else:
            break
    time, (c0, c1, c2, c3) = best
    dt = 0.0 if time < -1e30 else t - time
    return ((c0 * dt + c1) * dt + c2) * dt + c3


def original_curves(bundle_path, names, curve_filter=None):
    """{clip name: (sample rate, [(parameter, [(time, value)])])} from an original bundle."""
    import UnityPy
    env = UnityPy.load(bundle_path)
    out = {}
    for obj in env.objects:
        if obj.type.name != 'AnimationClip':
            continue
        tree = obj.read_typetree()
        bindings = tree['m_ClipBindingConstant']['genericBindings']
        wanted = [(i, names[b['attribute']]) for i, b in enumerate(bindings)
                  if b['classID'] == 95 and b['customType'] == 0 and b['attribute'] in names]
        if curve_filter is not None:
            wanted = [w for w in wanted if curve_filter(w[1])]
        if not wanted:
            continue
        muscle = tree['m_MuscleClip']
        clip = muscle['m_Clip']['data']
        streamed = clip['m_StreamedClip']
        dense = clip['m_DenseClip']
        constant = clip['m_ConstantClip']['data']
        n_streamed = streamed['curveCount']
        n_dense = dense['m_CurveCount']
        skeys = _streamed_keys(streamed['data']) if n_streamed else {}
        rate = tree['m_SampleRate'] or 30.0
        start, stop = muscle['m_StartTime'], muscle['m_StopTime']
        frames = max(1, int(round((stop - start) * rate)) + 1)
        times = [min(start + i / rate, stop) for i in range(frames)]
        curves = []
        for index, name in wanted:
            if index < n_streamed:
                k = skeys.get(index)
                if not k:
                    continue
                values = [_eval_streamed(k, t) for t in times]
            elif index < n_streamed + n_dense:
                col = index - n_streamed
                samples = dense['m_SampleArray']
                d_rate, begin = dense['m_SampleRate'], dense['m_BeginTime']
                values = []
                for t in times:
                    f = min(max(int(round((t - begin) * d_rate)), 0), dense['m_FrameCount'] - 1)
                    values.append(samples[f * n_dense + col])
            else:
                c = index - n_streamed - n_dense
                if c >= len(constant):
                    continue
                values = [constant[c]] * len(times)
            curves.append((name, list(zip(times, values))))
        if curves:
            out.setdefault(tree['m_Name'], (rate, curves))
            # the rip writes characters file names can't hold as '_' (deflect_ul_footr>>attacker_ur)
            out.setdefault(UNSAFE_NAME_RE.sub('_', tree['m_Name']), (rate, curves))
    return out


def _fmt(x):
    s = repr(float(x))
    return s[:-2] if s.endswith('.0') else s


def curve_yaml(name, keys):
    lines = ['  - curve:', '      serializedVersion: 2', '      m_Curve:']
    for t, v in keys:
        lines += ['      - time: %s' % _fmt(t), '        value: %s' % _fmt(v), '        inSlope: 0',
                  '        outSlope: 0', '        tangentMode: 0']
    lines += ['      m_PreInfinity: 2', '      m_PostInfinity: 2', '    attribute: %s' % name, '    path: ',
              '    classID: 95', '    script: {fileID: 0}']
    return '\n'.join(lines) + '\n'


def restore_clip(text, curves):
    """Add the parameter curves a ripped .anim lacks. Returns (text, number added)."""
    have = set(re.findall(r'^    attribute: (\S+)\n    path: ?\n    classID: 95\n', text, re.M))
    add = [(n, k) for n, k in curves if n not in have]
    if not add:
        return text, 0
    m = FLOAT_CURVES_RE.search(text)
    i = text.find(PPTR_CURVES)
    if not m or i < 0:
        return text, 0
    block = ''.join(curve_yaml(n, k) for n, k in add)
    if m.group(1):  # m_FloatCurves: []
        text = text[:m.start()] + '  m_FloatCurves:\n' + block + text[m.end():]
    else:
        text = text[:i] + block + text[i:]
    return text, len(add)


KEY_RE = re.compile(r'(      - time: (\S+)\n        value: (\S+)\n        inSlope: )\S+(\n        outSlope: )\S+(\n)')


def smooth_tangents(text, keep_flat):
    """The rip samples every curve once per frame with flat tangents (inSlope = outSlope = 0), so Unity eases in and
    out of every key and the motion stutters at the sample rate (the choppy run). Give each key the slope through
    its neighbours (one-sided at the ends); the samples are dense, so this follows the original curves closely.
    Curves of animator parameters (keep_flat: Foot_R_front, ...) are steps and stay flat. Returns (text, curves)."""
    start = text.find('  m_FloatCurves:\n')
    end = text.find(PPTR_CURVES, start)
    if start < 0 or end < 0:
        return text, 0
    section = text[start:end]
    parts = re.split(r'(?=^  - curve:\n)', section, flags=re.M)
    changed = 0
    for n, part in enumerate(parts):
        m = re.search(r'^    attribute: (\S+)', part, re.M)
        if not m or m.group(1) in keep_flat:
            continue
        keys = [(float(k.group(2)), float(k.group(3))) for k in KEY_RE.finditer(part)]
        if len(keys) < 2:
            continue
        slopes = []
        for i in range(len(keys)):
            a = keys[max(i - 1, 0)]
            b = keys[min(i + 1, len(keys) - 1)]
            slopes.append((b[1] - a[1]) / (b[0] - a[0]) if b[0] != a[0] else 0.0)
        it = iter(slopes)

        def put(k):
            s = '%.7g' % next(it)
            return k.group(1) + s + k.group(4) + s + k.group(5)
        parts[n] = KEY_RE.sub(put, part)
        changed += 1
    return text[:start] + ''.join(parts) + text[end:], changed


def restore_folder(folder, bundle_path, names=None):
    """Restore the parameter curves of every .anim in an imported bundle folder and smooth the tangents of the
    others. Returns (clips with curves added, curves added, clips smoothed)."""
    names = names if names is not None else parameter_names(folder)
    anims = []
    for d, _, files in os.walk(folder):
        anims += [os.path.join(d, f) for f in files if f.endswith('.anim')]
    if not anims:
        return 0, 0, 0
    originals = original_curves(bundle_path, names) if names else {}
    keep_flat = set(names.values())
    clips = added = smoothed = 0
    for p in anims:
        with open(p, 'rb') as fh:
            original_text = text = fh.read().decode('utf-8', errors='surrogateescape')
        m = re.search(r'^  m_Name: (.*)$', text, re.M)
        entry = originals.get(m.group(1).strip()) if m else None
        if entry:
            text, n = restore_clip(text, entry[1])
            if n:
                clips += 1
                added += n
        text, n = smooth_tangents(text, keep_flat)
        if n:
            smoothed += 1
        if text != original_text:
            with open(p, 'wb') as fh:
                fh.write(text.encode('utf-8', errors='surrogateescape'))
    return clips, added, smoothed
