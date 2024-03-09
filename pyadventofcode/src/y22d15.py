'''
https://adventofcode.com/2022/day/15
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
https://github.com/subhajeet2107/pylexer/
'''
import os
import stuff
import re

def readInfo(fileName) :
    ex = re.compile(r'Sensor at x=([-\d]+), y=([-\d]+): closest beacon is at x=([-\d]+), y=([-\d]+)')
    for line in stuff.readStructuredContents(fileName, ex) :
        yield ((int(line[0]), int(line[1])), (int(line[2]), int(line[3])))

tcDistanceBetweenPoints = lambda a, b : abs(a[0] - b[0]) + abs(a[1] - b[1])

rowHasSensorCoverage = lambda row, sc : tcDistanceBetweenPoints((sc[0][0], row), sc[0]) > sc[1]

def pointsInTCRange(sc, row) :
    sensor, tcRange = sc
    idealX, _ = sensor
    testSpot = (idealX, row)
    tcDiff = tcRange - tcDistanceBetweenPoints(testSpot, sensor)
    if tcDiff >= 0 :
        yield (testSpot[0], row)
        for d in range(1, tcDiff+1) :
            yield (testSpot[0]-d, row)
            yield (testSpot[0]+d, row)

def part1(fileName) :
    beacons = []
    sc = []

    for e in readInfo(fileName) :
        sc.append((e[0], tcDistanceBetweenPoints(e[0],e[1])))
        beacons.append(e[1])

    pointsInRowIters = [pointsInTCRange(p, 2000000) for p in sc]
    pointsInRow = set()
    for pi in pointsInRowIters :
        for p in pi :
            pointsInRow.add(p)
        j = 0

    for p in beacons :
        pointsInRow.discard(p)
    numInRow = len(pointsInRow)

#######
#    for s in sc :
#        print("Sensor at %s tcRange is %d" % (s[0], s[1]))
#######
#    for a in sorted(pointsInRow) :
#        print("not in " + str(a))
#######
#    for sInRange in [a for a in sc if rowHasSensorCoverage(10, a)] :
#        print("Sensor at %s with tcRange of %d is in range!" % (sInRange[0], sInRange[1]))
#######
    return numInRow

def part2(fileName) :
    for line in stuff.readContentsByLine(fileName) :
        print(line)

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d15Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d15.txt'
    #print(stuff.expected(part1, sampleFileName, 26))
    # 5119085 is to low (because I didn't change the row)
    print(stuff.expected(part1, fileName, 5688618))
#    print(stuff.expected(part2, sampleFileName, 93))
#    print(stuff.expected(part2, fileName, 27194))
