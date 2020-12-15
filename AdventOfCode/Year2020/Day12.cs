using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public static class NumericExtensions
    {
        public static double ToRadians(this double val)
        {
            return (Math.PI / 180) * val;
        }
    }

    public enum Day11Actions
    {
        N,
        S,
        E,
        W,
        L,
        R,
        F
    }

    public class NavInstruction
    {
        public Day11Actions Action { get; set; }
        public int Value { get; set; }
    }

    public class WaypointBoat
    {
        public (long X, long Y) Position { get; private set; }
        public (double X, double Y) Waypoint { get; private set; }

        private const double BackMin = Math.PI / 2;
        private const double BackMax = Math.PI / 2 * 3;
        private ITestOutputHelper output;

        private double velocity;
        private double radians;

        public WaypointBoat(ITestOutputHelper output)
        {
            Position = (0, 0);
            Waypoint = (10, 1);
            radians = Math.Tan(Waypoint.Y / Waypoint.X);
            velocity = Math.Sqrt(Math.Pow(Waypoint.Y, 2) + Math.Pow(Waypoint.X, 2));
            this.output = output;
        }

        public void Move(NavInstruction instruction)
        {
            if (instruction.Action == Day11Actions.F)
            {
                double xVal = Math.Round(Math.Cos(radians) * velocity) * instruction.Value;
                double yVal = Math.Round(Math.Sin(radians) * velocity) * instruction.Value;

                //if (radians > Math.PI) yVal = 0 - yVal;
                //if (radians > Math.PI / 2 && radians < Math.PI / 2 * 3) xVal = 0 - xVal;

                Position = (Position.X + (long)xVal, Position.Y + (long)yVal);

                output.WriteLine($"Position({Position.X}, {Position.Y}) WayPoint({Waypoint.X}, {Waypoint.Y}) radians={radians} velocity={velocity}");

                return;
            }

            switch (instruction.Action)
            {
                case Day11Actions.R:
                    {
                        double rads = ((double)instruction.Value).ToRadians();
                        radians -= rads;
                        while (radians < 0) radians += 2 * Math.PI;
                        while (radians >= 2 * Math.PI) radians -= 2 * Math.PI;
                        double xVal = Math.Round(Math.Cos(radians) * velocity);
                        double yVal = Math.Round(Math.Sin(radians) * velocity);
                        Waypoint = (xVal, yVal);
                        velocity = Math.Abs(Waypoint.X / Math.Cos(radians));
                        break;
                    }
                case Day11Actions.L:
                    {
                        double rads = ((double)instruction.Value).ToRadians();
                        radians += rads;
                        while (radians < 0) radians += 2 * Math.PI;
                        while (radians >= 2 * Math.PI) radians -= 2 * Math.PI;
                        double xVal = Math.Round(Math.Cos(radians) * velocity);
                        double yVal = Math.Round(Math.Sin(radians) * velocity);
                        Waypoint = (xVal, yVal);
                        velocity = Math.Abs(Waypoint.X / Math.Cos(radians));
                        break;
                    }
                case Day11Actions.N:
                    {
                        Waypoint = (Waypoint.X, Waypoint.Y + instruction.Value);
                        if (Waypoint.X == 0)
                        {
                            radians = Waypoint.Y > 0 ? Math.PI / 2
                                : Waypoint.Y < 0 ? Math.PI / 2 * 3
                                : 0;
                        }
                        else
                        {
                            radians = Math.Tan(Waypoint.Y / Waypoint.X);
                        }
                        while (radians < 0) radians += 2 * Math.PI;
                        while (radians >= 2 * Math.PI) radians -= 2 * Math.PI;
                        velocity = Waypoint.X == 0 ? Waypoint.Y
                            : Waypoint.Y == 0 ? Waypoint.X
                            : Waypoint.X / Math.Cos(radians);
                        break;
                    }
                case Day11Actions.E:
                    {
                        Waypoint = (Waypoint.X + instruction.Value, Waypoint.Y);
                        if (Waypoint.X == 0)
                        {
                            radians = Waypoint.Y > 0 ? Math.PI / 2
                                : Waypoint.Y < 0 ? Math.PI / 2 * 3
                                : 0;
                        }
                        else
                        {
                            radians = Math.Tan(Waypoint.Y / Waypoint.X);
                        }
                        while (radians < 0) radians += 2 * Math.PI;
                        while (radians >= 2 * Math.PI) radians -= 2 * Math.PI;
                        velocity = Waypoint.X == 0 ? Waypoint.Y
                            : Waypoint.Y == 0 ? Waypoint.X
                            : Waypoint.X / Math.Cos(radians);
                        break;
                    }

                case Day11Actions.S:
                    {
                        Waypoint = (Waypoint.X, Waypoint.Y - instruction.Value);
                        if (Waypoint.X == 0)
                        {
                            radians = Waypoint.Y > 0 ? Math.PI / 2
                                : Waypoint.Y < 0 ? Math.PI / 2 * 3
                                : 0;
                        } else
                        {
                            radians = Math.Tan(Waypoint.Y / Waypoint.X);
                        }

                        while (radians < 0) radians += 2 * Math.PI;
                        while (radians >= 2 * Math.PI) radians -= 2 * Math.PI;
                        velocity = Waypoint.X == 0 ? Waypoint.Y
                            : Waypoint.Y == 0 ? Waypoint.X
                            : Waypoint.X / Math.Cos(radians);

                        break;
                    }

                case Day11Actions.W:
                    {
                        Waypoint = (Waypoint.X - instruction.Value, Waypoint.Y);
                        if (Waypoint.X == 0)
                        {
                            radians = Waypoint.Y > 0 ? Math.PI / 2
                                : Waypoint.Y < 0 ? Math.PI / 2 * 3
                                : 0;
                        }
                        else
                        {
                            radians = Math.Tan(Waypoint.Y / Waypoint.X);
                        }
                        while (radians < 0) radians += 2 * Math.PI;
                        while (radians >= 2 * Math.PI) radians -= 2 * Math.PI;
                        velocity = Waypoint.X == 0 ? Waypoint.Y
                            : Waypoint.Y == 0 ? Waypoint.X
                            : Waypoint.X / Math.Cos(radians);
                        break;
                    }
            }
            output.WriteLine($"Position({Position.X}, {Position.Y}) WayPoint({Waypoint.X}, {Waypoint.Y}) radians={radians} velocity={velocity}");
        }

        public long ManhattanDistance() => Math.Abs(Position.X) + Math.Abs(Position.Y);
    }

    public class Boat
    {
        public (int X, int Y) Position { get; private set; }
        private double radians;
        private readonly double DEGREES_EAST = 0D.ToRadians();
        private readonly double DEGREES_NORTH = 90D.ToRadians();
        private readonly double DEGREES_SOUTH = 270D.ToRadians();
        private readonly double DEGREES_WEST = 180D.ToRadians();

        public Boat()
        {
            Position = (0, 0);
            radians = DEGREES_EAST;
        }

        public void Move(NavInstruction instruction)
        {
            if (instruction.Action == Day11Actions.F)
            {
                if (radians == DEGREES_EAST) instruction.Action = Day11Actions.E;
                else if (radians == DEGREES_SOUTH) instruction.Action = Day11Actions.S;
                else if (radians == DEGREES_NORTH) instruction.Action = Day11Actions.N;
                else if (radians == DEGREES_WEST) instruction.Action = Day11Actions.W;
            }
            switch(instruction.Action)
            {
                case Day11Actions.R:
                    {
                        radians -= ((double)instruction.Value).ToRadians();
                        while (radians < 0) radians += 2 * Math.PI;
                        while (radians >= 2 * Math.PI) radians -= 2 * Math.PI;
                        break;
                    }
                case Day11Actions.L:
                    {
                        radians += ((double)instruction.Value).ToRadians();
                        while (radians < 0) radians += 2 * Math.PI;
                        while (radians >= 2 * Math.PI) radians -= 2 * Math.PI;
                        break;
                    }
                case Day11Actions.N:
                    {
                        Position = (Position.X, Position.Y + instruction.Value);
                        break;
                    }
                case Day11Actions.E:
                    {
                        Position = (Position.X + instruction.Value, Position.Y);
                        break;
                    }

                case Day11Actions.S:
                    {
                        Position = (Position.X, Position.Y - instruction.Value);
                        break;
                    }

                case Day11Actions.W:
                    {
                        Position = (Position.X - instruction.Value, Position.Y);
                        break;
                    }
            }
        }

        public int ManhattanDistance() => Math.Abs(Position.X) + Math.Abs(Position.Y);
    }

    // https://adventofcode.com/2020/day/11
    public class Day12
    {
        private readonly ITestOutputHelper output;

        private IEnumerable<NavInstruction> ReadInstructions()
        {
            using var reader = new StreamReader(InputClient.GetFileStream(2020, 12, ""));
            string input;

            while ((input = reader.ReadLine()) != null) {
                yield return new NavInstruction {
                    Action = Enum.Parse<Day11Actions>(input.Substring(0, 1)),
                    Value = int.Parse(input.Substring(1))
                };
            }
        }

        public Day12(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Part1()
        {
            var boat = new Boat();
            foreach(var instruction in ReadInstructions())
            {
                boat.Move(instruction);
            }
            // NOT 1740
            // NOT 3371
            // NOT 2830
            // NOT 2075
            // NOT 296
            Assert.Equal(1177, boat.ManhattanDistance());
        }
        [Fact]
        public void Part2()
        {
            var boat = new WaypointBoat(output);
            foreach (var instruction in ReadInstructions())
            {
                boat.Move(instruction);
            }
            // NOT 226462
            // NOT 2147483648
            Assert.Equal(286, boat.ManhattanDistance());
        }
    }
}
