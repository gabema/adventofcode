'''
https://adventofcode.com/2020/day/20
'''

import stuff
import os
import re

def readTile(fileName) :
    for tileNum, contents in stuff.readStructuredContents(fileName, r'Tile (\d\d\d\d):\n([#\.\n]+)^\n', re.MULTILINE) :
        contents = contents.split('\n')[0:-1]
        top = contents[0]
        bottom = contents[-1]
        left = ''.join([c[0] for c in contents])
        right = ''.join([c[-1] for c in contents])
        yield(int(tileNum), top, left, bottom, right)

def tileIterator(tile) :
    t1 = tile
    t2 = flipTile(tile)
    yield t1
    yield t2
    t1 = rotateTileRight(t1)
    t2 = rotateTileRight(t2)
    yield t1
    yield t2
    t1 = rotateTileRight(t1)
    t2 = rotateTileRight(t2)
    yield t1
    yield t2
    t1 = rotateTileRight(t1)
    t2 = rotateTileRight(t2)
    yield t1
    yield t2


uniqueTopTiles = lambda tiles : uniqueSideTiles(tiles, 1, 3)
uniqueBottomTiles = lambda tiles : uniqueSideTiles(tiles, 3, 1)
uniqueLeftTiles = lambda tiles : uniqueSideTiles(tiles, 2, 4)
uniqueRightTiles = lambda tiles : uniqueSideTiles(tiles, 4, 2)

def uniqueSideTiles(tiles, a, b) :
    tops = dict([(t[a], t) for t in tiles ])

    for t in tiles :
        if t[b] in tops :
            tops.pop(t[b])
    return list(tops.values())

def flipTile(tiles, tileNum) :
    index = 0
    for i in range(len(tiles)) :
        if tiles[i][0] == tileNum :
            index = i
            break

    tiles[index] = flipTile(tiles[index])

def flipTile(tile) :    
    tileNum, top, left, bottom, right = tile
    newLeft = left[::-1]
    newRight = right[::-1]
    return (tileNum, bottom, newLeft, top, newRight)

def rotateTileRight(tiles, tileNum) :
    index = 0
    for i in range(len(tiles)) :
        if tiles[i][0] == tileNum :
            index = i
            break

    tiles[index] = rotateTileRight(tiles[index])

def rotateTileRight(tile) :
    tileNum, top, left, bottom, right = tile
    newRight = top
    newBottom = right[::-1]
    newLeft = bottom
    newTop = left[::-1]
    return (tileNum, newTop, newLeft, newBottom, newRight)

def part1(fileName) :
    possibleTiles = []
    for tile in readTile(fileName) :
        for pt in tileIterator(tile) :
            possibleTiles.append(pt)

    print(possibleTiles)
    print(len(possibleTiles))

def part1Brute(fileName) :
    '''
    2971 TOP/LEFT
    3079 BOTTOM/RIGHT
    1951 BOTTOM/LEFT
    1489 TOP/RIGHT
    '''
    tiles = [t for t in readTile(fileName)]
    flipTile(tiles,        1171)
    rotateTileRight(tiles, 1171)
    rotateTileRight(tiles, 1171)
    flipTile(tiles,        1427)
    flipTile(tiles,        1489)
    flipTile(tiles,        1951)
    flipTile(tiles,        2311)
    flipTile(tiles,        2473)
    rotateTileRight(tiles, 2473)
    rotateTileRight(tiles, 2473)
    rotateTileRight(tiles, 2473)
    flipTile(tiles,        2729)
    flipTile(tiles,        2971)
    ut = uniqueTopTiles(tiles)
    ub = uniqueBottomTiles(tiles)
    ul = uniqueLeftTiles(tiles)
    ur = uniqueRightTiles(tiles)
    print(sorted([t[0] for t in tiles]))
    print("Top    Top           Left          Bottom        Right")
    for t in sorted(ut) : print(t)
    print("Bottom Top           Left          Bottom        Right")
    for t in sorted(ub) : print(t)
    print("Left   Top           Left          Bottom        Right")
    for t in sorted(ul) : print(t)
    print("Right  Top           Left          Bottom        Right")
    for t in sorted(ur) : print(t)

if __name__ == '__main__' :
    fileName = os.path.dirname(__file__) + '/y20d20.txt'
    stuff.expected(part1, fileName, 0)
