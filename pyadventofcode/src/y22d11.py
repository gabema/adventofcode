'''
https://adventofcode.com/2022/day/11
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
https://github.com/subhajeet2107/pylexer/
'''
import os
import stuff
import re
from collections import deque

myEval = lambda equation : lambda old : eval(equation, {'old': old})

def monkeyIt(fileName) :
    monkeyEx = re.compile(r'Monkey (\d+):')
    startingItemsEx = re.compile(r'\s+Starting items: ([\d, ]+)')
    opEx = re.compile(r'\s+Operation: new = ([^$]+)')
    testEx = re.compile(r'\s+Test: divisible by (\d+)')
    condEx = re.compile(r'\s+If (true|false): throw to monkey (\d+)')
    lineIt = iter(stuff.readContentsByLine(fileName))

    line = next(lineIt, "Done")
    while line != "Done" :
        if line == '\n' :
            line = next(lineIt, "Done")
        else :
            monkeyNum = int(monkeyEx.match(line)[1])        
            line = next(lineIt)
            sItems = startingItemsEx.match(line)
            sItems = sItems[1].split(', ')
            items = deque([int(i) for i in sItems])
            line = next(lineIt)
            op = myEval(opEx.match(line)[1][:-1])
            line = next(lineIt)
            testDivBy = int(testEx.match(line)[1])
            line = next(lineIt)
            trueMonkey = int(condEx.match(line)[2])
            line = next(lineIt)
            falseMonkey = int(condEx.match(line)[2])
            yield (monkeyNum, items, op, testDivBy, trueMonkey, falseMonkey)
            line = next(lineIt, "Done")

def part1(fileName) :
    monkeys = [monkey for monkey in monkeyIt(fileName) ]
    numMonkeys = len(monkeys)
    monkeyInspectionCount = [0 for m in monkeys ]
    for _ in range(20) :
        for mIndex in range(numMonkeys) :
            _, items, op, testDivBy, trueMonkey, falseMonkey = monkeys[mIndex]
            monkeyInspectionCount[mIndex] = monkeyInspectionCount[mIndex] + len(items)
            while len(items) > 0 :
                val = items.popleft()
                newVal = op(val) // 3
                if newVal % testDivBy == 0 :
                    monkeys[trueMonkey][1].append(newVal)
                else :
                    monkeys[falseMonkey][1].append(newVal)
    monkeyInspectionCount = sorted(monkeyInspectionCount)
    return monkeyInspectionCount[-2] * monkeyInspectionCount[-1]
    
def part2(fileName) :
    monkeys = [monkey for monkey in monkeyIt(fileName) ]
    numMonkeys = len(monkeys)
    monkeyInspectionCount = [0 for m in monkeys ]
    commonDivisor = 1
    for m in monkeys :
        commonDivisor *= m[3]

    for round in range(10000) :
        print("Round %i" % round)
        if round % 100 == 0 :
            pass

        for mIndex in range(numMonkeys) :
            _, items, op, testDivBy, trueMonkey, falseMonkey = monkeys[mIndex]
            monkeyInspectionCount[mIndex] = monkeyInspectionCount[mIndex] + len(items)

            try :
                val = items.popleft()
                while True:
                    newVal = op(val)
                    if newVal % commonDivisor == 0 :
                        newVal = commonDivisor
                    if newVal % testDivBy == 0 :
                        monkeys[trueMonkey][1].append(newVal)
                    else :
                        monkeys[falseMonkey][1].append(newVal)
                    val = items.popleft()
            except IndexError :
                pass

    monkeyInspectionCount = sorted(monkeyInspectionCount)
    return monkeyInspectionCount[-2] * monkeyInspectionCount[-1]

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d11Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d11.txt'
#    print(stuff.expected(part1, sampleFileName, 10605))
    print(stuff.expected(part1, fileName, 4536))
#    print(stuff.expected(part2, sampleFileName, 36))
    #print(stuff.expected(part2, fileName, 2529))
