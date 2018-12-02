/*
http://adventofcode.com/2017/day/12
*/

const fs = require('fs');
const readline = require('readline');

const rl = readline.createInterface({
  input: fs.createReadStream('./day12input.txt'), // process.stdin
  crlfDelay: Infinity,
});

const inputRegEx = /(\d+) <-> ([\d ,]+)/;

rl.on('line', (input) => {
  const match = inputRegEx.exec(input);
  console.log(match);
});
