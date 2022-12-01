'''
https://adventofcode.com/2020/day/1
'''
import os
import stuff

def part1(fileName) :
    nums = [int(a) for a in stuff.readContentsByLine(fileName)]
    count = len(nums)
    for i in range(count) :
        for j in range(i, count) :
            if nums[i] + nums[j] == 2020 :
                return nums[i] * nums[j]
    return 0

def part2(fileName) :
    nums = [int(a) for a in stuff.readContentsByLine(fileName)]
    count = len(nums)
    for i in range(count) :
        for j in range(i, count) :
            for k in range(j, count) :
                if nums[i] + nums[j] + nums[k] == 2020 :
                    return nums[i] * nums[j] * nums[k]
    return 0

if __name__ == "__main__" :
    fileName = os.path.dirname(__file__) + '/y20d01.txt'
    print(stuff.expected(part1, fileName, 692916))
    print(stuff.expected(part2, fileName, 289270976))
