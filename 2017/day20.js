/*
http://adventofcode.com/2017/day/20
*/

const fs = require('fs');
const readline = require('readline');
const sortIndexBy = require('lodash.sortedindexby');

const rl = readline.createInterface({
  input: fs.createReadStream('./day20input.txt'), // process.stdin
  crlfDelay: Infinity,
});

// p=<1199,-2918,1457>, v=<-13,115,-8>, a=<-7,8,-10>
const inputRegEx = /p=<(-?\d+),(-?\d+),(-?\d+)>, v=<(-?\d+),(-?\d+),(-?\d+)>, a=<(-?\d+),(-?\d+),(-?\d+)>/;

let particles = [];

const particleSort = particle => Math.abs(particle.position.x) + Math.abs(particle.position.y) + Math.abs(particle.position.z);

const particleSortByLowestAccel = particle => Math.abs(particle.accel.x) + Math.abs(particle.accel.y) + Math.abs(particle.accel.z);

const addParticle = (particleArray, particle, sortFunct = particleSortByLowestAccel /*particleSort*/) => {
  const index = sortIndexBy(particleArray, particle, sortFunct);
  particleArray.splice(index, 0, particle);
  return particleArray;
}

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

let particleNum = 0;
rl.on('line', (input) => {
  if (input.length > 0) {
    const match = inputRegEx.exec(input);
    let p = {
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

    let closestParticleNum = particles[0].num;
    let closestCount = 0;
    while (closestCount < 10000) {
        particles = nextTick(particles);
        if (closestParticleNum === particles[0].num) {
            closestCount+=1;
        } else {
            closestParticleNum = particles[0].num;
            closestCount = 0;
        }
    }
    console.log(particles[0]);
    //    console.log({closestParticleNum, cp: particles[0], cps: particleSortByLowestAccel(particles[0]), ncp: particles[1], ncps: particleSort(particles[1])});
  }
});
