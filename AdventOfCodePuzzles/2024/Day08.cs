using System.Numerics;

namespace AdventOfCodePuzzles._2024;

internal sealed class Day08 : BenchmarkableBase
{
    private readonly Dictionary<char, List<Vector2>> _antennaMap = [];

    protected override void InternalOnLoad()
    {
        for (var y = 0; y < Input.Lines.Length; y++)
        {
            var line = Input.Lines[y];
            for (var x = 0; x < line.Length; x++)
            {
                var item = line[x];

                if (item is '.')
                {
                    continue;
                }

                if (!_antennaMap.TryGetValue(item, out var points))
                {
                    points = [];
                    _antennaMap.Add(item, points);
                }
                
                points.Add(new Vector2(x, y));
            }
        }
    }

    protected override object InternalPart1()
    {
        var maxY = Input.Lines.Length;
        var maxX = Input.Lines[0].Length;
        
        var antiNodes = new HashSet<Vector2>();

        foreach (var (_, points) in _antennaMap)
        {
            var pairs = MakePairs(points);
            foreach (var (closer, farther) in pairs)
            {
                var scaledDirection = (farther - closer) * 2;
                
                AddIfInBounds(farther - scaledDirection);
                AddIfInBounds(closer + scaledDirection);
            }

        }
        return antiNodes.Count;

        void AddIfInBounds(Vector2 x)
        {
            if (x.X >= 0 && x.X < maxX && x.Y >= 0 && x.Y < maxY)
            {
                antiNodes.Add(x);
            }
        }
    }

    protected override object InternalPart2()
    {
        var maxY = Input.Lines.Length;
        var maxX = Input.Lines[0].Length;
        
        var antiNodes = new HashSet<Vector2>();

        foreach (var (_, points) in _antennaMap)
        {
            var pairs = MakePairs(points);
            foreach (var (closer, farther) in pairs)
            {
                var scaleFactor = 1;
                var minusOpen = true;
                var plusOpen = true;
                while (true)
                {
                    var scaledDirection = (farther - closer) * scaleFactor;
                    if (minusOpen)
                    {
                        minusOpen = AddIfInBounds(farther - scaledDirection);   
                    }

                    if (plusOpen)
                    {
                        plusOpen = AddIfInBounds(closer + scaledDirection);
                    }

                    if (!minusOpen && !plusOpen)
                    {
                        break;
                    }

                    scaleFactor++;
                }
            }
            
        }
        return antiNodes.Count;

        bool AddIfInBounds(Vector2 x)
        {
            if (!(x.X >= 0) || !(x.X < maxX) || !(x.Y >= 0) || !(x.Y < maxY))
            {
                return false;
            }

            antiNodes.Add(x);
            return true;

        }
    }
    private static List<(Vector2 Closer, Vector2 Farther)> MakePairs(List<Vector2> points)
    {
        return points.Select((x, i) => points.Skip(i + 1).Select(y =>
        {
            var distanceOne = Vector2.DistanceSquared(Vector2.Zero, y);
            var distanceTwo = Vector2.DistanceSquared(Vector2.Zero, x);

            return distanceOne <= distanceTwo ? (y, x) : (x, y);
        })).SelectMany(x => x).ToList();
    }
}