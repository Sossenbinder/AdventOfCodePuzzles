namespace AdventOfCodePuzzles._2025;

internal sealed class Day07 : TestableBenchmarkableBase
{
    private readonly record struct Point(int X, int Y);
    
    protected override object InternalPart1()
    {
        var start = FindStart();
        
        var splitPoints = new HashSet<Point>();

        var openPointQueue = new Queue<Point>();
        openPointQueue.Enqueue(start with { Y = start.Y + 1});
        while (openPointQueue.TryDequeue(out var nextPoint))
        {
            if (nextPoint.Y >= Input.Lines.Length)
            {
                continue;
            }
            
            var cellValue = Input.Lines[nextPoint.Y][nextPoint.X];

            switch (cellValue)
            {
                case '.':
                    openPointQueue.Enqueue(nextPoint with { Y = nextPoint.Y + 1 });
                    break;
                case '^':
                    if (splitPoints.Add(nextPoint))
                    {
                        openPointQueue.Enqueue(new Point(nextPoint.X - 1, nextPoint.Y + 1));
                        openPointQueue.Enqueue(new Point(nextPoint.X + 1, nextPoint.Y + 1));
                    }
                    break;
            }
        }

        return splitPoints.Count;
    }

    protected override object InternalPart2()
    {
        var start = FindStart();
        
        var splitPoints = new HashSet<Point>();

        var openPointQueue = new Queue<Point>();
        openPointQueue.Enqueue(start with { Y = start.Y + 1});
        while (openPointQueue.TryDequeue(out var nextPoint))
        {
            if (nextPoint.Y >= Input.Lines.Length)
            {
                continue;
            }
            
            var cellValue = Input.Lines[nextPoint.Y][nextPoint.X];

            switch (cellValue)
            {
                case '.':
                    openPointQueue.Enqueue(nextPoint with { Y = nextPoint.Y + 1 });
                    break;
                case '^':
                    if (splitPoints.Add(nextPoint))
                    {
                        openPointQueue.Enqueue(new Point(nextPoint.X - 1, nextPoint.Y + 1));
                        openPointQueue.Enqueue(new Point(nextPoint.X + 1, nextPoint.Y + 1));
                    }
                    break;
            }
        }

        var ascendingSplitPoints = splitPoints.OrderByDescending(x => x.Y);

        var splitLookup = new Dictionary<Point, long>();

        foreach (var splitPoint in ascendingSplitPoints)
        {
            var nextLeft = new Point(splitPoint.X - 1, splitPoint.Y + 1);
            var nextRight = new Point(splitPoint.X + 1, splitPoint.Y + 1);

            var nextLeftSplitter = FindNextSplitter(nextLeft);
            var nextRightSplitter = FindNextSplitter(nextRight);

            long paths = 0;

            if (nextLeftSplitter is not null)
            {
                paths += splitLookup[nextLeftSplitter.Value];
            }
            else
            {
                paths += 1;
            }
            
            if (nextRightSplitter is not null)
            {
                paths += splitLookup[nextRightSplitter.Value];
            }
            else
            {
                paths += 1;
            }

            splitLookup[splitPoint] = paths;
        }

        return splitLookup[FindNextSplitter(start)!.Value];
    }

    private Point? FindNextSplitter(Point start)
    {
        for (var y = start.Y; y < Input.Lines.Length; y++)
        {
            if (Input.Lines[y][start.X] == '^')
            {
                return start with {Y = y};
            }
        }

        return null;
    }

    private Point FindStart()
    {
        for (var y = 0; y < Input.Lines.Length; y++)
        {
            for (var x = 0; x < Input.Lines[y].Length; x++)
            {
                if (Input.Lines[y][x] == 'S')
                {
                    return new Point(x, y);
                }
            }
        }
        
        throw new Exception();
    }

    protected override bool UseDayTestInput => false;

    protected override string DayTestInput => """
        .......S.......
        ...............
        .......^.......
        ...............
        ......^.^......
        ...............
        .....^.^.^.....
        ...............
        ....^.^...^....
        ...............
        ...^.^...^.^...
        ...............
        ..^...^.....^..
        ...............
        .^.^.^.^.^...^.
        ...............
        """;
}