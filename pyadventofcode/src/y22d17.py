'''
https://adventofcode.com/2022/day/17
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
https://github.com/subhajeet2107/pylexer/
'''
import os
import stuff

pieces = {
    1 : {
        'heights' : [1,1,1,1],
        'width' : 4,
        'leftX' : 0,
        'bottomY' : 0,
    },
    2 : {
        'heights' : [2,3,2],
        'width' : 3,
        'leftX' : 0,
        'bottomY' : 0,
    },
    3 : {
        'heights' : [1,1,3],
        'width' : 3,
        'leftX' : 0,
        'bottomY' : 0,
    },
    4 : {
        'heights' : [4],
        'width' : 1,
        'leftX' : 0,
        'bottomY' : 0,
    },
    5 : {
        'heights' : [2,2],
        'width' : 2,
        'leftX' : 0,
        'bottomY' : 0,
    },
}

def pushLeft(piece) :
    '''
    Make sure it can move left if it's
    blocked by a higher part or the side
    it doesn't move
    otherwise it moves
    '''
    global bottomFilled


def pushRight(piece) :
    '''
    Make sure it can move right if it's
    blocked by a higher part or the side
    it doesn't move
    otherwise it moves
    '''
    global bottomFilled


def dropPiece(piece) :
    global bottomFilled
    return True


bottomFilled = [0,0,0,0,0,0,0]

def pieceIterator() :
    global pieces
    while True:
        for i in range(len(pieces)) :
            yield pieces[i+1]

def readJetPattern(fileName) :
    line = stuff.readContents(fileName)
    for c in line :
        if c == '<' or c == '>' :
            yield c

def startPiece(piece) :
    global bottomFilled
    Y = max(bottomFilled) + 3 + max(piece['heights'])
    X = round((len(bottomFilled) - piece['width']) / 2)
    piece['bottomY'] = Y
    piece['leftX'] = X

def part1(fileName) :
    pieceIt = pieceIterator()
    pushFunc = {
        '<' : pushLeft,
        '>' : pushRight
    }
    currentPiece = None
    for j in readJetPattern(fileName) :
        if currentPiece == None :
            currentPiece = next(pieceIt)
            startPiece(currentPiece)

        pushFunc[j](currentPiece)

        if not dropPiece(currentPiece) :
            currentPiece = None



def part2(fileName) :
    pass

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d17Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d17.txt'
    print(stuff.expected(part1, sampleFileName, 26))
#    print(stuff.expected(part1, fileName, 5688618))
#    print(stuff.expected(part2, sampleFileName, 93))
#    print(stuff.expected(part2, fileName, 27194))
