"""Decode the game's localisation tables (runtimeassets/localization/<language>_bin.bytes in the
UnityAcier_Common_Loca_* bundles) into ID <TAB> text.

Format: uint32 count, then per entry uint32 ID, uint32 0, 7-bit length-prefixed UTF-8 text.

The tables hold the story layer of the server-side missions: campaign and mission titles, mission summaries,
intro and outro stories, and per step the hint text and the objective ("MEET Leonardo", "ESCORT Demetrio"),
grouped by ID. With the NPC definitions in the game database that is enough to re-author the missions.

    python decode_loca.py <xx_bin.bytes> <out.tsv>
"""
import struct
import sys


def decode(path):
    d = open(path, 'rb').read()
    count = struct.unpack_from('<I', d, 0)[0]
    pos = 4
    entries = []
    for _ in range(count):
        text_id = struct.unpack_from('<I', d, pos)[0]
        pos += 8
        length = shift = 0
        while True:
            b = d[pos]
            pos += 1
            length |= (b & 0x7f) << shift
            shift += 7
            if b < 0x80:
                break
        entries.append((text_id, d[pos:pos + length].decode('utf-8', 'replace')))
        pos += length
    if pos != len(d):
        raise ValueError('%s: %d bytes left after %d entries' % (path, len(d) - pos, count))
    return entries


def main():
    entries = decode(sys.argv[1])
    with open(sys.argv[2], 'w', encoding='utf-8', newline='\n') as fh:
        for text_id, text in sorted(entries):
            fh.write('%d\t%s\n' % (text_id, text.replace('\n', '\\n').replace('\t', ' ')))
    print('%d entries -> %s' % (len(entries), sys.argv[2]))


if __name__ == '__main__':
    main()
