/*
http://adventofcode.com/2017/day/4
aa bb cc dd ee
aa bb cc dd aa
aa bb cc dd aaa
*/
const readline = require('readline');

const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

const re = /\s+/;

let validPassphraseCount = 0;

const sortCharacters = input => input.split('').sort().join('');

rl.on('line', (input) => {
    let dictionary = new Map();
    let values = input.split(re);

    const len = values.length;
    let isValid = true;
    for (let i = 0; i < len && isValid; i++) {
        let word = sortCharacters(values[i]);
        isValid = !dictionary.has(word);
        if (isValid) {
            dictionary.set(word, 1);
        }
    }

    if (isValid) {
        validPassphraseCount++;
        console.log(`${input} is valid. ${validPassphraseCount} valid passphrases.`);
    } else {
        console.log(`${input} is not valid.`);
    }
});
