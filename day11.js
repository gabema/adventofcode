/*
http://adventofcode.com/2017/day/11

ne,ne,ne is 3 steps away.
ne,ne,sw,sw is 0 steps away (back where you started).
ne,ne,s,s is 2 steps away (se,se).
se,sw,se,sw,sw is 3 steps away (s,s,sw).

private leaderboard: http://adventofcode.com/2017/leaderboard/private/view/52925
*/

// "ne,ne,ne",
// "ne,ne,sw,sw",
// "ne,ne,s,s",
// "se,sw,se,sw,sw",
//broken: ne,nw,ne,nw,nw

const readline = require('readline');

const positions = "n,ne,se,s,sw,nw".split(',');

const converPositionsToNum = input => input.map(e => positions.indexOf(e));
const converNumsToPosition = input => input.map(e => positions[e]);

const reducer = num => {
    let nums = [...num];
    for (let i=0;i<nums.length;i++) {
        for (let j=i+1;j<nums.length;j++) {
            const p0 = nums[i];
            const p1 = nums[j];
            const diff = p1 - p0;
            let newNums;
            switch(diff) {
                case 3:
                case -3:
                    nums.splice(j, 1);
                    nums.splice(i, 1);
                    return [...nums];
                case 2:
                    nums.splice(j, 1);
                    nums.splice(i, 1);
                    newNums = [(p0+1)%6, ...nums];
                    return newNums;
                case -2:
                    nums.splice(j, 1);
                    nums.splice(i, 1);
                    newNums = [(p1+1)%6, ...nums];
                    return newNums;
                case 4:
                    nums.splice(j, 1);
                    nums.splice(i, 1);
                    newNums = [(p1+1)%6, ...nums];
                    return newNums;
                case -4:
                    nums.splice(j, 1);
                    nums.splice(i, 1);
                    newNums = [(p0+1)%6, ...nums];
                    return newNums;
                default:
                break;
            }
        }
    }
    return nums;
};

const repeatingReducer = input => {
    let len = 0;
    let output = [...input];
    while (output.length != len) {
        len = output.length;
        output = reducer(output);
    }
    return output;
};

const numMoves = input => input.reduce((accum, entry) => {
    accum['' + entry] = accum['' + entry] ? accum['' + entry]+1 : 1;
    return accum;
}, {});

const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
  });

rl.on('line', input => {
    const output = repeatingReducer(converPositionsToNum(input.split(',')))
    console.log({input, numMoves: numMoves(output), /*moves: converNumsToPosition(output),*/ len: output.length});
});