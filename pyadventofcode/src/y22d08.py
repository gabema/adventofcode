'''
https://adventofcode.com/2022/day/8
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
https://github.com/subhajeet2107/pylexer/
'''
import os
import stuff

def readTrees2(fileName) :
    lines = stuff.readContentsByLine(fileName)
    treeValues = []
    for line in lines :
        treeValues.append([int(t) for t in line[:-1]])
    rows = len(treeValues)
    columns = len(treeValues[0])
    maxHeights = {
        'ViewFromTop': [-1] * columns,
        'ViewFromBottom': [-1] * columns,
        'ViewFromLeft': [-1] * rows,
        'ViewFromRight': [-1] * rows,
    }
    visibility = [[False for _ in range(columns)] for _ in range(rows)]
    viewableFrom(treeValues, maxHeights,visibility,      0, rows,  1,         0, columns,  1, 'ViewFromLeft')
    viewableFrom(treeValues, maxHeights,visibility, rows-1,   -1, -1, columns-1,      -1, -1, 'ViewFromBottom')
    viewableFrom(treeValues, maxHeights,visibility,      0, rows,  1,         0, columns,  1, 'ViewFromTop')
    viewableFrom(treeValues, maxHeights,visibility, rows-1,   -1, -1, columns-1,      -1, -1, 'ViewFromRight')
    return treeValues

def viewableFrom(trees, maxHeights, visibility, startRow, endRow, rowInc, startColumn, endCol, colInc, maxName) :
    if maxName == 'ViewFromTop' or maxName == 'ViewFromBottom' :
        for y in range(startRow, endRow, rowInc) :
            for x in range(startColumn, endCol, colInc) :
                if trees[y][x] > maxHeights[maxName][y] :
                    visibility[y][x] = True
                    maxHeights[maxName][y] = trees[y][x]
    else :
        for x in range(startColumn, endCol, colInc) :
            for y in range(startRow, endRow, rowInc) :
                if trees[y][x] > maxHeights[maxName][x] :
                    visibility[y][x] = True
                    maxHeights[maxName][x] = trees[y][x]


def readTrees(fileName) :
    lines = stuff.readContentsByLine(fileName)
    trees = []
    for line in lines :
        trees.append([invisibleTree(int(t)) for t in line[:-1]])

    width = len(trees[0])
    height = len(trees)

    for x in range(0, width) :
        trees[0][x] = setVisibleTop(trees[0][x])
        trees[-1][x] = setVisibleBottom(trees[-1][x])

    for y in range(0, height) :
        trees[y][0] = setVisibleLeft(trees[y][0])
        trees[y][-1] = setVisibleRight(trees[y][-1])

    for x in range(1, width) :
        for y in range(1, height) :
            if isTreeVisible(trees[y][x-1]) and trees[y][x-1][4] < trees[y][x][4]:
                trees[y][x] = setVisibleLeft(trees[y][x])

    for y in range(1, height) :
        for x in range(1, width) :
            if isTreeVisible(trees[y-1][x]) and trees[y-1][x][4] < trees[y][x][4]:
                trees[y][x] = setVisibleTop(trees[y][x])

    for x in range(width-2, 0, -1) :
        for y in range(width-2, 0, -1) :
            if isTreeVisible(trees[y][x+1]) and trees[y][x+1][4] < trees[y][x][4]:
                trees[y][x] = setVisibleRight(trees[y][x])

    for y in range(width-2, 0, -1) :
        for x in range(width-2, 0, -1) :
            if isTreeVisible(trees[y+1][x]) and trees[y+1][x][4] < trees[y][x][4]:
                trees[y][x] = setVisibleBottom(trees[y][x])

    return trees

isTreeVisible = lambda t : t[0] or t[1] or t[2] or t[3]
invisibleTree = lambda v : (False, False, False, False, v)
setVisibleLeft = lambda t : (t[0], True, t[2], t[3], t[4])
setVisibleRight = lambda t : (t[0], t[1], t[2], True, t[4])
setVisibleTop = lambda t : (True, t[1], t[2], t[3], t[4])
setVisibleBottom = lambda t : (t[0], t[1], True, t[3], t[4])

def part1(fileName) :
    trees = readTrees2(fileName)
    width = len(trees[0])
    height = len(trees)
    visibleTrees = 0
    for y in range(0, height) :
        for x in range(0, width) :
            if (isTreeVisible(trees[y][x])) :
                print("Tree %i, %i is visible %s" % (x, y, trees[y][x]))
                visibleTrees += 1

    return visibleTrees

def part2(fileName) :
    pass

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d08Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d08.txt'
    print(stuff.expected(part1, sampleFileName, 21))
    #print(stuff.expected(part1, fileName, 1443806))
    #print(stuff.expected(part2, sampleFileName, 24933642))
    #print(stuff.expected(part2, fileName, 942298))
