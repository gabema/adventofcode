'''
https://adventofcode.com/2020/day/20
'''

import stuff
import os
import re

def part1(fileName) :
    for tileNum, contents in stuff.readStructuredContents(fileName, r'Tile (\d\d\d\d):\n([#\.\n]+)^\n', re.MULTILINE) :
        contents = contents.split('\n')[0:-1]
        print(tileNum)
        print(contents)

if __name__ == '__main__' :
    fileName = os.path.dirname(__file__) + '/y20d20.txt'
    print(f"Part1: {part1(fileName)}")