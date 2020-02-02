import sys
import random

bpmsong = sys.argv[1]
song = sys.argv[2]
bpm = float(sys.argv[3])

bpmf = open(bpmsong, 'r')
sng = open(song, 'w')
last_time = -1
for line in bpmf.readlines():
    line = line.split()
    time = float(line[0])
    time /= bpm / 60
    sng.write(f"{time} {line[1]} {line[2]} {line[3]}\n")

sng.close()
bpmf.close()