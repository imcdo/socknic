import sys
import random

bpmsong = sys.argv[1]
song = sys.argv[2]
bpm = sys.argv[3]

bpmf = open(bpmsong, 'r')
sng = open(song, 'w')
last_time = -1
for line in bpmf.readlines():
    line = line.split()
    time = float(line[0])
    time /= 60 * bpm
    sng.write(f"{time} {line[1]} {line[2]} {line[3]}\n")

sng.close()
aud.close()