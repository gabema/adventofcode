const readline = require('readline');

const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

const re = /\s+/;

let checksum = 0;

rl.on('line', (input) => {
    let values = input.split(re);
    let minVal = 0, maxVal = 0;
    while (values.length > 0 && (minVal === 0 && maxVal === 0)) {
        let valOne = parseInt(values.shift(), 10);
        for(var i=0; i<values.length; i++) {
            let valTwo = parseInt(values[i], 10);
            let curMin = Math.min(valOne, valTwo);
            let curMax = Math.max(valOne, valTwo);
            if (curMax % curMin === 0) {
                minVal = curMin;
                maxVal = curMax;
                break;
            }
        }            
    }

    checksum += (maxVal / minVal);
    console.log(`checksum = ${checksum}`);
});
