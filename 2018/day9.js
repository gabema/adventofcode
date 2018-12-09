const games = [
  {
    numPlayers: 9,
    lastMarble: 25
  },
  // {
  //   numPlayers: 10,
  //   lastMarble: 1618
  // },
  // {
  //   numPlayers: 13,
  //   lastMarble: 7999
  // },
  // {
  //   numPlayers: 17,
  //   lastMarble: 1104,
  // },
  // {
  //   numPlayers: 21,
  //   lastMarble: 6111,
  // },
  // {
  //   numPlayers: 30,
  //   lastMarble: 5807,
  // },
  // {
  //   numPlayers: 439,
  //   lastMarble: 71307,
  // },
  // {
  //   numPlayers: 439,
  //   lastMarble: 7130700,
  // },
];

const newGame = numPlayers => ({
  board: [0],
  currentMarbleIndex: 0,
  currentPlayer: -1,
  playerScores: new Array(numPlayers),
  nextMarble: 1,
});

const renderBoard = game => {
  const currentPlayer = game.currentPlayer;
  const marbles = game.board.reduce((marbles, current, ind) => {
    return marbles + (ind === game.currentMarbleIndex ? '(' + current + ')' : current) + ' ';
  }, '');
  console.log(`[${currentPlayer+1}]  ${marbles}`);
};

const marblesBefore = (board = [], index) => {
  return board.slice(0, index);
};

const marblesAfter = (board = [], index = 0) => {
  return board.slice(index);
};

const nextMarbleIndex = (board = [], index = 0) => {
  const newIndex = (index % board.length) + 1;
  return newIndex; 
}

const prevMarbleIndex = (board = [], index = 0) => {
  return index - 1 >= 0 ? index - 1 : board.length - 1;
}

const sevenPrev = (board, index) =>
  prevMarbleIndex(board,
    prevMarbleIndex(board,
      prevMarbleIndex(board,
        prevMarbleIndex(board,
          prevMarbleIndex(board,
            prevMarbleIndex(board,
              prevMarbleIndex(board, index)))))));

const playMarble = game => {
  const newMarble = game.nextMarble;
  game.currentPlayer = (game.currentPlayer + 1) % game.playerScores.length;
  if (newMarble % 23 === 0) {
    const playerScore = (game.playerScores[game.currentPlayer] || 0) + newMarble;
    const originalIndex = game.currentMarbleIndex;
    const sevenMarbleIndex = sevenPrev(game.board, originalIndex);
    game.playerScores[game.currentPlayer] = playerScore + game.board[sevenMarbleIndex];
    const nextIndex = nextMarbleIndex(game.board, sevenMarbleIndex);
    game.board = [...marblesBefore(game.board, sevenMarbleIndex), ...marblesAfter(game.board, nextIndex)];
    game.currentMarbleIndex = sevenMarbleIndex;
  } else {
    const firstMarbleIndex = nextMarbleIndex(game.board, game.currentMarbleIndex);
    const newMarbleIndex = nextMarbleIndex(game.board, firstMarbleIndex);  
    game.board = [...marblesBefore(game.board, newMarbleIndex), newMarble, ...marblesAfter(game.board, newMarbleIndex)];
    game.currentMarbleIndex = newMarbleIndex;
  }
  game.nextMarble++;
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
  let game = newGame(gameInfo.numPlayers);
  while(gameInfo.lastMarble >= game.nextMarble) {
    game = playMarble(game);
  //  renderBoard(game);
  }
  console.log(highestScore(game));
});
