using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public class HyperCube
    {
        private HashSet<(int X, int Y, int Z, int W)> activePositions;
        public HyperCube()
        {
            activePositions = new HashSet<(int X, int Y, int Z, int W)>();
        }

        public void AddActivePosition((int X, int Y, int Z, int W) position)
        {
            activePositions.Add(position);
        }

        public int ActiveCubes { get => activePositions.Count; }

        internal void NextCycle()
        {
            HashSet<(int X, int Y, int Z, int W)> checkPositions = new HashSet<(int X, int Y, int Z, int W)>(activePositions);
            HashSet<(int X, int Y, int Z, int W)> newActiveCubes = new HashSet<(int X, int Y, int Z, int W)>(activePositions);
            foreach (var cube in activePositions)
            {
                checkPositions.UnionWith(AdjacentCubes(cube));
            }

            foreach (var position in checkPositions)
            {
                bool activePosition = activePositions.Contains(position);
                int activeNeighborCount = 0;
                foreach (var neighborPosition in AdjacentCubes(position))
                {
                    if (activePositions.Contains(neighborPosition)) activeNeighborCount++;
                    if (activeNeighborCount > 3) break;
                }
                if (activePosition)
                {
                    if (activeNeighborCount != 2 && activeNeighborCount != 3)
                    {
                        newActiveCubes.Remove(position);
                    }
                }
                else
                {
                    // Not currently active position
                    if (activeNeighborCount == 3)
                    {
                        newActiveCubes.Add(position);
                    }
                }
            }

            activePositions = newActiveCubes;
        }

        private IEnumerable<(int X, int Y, int Z, int W)> AdjacentCubes((int X, int Y, int Z, int W) position)
        {
            for (int xDiff = -1; xDiff < 2; xDiff++)
            {
                for (int yDiff = -1; yDiff < 2; yDiff++)
                {
                    for (int zDiff = -1; zDiff < 2; zDiff++)
                    {
                        for (int wDiff = -1; wDiff < 2; wDiff++)
                        {
                            if (xDiff == 0 && yDiff == 0 && zDiff == 0 && wDiff == 0) continue;
                            yield return (position.X + xDiff, position.Y + yDiff, position.Z + zDiff, position.W + wDiff);
                        }
                    }
                }
            }
        }
    }

    public class Cube
    {
        private HashSet<(int X, int Y, int Z)> activeCubes;
        public Cube()
        {
            activeCubes = new HashSet<(int X, int Y, int Z)>();
        }

        public void AddActivePosition((int X, int Y, int Z) position)
        {
            activeCubes.Add(position);
        }

        public int ActiveCubes { get => activeCubes.Count; }

        internal void NextCycle()
        {
            HashSet<(int X, int Y, int Z)> checkPositions = new HashSet<(int X, int Y, int Z)>(activeCubes);
            HashSet<(int X, int Y, int Z)> newActiveCubes = new HashSet<(int X, int Y, int Z)>(activeCubes);
            foreach (var cube in activeCubes)
            {
                checkPositions.UnionWith(AdjacentCubes(cube));
            }

            foreach(var position in checkPositions)
            {
                bool activePosition = activeCubes.Contains(position);
                int activeNeighborCount = 0;
                foreach(var neighborPosition in AdjacentCubes(position)) {
                    if (activeCubes.Contains(neighborPosition)) activeNeighborCount++;
                    if (activeNeighborCount > 3) break;
                }
                if (activePosition) {
                    if (activeNeighborCount != 2 && activeNeighborCount != 3)
                    {
                        newActiveCubes.Remove(position);
                    }
                }
                else
                {
                    // Not currently active position
                    if (activeNeighborCount == 3)
                    {
                        newActiveCubes.Add(position);
                    }
                }
            }

            activeCubes = newActiveCubes;
        }

        private IEnumerable<(int X, int Y, int Z)> AdjacentCubes((int X, int Y, int Z) position)
        {
            for (int xDiff = -1; xDiff < 2; xDiff++)
            {
                for (int yDiff = -1; yDiff < 2; yDiff++)
                {
                    for (int zDiff = -1; zDiff < 2; zDiff++)
                    {
                        if (xDiff == 0 && yDiff == 0 && zDiff == 0) continue;
                        yield return (position.X + xDiff, position.Y + yDiff, position.Z + zDiff);
                    }
                }
            }
        }
    }

    // https://adventofcode.com/2020/day/18
    public class Day17
    {
        private ITestOutputHelper output;

        private HyperCube CreateHyperCube()
        {
            var cube = new HyperCube();
            //cube.AddActivePosition((0, 0, 0, 0));
            //cube.AddActivePosition((1, 0, 0, 0));
            //cube.AddActivePosition((2, 0, 0, 0));
            //cube.AddActivePosition((2, 1, 0, 0));
            //cube.AddActivePosition((1, 2, 0, 0));

            cube.AddActivePosition((0, 0, 0, 0));
            cube.AddActivePosition((1, 0, 0, 0));
            cube.AddActivePosition((2, 0, 0, 0));
            cube.AddActivePosition((5, 0, 0, 0));
            cube.AddActivePosition((6, 0, 0, 0));
            cube.AddActivePosition((3, 1, 0, 0));
            cube.AddActivePosition((6, 1, 0, 0));
            cube.AddActivePosition((7, 1, 0, 0));
            cube.AddActivePosition((1, 2, 0, 0));
            cube.AddActivePosition((2, 2, 0, 0));
            cube.AddActivePosition((4, 2, 0, 0));
            cube.AddActivePosition((7, 2, 0, 0));
            cube.AddActivePosition((3, 3, 0, 0));
            cube.AddActivePosition((5, 3, 0, 0));
            cube.AddActivePosition((7, 3, 0, 0));
            cube.AddActivePosition((1, 4, 0, 0));
            cube.AddActivePosition((2, 4, 0, 0));
            cube.AddActivePosition((4, 4, 0, 0));
            cube.AddActivePosition((5, 4, 0, 0));
            cube.AddActivePosition((0, 5, 0, 0));
            cube.AddActivePosition((1, 5, 0, 0));
            cube.AddActivePosition((5, 5, 0, 0));
            cube.AddActivePosition((7, 5, 0, 0));
            cube.AddActivePosition((0, 6, 0, 0));
            cube.AddActivePosition((4, 6, 0, 0));
            cube.AddActivePosition((5, 6, 0, 0));
            cube.AddActivePosition((7, 6, 0, 0));
            cube.AddActivePosition((1, 7, 0, 0));
            cube.AddActivePosition((2, 7, 0, 0));
            cube.AddActivePosition((5, 7, 0, 0));
            cube.AddActivePosition((7, 7, 0, 0));
            return cube;
        }

        private Cube CreateCube()
        {
            var cube = new Cube();
            //cube.AddActivePosition((0, 0, 0));
            //cube.AddActivePosition((1, 0, 0));
            //cube.AddActivePosition((2, 0, 0));
            //cube.AddActivePosition((2, 1, 0));
            //cube.AddActivePosition((1, 2, 0));

            /*
.##..#.#
#...##.#
##...#.#
.##.##..
...#.#.#
.##.#..#
...#..##
###..##.
             */
            cube.AddActivePosition((0, 0, 0));
            cube.AddActivePosition((1, 0, 0));
            cube.AddActivePosition((2, 0, 0));
            cube.AddActivePosition((5, 0, 0));
            cube.AddActivePosition((6, 0, 0));
            cube.AddActivePosition((3, 1, 0));
            cube.AddActivePosition((6, 1, 0));
            cube.AddActivePosition((7, 1, 0));
            cube.AddActivePosition((1, 2, 0));
            cube.AddActivePosition((2, 2, 0));
            cube.AddActivePosition((4, 2, 0));
            cube.AddActivePosition((7, 2, 0));
            cube.AddActivePosition((3, 3, 0));
            cube.AddActivePosition((5, 3, 0));
            cube.AddActivePosition((7, 3, 0));
            cube.AddActivePosition((1, 4, 0));
            cube.AddActivePosition((2, 4, 0));
            cube.AddActivePosition((4, 4, 0));
            cube.AddActivePosition((5, 4, 0));
            cube.AddActivePosition((0, 5, 0));
            cube.AddActivePosition((1, 5, 0));
            cube.AddActivePosition((5, 5, 0));
            cube.AddActivePosition((7, 5, 0));
            cube.AddActivePosition((0, 6, 0));
            cube.AddActivePosition((4, 6, 0));
            cube.AddActivePosition((5, 6, 0));
            cube.AddActivePosition((7, 6, 0));
            cube.AddActivePosition((1, 7, 0));
            cube.AddActivePosition((2, 7, 0));
            cube.AddActivePosition((5, 7, 0));
            cube.AddActivePosition((7, 7, 0));

            return cube;
        }

        public Day17(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Part1()
        {
            var cube = CreateCube();
            cube.NextCycle();
            cube.NextCycle();
            cube.NextCycle();
            cube.NextCycle();
            cube.NextCycle();
            cube.NextCycle();
            Assert.Equal(359, cube.ActiveCubes);
        }

        [Fact]
        public void Part2()
        {
            var cube = CreateHyperCube();
            cube.NextCycle();
            cube.NextCycle();
            cube.NextCycle();
            cube.NextCycle();
            cube.NextCycle();
            cube.NextCycle();
            Assert.Equal(2228, cube.ActiveCubes);

        }
    }
}
