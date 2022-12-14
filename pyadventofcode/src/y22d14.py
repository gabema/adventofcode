'''
https://adventofcode.com/2022/day/14
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
https://github.com/subhajeet2107/pylexer/
'''
import os
import stuff

def readLineCoords(fileName) :
    for line in stuff.readContentsByLine(fileName) :
        yield [(int(coord[0]), int(coord[1])) for coord in [coord.split(',') for coord in line.strip('\n').split(' -> ')]]

def createMapWithFloor(fileName, leftGap, rightGap) :
    minX, maxY, map = createMap(fileName)
    lenX = len(map[0]) + leftGap + rightGap
    for index in range(len(map)) :
        map[index] = ['.'] * leftGap + map[index] + ['.'] * rightGap
    map.append(['.'] * lenX)
    map.append(['.'] * lenX)
    maxY += 2
    maxX = minX + lenX - leftGap
    minX -= leftGap
    newMap = (minX, maxY, map)
    addLineToMap((minX, maxY-1), (maxX-1, maxY-1), newMap)
    return newMap

def createMap(fileName) :
    lineCoords = [lc for lc in readLineCoords(fileName)]
    minX = 500
    maxX = 500
    maxY = 0
    for lc in lineCoords :
        xs = [p[0] for p in lc]
        ys = [p[1] for p in lc]
        minX = min(minX, min(xs)-1)
        maxX = max(maxX, max(xs)+1)
        maxY = max(maxY, max(ys)+1)

    map = []
    lenX = maxX - minX
    for _ in range(maxY) :
        map.append(['.'] * lenX)

    map = (minX, maxY, map)

    for lc in lineCoords :
        for i in range(len(lc)-1) :
            addLineToMap(lc[i], lc[i+1], map)

    return map

def addLineToMap(pointA, pointB, m) :
    minX, _, map = m
    if pointA[0] == pointB[0] :
        # Vertical Line
        x = pointA[0] - minX
        for y in range(min(pointA[1], pointB[1]), max(pointA[1], pointB[1])+1) :
            map[y][x] = '#'
    else :
        # Horizontal Line
        y = pointA[1]
        for x in range(min(pointA[0], pointB[0]), max(pointA[0], pointB[0])+1) :
            map[y][x-minX] = '#'


def addSand(x, y, m) :
    minX, maxY, map = m
    x = x - minX
    if map[y][x] != '.' :
        return False

    map[y][x] = 'o'
    while y < maxY:
        if y + 1 == maxY:
            return False

        if map[y+1][x] == '.' :
            map[y][x], map[y+1][x] = map[y+1][x], map[y][x]
        elif map[y+1][x-1] == '.':
            map[y][x], map[y+1][x-1] = map[y+1][x-1], map[y][x]
            x -= 1
        elif map[y+1][x+1] == '.':
            map[y][x], map[y+1][x+1] = map[y+1][x+1], map[y][x]
            x += 1
        else :
            break
        y += 1

    return not y == maxY

def part1(fileName) :
    map = createMap(fileName)
    numSands = 0
    while addSand(500, 0, map) :
        numSands += 1
    return numSands    

def part2(fileName) :
    map = createMapWithFloor(fileName, 100, 200)
    numSands = 0
    while addSand(500, 0, map) :
        numSands += 1
    #print(map[2])
    return numSands

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d14Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d14.txt'
    print(stuff.expected(part1, sampleFileName, 24))
    print(stuff.expected(part1, fileName, 610))
    print(stuff.expected(part2, sampleFileName, 93))
    print(stuff.expected(part2, fileName, 27194))
