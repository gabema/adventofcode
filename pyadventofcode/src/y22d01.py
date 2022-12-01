'''
https://adventofcode.com/2022/day/1
'''
import os
import stuff

def calorieSums(fileName) :
    lines = stuff.readContentsByLine(fileName)
    lines = [l.strip('\n') for l in lines]
    calories = []
    elves = []
    for l in lines :
        if l == '' :
            elves.append(calories)
            calories = []
        else :
            calories.append(int(l))

    calSums = []
    for e in elves :
        calSums.append(sum(e))
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
    #fileName = os.path.dirname(__file__) + '/y22d01Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d01.txt'
    print(part1(fileName))
    print(part2(fileName))
