/*
https://adventofcode.com/2018/day/8
--- Day 8: Memory Maneuver ---
The second check is slightly more complicated: you need to find the value of the root node (A in the example above).

The value of a node depends on whether it has child nodes.

If a node has no child nodes, its value is the sum of its metadata entries. So, the value of node B is 10+11+12=33, and the value of node D is 99.

However, if a node does have child nodes, the metadata entries become indexes which refer to those child nodes. A metadata entry of 1 refers to the first child node, 2 to the second, 3 to the third, and so on. The value of this node is the sum of the values of the child nodes referenced by the metadata entries. If a referenced child node does not exist, that reference is skipped. A child node can be referenced multiple time and counts each time it is referenced. A metadata entry of 0 does not refer to any child node.

For example, again using the above nodes:

Node C has one metadata entry, 2. Because node C has only one child node, 2 references a child node which does not exist, and so the value of node C is 0.
Node A has three metadata entries: 1, 1, and 2. The 1 references node A's first child node, B, and the 2 references node A's second child node, C. Because node B has a value of 33 and node C has a value of 0, the value of node A is 33+33+0=66.
So, in this example, the value of the root node is 66.

What is the value of the root node?
*/

const readline = require('readline');
const rl = readline.createInterface({
  input: require('fs').createReadStream('day8input.txt'),
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
  let childNodes = new Array(nodeCount);
  let metaData = [];
  for (let childNodeIndex = 0; childNodeIndex < nodeCount; childNodeIndex++) {
    let childNodeCount = inputIterator.next().value;
    let childMetadataCount = inputIterator.next().value;
    for (let node of makeNodeIterator(childNodeCount, childMetadataCount)) {
      yield node;
      childNodes[childNodeIndex] = node;
    }
  }
  for (let metadata of makeMetadataIterator(metadataCount)) {
    metaData.push(metadata);
  }
  let nodeValue = 0;
  if (childNodes.length === 0) {
    nodeValue = metaData.reduce((nv, meta) => nv + meta, 0);
  }
  else {
    nodeValue = metaData.reduce((nv, meta) => {
      let childNodeValue = (childNodes.length > (meta-1)) ? childNodes[meta-1].nodeValue : 0;
      return nv + childNodeValue;
    }, 0);
  }

  let node = {
    childNodes,
    metaData,
    nodeValue
  };
  yield node;
}

const evaluateEntries = () => {
  let metadataSum = 0;
  let rootNodeValue;
  for (let node of makeInputIterator()) {
    metadataSum = node.metaData.reduce((sum, meta) => sum + meta, metadataSum);
    rootNodeValue = node.nodeValue;
  }
  console.log(`evaluateEntries Metadata sum is ${metadataSum}`);
  console.log(`evaluateEntries rootNodeValue is ${rootNodeValue}`);
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
