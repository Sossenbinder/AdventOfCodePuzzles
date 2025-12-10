namespace AdventOfCodePuzzles._2025;

internal sealed class Day09 : TestableBenchmarkableBase
{
    private readonly record struct Point(long X, long Y);
    
    protected override object InternalPart1()
    {
        var pointPermutations = GetPointPermutations();
        
        long maxArea = 0;
        foreach (var permutation in pointPermutations)
        {
            var (from, to) = permutation;
            
            var xDist = Math.Abs(from.X - to.X) + 1;
            var yDist = Math.Abs(from.Y - to.Y) + 1;
            var area = yDist * xDist;
            
            if (area > maxArea)
            {
                maxArea = area;
            }
        }
        
        return maxArea;
    }

    protected object InternalPart2FirstVersion()
    {
        var points = ParsePoints();
        var pointPermutations = GetPointPermutations();
        
        var areaLookup = new Dictionary<(Point From, Point To), long>();
        
        long maxArea = 0;
        foreach (var permutation in pointPermutations)
        {
            var (from, to) = permutation;
            
            var xDist = Math.Abs(from.X - to.X) + 1;
            var yDist = Math.Abs(from.Y - to.Y) + 1;
            var area = yDist * xDist;
            
            areaLookup[(from, to)] = area;
        }
        
        var containmentMap = new Dictionary<(long X, long Y), bool>();
        
        void AddCombinationToTile((Point From, Point To) combination)
        {
            var (from, to) = combination;
            var xStart = Math.Min(from.X, to.X);
            var xEnd = Math.Max(from.X, to.X);
            var yStart = Math.Min(from.Y, to.Y);
            var yEnd = Math.Max(from.Y, to.Y);

            for (var x = xStart; x <= xEnd; ++x)
            {
                for (var y = yStart; y <= yEnd; ++y)
                {
                    containmentMap[(x, y)] = true;
                }
            }
        }

        AddCombinationToTile((points.Last(), points.First()));
        for (var i = 1; i < points.Count - 1; ++i)
        {
            AddCombinationToTile((points[i - 1], points[i]));
            AddCombinationToTile((points[i], points[i + 1]));
        }

        var fillTileStarter = new Point(3, 4);
        
        var fillQueue = new Queue<Point>();
        fillQueue.Enqueue(fillTileStarter);
        
        
        var maxX = (int)points.Max(x => x.X) + 1;
        var maxY = (int)points.Max(x => x.Y) + 1;

        while (fillQueue.TryDequeue(out var fillPoint))
        {
            if (!containmentMap.TryGetValue((fillPoint.X, fillPoint.Y), out var val))
            {
                continue;
            }

            containmentMap[(fillPoint.X, fillPoint.Y)] = true;

            var adjacentPoints = new List<Point>
            {
                fillPoint with {X = fillPoint.X - 1},
                fillPoint with {X = fillPoint.X + 1},
                fillPoint with {Y = fillPoint.Y - 1},
                fillPoint with {Y = fillPoint.Y + 1},
            };

            foreach (var adjacentPoint in adjacentPoints)
            {
                if (adjacentPoint.X < 0 || adjacentPoint.Y < 0 ||
                    adjacentPoint.X >= maxX || adjacentPoint.Y >= maxY)
                {
                    continue;
                }

                if (!containmentMap[(adjacentPoint.X, adjacentPoint.Y)])
                {
                    fillQueue.Enqueue(adjacentPoint);
                }
            }
            
        }

        bool CheckIfAllPointsInTile(Point from, Point to)
        {
            var xStart = Math.Min(from.X, to.X);
            var xEnd = Math.Max(from.X, to.X);
            var yStart = Math.Min(from.Y, to.Y);
            var yEnd = Math.Max(from.Y, to.Y);
            
            for (var x = xStart; x <= xEnd; ++x)
            {
                for (var y = yStart; y <= yEnd; ++y)
                {
                    if (!containmentMap[(x, y)])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        
        var highestMatch = areaLookup
            .OrderByDescending(x => x.Value)
            .First(x => CheckIfAllPointsInTile(x.Key.From, x.Key.To));
        
        return highestMatch.Value;
    }
    
    protected override object InternalPart2()
    {
        var points = ParsePoints();
        var pointPermutations = GetPointPermutations();
        
        var areaLookup = new Dictionary<(Point From, Point To), long>();
        
        foreach (var permutation in pointPermutations)
        {
            var (from, to) = permutation;
            
            var xDist = Math.Abs(from.X - to.X) + 1;
            var yDist = Math.Abs(from.Y - to.Y) + 1;
            var area = yDist * xDist;
            
            areaLookup[(from, to)] = area;
        }
        
        var highestMatch = areaLookup
            .OrderByDescending(x => x.Value)
            .First(x => IsRectangleValid(x.Key.From, x.Key.To, points));

        return highestMatch;
    }

    private static bool IsRectangleValid(Point p1, Point p2, List<Point> polygon)
    {
        var minX = Math.Min(p1.X, p2.X);
        var maxX = Math.Max(p1.X, p2.X);
        var minY = Math.Min(p1.Y, p2.Y);
        var maxY = Math.Max(p1.Y, p2.Y);

        for (var i = 0; i < polygon.Count; i++)
        {
            var a = polygon[i];
            var b = polygon[(i + 1) % polygon.Count];

            if (a.X == b.X)
            {
                if (a.X > minX && a.X < maxX)
                {
                    var wallMinY = Math.Min(a.Y, b.Y);
                    var wallMaxY = Math.Max(a.Y, b.Y);
                    
                    if (Math.Max(minY, wallMinY) < Math.Min(maxY, wallMaxY)) return false;
                }
            }
            else
            {
                if (a.Y > minY && a.Y < maxY)
                {
                    var wallMinX = Math.Min(a.X, b.X);
                    var wallMaxX = Math.Max(a.X, b.X);

                    if (Math.Max(minX, wallMinX) < Math.Min(maxX, wallMaxX)) return false;
                }
            }
        }

        var testX = (minX + maxX) / 2.0;
        var testY = (minY + maxY) / 2.0;
        
        var inside = false;
        for (var i = 0; i < polygon.Count; i++)
        {
            var a = polygon[i];
            var b = polygon[(i + 1) % polygon.Count];

            if (a.Y > testY != (b.Y > testY) && testX < (b.X - a.X) * (testY - a.Y) / (b.Y - a.Y) + a.X)
            {
                inside = !inside;
            }
        }

        return inside;
    }
    
    private List<Point> ParsePoints()
    {
        return Input.Lines.Select(x => x.Split(',')).Select(x => new Point(
            int.Parse(x[0]),
            int.Parse(x[1])
        )).ToList();
    }
    
    private List<(Point From, Point To)> GetPointPermutations()
    {
        var points = ParsePoints();
        
        var permutations = new List<(Point From, Point To)>();

        for (var i = 0; i < points.Count; ++i)
        {
            for (var j = 0; j < points.Count; ++j)
            {
                if (i == j)
                {
                    continue;
                }

                permutations.Add((points[i], points[j]));
            }
        }

        return permutations;
    }

    protected override bool UseDayTestInput => false;
    protected override string DayTestInput =>
        """
        7,1
        11,1
        11,7
        9,7
        9,5
        2,5
        2,3
        7,3
        """;
}