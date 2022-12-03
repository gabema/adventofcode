'''
https://adventofcode.com/2022/day/3
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
'''

import os
import stuff

def compartments(fileName) :
    lines = stuff.readContentsByLine(fileName)
    for l in lines :
        l = l.strip("\n")
        mid = int(len(l)/2)
        left = l[:mid]
        right = l[mid:]
        leftItems = {i for i in left}
        rightItems = {i for i in right}
        sharedItems = leftItems & rightItems
        yield sharedItems

def groups(fileName) :
    lines = iter(stuff.readContentsByLine(fileName))
    l = next(lines, '').strip("\n")
    while l != '' :
        items = {i for i in l}
        l = next(lines).strip("\n")
        items = items & {i for i in l}
        l = next(lines).strip("\n")
        items = items & {i for i in l}
        l = next(lines, '').strip("\n")
        yield items

def ordValue(v) :
    ov = ord(v)
    return ov - 96 if ov > 96 else ov - 38

def part1(fileName) :
    vals = [ordValue(v.pop()) for v in compartments(fileName)]
    return sum(vals)

def part2(fileName) :
    v = [ordValue(i.pop()) for i in groups(fileName)]
    return sum(v)

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d03Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d03.txt'
    print(stuff.expected(part1, sampleFileName, 157))
    print(stuff.expected(part1, fileName, 8109))
    print(stuff.expected(part2, sampleFileName, 70))
    print(stuff.expected(part2, fileName, 2738))
