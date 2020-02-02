import sys
import random

audacity = sys.argv[1]
song = sys.argv[2]

thresh = .05

aud = open(audacity, 'r')
sng = open(song, 'w')
last_time = -1
for line in aud.readlines():
    time = line.split()[0]
    # if last_time - float(time) < thresh:
        # continue
    sng.write(f"{time} {(random.random() * 4 - 2)} {random.choice(['Ground'])} {random.choice(['Player2'])}\n")
    last_time = float(time)

sng.close()
aud.close()