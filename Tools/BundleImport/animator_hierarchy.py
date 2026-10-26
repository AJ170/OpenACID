"""Restore the sub-state machines of ripped AnimatorControllers.

The legacy AssetRipper flattens every layer of a Unity 5 AnimatorController into one state machine. A state's
identity at runtime is the hash of its full path ("Base.Jump_down.LandOnGround.damage_land_roll"), so every nested
state gets a different hash than the game's code and its state tables expect (AnimatorStateMachine.OnStateChange
KeyNotFoundException, Animator.Play of a missing state).

The game ships a table per controller (<controller>_statemachine.bytes, read by AnimatorStateMachineStatic) that
names every state and transition by full path. For each layer this puts every ripped state back into the
sub-state machine its path names, creating the machines. Transitions point at states by file ID and stay valid.
Two states with the same name in one layer (e.g. Strafe_light_footl in two strafe machines) are told apart by
their transitions, which the table also lists by full path.

    python animator_hierarchy.py <folder>...   (every *.controller that has a *_statemachine.bytes next to it)
"""
import os
import random
import re
import sys

DOC_SPLIT = re.compile(r'(?=^--- !u!)', re.M)
HEADER = re.compile(r'^--- !u!(\d+) &(-?\d+)')


def table_strings(path, layers):
    """Length-prefixed '<layer>.…' strings of a state table: (paths, transitions as (src, dst))."""
    d = open(path, 'rb').read()
    pat = re.compile(b'(?:' + b'|'.join(re.escape(l.encode()) for l in layers) + rb')\.')
    paths, transitions = set(), set()
    for m in pat.finditer(d):
        i = m.start()
        n = d[i - 1]
        s = d[i:i + n]
        if len(s) != n or not all(32 <= c < 127 for c in s):
            continue
        t = s.decode()
        if ' -> ' in t:
            a, b = t.split(' -> ', 1)
            transitions.add((a, b))
        elif '.' in t and not t.endswith('.'):
            paths.add(t)
    return paths, transitions


def field(doc, name):
    m = re.search(r'^  %s: ([^\n]*)$' % name, doc, re.M)
    return m.group(1) if m else None


