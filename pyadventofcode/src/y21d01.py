'''
https://adventofcode.com/2021/day/1
'''
import os
import stuff

def part1(fileName) :
    nums = [int(a) for a in stuff.readContentsByLine(fileName)]
    lastNum = nums[0]
    numIncreasing = 0
    for i in range(1, len(nums)) :
        if nums[i] > lastNum :
            numIncreasing += 1
        lastNum = nums[i]

    return numIncreasing

def repeatingIndexer(l = 0) :
    while True :
        for i in range(l) :
            yield i

def part2(fileName) :
    nums = [int(a) for a in stuff.readContentsByLine(fileName)]
    numGroups = [
        [nums[0], nums[1], nums[2]],
        [nums[1], nums[2]],
        [nums[2]],
        [],
    ]
    it = repeatingIndexer(len(numGroups))
    sumIncreasing = 0
    for i in range(3, len(nums)) :
        leftIndex = next(it)
        rightIndex = next(it)
        numGroups[rightIndex].append(nums[i])
        numGroups[next(it)].append(nums[i])
        numGroups[next(it)].append(nums[i])
        numGroups[leftIndex] = numGroups[leftIndex][-3:]
        numGroups[rightIndex] = numGroups[rightIndex][-3:]
        sumLeft = sum(numGroups[leftIndex])
        sumRight = sum(numGroups[rightIndex])
        if sumRight > sumLeft :
            sumIncreasing += 1
        # burn 1 iterator
        next(it)
    return sumIncreasing

if __name__ == "__main__" :
    #fileName = os.path.dirname(__file__) + '/y21d01Sample.txt'
    fileName = os.path.dirname(__file__) + '/y21d01.txt'
    print(part1(fileName))
    print(part2(fileName))
