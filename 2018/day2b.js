const readline = require('readline');
const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

let inputs = [];
const entryMatching = (left, right) => left.split('').reduce((matchingChars, val, index) => matchingChars + (val === right[index]?val:''), ''); 

rl.on('line', (input) => {
  if (input.length === 0) {
    const mostMatching = inputs.reduce((accum, entry, index, arr) => {
      return arr.slice(index+1).reduce((accum, entryInner) => {
        const current = entryMatching(entry, entryInner);
        return (current.length > accum.length) ? current : accum;
      }, accum);
    }, '');

    console.log(mostMatching);
    return;
  }
  inputs.push(input);
});

// correct answer: tjxmoewpdkyaihvrndfluwbzc