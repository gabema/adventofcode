/*
http://adventofcode.com/2017/day/9
{}
{{{}}}
{{},{}}
{{{},{},{{}}}}
{<a>,<a>,<a>,<a>}
{{<ab>},{<ab>},{<ab>},{<ab>}}
{{<!!>},{<!!>},{<!!>},{<!!>}}
{{<a!>},{<a!>},{<a!>},{<ab>}}
----
{}, score of 1.
{{{}}}, score of 1 + 2 + 3 = 6.
{{},{}}, score of 1 + 2 + 2 = 5.
{{{},{},{{}}}}, score of 1 + 2 + 3 + 3 + 3 + 4 = 16.
{<a>,<a>,<a>,<a>}, score of 1.
{{<ab>},{<ab>},{<ab>},{<ab>}}, score of 1 + 2 + 2 + 2 + 2 = 9.
{{<!!>},{<!!>},{<!!>},{<!!>}}, score of 1 + 2 + 2 + 2 + 2 = 9.
{{<a!>},{<a!>},{<a!>},{<ab>}}, score of 1 + 2 = 3.
*/
const Tokenizer = require('tokenizer2');
const readline = require('readline');

const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
  });

rl.on('line', (input) => {
    let processingGarbage = false;
    let groupLevel = 0;
    let totalScore = 0;
    let t = new Tokenizer();
    
    t.addRule(/^!.$/, 'cancelled');
    t.addRule(/^<$/, 'open garbage');
    t.addRule(/^>$/, 'close garbage');
    t.addRule(/^{$/, 'open group');
    t.addRule(/^}$/, 'close group');
    t.addRule(/^.$/, 'noise');
    
    t.on('end', () => {
        console.log({totalScore});
    });
    
    t.on('data', data => {
        const state = {totalScore, groupLevel, processingGarbage};
        const token = data.src;
        const type = data.type;
        // also can get line/col from data if important
        switch(data.type) {
            case 'open group':
                if (!processingGarbage) {
                    groupLevel++;
                }
                break;
            case 'close group':
                if (!processingGarbage) {
                    totalScore += groupLevel;
                    groupLevel--;
                }
                break;
            case 'open garbage':
                processingGarbage = true;
                break;
            case 'close garbage':
                processingGarbage = false;
                break;        
        }
    });
    
    t.on('error', err => {
        console.log(`Error! ${err}`);
    });

    t.end(input);
});