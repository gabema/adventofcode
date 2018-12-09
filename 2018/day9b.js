const games = [
  {
    numPlayers: 9,
    lastMarble: 25
  },
  {
    numPlayers: 10,
    lastMarble: 1618
  },
  {
    numPlayers: 13,
    lastMarble: 7999
  },
  {
    numPlayers: 17,
    lastMarble: 1104,
  },
  {
    numPlayers: 21,
    lastMarble: 6111,
  },
  {
    numPlayers: 30,
    lastMarble: 5807,
  },
  {
    numPlayers: 439,
    lastMarble: 71307,
  },
  {
    numPlayers: 439,
    lastMarble: 7130700,
  },
];

const newMarble = number => ({
  val: number,
  next: null,
  prev: null,
});

const newGame = (numPlayers, rootMarble) => ({
  currentMarble: rootMarble,
  rootMarble,
  currentPlayer: -1,
  playerScores: new Array(numPlayers),
  nextMarbleNumber: rootMarble.val + 1,
});

const nextClockwiseMarble = marble => marble.next;
const nextCounterClockwiseMarble = marble => marble.prev;

const renderBoard = game => {
  const currentPlayer = game.currentPlayer;
  const rootMarble = game.rootMarble;
  const currentMarble = game.currentMarble;
  let marbles = '' + (currentMarble === rootMarble ? '(' + rootMarble.val + ')' : rootMarble.val) + ' ';
  let nextMarble = nextClockwiseMarble(rootMarble);
  while (nextMarble !== rootMarble) {
    marbles = marbles + (currentMarble === nextMarble ? '(' + nextMarble.val + ')' : nextMarble.val) + ' ';
    nextMarble = nextClockwiseMarble(nextMarble);
  }
  console.log(`[${currentPlayer+1}]  ${marbles}`);
};

const sevenCounterClockwiseMarble = marble =>
  nextCounterClockwiseMarble(
    nextCounterClockwiseMarble(
      nextCounterClockwiseMarble(
        nextCounterClockwiseMarble(
          nextCounterClockwiseMarble(
            nextCounterClockwiseMarble(
              nextCounterClockwiseMarble(marble)))))));

const addMarbleClockwise = (marble, addMarble) => {
  let marbleNext = marble.next;
  addMarble.prev = marble;
  addMarble.next = marbleNext;
  marble.next = addMarble;
  marbleNext.prev = addMarble;
}

const removeMarble = marble => {
  let marbleNext = marble.next;
  let marblePrev = marble.prev;
  marbleNext.prev = marblePrev;
  marblePrev.next = marbleNext;
}

const playMarble = game => {
  const nextMarble = newMarble(game.nextMarbleNumber);
  game.currentPlayer = (game.currentPlayer + 1) % game.playerScores.length;
  if (nextMarble.val % 23 === 0) {
    const playerScore = (game.playerScores[game.currentPlayer] || 0) + nextMarble.val;
    const currentMarble = game.currentMarble;
    const sevenMarble = sevenCounterClockwiseMarble(currentMarble);
    game.playerScores[game.currentPlayer] = playerScore + sevenMarble.val;
    const marbleAfterSeven = nextClockwiseMarble(sevenMarble);
    removeMarble(sevenMarble);
    game.currentMarble = marbleAfterSeven;
  } else {
    const afterMarble = nextClockwiseMarble(game.currentMarble);
    addMarbleClockwise(afterMarble, nextMarble);
    game.currentMarble = nextMarble;
  }
  game.nextMarbleNumber++;
  return game;
};

const highestScore = game => {
  return game.playerScores.reduce((highestPlayer, player, playerIndex) => {
    if (highestPlayer === null) return {player: playerIndex+1, playerScore: player};
    if (highestPlayer.playerScore < player) return {player: playerIndex+1, playerScore: player};
    return highestPlayer;
  }, null);
}

games.forEach(gameInfo => {
  let rootMarble = newMarble(0);
  rootMarble.next = rootMarble;
  rootMarble.prev = rootMarble;
  let game = newGame(gameInfo.numPlayers, rootMarble);
  renderBoard(game);
  while(gameInfo.lastMarble >= game.nextMarbleNumber) {
    game = playMarble(game);
    // renderBoard(game);
  }
  console.log(highestScore(game));
});
