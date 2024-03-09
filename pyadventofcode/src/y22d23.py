'''
https://adventofcode.com/2022/day/23
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
https://github.com/subhajeet2107/pylexer/
'''
import os
import stuff
import re

def possibleNextPosition(elf) :
    x, y = elf
    yield (x-1, y-1)
    yield (x, y-1)
    yield (x+1, y-1)
    yield (x-1, y)
    yield (x+1, y)
    yield (x-1, y+1)
    yield (x, y+1)
    yield (x+1, y+1)

def readElfPositions(fileName) :
    y = 0
    elfId = 0
    for line in stuff.readContentsByLine(fileName) :
        for x in range(len(line)) :
            if line[x] == '#' :
                yield (x,y)
                elfId += 1
        y += 1

def part1(fileName) :
    elves = {e for e in readElfPositions(fileName)}
    print(elves)
    return len(elves)

def part2(fileName) :
    pass

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d23Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d23.txt'
    print(stuff.expected(part1, sampleFileName, 64))
#    print(stuff.expected(part1, fileName, 5688618))
#    print(stuff.expected(part2, sampleFileName, 58))
#    print(stuff.expected(part2, fileName, 27194))
