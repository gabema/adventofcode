'''
https://adventofcode.com/2022/day/13
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
https://github.com/subhajeet2107/pylexer/
'''
import os
import stuff
from functools import cmp_to_key

def readPairs(fileName) :
    it = iter(stuff.readContentsByLine(fileName))

    try :
        while True :
            a = eval(next(it).removesuffix("\n"))
            b = eval(next(it).removesuffix("\n"))
            yield (a, b)
            next(it)
    except StopIteration :
        pass

def readPackets(fileName) :
    it = iter(stuff.readContentsByLine(fileName))

    try :
        while True: 
            v = next(it).removesuffix("\n")
            if v != '' :
                yield(eval(v))
    except StopIteration :
        pass

def getChecker(left, right) :
    if type(left) == int and type(right) == int :
        return checkInts
    else :
        return checkLists    


def checkInts(left, right) :
        if left < right :
            return -1
        elif right < left :
            return 1
        else :
            return 0

def checkLists(left, right) :
    if type(left) == int and type(right) == list :
        left = [ left ]
    elif type(left) == list and type(right) == int :
        right = [ right ]
    lenLeft = len(left)
    lenRight = len(right)

    if lenLeft == lenRight and lenRight == 0 :
        return 0
    if lenLeft == 0 and lenRight != 0 :
        return -1
    elif lenLeft != 0 and lenRight == 0:
        return 1

    for i in range(min(lenLeft, lenRight)) :
        checker = getChecker(left[i], right[i])
        result = checker(left[i], right[i])
        if result != 0 :
            return result
    if lenLeft < lenRight :
        return -1
    elif lenRight < lenLeft :
        return 1
    else :
        return 0

def checkOrder(left, right) :
    check =  checkInts(left, right) if type(left) == int and type(right) == int else checkLists(left, right)
    return check == -1

def findSmallestIndex(packets, startIndex) :
    plen = len(packets)
    k = startIndex
    for i in range(startIndex, plen) :
        if i < startIndex :
            continue

        for j in range(i+1, plen) :
            if i == j :
                continue
            if not checkOrder(packets[i], packets[j]) :
                break
            else :
                k = j

        if k == plen - 1 :
            return i
    return plen - 1

def part1(fileName) :
    indecesSum = 0
    index = 1
    for p in readPairs(fileName) :
        indecesSum += index if checkOrder(p[0], p[1]) else 0
        index += 1
    return indecesSum

def part2(fileName) :
    packets = [ a for a in readPackets(fileName) ]
    packets.append([[2]])
    packets.append([[6]])
    for i in range(len(packets)) :
        si = findSmallestIndex(packets, i)
        h = packets[i]
        packets[i] = packets[si]
        packets[si] = h
    #print("\n".join([str(s) for s in packets]))
    i = packets.index([[2]]) + 1
    j = packets.index([[6]]) + 1
    return i * j

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d13Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d13.txt'
    print(stuff.expected(part1, sampleFileName, 13))
    print(stuff.expected(part1, fileName, 5198))
    print(stuff.expected(part2, sampleFileName, 140))
    print(stuff.expected(part2, fileName, 22344))