def restore(controller, table):
    text = open(controller, encoding='utf-8').read()
    docs = DOC_SPLIT.split(text)
    head, docs = docs[0], docs[1:]
    by_id = {}
    for i, d in enumerate(docs):
        m = HEADER.match(d)
        by_id[m.group(2)] = (int(m.group(1)), i)
    layers = re.findall(r'- serializedVersion: \d+\n    m_Name: ([^\n]*)\n    m_StateMachine: \{fileID: (-?\d+)\}', docs[0])
    if not layers:
        return 'no layers'
    paths, table_transitions = table_strings(table, [n for n, _ in layers])
    machines_in_table = {p.rsplit('.', 1)[0] for p in paths}

    def state_name(fid):
        return field(docs[by_id[fid][1]], 'm_Name')

    def out_targets(fid):
        doc = docs[by_id[fid][1]]
        block = re.search(r'^  m_Transitions:\n((?:  - [^\n]*\n)*)', doc, re.M)
        names = []
        for t in re.findall(r'fileID: (-?\d+)', block.group(1) if block else ''):
            if t in by_id:
                dst = re.search(r'^  m_DstState: \{fileID: (-?\d+)\}', docs[by_id[t][1]], re.M)
                if dst and dst.group(1) in by_id:
                    names.append(state_name(dst.group(1)))
        return names

    used_ids = set(by_id)

    def new_id():
        while True:
            fid = '1107' + ''.join(random.choice('0123456789') for _ in range(15))
            if fid not in used_ids:
                used_ids.add(fid)
                return fid

    new_docs = []
    report = []
    for layer, root_id in layers:
        root_index = by_id[root_id][1]
        root = docs[root_index]
        child_block = re.search(r'^  m_ChildStates:\n((?:  - m_State: [^\n]*\n    m_Position: [^\n]*\n)*)', root, re.M)
        if child_block is None or re.search(r'^  m_ChildStateMachines: \[\]', root, re.M) is None:
            report.append('%s: not a flat layer, left alone' % layer)
            continue
        states = re.findall(r'  - m_State: \{fileID: (-?\d+)\}', child_block.group(1))
        by_short = {}
        for p in paths:
            if p.startswith(layer + '.') and p not in machines_in_table:
                by_short.setdefault(p.rsplit('.', 1)[1], []).append(p)
        placed = {}
        for fid in states:
            name = state_name(fid)
            cands = by_short.get(name, [])
            if len(cands) > 1:
                outs = out_targets(fid)
                cands = sorted(cands, key=lambda c: -sum(1 for a, b in table_transitions if a == c and b.rsplit('.', 1)[1] in outs))
            placed[fid] = cands[0] if cands else layer + '.' + name
        # machine tree
        machine_ids = {layer: root_id}
        children = {layer: []}
        for fid, path in placed.items():
            parts = path.split('.')
            for depth in range(2, len(parts)):
                mpath = '.'.join(parts[:depth])
                if mpath not in machine_ids:
                    machine_ids[mpath] = new_id()
                    children[mpath] = []
                    children['.'.join(parts[:depth - 1])].append(('m', mpath))
            children['.'.join(parts[:-1])].append(('s', fid))

        def child_lists(mpath):
            st, sm = [], []
            for i, (kind, ref) in enumerate(children[mpath]):
                pos = '{x: %d, y: %d, z: 0}' % ((i % 6) * 250, (i // 6) * 100)
                if kind == 's':
                    st.append('  - serializedVersion: 1\n    m_State: {fileID: %s}\n    m_Position: %s\n' % (ref, pos))
                else:
                    sm.append('  - serializedVersion: 1\n    m_StateMachine: {fileID: %s}\n    m_Position: %s\n' % (machine_ids[ref], pos))
            return ('  m_ChildStates:\n' + ''.join(st)) if st else '  m_ChildStates: []\n', \
                   ('  m_ChildStateMachines:\n' + ''.join(sm)) if sm else '  m_ChildStateMachines: []\n'

        st, sm = child_lists(layer)
        root = root[:child_block.start()] + st + root[child_block.end():]
        root = root.replace('  m_ChildStateMachines: []\n', sm, 1)
        docs[root_index] = root
        for mpath, mid in machine_ids.items():
            if mpath == layer:
                continue
            st, sm = child_lists(mpath)
            first_state = next((ref for kind, ref in children[mpath] if kind == 's'), '0')
            new_docs.append(
                '--- !u!1107 &%s\nAnimatorStateMachine:\n  serializedVersion: 5\n  m_ObjectHideFlags: 1\n'
                '  m_PrefabParentObject: {fileID: 0}\n  m_PrefabInternal: {fileID: 0}\n  m_Name: %s\n%s%s'
                '  m_AnyStateTransitions: []\n  m_EntryTransitions: []\n  m_StateMachineTransitions: {}\n'
                '  m_StateMachineBehaviours: []\n  m_AnyStatePosition: {x: 0, y: -250, z: 0}\n'
                '  m_EntryPosition: {x: 250, y: -250, z: 0}\n  m_ExitPosition: {x: 500, y: -250, z: 0}\n'
                '  m_ParentStateMachinePosition: {x: 800, y: 20, z: 0}\n  m_DefaultState: {fileID: %s}\n'
                % (mid, mpath.rsplit('.', 1)[1], st, sm, first_state))
        nested = sum(1 for p in placed.values() if p.count('.') > 1)
        report.append('%s: %d states, %d moved into %d sub-state machines' % (layer, len(states), nested, len(machine_ids) - 1))

    out = head + ''.join(docs)
    if not out.endswith('\n'):
        out += '\n'
    out += ''.join(new_docs)
    with open(controller, 'w', encoding='utf-8', newline='\n') as fh:
        fh.write(out)
    return '; '.join(report)


def restore_folder(folder):
    results = []
    for d, _, files in os.walk(folder):
        for f in files:
            if f.endswith('.controller'):
                table = os.path.join(d, f[:-len('.controller')] + '_statemachine.bytes')
                if os.path.exists(table):
                    results.append((os.path.join(d, f), restore(os.path.join(d, f), table)))
    return results


if __name__ == '__main__':
    for folder in sys.argv[1:]:
        for path, result in restore_folder(folder):
            print(os.path.basename(path), '->', result)
