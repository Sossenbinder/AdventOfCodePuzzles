using System.Text.RegularExpressions;

namespace AdventOfCodePuzzles._2024;

internal sealed class Day14 : BenchmarkableBase
{

    private readonly record struct Position(
        int X,
        int Y);

    private readonly record struct Velocity(
        int X,
        int Y);

    private class Line(Position position, Velocity velocity)
    {
        public Position Position { get; set; } = position;
        
        public Velocity Velocity { get; init; } = velocity;
    }

    private readonly List<Line> lines = [];
    
    protected override void InternalOnLoad()
    {
        foreach (var line in Input.Lines)
        {
            var split = line.Split(" ");
            var positionPart = split[0].Trim().Split('=')[1].Split(',');
            var velocityPart = split[1].Trim().Split('=')[1].Trim().Split(',');
            
            var position = new Position(int.Parse(positionPart[0]), int.Parse(positionPart[1]));
            var velocity = new Velocity(int.Parse(velocityPart[0]), int.Parse(velocityPart[1]));
            
            lines.Add(new Line(position, velocity));
        }
    }

    protected override object InternalPart1()
    {
        var maxX = 101;
        var maxY = 103;

        var seconds = 100;
        foreach (var _ in Enumerable.Range(0, seconds))
        {
            foreach (var line in lines)
            {
                var velocity = line.Velocity;

                var newX = line.Position.X + velocity.X;
                if (newX < 0)
                {
                    newX = maxX + newX;
                }
                newX %= maxX;
                
                var newY = line.Position.Y + velocity.Y;
                if (newY < 0)
                {
                    newY = maxY + newY;
                }

                newY %= maxY;
                
                line.Position = new Position(newX, newY);
            }
        }

        var halfX = maxX / 2;
        var halfY = maxY / 2;

        var quadrantMap = new Dictionary<int, int>()
        {
            [0] = 0,
            [1] = 0,
            [2] = 0,
            [3] = 0
        };

        foreach (var line in lines)
        {
            var (x, y) = line.Position;

            if (x == halfX || y == halfY)
            {
                continue;
            }

            if (x < halfX)
            {
                if (y < halfY)
                {
                    quadrantMap[0] += 1;
                }
                else
                {
                    quadrantMap[1] += 1;
                }
            }
            else
            {
                if (y < halfY)
                {
                    quadrantMap[2] += 1;
                }
                else
                {
                    quadrantMap[3] += 1;
                }

            }
        }

        return quadrantMap.Aggregate(1, (agg, curr) => agg * curr.Value);
    }

    protected override object InternalPart2()
    {
        var maxX = 101;
        var maxY = 103;

        var seconds = 0;
        while (true)
        {
            foreach (var line in lines)
            {
                var velocity = line.Velocity;

                var newX = (line.Position.X + velocity.X + maxX) % maxX;
                var newY = (line.Position.Y + velocity.Y + maxY) % maxY;
                line.Position = new Position(newX, newY);
            }

            seconds++;

            if (lines.GroupBy(x => x.Position).All(x => x.Count() == 1))
            {
                break;
            }
        }

        return seconds;
    }
}