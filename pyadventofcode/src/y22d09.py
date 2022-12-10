'''
https://adventofcode.com/2022/day/9
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
https://github.com/subhajeet2107/pylexer/
'''
import os
import stuff
from math import fabs

def moveHead(headPosition, direction) :
    x, y = headPosition
    if direction == 'L' :
        newPosition = (x-1, y)
    elif direction == 'U' :
        newPosition = (x, y+1)
    elif direction == 'D' :
        newPosition = (x, y-1)
    else : # 'R'
        newPosition = (x+1, y)
    return newPosition

def moveTail(tailPosition, headPosition) :
    '''
    .T     .    h1=0,1 t1=1,2
    H.  -> T    h2=0,0 t2=0,1   distance between h2 and t1 xd=1, yd=2 sqrt(5)
    ..     H    
    '''
    headX, headY = headPosition
    tailX, tailY = tailPosition
    diffX = headX - tailX
    diffY = headY - tailY
    absDiffX = fabs(diffX)
    absDiffY = fabs(diffY)
    if absDiffX + absDiffY >= 3.0 :
        if absDiffX > absDiffY :
            return (headX-1 if diffX > 0 else headX+1, headY)
        elif absDiffX < absDiffY:
            return (headX, headY-1 if diffY > 0 else headY+1)
        else :
            return (headX-1 if diffX > 0 else headX+1, headY-1 if diffY > 0 else headY+1)
    elif absDiffX > 1 :
        return (headX - 1 if diffX > 0 else headX + 1, headY)
    elif absDiffY > 1 :
        return (headX, headY - 1 if diffY > 0 else headY + 1)
    else :
        return tailPosition


def genDirections(fileName) :
    for line in stuff.readContentsByLine(fileName) :
        yield (line[0], int(line[2:]))


def part1(fileName) :
    headPosition = (0,0)
    tailPosition = (0,0)
    tailPositionsVisited = set()
    tailPositionsVisited.add(tailPosition)
    for dir, num in genDirections(fileName) :
        for _ in range(0, num) :
            headPosition = moveHead(headPosition, dir)
            tailPosition = moveTail(tailPosition, headPosition)
            tailPositionsVisited.add(tailPosition)
            #print("head=%s, tail=%s" % (headPosition, tailPosition))
    numTailVisited = len(tailPositionsVisited)
    return numTailVisited

def recursiveIndexIterator(size) :
    while True:
        for i in range(size) :
            yield i

def part2(fileName) :
    positions = []
    numKnots = 9
    it = recursiveIndexIterator(numKnots + 1)
    for _ in range(numKnots + 1) :
        positions.append((0,0))
    tailPositionsVisited = set()
    tailPositionsVisited.add((0,0))
    for dir, num in genDirections(fileName) :
        for _ in range(0, num) :
            positions[0] = moveHead(positions[0], dir)
            for i in range(1, numKnots + 1) :
                newTail = moveTail(positions[i], positions[i-1])
                if (newTail == positions[i]) :
                    break
                else :
                    positions[i] = newTail

                if (i == numKnots) :
                    tailPositionsVisited.add(newTail)
                    #print("tail=%i, %i" % (newTail[0], newTail[1]))
            #print(positions)
        #print("Moved %s %i" % (dir, num))

    print(sorted([p for p in tailPositionsVisited]))

    numTailVisited = len(tailPositionsVisited)
    return numTailVisited


if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d09Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d09.txt'
    #print(stuff.expected(part1, sampleFileName, 13))
    #print(stuff.expected(part1, fileName, 6023))
    print(stuff.expected(part2, sampleFileName, 36))
    # 2529 is too low
    # 2324 is too low
    # 2333 not the right answer
    print(stuff.expected(part2, fileName, 2533))
