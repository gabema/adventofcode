/*
http://adventofcode.com/2017/day/6
0 2 7 0
3 1 2 3
*/

const readline = require('readline');

const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

const keyBank = bank => bank.join(',');

const maxIndex = memBank => memBank.reduce((maxInd, val, curIndex, arr) => {
        if (arr[maxInd] < val)
            return curIndex;
        else
            return maxInd;
    }, 0);

const redistribute = memBank => {
    const mi = maxIndex(memBank);
    const memBankLen = memBank.length;
    let redistributeAmount = memBank[mi];
    let ind = mi;
    memBank[mi] = 0;
    for(; redistributeAmount > 0; redistributeAmount--) {
        ind = ind === memBankLen - 1 ? 0 : ind + 1;        
        memBank[ind]++;
    }
    return memBank;
}

const processInput = memBankValues => {
    let memBankInts = memBankValues.split(/\s+/).map(entry => parseInt(entry, 10));
    let visitedBlocks = new Map();
    let steps = 0;
    let bankKey = keyBank(memBankInts);
    while(!visitedBlocks.has(bankKey)) {
        visitedBlocks.set(bankKey, 0);
        steps++
        memBankInts = redistribute(memBankInts);
        bankKey = keyBank(memBankInts);
    }

    console.log({steps, bankKey});
};

rl.on('line', input => {
    processInput(input);
});
