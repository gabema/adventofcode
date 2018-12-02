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
const differencyBy = require('lodash.differenceby');
const groupBy = require('lodash.groupby');
const size = require('lodash.size');
const forEach = require('lodash.foreach');

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

const weighChildren = (program) => {
    if (program.children.length === 0) {
        const leaf = {name: program.name, weight: program.weight, childrenWeight: 0, totalWeight: program.weight};
        return leaf;
    } else {
        const childrenWeights = program.children.map(child => {
            const childrenWeightInfo = weighChildren(child);
            return {
                name: childrenWeightInfo.name,
                weight: childrenWeightInfo.weight,
                childrenWeight: childrenWeightInfo.childrenWeight,
                totalWeight: childrenWeightInfo.totalWeight
            };
        });
        if (childrenWeights.length > 0) {
            let groupedByTotalChildrenWeights = groupBy(childrenWeights, 'totalWeight');
            if (size(groupedByTotalChildrenWeights) > 1) {
                let goodSize, badSize, badWeight, badName;
                for(let k in groupedByTotalChildrenWeights) {
                    if (groupedByTotalChildrenWeights[k].length > 1) {
                        goodSize = groupedByTotalChildrenWeights[k][0].totalWeight;
                    } else {
                        badSize = groupedByTotalChildrenWeights[k][0].totalWeight;
                        badWeight = groupedByTotalChildrenWeights[k][0].weight;
                        badName = groupedByTotalChildrenWeights[k][0].name;
                    }
                }
                console.log(`${badName} weight is ${badWeight} needs to be ${goodSize - badSize + badWeight}`);
            }
        }
        const totalChildrenWeights = childrenWeights.reduce((sumWeight, child) => sumWeight + child.totalWeight, 0);
        return {
            name: program.name,
            weight: program.weight,
            totalWeight: program.weight + totalChildrenWeights,
            childrenWeight: totalChildrenWeights
        };
    }
};

rl.on('line', input => {
    if (input.length === 0) {
        let baseProgramName;
        programs.forEach(program => {
            if (program.parent === null) {
                baseProgramName = program.name;
                console.log(`base program name is ${baseProgramName}`);
            }
        });

        weighChildren(programs.get(baseProgramName));
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
