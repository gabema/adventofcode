'''
https://adventofcode.com/2022/day/2
https://programming-idioms.org/cheatsheet/Python
https://regex101.com/
'''

import os
import stuff

rps = {
    'A': 'Rock',
    'X': 'Rock',
    'B': 'Paper',
    'Y': 'Paper',
    'C': 'Scissor',
    'Z': 'Scissor'
}

revRps = {
    'Rock': 'A',
    'Paper': 'B',
    'Scissor': 'C'
}

outcomePiece = {
    'Win': {
        'Rock': 'Paper',
        'Paper': 'Scissor',
        'Scissor': 'Rock'
    },
    'Lose': {
        'Rock': 'Scissor',
        'Paper': 'Rock',
        'Scissor': 'Paper'
    },
    'Draw': {
        'Rock': 'Rock',
        'Paper': 'Paper',
        'Scissor': 'Scissor'
    }
}

outcome = {
    'X': 'Lose',
    'Y': 'Draw',
    'Z': 'Win'
}

value = {
    'Rock': 1,
    'Paper': 2,
    'Scissor': 3
}

lostValue = 0
tieValue = 3
winValues = 6

def scoreRound(game) :
    them, us = game
    score = 0
    if them == us :
        score += tieValue
    if (them == 'Rock' and us == 'Paper') or (them=='Paper' and us == 'Scissor') or (them=='Scissor' and us == 'Rock') :
        score += winValues
    else :
        score += lostValue
    score += value[us]
    return score

def part1(fileName) :
    lines = stuff.readContentsByLine(fileName)
    games = [(rps[g[0]], rps[g[1]]) for g in [l.strip('\n').split(' ') for l in lines ] ]
    scores = [scoreRound(game) for game in games ]
    return sum(scores)

def part2(fileName) :
    lines = stuff.readContentsByLine(fileName)
    games = [(rps[g[0]], outcome[g[1]]) for g in [l.strip('\n').split(' ') for l in lines ] ]
    games = [(g[0], outcomePiece[g[1]][g[0]]) for g in games ]
    scores = [scoreRound(game) for game in games ]
    return sum(scores)

if __name__ == "__main__" :
    sampleFileName = os.path.dirname(__file__) + '/y22d02Sample.txt'
    fileName = os.path.dirname(__file__) + '/y22d02.txt'
    print(stuff.expected(part1, sampleFileName, 15))
    print(stuff.expected(part1, fileName, 11449))
    print(stuff.expected(part2, sampleFileName, 12))
    print(stuff.expected(part2, fileName, 13187))
