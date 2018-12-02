/*
http://adventofcode.com/2017/day/20
Tried 702
*/

const fs = require('fs');
const readline = require('readline');
const sortIndexBy = require('lodash.sortedindexby');

const rl = readline.createInterface({
  input: fs.createReadStream('./day20input.txt'), // process.stdin
  crlfDelay: Infinity,
});

const inputRegEx = /p=<(-?\d+),(-?\d+),(-?\d+)>, v=<(-?\d+),(-?\d+),(-?\d+)>, a=<(-?\d+),(-?\d+),(-?\d+)>/;

let particles = [];

const particleSort = particle => Math.abs(particle.position.x) + Math.abs(particle.position.y) + Math.abs(particle.position.z);

const particleSortByLowestAccel = particle => Math.abs(particle.accel.x) + Math.abs(particle.accel.y) + Math.abs(particle.accel.z);

const addParticle = (particleArray, particle, sortFunct = particleSort) => {
  const index = sortIndexBy(particleArray, particle, sortFunct);
  particleArray.splice(index, 0, particle);
  return particleArray;
};

const removeDupsFromSortedArr = (pArr) => {
  let len = pArr.length;
  for (let i = 0; i < len; i += 1) {
    const leftItem = pArr[i];
    let matchedItem = false;
    while (i + 1 < len && leftItem.position.x === pArr[i + 1].position.x
      && leftItem.position.y === pArr[i + 1].position.y
      && leftItem.position.z === pArr[i + 1].position.z) {
      matchedItem = true;
      pArr.splice(i + 1, 1);
      len = pArr.length
    }
    if (matchedItem) {
      pArr.splice(i + 1, 1);
      len = pArr.length
      i -= 1;
    }
  }
};

const nextTick = oldTick => oldTick.reduce((newParticles, particle) => {
  const newVelocity = {
    x: particle.velocity.x + particle.accel.x,
    y: particle.velocity.y + particle.accel.y,
    z: particle.velocity.z + particle.accel.z,
  };
  addParticle(newParticles, {
    num: particle.num,
    accel: particle.accel,
    velocity: newVelocity,
    position: {
      x: particle.position.x + newVelocity.x,
      y: particle.position.y + newVelocity.y,
      z: particle.position.z + newVelocity.z,
    },
  });
  return newParticles;
}, []);

const particleReducer = particles => particles.reduce((diffs, particle, index, arr) => {
  if (index !== 0) {
    const previousParticle = arr[index - 1];
    diffs.push({
      accel: {
        x: particle.accel.x - previousParticle.accel.x,
        y: particle.accel.y - previousParticle.accel.y,
        z: particle.accel.z - previousParticle.accel.z,
      },
      velocity: {
        x: particle.velocity.x - previousParticle.velocity.x,
        y: particle.velocity.y - previousParticle.velocity.y,
        z: particle.velocity.z - previousParticle.velocity.z,
      },
      position: {
        x: particle.position.x - previousParticle.position.x,
        y: particle.position.y - previousParticle.position.y,
        z: particle.position.z - previousParticle.position.z,
      },
    });
  }
}, []);

let particleNum = 0;
rl.on('line', (input) => {
  if (input.length > 0) {
    const match = inputRegEx.exec(input);
    const p = {
      num: particleNum,
      accel: {
        x: parseInt(match[7], 10),
        y: parseInt(match[8], 10),
        z: parseInt(match[9], 10),
      },
      velocity: {
        x: parseInt(match[4], 10),
        y: parseInt(match[5], 10),
        z: parseInt(match[6], 10),
      },
      position: {
        x: parseInt(match[1], 10),
        y: parseInt(match[2], 10),
        z: parseInt(match[3], 10),
      },
    };
    addParticle(particles, p);
    particleNum += 1;
  } else {
    let knownLength = particles.length;
    for (let i = 0; i < 10000000; i += 1) {
      particles = nextTick(particles);
      removeDupsFromSortedArr(particles);
      if (knownLength !== particles.length) {
        console.log (`Length changed! ${knownLength} to ${particles.length}`);
        knownLength = particles.length;

        if (knownLength === 720) {
          const particleDiffs = particleDiffs(particles);
          console.log(particleDiffs);
        }
      }
    }
    //    console.log({closestParticleNum, cp: particles[0], cps: particleSortByLowestAccel(particles[0]), ncp: particles[1], ncps: particleSort(particles[1])});
  }
});
