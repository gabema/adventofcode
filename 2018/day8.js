/*
https://adventofcode.com/2018/day/8
--- Day 8: Memory Maneuver ---
The sleigh is much easier to pull than you'd expect for something its weight. Unfortunately, neither you nor the Elves know which way the North Pole is from here.

You check your wrist device for anything that might help. It seems to have some kind of navigation system! Activating the navigation system produces more bad news: "Failed to start navigation system. Could not read software license file."

The navigation system's license file consists of a list of numbers (your puzzle input). The numbers define a data structure which, when processed, produces some kind of tree that can be used to calculate the license number.

The tree is made up of nodes; a single, outermost node forms the tree's root, and it contains all other nodes in the tree (or contains nodes that contain nodes, and so on).

Specifically, a node consists of:

A header, which is always exactly two numbers:
The quantity of child nodes.
The quantity of metadata entries.
Zero or more child nodes (as specified in the header).
One or more metadata entries (as specified in the header).
Each child node is itself a node that has its own header, child nodes, and metadata. For example:

2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2
A----------------------------------
    B----------- C-----------
                     D-----
In this example, each node of the tree is also marked with an underline starting with a letter for easier identification. In it, there are four nodes:

A, which has 2 child nodes (B, C) and 3 metadata entries (1, 1, 2).
B, which has 0 child nodes and 3 metadata entries (10, 11, 12).
C, which has 1 child node (D) and 1 metadata entry (2).
D, which has 0 child nodes and 1 metadata entry (99).
The first check done on the license file is to simply add up all of the metadata entries. In this example, that sum is 1+1+2+10+11+12+2+99=138.

What is the sum of all metadata entries?
*/

const readline = require('readline');
const rl = readline.createInterface({
  input: require('fs').createReadStream('day8input.txt'),
//  output: process.stdout,
  console: false
});

const pattern = /\d+/g;

let inputs = [];

/**
 * Processes a line of input or returns null.
 * @param {*} input 
 */
const processEntry = (input) => {
  let inputs = [];
  let parts;
  while ((parts = pattern.exec(input)) !== null) {
    inputs.push(parseInt(parts[0], 10));
  }
  if (inputs.length === 0) return null;
  return inputs;
};

let entries = {
  input: [],
  nodes: []
};
const addEntry = entry => {
  entries.input = [...entries.input, ...entry];
};

function* makeNextIntIterator() {
  while(true) {
    if (entries.input.length > 0) {
      let input = entries.input[0];
      entries.input = entries.input.slice(1);
      yield input;
    }
  }
}

let inputIterator = makeNextIntIterator();

function* makeInputIterator() {
  let childNodeCount = inputIterator.next().value;
  let metadataCount = inputIterator.next().value;
  for (let node of makeNodeIterator(childNodeCount, metadataCount)) {
    yield node;
  }
}

function *makeMetadataIterator(metadataCount = 0) {
  while(metadataCount-- > 0) {
    let metadata = inputIterator.next().value;
    yield metadata;
  }
}

function* makeNodeIterator(nodeCount = 0, metadataCount = 0) {
  let childNodes = [];
  let metaData = [];
  while(nodeCount-- > 0) {
    let childNodeCount = inputIterator.next().value;
    let childMetadataCount = inputIterator.next().value;
    for (let node of makeNodeIterator(childNodeCount, childMetadataCount)) {
      yield node;
      childNodes.push(node);
    }
  }
  for (let metadata of makeMetadataIterator(metadataCount)) {
    metaData.push(metadata);
  }
  let node = {
    childNodes,
    metaData
  };
  yield node;
}

const evaluateEntries = () => {
  let metadataSum = 0;
  for (let node of makeInputIterator()) {
    metadataSum = node.metaData.reduce((sum, meta) => sum + meta, metadataSum);
  }
  console.log(`evaluateEntries Metadata sum is ${metadataSum}`);
};

rl.on('line', (input) => {
  const processedEntry = processEntry(input);
  if (processedEntry === null) {
    evaluateEntries();
    return;
  }

  addEntry(processedEntry);
});

rl.on('end', () => {
  evaluateEntries();
});
