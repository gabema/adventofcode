'''
https://adventofcode.com/2021/day/16
'''
import os
import stuff

class VersionAccumulator(object):
    def __init__(self):
        self._sum = 0

    def AddVersion(self, v) :
        self._sum += v

    @property
    def VersionSum(self):
        """
        Doc for x
        """
        return self._sum

versionAccum = VersionAccumulator()

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
    bitsRead = 5
    while notLast :
        notLast = next(it) == '1'
        contentPart += next(it) + next(it) + next(it) + next(it)
        bitsRead += 5
    contentPart = int(contentPart, base=2)
    return (contentPart, bitsRead)

def readOpPacket(it, op) :
    lenID = next(it)
    bitsRead = 1
    numSubPackets = 0
    totalLenInBits = 0
    if lenID == '1' :
        val = next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it)
        bitsRead += 11
        numSubPackets = int(val, base=2)
    else :
        # assuming lenID == '0'
        val = next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it) + next(it)
        bitsRead += 15
        totalLenInBits = int(val, base=2)

    subpackets = []
    if numSubPackets > 0 :
        for _ in range(numSubPackets) :
            _, _, subPacketBits, content = readPacketHeader(it)
            bitsRead += subPacketBits
            subpackets.append(content)
    elif totalLenInBits > 0 :
        bitsRemaining = totalLenInBits
        while bitsRemaining > 10 :
            _, _, subPacketBits, content = readPacketHeader(it)
            bitsRead += subPacketBits
            subpackets.append(content)
            bitsRemaining -= subPacketBits

        for _ in range(bitsRemaining) :
            next(it)

    val = 0
    if op == 0 :
        val = sum(subpackets)
    elif op == 1 :
        val = 1
        for v in subpackets :
            val *= v
    elif op == 2 :
        val = min(subpackets)
    elif op == 3 :
        val = max(subpackets)
    elif op == 5 :
        val = 1 if subpackets[0] > subpackets[1] else 0
    elif op == 6 :
        val = 1 if subpackets[0] < subpackets[1] else 0
    elif op == 7 :
        val = 1 if subpackets[0] == subpackets[1] else 0

    return (val, bitsRead)

def readPacketHeader(it) :
    global versionAccum
    version = int(next(it) + next(it) + next(it), base=2)
    versionAccum.AddVersion(version)
    pType = int(next(it) + next(it) + next(it), base=2)
    bitsRead = 6
    packet = (0, 0, 0)
    if pType == 4 :
        content, subBitsRead = readLiteralPacket(it)
        bitsRead += subBitsRead
        packet = (version, pType, bitsRead, content)
    else :
        content, subBitsRead = readOpPacket(it, pType)
        bitsRead += subBitsRead
        packet = (version, pType, bitsRead, content)

    return packet

def part1(fileName) :
    global versionAccum
    versionAccum = VersionAccumulator()
    it = bitIterator(fileName)
    readPacketHeader(it)
    return versionAccum.VersionSum

def part2(fileName) :
    it = bitIterator(fileName)
    _, _, _, val = readPacketHeader(it)
    return val

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y21d16Sample.txt'
    fileName = os.path.dirname(__file__) + '/y21d16.txt'
    #print(stuff.expected(part1, sampleFileName, 31))
    print(stuff.expected(part1, fileName, 875))
    #print(stuff.expected(part2, sampleFileName, 1))
    print(stuff.expected(part2, fileName, 1264857437203))
