'''
https://adventofcode.com/2022/day/24
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
https://github.com/subhajeet2107/pylexer/
'''
import os
import stuff

class Blizzard :
    

def readMap(fileName) :
    map = []
    for line in stuff.readContentsByLine(fileName) :
        map.append(list(line.removesuffix("\n")))
    mapWidth = len(map[0]) - 1
    mapHeight = len(map) -1


def part1(fileName) :
    map = readMap(fileName)
    print(map)


def part2(fileName) :
    pass

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d2tSample.txt'
    fileName = os.path.dirname(__file__) + '/y22d24.txt'
    print(stuff.expected(part1, sampleFileName, 64))
#    print(stuff.expected(part1, fileName, 5688618))
#    print(stuff.expected(part2, sampleFileName, 58))
#    print(stuff.expected(part2, fileName, 27194))
