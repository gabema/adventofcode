'''
https://adventofcode.com/2022/day/22
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
https://github.com/subhajeet2107/pylexer/
'''
import os
import stuff
import re
from collections import deque

def readMapAndDirections(fileName) :
    map = []
    directions = ''
    readDirections = False
    for line in stuff.readContentsByLine(fileName) :
        line = line.removesuffix('\n')
        if readDirections :
            directions = [m[0] for m in re.findall(r'((\d+)|([LR]))', line)]
            break
        if line == '' :
            readDirections = True
            continue
        lx = 0
        rx = 0
        for i in range(len(line)) :
            if line[i] != ' ' :
                lx = i
                break
        for i in range(len(line)-1, -1, -1) :
            if  line[i] != ' ' :
                rx = i
                break
        lenX = rx-lx+1
        row = list(line[lx:rx+1])
        map.append((row, lx, lenX))

    return (map, directions)

def rowIndexRightIter(xOffset, len, sx) :
    for i in range(sx-xOffset+1, xOffset + len - sx) :
        yield i + xOffset
    while True :
        for i in range(0, len) :
            yield i + xOffset

def rowIndexLeftIter(xOffset, len, sx) :
    for i in range(sx-xOffset + len - sx, -1, -1) :
        yield i
    while True :
        for i in range(len-1, xOffset-1, -1) :
            yield i

def columnIndexDownIter(map, sy, x) :
    topY = sy
    bottomY = sy
    content, sx, xLen = map[sy-1]
    yield sy

def move(map, step, position) :
    x,y,facing = position
    if facing == '>' :
        content, lx, lenX = map[y]
        it = iter(rowIndexRightIter(lx, lenX, x))
        while step > 0 :
            nextX = next(it)
            if content[nextX-lx] == '#' :
                break
            x = nextX
            step -= 1
    elif facing == '^' :
        pass
    elif facing == '<' :
        lx, lenX, content = map[y]
        it = iter(rowIndexLeftIter(lx, lenX, x))
        while step > 0 :
            nextX = next(it)
            if content[nextX-lx] == '#' :
                break
            x = nextX
            step -= 1
    else : # v
        it = iter(columnIndexDownIter(map, y, x))
        while step > 0 :
            nextY = next(it)
            content, _, _ = map[nextY]
            if content[x] == '#' :
                break
            y = nextY
            step -= 1

x    return (x, y, facing)

facings = ">v<^"

def isInt(v) :
    try :
        int(v)
        return True
    except ValueError:
        return False

def processDirections(map, directions, position) :
    global facings
    nFacings = len(facings)
    for step in directions :
        if isInt(step) :
            position = move(map, int(step), position)
        else :
            x,y,facing = position
            n = -1 if step == 'L' else 1
            i = facings.find(facing)
            i = (i + n + nFacings) % nFacings
            facing = facings[i]
            position = (x, y, facing)

    print(map)
    print(directions)
    print(position)

def part1(fileName) :
    map, directions = readMapAndDirections(fileName)
    position = (map[0][1], 0, '>')
    position = processDirections(map, directions, position)

def part2(fileName) :
    pass

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d22Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d22.txt'
    print(stuff.expected(part1, sampleFileName, 64))
#    print(stuff.expected(part1, fileName, 5688618))
#    print(stuff.expected(part2, sampleFileName, 58))
#    print(stuff.expected(part2, fileName, 27194))
