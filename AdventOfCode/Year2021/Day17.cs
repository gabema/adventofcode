using System;
using System.Drawing;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2021;

// https://adventofcode.com/2021/day/17
public class Day17
{
    private readonly ITestOutputHelper _output;
    public Day17(ITestOutputHelper output)
    {
        _output = output;
    }

    public class Shot
    {
        public Point Point { get; private init; }
        private int XVelocity { get; init; }
        private int YVelocity { get; init; }

        public Shot(Point p, int xvel, int yvel)
        {
            Point = p;
            XVelocity = xvel;
            YVelocity = yvel;
        }

        public Shot Next()
        {
            return new Shot(
                new Point(Point.X + XVelocity, Point.Y + YVelocity),
                xvel: XVelocity > 0 ? XVelocity - 1 : XVelocity < 0 ? XVelocity + 1 : XVelocity,
                yvel: YVelocity - 1
            );
        }

        public ShotStatus ShotComparedToTarget(Rectangle target) => 
                target.Contains(Point) ? ShotStatus.Hit :
                target.Top > Point.Y ? ShotStatus.Below :
                target.Right < Point.X ? ShotStatus.Beyond : ShotStatus.Short;

        public override string ToString()
        {
            return $"[Point={Point}, XVelocity={XVelocity}, YVelocity={YVelocity}]";
        }
    }

    public enum ShotStatus {
        Hit,
        Below,
        Beyond,
        Short
    };

    (ShotStatus finalStatus, int maxHeight) MaximumHeight(Rectangle target, Shot shot)
    {
        int maxHeight = 0;
        ShotStatus status = ShotStatus.Short;
        for (int y = 1; status == ShotStatus.Short; shot = shot.Next(), status = shot.ShotComparedToTarget(target))
        {
            if (maxHeight < shot.Point.Y) maxHeight = shot.Point.Y;
            //_output.WriteLine($"status={status}, point={shot.Point}");
        }
        //_output.WriteLine($"final status={status}, point={shot.Point}");

        return (status, maxHeight);
    }

    [Fact]
    public void PartA()
    {
        var targetBox = ReadBox("");
        int maxHeight = int.MinValue;

        for (int xVel = 1; xVel < targetBox.Left; xVel++)
        {
            for (int yVel = targetBox.Top; yVel < 1000; yVel++)
            {
                var shot = new Shot(Point.Empty, xVel, yVel);
                var result = MaximumHeight(targetBox, shot);
                if (result.finalStatus == ShotStatus.Hit && result.maxHeight > maxHeight)
                {
                    maxHeight = result.maxHeight;
                }
            }
        }

        Assert.Equal(4095, maxHeight);
    }

    [Fact]
    public void PartB()
    {
        var targetBox = ReadBox("");
        int numHits = 0;

        for (int xVel = 1; xVel < targetBox.Right; xVel++)
        {
            for (int yVel = targetBox.Top; yVel < 1000; yVel++)
            {
                var shot = new Shot(Point.Empty, xVel, yVel);
                var result = MaximumHeight(targetBox, shot);
                if (result.finalStatus == ShotStatus.Hit)
                {
                    _output.WriteLine($"{xVel},{yVel}");
                    numHits++;
                }
            }
        }

        Assert.Equal(3773, numHits);

    }

    private Rectangle ReadBox(string variant)
    {
        var match = InputClient.GetRegularExpressionRows(2021, 17, variant, @"x=([\-\d]+)\.\.([\-\d]+), y=([\-\d]+)\.\.([\-\d]+)").First();

        var x1 = int.Parse(match[1].Value);
        var x2 = int.Parse(match[2].Value);
        var y1 = int.Parse(match[3].Value);
        var y2 = int.Parse(match[4].Value);

        var x = Math.Min(x1, x2);
        var y = Math.Min(y1, y2);
        var width = Math.Abs(x2 - x1) + 1;
        var height = Math.Abs(y2 - y1) + 1;

        return new Rectangle(x, y, width, height);
    }
}
