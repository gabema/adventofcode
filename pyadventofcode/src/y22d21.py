'''
https://adventofcode.com/2022/day/20
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
https://github.com/subhajeet2107/pylexer/
'''
import os
import stuff
import re

def addValueAndBackfill(map, future, key, value) :
    map[key] = value
    if key in future :
        for otherKeys in future[key] :
            

def loadMap(fileName) :
    map = dict()
    futureLoad = dict()
    ex = re.compile(r'(\w{4}): (\d+)?((\w{4}) ([\*\-\+\/]) (\w{4}))?')
    for line in stuff.readStructuredContents(fileName, ex) :
        print(line)
        if line[1] != '' :
            map[line[0]] = int(line[1])
        else :
            if line[3] not in map :
                futureLoad[line[3]] += [ line[1] ]
            if line[5] not in map :
                futureLoad[line[5]] += [ line[1] ]

            map[line[0]] = (line[])
        yield (line[0], int(line[1]) if line[1] != '' else f'eval({line[2]})')

def part1(fileName) :
    map = dict(loadMap(fileName))
    return eval("eval(root)", map)

def part2(fileName) :
    pass

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d21Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d21.txt'
    print(stuff.expected(part1, sampleFileName, 64))
#    print(stuff.expected(part1, fileName, 5688618))
#    print(stuff.expected(part2, sampleFileName, 58))
#    print(stuff.expected(part2, fileName, 27194))
