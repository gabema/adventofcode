'''
https://adventofcode.com/2022/day/1
'''
import os
import stuff

def calorieSums(fileName) :
    elves = [s for s in stuff.readIntLinesArrays(fileName)]
    calSums = [sum(e) for e in elves]
    return calSums

def part1(fileName) :
    calSums = calorieSums(fileName)
    return max(calSums)

def part2(fileName) :
    calSums = calorieSums(fileName)
    sums = sorted(calSums)
    sums = sums[-3:]
    return sum(sums)

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d01Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d01.txt'
    print(stuff.expected(part1, sampleFileName, 24000))
    print(stuff.expected(part1, fileName, 68442))
    print(stuff.expected(part2, sampleFileName, 45000))
    print(stuff.expected(part2, fileName, 204837))
