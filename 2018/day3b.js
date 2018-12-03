const readline = require('readline');
const forEach = require('lodash.foreach');
const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

const pattern = /#(\d+) @ (\d+),(\d+): (\d+)x(\d+)/;

let accumSpace = {};
let nonOverlaps = {};
const entryMatching = (left, right) => left.split('').reduce((matchingChars, val, index) => matchingChars + (val === right[index]?val:''), ''); 

const processEntry = (input) => {
  const parts = pattern.exec(input);
  if (!parts) return null;
  return {
    id: parseInt(parts[1], 10),
    x: parseInt(parts[2], 10),
    y: parseInt(parts[3], 10),
    width: parseInt(parts[4], 10),
    height: parseInt(parts[5], 10)
  };
};

const addEntry = entry => {
  const endY = entry.y+entry.height;
  nonOverlaps['' + entry.id] = entry;
  for (let y = entry.y; y < endY; y++) {
    const key = y + '';
    accumSpace[key] = combineRow(accumSpace[key], entry.x, entry.width, entry.id);
  }
};

const combineRow = (row = [], x, len, id) => {
  const minWidth = x + len;
  if (minWidth > row.length) row = [...row, ...Array(minWidth - row.length)];
  for (let xID = x; xID < minWidth; xID++) {
    if (row[xID]) {
      const oldID = row[xID];
      row[xID] = -1;
      if (nonOverlaps['' + oldID]) delete nonOverlaps['' + oldID];
      if (nonOverlaps['' + id]) delete nonOverlaps['' + id];
    } else {
      row[xID] = id;
    }
  }
  return row;
};

rl.on('line', (input) => {
  const processedEntry = processEntry(input);
  if (processedEntry === null) {
    console.log(nonOverlaps);
    return;
  }

  addEntry(processedEntry);
});
