/*
http://adventofcode.com/2017/day/10
To achieve this, begin with a list of numbers from 0 to 255, a current position which begins at 0
 (the first element in the list), a skip size (which starts at 0), and a sequence of lengths (your puzzle input).
 Then, for each length:

Reverse the order of that length of elements in the list, starting with the element at the current position.
Move the current position forward by that length plus the skip size.
Increase the skip size by one.


That's not the right answer; your answer is too low. If you're stuck, there are some general tips on the about page, or you can ask for hints on the subreddit. Please wait one minute before trying again. (You guessed 14762.) [Return to Day 10]
https://www.reddit.com/r/adventofcode/
*/

const reduce = require('lodash.reduce');
const input = "46,41,212,83,1,255,157,65,139,52,39,254,2,86,0,204";
const inputSize = 256;

// build list
let inputList = [];
for (let i=0; i<inputSize; i++) {
    inputList.push(i);
};

const inputLengths = [
    ...reduce(input, (arr, val) => {arr.push(val.charCodeAt(0)); return arr;}, [])
    , 17, 31, 73, 47, 23];


let skipSize = 0;
let currentPosition = 0;

const reIndexArray = (oa, offset) => offset >= 0 ? [...oa.slice(offset), ...oa.slice(0, offset)]
    : [...oa.slice(oa.length + offset, oa.length), ...oa.slice(0, oa.length + offset)];

const reverseSubArray = (oa, offset, len) => {
    const reInArr = reIndexArray(oa, offset);
    const reverseSubArray = [...reInArr.slice(0, len).reverse(), ...reInArr.slice(len)];
    return reIndexArray(reverseSubArray, 0 - offset);
}

for (let round = 0; round < 64; round++) {
    inputLengths.forEach(reverseSize => {
        inputList = reverseSubArray(inputList, currentPosition, reverseSize);
        currentPosition += reverseSize + skipSize;
        currentPosition = currentPosition % inputSize;
        skipSize++;
    });
}

const sparseHash = inputList;
const denseHash = sparseHash.reduce((dh, val, valInd) => {
    if (valInd % 16 === 0) {
        dh.push(val);
    } else {
        let dhIndex = Math.floor(valInd / 16);
        dh[dhIndex] =  dh[dhIndex] ^ val;
    }
    return dh;
}, []);

const hash = denseHash.reduce((str, val) => str + ("0" + val.toString(16)).substr(-2), "");
console.log({hash});
