'''
https://adventofcode.com/2022/day/5
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
'''
import os
import stuff
import re

def moveCrates(move, stacks) :
    numToMove, source, destination = move
    for _ in range(numToMove) :
        crate = stacks[source].pop()
        stacks[destination].append(crate)

def moveCratesV2(move, stacks) :
    numToMove, source, destination = move
    numToTakeOff = 0 - numToMove
    crates = stacks[source][numToTakeOff:]
    stacks[destination] += crates
    stacks[source] = stacks[source][0:numToTakeOff]

def move(fileName, moveFunc) :
    indexes = [0,1,5,9,13,17,21,25,29,33]
    stacks = dict()
    for i in range(1, 10) :
        stacks[i] = []
    moves = []

    lines = stuff.readContentsByLine(fileName)
    crateEx = re.compile(r'\[(\w)\]')
    moveEx = re.compile(r'move (\d+) from (\d+) to (\d+)')
    for line in lines :
        match = [(m.start(0)+1, line[m.start(0)+1]) for m in re.finditer(crateEx, line)]
        if len(match) > 0 :
            for m in match :
                index = indexes.index(m[0])
                stacks[index].insert(0, m[1])
        else :
            match = moveEx.match(line)
            if match :
                moves.append( (int(match[1]), int(match[2]), int(match[3]) ) )

    for move in moves :
        moveFunc(move, stacks)

    output = ''
    for i in range(1, 10) :
        if len(stacks[i]) > 0 :
            output += stacks[i][-1]
    return output

def part1(fileName) :
    return move(fileName, moveCrates)

def part2(fileName) :
    return move(fileName, moveCratesV2)

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d05Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d05.txt'
    print(stuff.expected(part1, sampleFileName, 'CMZ'))
    print(stuff.expected(part1, fileName, 'GFTNRBZPF'))
    print(stuff.expected(part2, sampleFileName, 'MCD'))
    print(stuff.expected(part2, fileName, 'VRQWPDSGP'))
