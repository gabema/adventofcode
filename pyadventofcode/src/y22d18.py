'''
https://adventofcode.com/2022/day/18
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
https://github.com/subhajeet2107/pylexer/
'''
import os
import stuff

def get3DPositions(fileName) :
    for line in stuff.readContentsByLine(fileName) :
        parts = line.removesuffix("\n").split(',')
        yield int(parts[0]), int(parts[1]), int(parts[2])

def loadGrid(fileName) :
    grid = dict()
    for d in get3DPositions(fileName) :
        grid[d] = 6
    return grid

def neighborPoints(p) :
    x, y, z = p
    yield (x, y, z+1)
    yield (x, y+1, z)
    yield (x+1, y, z)

def allNeighborPoints(p) :
    x, y, z = p
    yield (x, y, z+1)
    yield (x, y+1, z)
    yield (x+1, y, z)
    yield (x, y, z-1)
    yield (x, y-1, z)
    yield (x-1, y, z)

def part1(fileName) :
    grid = loadGrid(fileName)
    for p in grid.keys() :
        for np in neighborPoints(p) :
            if np in grid :
                grid[p], grid[np] =  grid[p]-1, grid[np] - 1
    return sum(grid.values())

def part2(fileName) :
    grid = loadGrid(fileName)
    xs = [x for x,_,_ in grid.keys()]
    ys = [y for _,y,_ in grid.keys()]
    zs = [z for _,_,z in grid.keys()]
    minX = min(xs)
    maxX = max(xs)
    minY = min(ys)
    maxY = max(ys)
    minZ = min(zs)
    maxZ = max(zs)
    sideCounts = 0
    for x in range(minX, maxX+1) :
        for y in range(minY, maxY + 1) :
            for z in range(minZ, maxZ+1) :
                if (x,y,z) in grid :
                    print("Found side 1 min %x, %x, %x" % (x,y,z))
                    sideCounts += 1
                    break
            for z in range(maxZ, minZ-1, -1) :
                if (x,y,z) in grid :
                    print("Found side 1 max %x, %x, %x" % (x,y,z))
                    sideCounts += 1
                    break

    for x in range(minX, maxX+1) :
        for y in range(maxY, minY - 1, -1) :
            for z in range(minZ, maxZ+1) :
                if (x,y,z) in grid :
                    print("Found side 2 min %x, %x, %x" % (x,y,z))
                    sideCounts += 1
                    break
            for z in range(maxZ, minZ-1, -1) :
                if (x,y,z) in grid :
                    print("Found side 2 max %x, %x, %x" % (x,y,z))
                    sideCounts += 1
                    break
    for x in range(maxX, minX-1, -1) :
        for y in range(minY, maxY + 1) :
            for z in range(minZ, maxZ+1) :
                if (x,y,z) in grid :
                    print("Found side 3 min %x, %x, %x" % (x,y,z))
                    sideCounts += 1
                    break
            for z in range(maxZ, minZ-1, -1) :
                if (x,y,z) in grid :
                    print("Found side 3 max %x, %x, %x" % (x,y,z))
                    sideCounts += 1
                    break

    return sideCounts

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d18Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d18.txt'
#    print(stuff.expected(part1, sampleFileName, 64))
#    print(stuff.expected(part1, fileName, 5688618))
    print(stuff.expected(part2, sampleFileName, 58))
#    print(stuff.expected(part2, fileName, 27194))
