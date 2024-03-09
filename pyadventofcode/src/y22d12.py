'''
https://adventofcode.com/2022/day/12
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
https://github.com/subhajeet2107/pylexer/
'''
import os
import stuff

def readMap(fileName) :
    startPosition = (0,0)
    endPosition = (0,0)
    y = 0
    map = []
    for line in stuff.readContentsByLine(fileName) :
        x = 0
        row = []
        for i in line.strip() :
            o = ord(i)
            if o == 83 :
                startPosition = (x, y)
                o = -1
            elif o == 69 :
                endPosition = (x, y)
                o = 26
            else :
                o = o - 97
            row.append(o)
            x += 1
        map.append(row)
        y += 1
    return (map, startPosition, endPosition)

def part1(fileName) :
    map = readMap(fileName)
    print(map)

def part2(fileName) :
    pass

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d12Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d12.txt'
    print(stuff.expected(part1, sampleFileName, 10605))
#    print(stuff.expected(part1, fileName, 4536))
#    print(stuff.expected(part2, sampleFileName, 36))
    #print(stuff.expected(part2, fileName, 2529))
