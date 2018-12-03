let result = 0;
const forEach = require('lodash.foreach');
const readline = require('readline');
const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

const valCounts = {
  'TWOS': 0,
  'THREES': 0
};

const checkInput = input => {
  let counts = {
    'TWOS': 0,
    'THREES': 0
  };

  forEach(input.split('').reduce((accum, entry) => {
    if (!accum[entry]) {
      accum[entry] = 1;
    } else {
      accum[entry] +=1;
    }
    return accum;
  }, {}), val => {
    if (val === 2) counts.TWOS = 1;
    else if (val === 3) counts.THREES = 1;
  });
  return counts;
};

rl.on('line', (input) => {
  if (input.length === 0) {
    console.log(valCounts.TWOS * valCounts.THREES);
    return;
  }
  console.log(valCounts);
  const checkedInput = checkInput(input);
  valCounts.TWOS += checkedInput.TWOS;
  valCounts.THREES += checkedInput.THREES;
});

// That's not the right answer; your answer is too high: 20032
// 7872
