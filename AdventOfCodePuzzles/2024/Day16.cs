namespace AdventOfCodePuzzles._2024;

internal sealed class Day16 : BenchmarkableBase
{
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    
    private readonly record struct MoveRecord(
        int Steps,
        int Turns)
    {
        public int Score => Steps + Turns * 1000;
    }
    
    private readonly record struct MoveRecordP2(
        int Steps,
        int Turns,
        HashSet<Point> Points)
    {
        public int Score => Steps + Turns * 1000;
    }
    
    private readonly record struct Point(int X, int Y);

    private readonly HashSet<Point> _reachablePoints = [];

    private Point _start;
    
    private Point _end;

    protected override void InternalOnLoad()
    {
        for (var y = 0; y < Input.Lines.Length; y++)
        {
            for (var x = 0; x < Input.Lines[y].Length; x++)
            {
                var symbol = Input.Lines[y][x];
                var point = new Point(x, y);
                if (symbol is '.')
                {
                    _reachablePoints.Add(point);
                }
                else if (symbol is 'E')
                {
                    _end = point;
                    _reachablePoints.Add(point);
                }
                else if (symbol is 'S')
                {
                    _start = point;
                }
            }
        }
    }

    protected override object InternalPart1()
    {
        var lowestMoveRecordDict = new Dictionary<Point, MoveRecord>();
        
        var openQueue = new Queue<(Point Point, Direction Direction, MoveRecord MoveRecord)>();
        openQueue.Enqueue((_start, Direction.Right, new MoveRecord(0,0)));

        while (openQueue.TryDequeue(out var result))
        {
            var (point, direction, moveRecord) = result;
            
            var reachables = FindAdjacentOpenPoints(point, direction);

            foreach (var reachable in reachables)
            {
                var isTurn = direction != reachable.Direction;
                var newMoveRecord = moveRecord with { Steps = moveRecord.Steps + 1, Turns = moveRecord.Turns + (isTurn ? 1 : 0) };
                if (lowestMoveRecordDict.TryGetValue(reachable.Point, out var existingRecord) && existingRecord.Score <= newMoveRecord.Score)
                {
                    continue;
                }
                
                lowestMoveRecordDict[reachable.Point] = newMoveRecord;

                if (reachable.Point != _end)
                {
                    openQueue.Enqueue((reachable.Point, reachable.Direction, newMoveRecord));
                }
            }

        }
        
        return lowestMoveRecordDict[_end].Score;
    }

    protected override object InternalPart2()
    {
        var validPoints = InitialRun();
        
        var visitedPoints = new HashSet<Point>();
        
        // Unwind the recorded moves until the end
        foreach (var movepoint in validPoints)
        {
            visitedPoints.Add(movepoint);
        }
        
        var lowestMoveRecordDict = new Dictionary<Point, MoveRecordP2>();
        
        var openQueue = new Queue<(Point Point, Direction Direction, MoveRecordP2 MoveRecord)>();
        openQueue.Enqueue((_start, Direction.Right, new MoveRecordP2(0, 0, [_start])));

        while (openQueue.TryDequeue(out var result))
        {
            var (point, direction, moveRecord) = result;
            
            if (lowestMoveRecordDict.TryGetValue(point, out var existingMoveRecord) && existingMoveRecord.Score < moveRecord.Score)
            {
                continue;
            }

            if (point != _end && (!lowestMoveRecordDict.ContainsKey(point) || existingMoveRecord.Score == moveRecord.Score))
            {
                if (validPoints.Contains(point))
                {
                    // Unwind the recorded moves so far
                    foreach (var movepoint in moveRecord.Points)
                    {
                        visitedPoints.Add(movepoint);
                    }
                }
            }
            
            var reachables = FindAdjacentOpenPoints(point, direction);

            foreach (var reachable in reachables)
            {
                var isTurn = direction != reachable.Direction;
                var traversedPoints = new HashSet<Point>(moveRecord.Points) {reachable.Point};

                var newMoveRecord = moveRecord with { Steps = moveRecord.Steps + 1, Turns = moveRecord.Turns + (isTurn ? 1 : 0), Points = traversedPoints};
                if (lowestMoveRecordDict.TryGetValue(reachable.Point, out var existingRecord) && existingRecord.Score < newMoveRecord.Score)
                {
                    continue;
                }

                lowestMoveRecordDict[reachable.Point] = newMoveRecord;

                if (reachable.Point != _end)
                {
                    openQueue.Enqueue((reachable.Point, reachable.Direction, newMoveRecord));
                }
            }
        }
        
        return visitedPoints.Count;
    }

    private HashSet<Point> InitialRun()
    {
        var lowestMoveRecordDict = new Dictionary<Point, MoveRecordP2>();
        
        var openQueue = new Queue<(Point Point, Direction Direction, MoveRecordP2 MoveRecord)>();
        openQueue.Enqueue((_start, Direction.Right, new MoveRecordP2(0, 0, [_start])));

        while (openQueue.TryDequeue(out var result))
        {
            var (point, direction, moveRecord) = result;
            
            var reachables = FindAdjacentOpenPoints(point, direction);

            foreach (var reachable in reachables)
            {
                var isTurn = direction != reachable.Direction;
                var traversedPoints = new HashSet<Point>(moveRecord.Points) {reachable.Point};

                var newMoveRecord = moveRecord with { Steps = moveRecord.Steps + 1, Turns = moveRecord.Turns + (isTurn ? 1 : 0), Points = traversedPoints};
                if (lowestMoveRecordDict.TryGetValue(reachable.Point, out var existingRecord) && existingRecord.Score <= newMoveRecord.Score)
                {
                    continue;
                }

                lowestMoveRecordDict[reachable.Point] = newMoveRecord;

                if (reachable.Point != _end)
                {
                    openQueue.Enqueue((reachable.Point, reachable.Direction, newMoveRecord));
                }
            }

        }
        
        return lowestMoveRecordDict[_end].Points;
    }

    private void Print(HashSet<Point> visitedPositions)
    {
        Console.Clear();
        for (var y = 0; y < Input.Lines.Length; y++)
        {
            for (var x = 0; x < Input.Lines[y].Length; x++)
            {
                var chr = Input.Lines[y][x];

                if (visitedPositions.Contains(new Point(x, y)))
                {
                    chr = 'O';
                }

                Console.Write(chr);
            }

            Console.Write('\n');
        }
    }


    private IEnumerable<(Point Point, Direction Direction)> FindAdjacentOpenPoints(Point pos, Direction direction)
    {
        var inaccessibleDirection = direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
        };
        
        var theoreticalPoints = new List<(Point Point, Direction Direction)>()
        {
            (pos with {X = pos.X - 1}, Direction.Left),
            (pos with {X = pos.X + 1}, Direction.Right),
            (pos with {Y = pos.Y - 1}, Direction.Up),
            (pos with {Y = pos.Y + 1}, Direction.Down)
        }.Where(x => x.Direction != inaccessibleDirection);
        
        return theoreticalPoints.Where(x => _reachablePoints.Contains(x.Point));;
    }
}