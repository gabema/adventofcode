/*
http://adventofcode.com/2017/day/7

pbga (66)
xhth (57)
ebii (61)
havc (66)
ktlj (57)
fwft (72) -> ktlj, cntj, xhth
qoyq (66)
padx (45) -> pbga, havc, qoyq
tknk (41) -> ugml, padx, fwft
jptl (61)
ugml (68) -> gyxo, ebii, jptl
gyxo (61)
cntj (57)
*/

const readline = require('readline');
const Tokenizer = require('tokenizer2');

const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

let programs = new Map();

const getOrCreate = (map, key) => {
    let value = map.get(key);
    if (!value) {
        value = { name: key, weight: 0, children: [], parent: null };
        map.set(key, value);
    }
    return value;
}

rl.on('line', input => {
    if (input.length === 0) {
        programs.forEach(program => {
            if (program.parent === null) {
                console.log(`base program name is ${program.name}`);
            }
        });
    }

    let t = new Tokenizer();
    let program;

    t.addRule(/^[a-z]+$/i, 'program name');
    t.addRule(/^\(\d+\)$/, 'program weight');
    t.addRule(/^->$/, 'children marker');
    t.addRule(/^[ ,]+$/, 'whitespace');
    t.on('data', data => {
        const token = data.src;
        const type = data.type;
        // also can get line/col from data if important
        switch(type) {
            case 'program name':
                if (!program) {
                    program = getOrCreate(programs, token);
                    program.name = token;
                } else {
                    let child = getOrCreate(programs, token);
                    program.children.push(child);
                    child.parent = program;
                }
                break;
            case 'program weight':
                program.weight = parseInt(token.substr(1, token.length - 2), 10);
                break;
            case 'children marker':
                break;
        }
    });
    t.on('error', err => {
        console.log(`Error! ${err}`);
    });
    t.end(input);
});
