'''
https://adventofcode.com/2022/day/20
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
https://github.com/subhajeet2107/pylexer/
'''
import os
import stuff

def readInts(fileName) :
    for line in stuff.readContentsByLine(fileName) :
        yield int(line.removesuffix('\n'))

def part1(fileName) :
    

def part2(fileName) :
    pass

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d20Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d20.txt'
    print(stuff.expected(part1, sampleFileName, 64))
#    print(stuff.expected(part1, fileName, 5688618))
#    print(stuff.expected(part2, sampleFileName, 58))
#    print(stuff.expected(part2, fileName, 27194))
