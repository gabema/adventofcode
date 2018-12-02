/*
http://adventofcode.com/2017/day/11

ne,ne,ne is 3 steps away.
ne,ne,sw,sw is 0 steps away (back where you started).
ne,ne,s,s is 2 steps away (se,se).
se,sw,se,sw,sw is 3 steps away (s,s,sw).

private leaderboard: http://adventofcode.com/2017/leaderboard/private/view/52925
*/

const fs = require('fs');
const readline = require('readline');

const toRadians = angle => angle * (Math.PI / 180);

const size1 = 1;
const size2 = Math.cos(toRadians(45)) / size1;

const moveMap = {
  move_n: pt => ({
    x: (pt.x + (2 * size1)),
    y: pt.y,
  }),
  move_ne: pt => ({
    x: pt.x + (2 * size2),
    y: pt.y + (2 * size2),
  }),
  move_se: pt => ({
    x: pt.x + (2 * size2),
    y: pt.y - (2 * size2),
  }),
  move_s: pt => ({
    x: pt.x,
    y: pt.y - (2 * size1),
  }),
  move_sw: pt => ({
    x: pt.x - (2 * size2),
    y: pt.y - (2 * size2),
  }),
  move_nw: pt => ({
    x: pt.x - (2 * size2),
    y: pt.y + (2 * size2),
  }),
};

const rl = readline.createInterface({
  input: fs.createReadStream('./day11.input'), // process.stdin
  crlfDelay: Infinity,
});

const distanceAway = (moves) => {
  let pt = { x: 0, y: 0 };
  moves.forEach((move) => {
    pt = moveMap[`move_${move}`](pt);
  });
  return pt;
};

const diffPts = (a, b) => ({ x: a.x - b.x, y: a.y - b.y });

const VERY_SMALL_NUMBER = 0.00000000001;
const nearlyEqual = (a, b) => Math.abs(a - b) < VERY_SMALL_NUMBER;

const shortestPathToLocation = (destination) => {
  let pt = { x: 0, y: 0 };
  let diff = diffPts(destination, pt);
  const path = [];
  while (!nearlyEqual(diff.x, 0) && !nearlyEqual(diff.y, 0)) {
    if (nearlyEqual(diff.x, 0) && diff.y > 0) {
      path.push('n');
      pt = moveMap.move_n(pt);
    } else if (nearlyEqual(diff.x, 0) && diff.y < 0) {
      path.push('s');
      pt = moveMap.move_s(pt);
    } else if (diff.x > 0 && diff.y > 0) {
      path.push('ne');
      pt = moveMap.move_ne(pt);
    } else if (diff.x < 0 && diff.y > 0) {
      path.push('nw');
      pt = moveMap.move_nw(pt);
    } else if (diff.x > 0 && diff.y < 0) {
      path.push('se');
      pt = moveMap.move_se(pt);
    } else if (diff.x < 0 && diff.y < 0) {
      path.push('sw');
      pt = moveMap.move_sw(pt);
    } else {
      throw new Error('Bad state!');
    }
    diff = diffPts(destination, pt);
  }

  return path;
};

rl.on('line', (input) => {
  const moves = input.split(',');
  const distance = distanceAway(moves);
  const shortestPath = shortestPathToLocation(distance);
  console.log(distance, shortestPath.length);
  if (shortestPath.length < 10) {
    console.log(shortestPath);
  }
});
