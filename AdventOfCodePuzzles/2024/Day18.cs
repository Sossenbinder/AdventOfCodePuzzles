namespace AdventOfCodePuzzles._2024;

internal sealed class Day18 : BenchmarkableBase
{
    private readonly record struct Point(int X, int Y);
    
    private readonly List<Point> _incomingBytePositions = [];

    private const int _height = 71;
    
    private const int _width = 71;

    private const int _blockers = 1024;
    
    
    protected override void InternalOnLoad()
    {
        foreach (var line in Input.Lines)
        {
            var split = line.AsSpan().Split(',');
            split.MoveNext();
            var x = int.Parse(line[split.Current]);
            split.MoveNext();
            var y = int.Parse(line[split.Current]);
            _incomingBytePositions.Add(new Point(x, y));
        }
    }

    protected override object InternalPart1()
    {
        var fallenBlockers = _incomingBytePositions[.._blockers].ToHashSet();

        var position = new Point(0, 0);
        
        var visitStepMap = new Dictionary<Point, int>();
        var openPoints = new Queue<(Point Point, int StepCount)>();
        openPoints.Enqueue((position, 0));

        while (openPoints.TryDequeue(out var currentPoint))
        {
            var adjacentPoints = FindAdjacentFreePoints(currentPoint.Point, fallenBlockers);

            var newStepCount = currentPoint.StepCount + 1;
            foreach (var adjacentPoint in adjacentPoints)
            {
                if (!visitStepMap.TryGetValue(adjacentPoint, out var visitedStepCount))
                {
                    visitStepMap[adjacentPoint] = newStepCount;
                } 
                else if (newStepCount >= visitedStepCount)
                {
                    continue;
                }
                
                visitStepMap[adjacentPoint] = newStepCount;
                openPoints.Enqueue((adjacentPoint, newStepCount));
            }
        }

        return visitStepMap[new Point(_height - 1, _width - 1)];
    }

    protected override object InternalPart2()
    {
        for (var i = 1; i < _incomingBytePositions.Count; i++)
        {
            var fallenBlockers = _incomingBytePositions[..i].ToHashSet();

            var position = new Point(0, 0);
        
            var visitStepMap = new Dictionary<Point, int>();
            var openPoints = new Queue<(Point Point, int StepCount)>();
            openPoints.Enqueue((position, 0));

            while (openPoints.TryDequeue(out var currentPoint))
            {
                var adjacentPoints = FindAdjacentFreePoints(currentPoint.Point, fallenBlockers);

                var newStepCount = currentPoint.StepCount + 1;
                foreach (var adjacentPoint in adjacentPoints)
                {
                    if (!visitStepMap.TryGetValue(adjacentPoint, out var visitedStepCount))
                    {
                        visitStepMap[adjacentPoint] = newStepCount;
                    } 
                    else if (newStepCount >= visitedStepCount)
                    {
                        continue;
                    }
                
                    visitStepMap[adjacentPoint] = newStepCount;
                    openPoints.Enqueue((adjacentPoint, newStepCount));
                }
            }

            if (!visitStepMap.ContainsKey(new Point(_height - 1, _width - 1)))
            {
                return _incomingBytePositions[..i].Last();
            }
        }

        return 0;
    }
    
    private static IEnumerable<Point> FindAdjacentFreePoints(Point pos, HashSet<Point> blockedPoints)
    {
        var points = new List<Point>();
        
        if (pos.X + 1 < _width)
        {
            points.Add(pos with { X = pos.X + 1 });
        }

        if (pos.X - 1 >= 0)
        {
            points.Add(pos with { X = pos.X - 1 });
        }

        if (pos.Y + 1 < _height)
        {
            points.Add(pos with {Y = pos.Y + 1});
        }

        if (pos.Y - 1 >= 0)
        {
            points.Add(pos with {Y = pos.Y - 1});
        }
        
        return points.Where(x => !blockedPoints.Contains(x));
    }

    private void Print(Dictionary<Point, int> pointMap, HashSet<Point> blockedPoints)
    {
        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                var chr = ".";

                if (blockedPoints.Contains(new Point(x, y)))
                {
                    chr = "#";
                }
                else if (pointMap.TryGetValue(new Point(x, y), out var steps))
                {
                    chr = steps.ToString();
                }
                
                Console.Write(chr);
            }

            Console.Write('\n');
        }
    }
}