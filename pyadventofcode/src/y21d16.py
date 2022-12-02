'''
https://adventofcode.com/2021/day/16
'''
import os
import stuff

def bitIterator(fileName) :
    contents = stuff.readContents(fileName)
    for c in contents :
        binStr = '000' + bin(int(c, base=16))[2:]
        binStr = binStr[-4:]
        for b in binStr :
            yield b

def readLiteralPacket(it) :
    contentPart = ''
    notLast = next(it) == '1'
    contentPart += next(it) + next(it) + next(it) + next(it)
    while notLast :
        notLast = next(it) == '1'
        contentPart += next(it) + next(it) + next(it) + next(it)
    contentPart = int(contentPart, base=2)
    return contentPart

def readOpPacket(it) :
    subType = next(it)
    numSubPackets = 0
    totalLenInBits = 0
    if subType == '1' :
        val = next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it)
        numSubPackets = int(val, base=2)
    else :
        val = next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it)
        totalLenInBits = int(val, base=2)

    #todo parse subpackets
    subpackets = []
    subpackets.append(readPacket(it))
    subpackets.append(readPacket(it))

    return (numSubPackets, totalLenInBits, subpackets)

def readPacket(it) :
    pType = int(next(it) + next(it) + next(it), base=2)
    packet = (0, 0, 0)
    if pType == 4 :
        packet = (pType, readLiteralPacket(it))
    else :
        packet = (pType, readOpPacket(it))

    return packet

def readPacketHeader(it) :
    version = int(next(it) + next(it) + next(it), base=2)
    packet = readPacket(it)
    return (version, packet)

def part1(fileName) :
    it = bitIterator(fileName)
    return readPacketHeader(it)

def part2(fileName) :
    pass

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y21d16Sample.txt'
    fileName = os.path.dirname(__file__) + '/y21d16.txt'
    print(stuff.expected(part1, sampleFileName, 24000))
    #print(stuff.expected(part1, fileName, 68442))
    #print(stuff.expected(part2, sampleFileName, 45000))
    #print(stuff.expected(part2, fileName, 204837))
