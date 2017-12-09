/*
http://adventofcode.com/2017/day/5
0
3
0
1
-3
*/

const readline = require('readline');

const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

let jumpInstructions = [];

const processInput = instructions => {
    let index = 0;
    let steps = 0;
    const instructLength = instructions.length;
    while (index >= 0 && index < instructLength) {
        steps++;
        const oldIndex = index;
        index += instructions[index];
        if (instructions[oldIndex] >= 3) {
            instructions[oldIndex] = instructions[oldIndex] - 1;
        } else {
            instructions[oldIndex] = instructions[oldIndex] + 1;    
        }
    }
    console.log({steps, index});
};

rl.on('line', input => {
    let inputInt = parseInt(input, 10);
    if (Number.isNaN(inputInt)) {
        processInput(jumpInstructions);
        jumpInstructions = [];
    } else {
        jumpInstructions.push(inputInt);
    }
});
