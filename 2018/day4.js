const readline = require('readline');
const forEach = require('lodash.foreach');
const reduce = require('lodash.reduce');
const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

/*
[1518-11-01 00:00] Guard #10 begins shift
[1518-11-01 00:05] falls asleep
[1518-11-01 00:25] wakes up
*/

const pattern = /\[\d+-(\d+)-(\d+) (\d+):(\d+)\] (.*)/;
const guardPattern = /Guard #(\d+) begins shift/;

const processEntry = (input) => {
  const parts = pattern.exec(input);
  if (!parts) return null;
  const month = parseInt(parts[1], 10);
  const day = parseInt(parts[2], 10);
  const ord = month * 31 + day;
  let entry = {
    month,
    day,
    ord,
    hour: parseInt(parts[3], 10),
    minute: parseInt(parts[4], 10),
  };

  if (parts[5] === 'falls asleep') {
    entry.type = 'a'
  } else if (parts[5] === 'wakes up') {
    entry.type = 'w';
  } else {
    const guard = guardPattern.exec(parts[5]);
    entry.guard = parseInt(guard[1], 10);
    entry.type = 'g';
  }

  return entry;
};

let entries = {};
const addEntry = entry => {
  let dayLog = entries['' + entry.ord];
  if (!dayLog) {
    dayLog = {
      guard: -1,
      wakeTimes: [],
      asleepTimes: [],
      ord: entry.ord,
      month: entry.month,
      day: entry.day,
      start: 0,
      minutes: []
    }
  }

  switch(entry.type) {
    case 'a':
      dayLog.asleepTimes.push(entry.minute);
      dayLog.minutes.push(entry.minute);
      break;
    case 'w':
      dayLog.wakeTimes.push(entry.minute);
      dayLog.minutes.push(entry.minute);
      break;
    case 'g':
      dayLog.guard = entry.guard;
      if (entry.hour > 0) {
        dayLog.minutes.push(entry.hour === 0 ? entry.minute : 0);        
      }
      break;
  }

  entries['' + entry.ord] = dayLog;
};

const findPreviousGuard = day => {
  let entry = entries['' + day];
  while (!entry || entry.guard === -1) {
    entry = entries['' + (day--)];
  }
  return entry.guard;
}

const guardMostAsleep = guards => {
  return reduce(guards, (acc, guard) => {
    return guard.minutesAsleep > acc.minutesAsleep ? guard : acc;
  })
};

const evaluateEntries = () => {
  let guards = {};

  forEach(entries, (val, key) => {
    if (val.guard === -1) {
      val.guard = findPreviousGuard(key);
      val.minutes.push(0);
    }
    val.minutes.push(60);
    val.minutes = val.minutes.sort((a, b) => a - b);
    val.minutesAsleep = val.minutes.reduce((acc, val, index, arr) => {
      if (index % 2 === 0) return acc;
      return acc + val - arr[index-1];
    }, 0);

    let guard = guards['' + val.guard];
    if (!guard) {
      guard = {
        guard: val.guard,
        minutes: val.minutes,
        minutesAsleep: val.minutesAsleep
      }
    } else {
      guard.minutes = [...guard.minutes, ...val.minutes];
      guard.minutesAsleep = guard.minutesAsleep + val.minutesAsleep;
    }
    guards['' + val.guard] = guard;
  });

  const sleepyGuard = guardMostAsleep(guards);
  console.log(sleepyGuard);
};

rl.on('line', (input) => {
  const processedEntry = processEntry(input);
  if (processedEntry === null) {
    evaluateEntries();
    return;
  }

  addEntry(processedEntry);
});
