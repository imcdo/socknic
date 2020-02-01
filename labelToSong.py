import sys

audacity = sys.argv[1]
song = sys.argv[2]

aud = open(audacity, 'r')
sng = open(song, 'w')

for line in aud.readlines():
    time = line.split()[0]
    sng.write(f"{time} 1 false\n")

sng.close()
aud.close()