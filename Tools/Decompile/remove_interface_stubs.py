"""Delete compiler generated interface forwarding stubs from decompiled C#.

ILSpy emits them as `virtual T IFoo.get_Bar() { return base.Bar; }`, which is not
valid C#. The C# compiler regenerates them on its own, so they can simply go.
Only stubs whose body is a single forwarding statement are removed; anything
else is reported and left alone.

usage: python -I Tools/Decompile/remove_interface_stubs.py <file.cs>...
"""
import re
import sys

STUB = re.compile(r'^([ \t]*)virtual [^\n(]*?\b(I\w+(?:<[^>]*>)?)\.(\w+)\(([^)]*)\)[ \t]*\n\1\{\n(.*?)\n\1\}\n(?:[ \t]*\n)?',
                  re.M | re.S)
FORWARD = re.compile(r'^\s*(return )?(base\.)?[\w.]+(\([^;]*\))?( = value)?;\s*$')


def process(path):
    src = open(path, encoding='utf8').read()
    removed, kept = [], []

    def repl(m):
        body = m.group(5)
        if '\n' not in body.strip() and FORWARD.match(body):
            removed.append(f'{m.group(2)}.{m.group(3)}')
            return ''
        kept.append(f'{m.group(2)}.{m.group(3)}: {body.strip()[:80]}')
        return m.group(0)

    out = STUB.sub(repl, src)
    if out != src:
        open(path, 'w', encoding='utf8', newline='').write(out)
    print(f'{path}: removed {len(removed)}')
    for k in kept:
        print(f'  KEPT (not a plain forward) {k}')


for p in sys.argv[1:]:
    process(p)
