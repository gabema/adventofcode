'''
https://adventofcode.com/2022/day/10
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
https://github.com/subhajeet2107/pylexer/
'''
import os
import stuff

noop = lambda x, y : x
addx = lambda x, y : x + y

def interestingCycles() :
    cycle = 20
    while True :
        yield(cycle)
        cycle += 40

def readCommands(fileName) :
    for line in stuff.readContentsByLine(fileName) :
        parts = line.removesuffix('\n').split(' ')
        yield (parts[0], int(parts[1:][0]) if len(parts[1:]) > 0 else 0)

def part1(fileName) :
    cycleNum = 0
    X = 1
    sigStrenghths = 0
    it = interestingCycles()
    interestingCycle = next(it)
    for command in readCommands(fileName) :
        op, opVal = command
        # begin cycle
        opCycles = 2 if op == 'addx' else 1
        for _ in range(opCycles) :
            # during cycle
            cycleNum += 1
            if cycleNum == interestingCycle :
                sigStrenghths += cycleNum * X
                interestingCycle = next(it)
            # end of cycle
        X += opVal

    return sigStrenghths

currentX = 1
def moveSprite(newX, sprite) :
    global currentX
    for i in range(-1, 2, 1) :
        sprite[currentX+1*i], sprite[newX+1*i] = sprite[newX+1*i], sprite[currentX+1*i]
    currentX = newX - 1

def startSprit(x, sprite) :
    global currentX
    currentX = x - 1
    for i in range(-1, 2, 1) :
        sprite[currentX+1*i] = '#'

def drawPixel(x, sprite) :
    pass #TODO    

def part2(fileName) :
    # TODO finish
    sprintPosition =  ['.'] * 40
    cycleNum = 1
    X = 1
    sigStrenghths = 0
    startSprit(X, sprintPosition)
    for command in readCommands(fileName) :
        op, opVal = command
        # begin cycle
        opCycles = 2 if op == 'addx' else 1
        for _ in range(opCycles) :
            # during cycle
            cycleNum += 1

            # end of cycle

        X += opVal
        moveSprite(X,sprintPosition)


    return sigStrenghths

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d10Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d10.txt'
    # not 31140
    #print(stuff.expected(part1, sampleFileName, 13140))
    #print(stuff.expected(part1, fileName, 14780))
    print(stuff.expected(part2, sampleFileName, 36))
    #print(stuff.expected(part2, fileName, 2529))
