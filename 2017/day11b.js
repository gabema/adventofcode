/*
http://adventofcode.com/2017/day/11

ne,ne,ne is 3 steps away.
ne,ne,sw,sw is 0 steps away (back where you started).
ne,ne,s,s is 2 steps away (se,se).
se,sw,se,sw,sw is 3 steps away (s,s,sw).

private leaderboard: http://adventofcode.com/2017/leaderboard/private/view/52925
*/

// "ne,ne,ne",
// "ne,ne,sw,sw",
// "ne,ne,s,s",
// "se,sw,se,sw,sw",
// broken: ne,nw,ne,nw,nw
// 898 is too low
const fs = require('fs');
const readline = require('readline');

const positions = 'n,ne,se,s,sw,nw'.split(',');

const convertPositionsToNum = input => input.map(e => positions.indexOf(e));
const convertNumsToPosition = input => input.map(e => positions[e]);

function* AcummIterator(input) {
  const inputLen = input.length;
  for (let i = 2; i < inputLen + 1; i += 1) {
    yield input.slice(0, i);
  }
}

const reducer = (nums) => {
  const numLen = nums.length;
  for (let i = 0; i < numLen; i += 1) {
    for (let j = i + 1; j < numLen; j += 1) {
      const p0 = nums[i];
      const p1 = nums[j];
      const diff = p1 - p0;
      switch (diff) {
        case 3:
        case -3:
          return [
            ...nums.slice(0, i),
            ...nums.slice(i + 1, j),
            ...nums.slice(j + 1, numLen)];
        case 2:
        case -4:
          return [
            (p0 + 1) % 6,
            ...nums.slice(0, i),
            ...nums.slice(i + 1, j),
            ...nums.slice(j + 1, numLen)];
        case -2:
        case 4:
          return [
            (p1 + 1) % 6,
            ...nums.slice(0, i),
            ...nums.slice(i + 1, j),
            ...nums.slice(j + 1, numLen)];
        default:
          break;
      }
    }
  }
  return nums;
};

const repeatingReducer = (input) => {
  let len = 0;
  let output = input;
  while (output.length !== len) {
    len = output.length;
    // console.log({pre: output});
    output = reducer(output);
    // console.log({post: output});
  }
  return output;
};

const rl = readline.createInterface({
  input: fs.createReadStream('./day11.input'), /*process.stdin*/
  crlfDelay: Infinity,
});

const accumMaxCount = (inputVals) => {
  let skipCount = 0;
  const inputCount = inputVals.length;
  let maxLen = 0;
  for (let item of AcummIterator(inputVals)) {
    if (skipCount > 0) {
      skipCount -= 1;
    } else {
      const output = repeatingReducer(item);
      console.log({inputCount, inputLen: item.length, distanceAway: output.length});
      if (maxLen <= output.length) {
        maxLen = output.length;
      } else {
        skipCount += maxLen - output.length;
      }
    }
  }
  return maxLen;
};

const sortAsc = (a, b) => a - b;
const sortDesc = (a, b) => b - a;

const findInputLengths = (input, searchLengths = [0], distances = new Map()) => {
  searchLengths.forEach((searchLen) => {
    if (!distances.has(searchLen)) {
      distances.set(searchLen, repeatingReducer(input.slice(0, searchLen)).length);
    }
  });
  return distances;
};

const accumer = (sortedKeyEntries, inputDistances) => sortedKeyEntries.reduce((accum, key) => {
  if (!accum.has('leftKey')) {
    accum.set('leftVal', inputDistances.get(key));
    accum.set('leftKey', key);
  } else if (!accum.has('rightKey')) {
    accum.set('rightVal', inputDistances.get(key));
    accum.set('rightKey', key);
  } else {
    const maxPossibleVal = Math.max(accum.get('leftVal'), accum.get('rightVal')) + ((accum.get('rightKey') - accum.get('leftKey')) / 2);
    if (!accum.has('maxPossibleVal') || accum.get('maxPossibleVal') < maxPossibleVal) {
      accum.set('maxPossibleVal', maxPossibleVal);
      accum.set('maxLeftKey', accum.get('leftKey'));
      accum.set('maxRightKey', accum.get('rightKey'));
      accum.delete('leftKey');
      accum.delete('rightKey');
    }
  }
  return accum;
}, new Map());

const findMaxInputLen = (input) => {
  // find next index lengths to search
  const inputDistances = findInputLengths(input, [1, input.length]);// [Math.floor(input.length * 0.3), Math.floor(input.length * 0.6)]);
  let sortedKeyEntries = [...inputDistances.keys()].sort(sortAsc);
  console.log({ inputDistances, sortedKeyEntries });
  findInputLengths(input, [sortedKeyEntries[0] + Math.floor((sortedKeyEntries[1] - sortedKeyEntries[0]) / 2)], inputDistances);
  sortedKeyEntries = [...inputDistances.keys()].sort(sortAsc);

  let nextEntry;
  let sortedValues;
  let nextIndex;
  for (let i = 1; i < 1000; i += 1) {
    nextEntry = accumer(sortedKeyEntries, inputDistances);
    nextIndex = [nextEntry.get('maxLeftKey') + Math.floor((nextEntry.get('maxRightKey') - nextEntry.get('maxLeftKey')) / 2)];
    findInputLengths(input, nextIndex, inputDistances);
    sortedKeyEntries = [...inputDistances.keys()].sort(sortAsc);
    sortedValues = [...inputDistances.values()].sort(sortDesc);
    console.log({ maxValue: sortedValues[0], nextIndex });
  }

  return 0;
};

rl.on('line', (input) => {
  const inputVals = convertPositionsToNum(input.split(','));
   const maxLen = accumMaxCount(inputVals);
  // const maxLen = findMaxInputLen(inputVals);
  console.log({ maxLen });
});
