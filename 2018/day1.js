let result = 0;
const readline = require('readline');
const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

rl.on('line', (input) => {
  const val = parseInt(input, 10);
  if (isNaN(val)) {
    console.log(result);    
  } else {
    result += val;
  }
});
// 576
