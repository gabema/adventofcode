'''
https://adventofcode.com/2022/day/4
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
'''
import os
import stuff
import re

def part1(fileName) :
    lines = stuff.readStructuredContents(fileName, r'(\d+)\-(\d+),(\d+)\-(\d+)', re.MULTILINE)
    numContained = 0
    for l in lines :
        s1 = set(range(int(l[0]), int(l[1])+1))
        s2 = set(range(int(l[2]), int(l[3])+1))
        if len(s1.difference(s2)) == 0 or len(s2.difference(s1)) == 0:
            numContained += 1
    return numContained

def part2(fileName) :
    lines = stuff.readStructuredContents(fileName, r'(\d+)\-(\d+),(\d+)\-(\d+)', re.MULTILINE)
    numOverlapping = 0
    for l in lines :
        s1 = set(range(int(l[0]), int(l[1])+1))
        for e in range(int(l[2]), int(l[3])+1) :
            if e in s1 :
                numOverlapping += 1
                break
    return numOverlapping

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d04Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d04.txt'
    print(stuff.expected(part1, sampleFileName, 2))
    print(stuff.expected(part1, fileName, 500))
    print(stuff.expected(part2, sampleFileName, 4))
    print(stuff.expected(part2, fileName, 815))
