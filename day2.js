const readline = require('readline');

const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

const re = /\s+/;

let checksum = 0;

rl.on('line', (input) => {
    let minVal = 0, maxVal = 0;
    let values = input.split(re);
    if (values.length > 0) {
        minVal = maxVal = parseInt(values.shift(), 10);
    }

    values.forEach(val => {
        let valInt = parseInt(val, 10);
        if (minVal > valInt) {
            minVal = valInt;
        }
        if (maxVal < valInt) {
            maxVal = valInt;
        }
    });
    checksum += maxVal - minVal;
    console.log(`checksum = ${checksum}`);
});
