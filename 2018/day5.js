
const sampleInput = 'dabAcCaCBAcCcaDA';

const createPolymer = (polymerVal = '') => {
  const polymer = polymerVal.toLocaleLowerCase();
  const isPositive = polymer === polymerVal;
  return {
    polymer,
    isPositive
  };
}

let newPolymerChain = () => ({
  start: null,
  end: null,
});

const addToPolymerChainWithReduction = (chain, polymer) => {
  let polymerLink = {
    polymer,
    next: null,
    prev: null,
  };
  if (chain.start === null && chain.end === null) {
    chain.start = chain.end = polymerLink;
  } else {
    if (chain.end.polymer.polymer === polymerLink.polymer.polymer && chain.end.polymer.isPositive !== polymerLink.polymer.isPositive) {
      let previous = chain.end.prev;
      chain.end.prev = null;
      previous.next = null;
      chain.end = previous;
    } else {
      chain.end.next = polymerLink;
      chain.end = polymerLink;  
    }
  }
};

const renderChain = chain => {
  let chainStr = '';
  let cur = chain.start;
  while (cur !== null) {
    chainStr += cur.polymer.polymer;
    cur = cur.polymer.next;
  }
  console.log(chainStr);
};

let chain = newPolymerChain();
addToPolymerChainWithReduction(chain, createPolymer('d'));
addToPolymerChainWithReduction(chain, createPolymer('a'));
addToPolymerChainWithReduction(chain, createPolymer('b'));
addToPolymerChainWithReduction(chain, createPolymer('A'));
addToPolymerChainWithReduction(chain, createPolymer('c'));
addToPolymerChainWithReduction(chain, createPolymer('C'));
addToPolymerChainWithReduction(chain, createPolymer('a'));
addToPolymerChainWithReduction(chain, createPolymer('C'));
addToPolymerChainWithReduction(chain, createPolymer('B'));
addToPolymerChainWithReduction(chain, createPolymer('A'));
addToPolymerChainWithReduction(chain, createPolymer('c'));
addToPolymerChainWithReduction(chain, createPolymer('C'));
addToPolymerChainWithReduction(chain, createPolymer('c'));
addToPolymerChainWithReduction(chain, createPolymer('a'));
addToPolymerChainWithReduction(chain, createPolymer('D'));
addToPolymerChainWithReduction(chain, createPolymer('A'));
renderChain(chain);