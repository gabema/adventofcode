/*
--- Day 7: The Sum of Its Parts ---
As you're about to begin construction, four of the Elves offer to help. "The sun will set soon; it'll go faster if we work together." Now, you need to account for multiple people working on steps simultaneously. If multiple steps are available, workers should still begin them in alphabetical order.

Each step takes 60 seconds plus an amount corresponding to its letter: A=1, B=2, C=3, and so on. So, step A takes 60+1=61 seconds, while step Z takes 60+26=86 seconds. No time is required between steps.

To simplify things for the example, however, suppose you only have help from one Elf (a total of two workers) and that each step takes 60 fewer seconds (so that step A takes 1 second and step Z takes 26 seconds). Then, using the same instructions as above, this is how each second would be spent:

Second   Worker 1   Worker 2   Done
   0        C          .        
   1        C          .        
   2        C          .        
   3        A          F       C
   4        B          F       CA
   5        B          F       CA
   6        D          F       CAB
   7        D          F       CAB
   8        D          F       CAB
   9        D          .       CABF
  10        E          .       CABFD
  11        E          .       CABFD
  12        E          .       CABFD
  13        E          .       CABFD
  14        E          .       CABFD
  15        .          .       CABFDE
Each row represents one second of time. The Second column identifies how many seconds have passed as of the beginning of that second. Each worker column shows the step that worker is currently doing (or . if they are idle). The Done column shows completed steps.

Note that the order of the steps has changed; this is because steps now take time to finish and multiple workers can begin multiple steps simultaneously.

In this example, it would take 15 seconds for two workers to complete these steps.

With 5 workers and the 60+ second step durations described above, how long will it take to complete all of the steps?
*/
const readline = require('readline');
const rl = readline.createInterface({
  input: require('fs').createReadStream('day7input.txt'),
  console: false
});

const pattern = /Step (\w) must be finished before step (\w) can begin/;

const indexOfChar = char => char.charCodeAt(0) - 'A'.charCodeAt(0);

const processEntry = (input) => {
  const parts = pattern.exec(input);
  if (!parts) return null;
  return {
    step: parts[1],
    dep: parts[2]
  };
};

const STATUS_NOTSTARTED = 0;
const STATUS_INPROGRESS = 1;
const STATUS_COMPLETED = 2;
//const TIMETOCOMPLETE = 0;
const TIMETOCOMPLETE = 60;
//const WORKERS = [-1, -1];
const WORKERS = [-1, -1, -1, -1, -1];

let entries = new Array(26);
const addEntry = newEntry => {
  const index = indexOfChar(newEntry.step);
  const depIndex = indexOfChar(newEntry.dep);

  let entry = entries[index] || {
    step: newEntry.step,
    deps: [],
    status: STATUS_NOTSTARTED,
    timeRemaining: TIMETOCOMPLETE + index + 1,
  };

  let depEntry = entries[depIndex] || {
    step: newEntry.dep,
    deps: [],
    status: STATUS_NOTSTARTED,
    timeRemaining: TIMETOCOMPLETE + depIndex + 1,
  };
  depEntry.deps = [...depEntry.deps, index];
  entries[index] = entry;
  entries[depIndex] = depEntry;
};

const depsComplete = deps => {
  return deps.reduce((completed, dep) => completed && entries[dep].status === STATUS_COMPLETED, true);
};

const nextAvailable = entries => {
  return entries.reduce((next, entry) => {
    if (next === null && entry && entry.status === STATUS_NOTSTARTED && depsComplete(entry.deps))
      return entry
    else
      return next;
  }, null);
};

const entriesCompleted = entries => {
  return entries.reduce((completed, entry) => completed && entry.status === STATUS_COMPLETED, true);
}

const evaluateEntries = () => {
  let nextEntry = null;
  let completionInfo = {
    workers: WORKERS,
    clock: 0,
    completedOrder: [],
  };

  while (!entriesCompleted(entries)) {
    // assign workers
    completionInfo.workers = completionInfo.workers.reduce((arr, workerIndex) => {
      if (workerIndex === -1) {
       let nextEntry = nextAvailable(entries);
       if (nextEntry != null) {
        workerIndex = indexOfChar(nextEntry.step);
        nextEntry.status = STATUS_INPROGRESS;
       }
      }
      arr.push(workerIndex);
      return arr;
    }, []);

    completionInfo.workers.forEach(workerIndex => {
      if (workerIndex >= 0) {
        let entry = entries[workerIndex];
        entry.timeRemaining--;
      }
    });

    let workerStatus = completionInfo.workers.reduce((str, worker) => {
      return str + '    ' + ((worker !== -1) ? String.fromCharCode('A'.charCodeAt(0) + worker) : '.');
    }, '');

    // unassign workers
    completionInfo.workers = completionInfo.workers.reduce((arr, workerIndex) => {
      if (workerIndex !== -1) {
        let prevEntry = entries[workerIndex];
        if (prevEntry.timeRemaining <= 0) {
          prevEntry.status = STATUS_COMPLETED;
          workerIndex = -1;
        }
      }
      arr.push(workerIndex);
      return arr;
    }, []);
    let completedOrderStr = completionInfo.completedOrder.reduce((order, entry) => order + entry, '');
    console.log(`${completionInfo.clock}   ${workerStatus}  ${completedOrderStr}`);
    completionInfo.clock++;
  }
  console.log(completionInfo.clock);
  console.log(completionInfo.completedOrder.reduce((order, entry) => order + entry, ''));
};

rl.on('line', (input) => {
  const processedEntry = processEntry(input);
  if (processedEntry === null) {
    evaluateEntries();
    return;
  }

  addEntry(processedEntry);
});

//887 is too high
rl.on('end', () => {
  evaluateEntries();
});
