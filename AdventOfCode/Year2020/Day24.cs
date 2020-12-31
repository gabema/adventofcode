using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public enum TileToken
    {
        NE = TileDirection.NE,
        NW = TileDirection.NW,
        SE = TileDirection.SE,
        SW = TileDirection.SW,
        E = TileDirection.E,
        W = TileDirection.W,
        TERMINATOR
    }

    public class TileLexer : SimpleLexer<TileToken>
    {
        private static readonly List<Tuple<TileToken, string>> TOKENS = new List<Tuple<TileToken, string>> {
            Tuple.Create(TileToken.NE, "^ne"),
            Tuple.Create(TileToken.NW, "^nw"),
            Tuple.Create(TileToken.SE, "^se"),
            Tuple.Create(TileToken.SW, "^sw"),
            Tuple.Create(TileToken.E, "^e"),
            Tuple.Create(TileToken.W, "^w"),
            Tuple.Create(TileToken.TERMINATOR, ""),
        };

        public TileLexer() : base(TOKENS) { }
    }

    public enum TileColor
    {
        White = default,
        Black
    }

    public enum TileDirection
    {
        E,
        NE,
        NW,
        SE,
        SW,
        W,
    }

    public class Tile
    {
        private static readonly double HORIZONTAL_X = 1D;
        private static readonly double DIAGNOL_X = HORIZONTAL_X / 2;
        private static readonly double DIAGNOL_Y = HORIZONTAL_X / 2 + HORIZONTAL_X / 2;

        public Tile((double X, double Y) position)
        {
            Position = position;
            Color = TileColor.White;
        }

        public TileColor Color { get; private set; }
        public (double X, double Y) Position { get; }
        public void Flip()
        {
            Color = Color == TileColor.White ? TileColor.Black : TileColor.White;
        }

        public static (double X, double Y) SE_Position((double X, double Y) Position) => (Position.X + DIAGNOL_X, Position.Y - DIAGNOL_Y);
        public static (double X, double Y) SW_Position((double X, double Y) Position) => (Position.X - DIAGNOL_X, Position.Y - DIAGNOL_Y);
        public static (double X, double Y) NE_Position((double X, double Y) Position) => (Position.X + DIAGNOL_X, Position.Y + DIAGNOL_Y);
        public static (double X, double Y) NW_Position((double X, double Y) Position) => (Position.X - DIAGNOL_X, Position.Y + DIAGNOL_Y);
        public static (double X, double Y) E_Position((double X, double Y) Position) => (Position.X + HORIZONTAL_X, Position.Y);
        public static (double X, double Y) W_Position((double X, double Y) Position) => (Position.X - HORIZONTAL_X, Position.Y);
    }

    public class TileGrid
    {
        private Dictionary<(double X, double Y), Tile> tiles;
        private Tile referenceTile;

        public TileGrid()
        {
            referenceTile = new Tile((0, 0));

            tiles = new Dictionary<(double X, double Y), Tile>();
            tiles[referenceTile.Position] = referenceTile;
        }

        public void FlipTile(IEnumerable<TileDirection> directions)
        {
            (double X, double Y) location = referenceTile.Position;
            foreach(var direction in directions)
            {
                switch (direction)
                {
                    case TileDirection.E:
                        location = Tile.E_Position(location);
                        break;
                    case TileDirection.W:
                        location = Tile.W_Position(location);
                        break;
                    case TileDirection.NE:
                        location = Tile.NE_Position(location);
                        break;
                    case TileDirection.NW:
                        location = Tile.NW_Position(location);
                        break;
                    case TileDirection.SE:
                        location = Tile.SE_Position(location);
                        break;
                    case TileDirection.SW:
                        location = Tile.SW_Position(location);
                        break;
                }
            }

            if (location == referenceTile.Position)
            {
                referenceTile.Flip();
            }
            else if (!tiles.Remove(location))
            {
                // Add a black tile
                Tile blackTile = new Tile(location);
                blackTile.Flip();
                tiles.Add(blackTile.Position, blackTile);
            }
            // else removed what would have become a white tile
        }

        public void FlipAllTiles()
        {
            HashSet<(double X, double Y)> locationsToCheck = tiles.Keys.ToHashSet();
            foreach (var tile in tiles.Values)
            {
                locationsToCheck.UnionWith(AdjacentTiles(tile.Position).Select(t => t.Position));
            }

            HashSet<Tile> tilesToBeFlipped = new HashSet<Tile> { };
            foreach (var location in locationsToCheck)
            {
                int blackTileCount = AdjacentTiles(location).Where(t => t.Color == TileColor.Black).Count();
                Tile tile = default;
                if (!tiles.TryGetValue(location, out tile))
                {
                    tile = new Tile(location);
                }

                if (tile.Color == TileColor.Black && (blackTileCount == 0 || blackTileCount > 2)) tilesToBeFlipped.Add(tile);
                else if (tile.Color == TileColor.White && blackTileCount == 2) tilesToBeFlipped.Add(tile);
            }
            foreach(var tile in tilesToBeFlipped)
            {
                tile.Flip();
                tiles.Remove(tile.Position);
                if (tile.Color == TileColor.Black)
                {
                    tiles.Add(tile.Position, tile);
                }
            }
        }

        private IEnumerable<Tile> AdjacentTiles((double X, double Y) position)
        {
            Tile adTile;

            yield return tiles.TryGetValue(Tile.E_Position(position), out adTile) ? adTile : new Tile(Tile.E_Position(position));
            yield return tiles.TryGetValue(Tile.NE_Position(position), out adTile) ? adTile : new Tile(Tile.NE_Position(position));
            yield return tiles.TryGetValue(Tile.SE_Position(position), out adTile) ? adTile : new Tile(Tile.SE_Position(position));
            yield return tiles.TryGetValue(Tile.W_Position(position), out adTile) ? adTile : new Tile(Tile.W_Position(position));
            yield return tiles.TryGetValue(Tile.NW_Position(position), out adTile) ? adTile : new Tile(Tile.NW_Position(position));
            yield return tiles.TryGetValue(Tile.SW_Position(position), out adTile) ? adTile : new Tile(Tile.SW_Position(position));
        }

        public int BlackTileCount() => tiles.Values.Where(t => t.Color == TileColor.Black).Count();
    }

    // https://adventofcode.com/2020/day/24
    public class Day24
    {
        private ITestOutputHelper output;

        private TileGrid ReadGrid()
        {
            var grid = new TileGrid();
            using var inputReader = new StreamReader(InputClient.GetFileStream(2020, 24, ""));
            var lexer = new TileLexer();
            string directions;
            while((directions = inputReader.ReadLine()) != null)
            {
                grid.FlipTile(lexer.Tokenize(directions).Where(tt => tt.TokenType != TileToken.TERMINATOR).Select(tt => (TileDirection)tt.TokenType));
            }
            return grid;
        }

        public Day24(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Part1()
        {
            var grid = ReadGrid();
            Assert.Equal(459, grid.BlackTileCount());
        }

        [Fact]
        public void Part2()
        {
            var grid = ReadGrid();
            int[] checkpoints = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            for (var i = 1; i <= 100; i++)
            {
                grid.FlipAllTiles();
                if (checkpoints.Contains(i))
                {
                    output.WriteLine($"Day {i}: {grid.BlackTileCount()}");
                }
            }
            Assert.Equal(4150, grid.BlackTileCount());
        }
    }
}
