const { PassThrough, Transform } = require('stream');

let result = 0;
let frequenciesFound = {'0':true};
let entries = [];

const readline = require('readline');
const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

// answer:77674

rl.on('line', (input) => {
  const val = parseInt(input, 10);
  if (isNaN(val)) {
    console.log('Repeating sequence until match found');
    let found = false;
    while(!found) {      
      entries.forEach(entry => {
        if (!found) {
          result += entry;
          if (frequenciesFound['' + result]) {
            console.log(`First matched frequency is ${result}`);
            found = true;
          } else {
            frequenciesFound['' + result] = true;
          }  
        }
      });
    }
  } else {
    entries.push(val);
    result += val;
    if (frequenciesFound['' + result]) {
      console.log(`First matched frequency is ${result}`);
      return;
     } else {
      frequenciesFound['' + result] = true;
    }
  }
});
