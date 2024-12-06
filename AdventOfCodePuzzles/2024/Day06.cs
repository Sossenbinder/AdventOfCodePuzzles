namespace AdventOfCodePuzzles._2024;

internal sealed class Day06 : BenchmarkableBase
{
    private enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
    
    private readonly record struct Point(int X, int Y);

    private readonly HashSet<Point> _blockedPoints = [];

    private Point _startingPoint;

    protected override void InternalOnLoad()
    {
        for (var y = 0; y < Input.Lines.Length; y++)
        {
            for (var x = 0; x < Input.Lines[y].Length; x++)
            {
                var item = Input.Lines[y][x];
                switch (item)
                {
                    case '#':
                        _blockedPoints.Add(new(x, y));
                        break;
                    case '^':
                        _startingPoint = new(x, y);
                        break;
                }
            }
        }
    }

    protected override object InternalPart1()
    {
        var visitedPositions = GetVisitedPositions();
        return visitedPositions.Count;
    }

    protected override object InternalPart2()
    {
        var visitedPositions = GetVisitedPositions();

        // Remove starting point as potential circle
        visitedPositions.Remove(_startingPoint);

        var loops = 0;
        foreach (var extraPosition in visitedPositions)
        {
            var extendedBlockedPoints = _blockedPoints.ToHashSet();
            extendedBlockedPoints.Add(extraPosition);

            if (IsLoop(extendedBlockedPoints))
            {
                loops++;
            }
        }

        return loops;
    }

    private bool IsLoop(HashSet<Point> blockedPoints)
    {
        var direction = Direction.Up;
        var position = _startingPoint;
        Dictionary<Point, int> visitCounter = new()
        {
            [_startingPoint] = 1,
        };
        
        while (true)
        {
            var nextPosition = direction switch
            {
                Direction.Up => position with { Y = position.Y - 1},
                Direction.Down => position with { Y = position.Y + 1},
                Direction.Left => position with { X = position.X - 1},
                Direction.Right => position with { X = position.X + 1},
            };

            if (nextPosition.Y < 0 || nextPosition.X < 0 || nextPosition.Y >= Input.Lines.Length || nextPosition.X >= Input.Lines[nextPosition.Y].Length)
            {
                // Out of bounds -> done
                break;
            }

            if (blockedPoints.Contains(nextPosition))
            {
                // If blocked, do the turn
                direction = (Direction)((int)(direction + 1) % 4);
                continue;
            }

            
            if (!visitCounter.TryGetValue(nextPosition, out var visitCount))
            {
                visitCount = 1;
            }
            else if (visitCount > 4)
            {
                return true;
            }
            visitCounter[nextPosition] = visitCount + 1;
            position = nextPosition;
        }
        
        return false;
    }

    private HashSet<Point> GetVisitedPositions()
    {
        var direction = Direction.Up;

        var position = _startingPoint;

        HashSet<Point> visitedPositions = [_startingPoint];
        
        while (true)
        {
            var nextPosition = direction switch
            {
                Direction.Up => position with { Y = position.Y - 1},
                Direction.Down => position with { Y = position.Y + 1},
                Direction.Left => position with { X = position.X - 1},
                Direction.Right => position with { X = position.X + 1},
            };

            if (nextPosition.Y < 0 || nextPosition.X < 0 || nextPosition.Y >= Input.Lines.Length || nextPosition.X >= Input.Lines[nextPosition.Y].Length)
            {
                // Out of bounds -> done
                break;
            }

            if (_blockedPoints.Contains(nextPosition))
            {
                // If blocked, do the turn
                direction = (Direction)((int)(direction + 1) % 4);
                continue;
            }
            
            visitedPositions.Add(nextPosition);
            position = nextPosition;
        }
        
        return visitedPositions;
    }

    private void Print(HashSet<Point> visitedPositions, Point position, Direction direction, HashSet<Point> blockedPoints)
    {
        for (var y = 0; y < Input.Lines.Length; y++)
        {
            for (var x = 0; x < Input.Lines[y].Length; x++)
            {
                var chr = '.';

                if (blockedPoints.Contains(new Point(x, y)))
                {
                    chr = '#';
                }

                if (visitedPositions.Contains(new Point(x, y)))
                {
                    chr = 'O';
                }

                if (_startingPoint.X == position.X && _startingPoint.Y == position.Y)
                {
                    chr = 'S';
                }

                if (position.X == x && position.Y == y)
                {
                    chr = direction switch
                    {
                        Direction.Up => '^',
                        Direction.Right => '>',
                        Direction.Down => 'v',
                        Direction.Left => '<',
                    };
                }
                
                Console.Write(chr);
            }
            Console.Write('\n');
        }
    }
}