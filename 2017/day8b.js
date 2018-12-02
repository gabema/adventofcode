/*
http://adventofcode.com/2017/day/8

b inc 5 if a > 1
a inc 1 if b < 5
c dec -10 if a >= 1
c inc -20 if c == 10
also valid registers <=, !=
*/
const readline = require('readline');
const reduce = require('lodash.reduce');

const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

let registers = new Map();

const reg = /([a-z]+) (inc|dec) ([\-\d]+) if ([a-z]+) ([!<>=]+) ([\-\d]+)/;

const getOrZero = (registers, name) => registers.get(name) || 0;

const inc = (left, right) => left + right;
const dec = (left, right) => left - right;

const getMaxEntry = registers => reduce([...registers.entries()], (curMin, currVal) => {
    return curMin.val > currVal[1] ?  curMin : {val:currVal[1], key:currVal[0]};
}, {val:Number.MIN_SAFE_INTEGER});

let maxValEver = Number.MIN_SAFE_INTEGER;

rl.on('line', input => {
    const match = reg.exec(input);
    if (!match) {
        console.log({endMaxEntry: getMaxEntry(registers), maxValEver} );
        return;
    }

    const result = {
        registerName: match[1],
        registerIncrement: match[2] === 'inc' ? inc : dec,
        registerAmount: parseInt(match[3], 10),
        conditionalName: match[4],
        conditionalOp: match[5],
        conditionalVal: parseInt(match[6], 10)
    };
    const regValue = getOrZero(registers, result.registerName);
    const condValue = getOrZero(registers, result.conditionalName);

    switch(result.conditionalOp) {
        case '<':
            registers.set(result.registerName, condValue < result.conditionalVal ? result.registerIncrement(regValue, result.registerAmount) : regValue);
            break;
        case '<=':
            registers.set(result.registerName, condValue <= result.conditionalVal ? result.registerIncrement(regValue, result.registerAmount) : regValue);
            break;
        case '>':
            registers.set(result.registerName, condValue > result.conditionalVal ? result.registerIncrement(regValue, result.registerAmount) : regValue);
            break;
        case '>=':
            registers.set(result.registerName, condValue >= result.conditionalVal ? result.registerIncrement(regValue, result.registerAmount) : regValue);
            break;
        case '!=':
            registers.set(result.registerName, condValue !== result.conditionalVal ? result.registerIncrement(regValue, result.registerAmount) : regValue);
            break;
        case '==':
            registers.set(result.registerName, condValue === result.conditionalVal ? result.registerIncrement(regValue, result.registerAmount) : regValue);
            break;
        default:
            console.log(`Unknown operation ${result.conditionalOp}`);
            break;
    }

    const currMaxVal = getMaxEntry(registers);
    if (currMaxVal.val > maxValEver) {
        maxValEver = currMaxVal.val;
    }
});
