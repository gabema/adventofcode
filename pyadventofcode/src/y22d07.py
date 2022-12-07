'''
https://adventofcode.com/2022/day/6
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
https://github.com/subhajeet2107/pylexer/
'''
import os
import stuff

def parsedLine(fileName) :
    lines = stuff.readContentsByLine(fileName)
    for line in lines :
        if line.startswith('$ ') :
            yield ('Command', line[2:-1].split(' '))
        else :
            yield ('Output', line[:-1].split(' '))


def combinedDirSizes(fileName) :
    it = parsedLine(fileName)
    cwd = []
    dirSizes = dict()
    line = next(it, None)
    while line != None :
        t, parts = line
        if t == 'Command' and parts[0] == 'cd' :
            if parts[1] == '..' :
                cwd.pop()
            else :
                cwd.append(parts[1])
            line = next(it, None)
        elif t == 'Command' and parts[0] == 'ls' :
            line = next(it, None)
            dirSize = 0
            while line != None and line[0] == 'Output' :
                _, parts = line
                if parts[0] != 'dir' :
                    dirSize += int(parts[0])
                line = next(it, None)
            dirSizes['/'.join(cwd)] = dirSize

    # combine subdir sizes
    combinedSizes = dict()
    totalUsed = 0
    for k in dirSizes :
        combinedSizes[k] = dirSizes[k]
        totalUsed += dirSizes[k]
        for kk in dirSizes :
            if k!=kk and kk.startswith(k) :
                combinedSizes[k] += dirSizes[kk]
    return combinedSizes, totalUsed

def part1(fileName) :
    combinedSizes, _ = combinedDirSizes(fileName)
    totalLessThan10k = 0
    for k in combinedSizes :
        size = combinedSizes[k]
        if size <= 100000 :
            totalLessThan10k += size
    return totalLessThan10k

def part2(fileName) :
    combinedSizes, totalSpaceUsed = combinedDirSizes(fileName)
    spaceAvailable = 70000000 - totalSpaceUsed
    spaceNeeded = 30000000 - spaceAvailable
    dirSizes = sorted([combinedSizes[k] for k in combinedSizes])
    spaceDeleted = 0
    for i in range(len(dirSizes)) :
        if dirSizes[i] > spaceNeeded :
            spaceDeleted = dirSizes[i]
            break

    return spaceDeleted

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d07Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d07.txt'
    print(stuff.expected(part1, sampleFileName, 95437))
    print(stuff.expected(part1, fileName, 1443806))
    print(stuff.expected(part2, sampleFileName, 24933642))
    print(stuff.expected(part2, fileName, 942298))
