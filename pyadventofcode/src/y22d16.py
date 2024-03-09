'''
https://adventofcode.com/2022/day/16
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
https://github.com/subhajeet2107/pylexer/
'''
import os
import stuff
import re

def readValves(fileName) :
    ex = re.compile(r'Valve (\w\w) has flow rate=(\d+); tunnels? leads? to valves? ([\w, ]+)')
    for o in stuff.readStructuredContents(fileName, ex) :
        yield (o[0], int(o[1]), o[2].split(', ')) 


def part1(fileName) :
    for v in readValves(fileName) :
        print(v)

def part2(fileName) :
    for line in stuff.readContentsByLine(fileName) :
        print(line)

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d16Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d16.txt'
    print(stuff.expected(part1, sampleFileName, 26))
#    print(stuff.expected(part1, fileName, 5688618))
#    print(stuff.expected(part2, sampleFileName, 93))
#    print(stuff.expected(part2, fileName, 27194))
