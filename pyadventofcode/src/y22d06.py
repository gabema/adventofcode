'''
https://adventofcode.com/2022/day/6
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
https://github.com/subhajeet2107/pylexer/
'''
import os
import stuff

def numCharacters(str, numChars) :
    for i in range(len(str)-numChars) :
        yield (i, str[i:i+numChars])

def unqiueCharacters(str, numUnique) :
    chars = set()
    for i, substr in numCharacters(str, numUnique) :
        chars.clear()
        for c in substr :
            chars.add(c)
        if len(chars) == numUnique :
            return i + numUnique

def part1(fileName) :
    lines = stuff.readContentsByLine(fileName)
    index = 0
    for line in lines :
        index = unqiueCharacters(line, 4)
        # print(line)
        # print(index)
    return index

def part2(fileName) :
    lines = stuff.readContentsByLine(fileName)
    index = 0
    for line in lines :
        index = unqiueCharacters(line, 14)
        # print(line)
        # print(index)
    return index

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d06Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d06.txt'
    print(stuff.expected(part1, sampleFileName, 11))
    print(stuff.expected(part1, fileName, 1920))
    print(stuff.expected(part2, sampleFileName, 26))
    print(stuff.expected(part2, fileName, 2334))
