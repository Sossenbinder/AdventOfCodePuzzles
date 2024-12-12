namespace AdventOfCodePuzzles._2024;

internal sealed class Day12 : BenchmarkableBase
{
    private enum Direction
    {
        Up,

        Right,

        Down,

        Left
    }

    private readonly record struct Point(
        int X,
        int Y);

    private readonly record struct RegionOne(
        int Area,
        int Perimeter);

    private readonly record struct RegionTwo(
        char Letter,
        int Sides,
        int Area);

    protected override object InternalPart1()
    {
        var visitedPoints = new HashSet<Point>();

        var regions = new List<RegionOne>();

        for (var y = 0; y < Input.Lines.Length; ++y)
        {
            for (var x = 0; x < Input.Lines[y].Length; ++x)
            {
                var point = new Point(x, y);
                if (visitedPoints.Contains(point))
                {
                    continue;
                }

                regions.Add(GetRegionOne(point, visitedPoints));
            }
        }

        return regions.Select(x => x.Area * x.Perimeter).Sum();
    }

    protected override object InternalPart2()
    {
        var visitedPoints = new HashSet<Point>();

        var regions = new List<RegionTwo>();

        for (var y = 0; y < Input.Lines.Length; ++y)
        {
            for (var x = 0; x < Input.Lines[y].Length; ++x)
            {
                var point = new Point(x, y);
                if (visitedPoints.Contains(point))
                {
                    continue;
                }

                regions.Add(GetRegionTwo(point, visitedPoints));
            }
        }

        return regions.Select(x => x.Sides * x.Area).Sum();
    }

    private RegionTwo GetRegionTwo(Point point, HashSet<Point> visitedPoints)
    {
        var letter = Input.Lines[point.Y][point.X];

        var openPointQueue = new Queue<Point>();
        openPointQueue.Enqueue(point);

        var area = 0;

        var regionPoints = new HashSet<Point>();
        while (openPointQueue.TryDequeue(out var openPoint))
        {
            if (visitedPoints.Contains(openPoint))
            {
                continue;
            }

            var adjacentPoints = FindAdjacentPointsWithLetter(openPoint, letter);

            foreach (var adjacentPoint in adjacentPoints)
            {
                openPointQueue.Enqueue(adjacentPoint);
            }

            area++;
            visitedPoints.Add(openPoint);
            regionPoints.Add(openPoint);
        }

        var sides = CalculateSides(regionPoints);

        return new RegionTwo(letter, sides, area);
    }


    private int CalculateSidesSideOnly(HashSet<Point> regionPoints)
    {
        var sides = 0;

        foreach (var point in regionPoints)
        {
            // Up
            if (!regionPoints.Contains(point with {Y = point.Y - 1}))
            {
                sides++;
            }

            // Right
            if (!regionPoints.Contains(point with {X = point.X + 1}))
            {
                sides++;
            }

            // Down
            if (!regionPoints.Contains(point with {Y = point.Y + 1}))
            {
                sides++;
            }

            // Left
            if (!regionPoints.Contains(point with {Y = point.Y + 1}))
            {
                sides++;
            }

        }

        return sides;
    }

    private int CalculateSides(HashSet<Point> regionPoints)
    {

        var adjacent = regionPoints
            .SelectMany(x => new List<(Point Point, Direction Direction)>
            {
                (x with { Y = x.Y - 1 }, Direction.Up),
                (x with { X = x.X + 1 }, Direction.Right),
                (x with { Y = x.Y + 1 }, Direction.Down),
                (x with { X = x.X - 1 }, Direction.Left)
            })
            .Where(x => !regionPoints.Contains(x.Point))
            .OrderBy(x => x.Direction)
            .ThenBy(x => x.Direction switch
            {
                Direction.Up => (x.Point.Y, x.Point.X),
                Direction.Right => (x.Point.X, x.Point.Y),
                Direction.Down => (x.Point.Y, -x.Point.X),
                Direction.Left => (x.Point.X, -x.Point.Y),
            });

        var sides = 0;
        (Point Point, Direction Direction)? previous = null;

        foreach (var current in adjacent)
        {
            if (previous == null ||
                current.Direction != previous.Value.Direction ||
                current.Direction switch
                {
                    Direction.Left => (current.Point.X, current.Point.Y) != (previous.Value.Point.X, previous.Value.Point.Y - 1),
                    Direction.Up => (current.Point.X, current.Point.Y) != (previous.Value.Point.X + 1, previous.Value.Point.Y),
                    Direction.Right => (current.Point.X, current.Point.Y) != (previous.Value.Point.X, previous.Value.Point.Y + 1),
                    Direction.Down => (current.Point.X, current.Point.Y) != (previous.Value.Point.X - 1, previous.Value.Point.Y),
                })
            {
                sides++;
            }

            previous = current;
        }

        return sides;
    }

    private int CalculateSidesShit(Point startingPosition, HashSet<Point> points)
    {
        var letter = Input.Lines[startingPosition.Y][startingPosition.X];
        var direction = Direction.Up;
        var position = startingPosition;

        var sides = 0;
        

        var visitedPoints = new HashSet<Point>();
        while (true)
        {
            var currentDirection = direction;
            Print(visitedPoints, position, direction, letter);
            var nextPosition = direction switch
            {
                Direction.Up => position with {Y = position.Y - 1},
                Direction.Down => position with {Y = position.Y + 1},
                Direction.Left => position with {X = position.X - 1},
                Direction.Right => position with {X = position.X + 1},
            };

            // Every step, move to the right if blocked, but keep looking if left is a block, if yes, go left

            if (nextPosition.Y < 0 || nextPosition.X < 0 || nextPosition.Y >= Input.Lines.Length || nextPosition.X >= Input.Lines[nextPosition.Y].Length ||
                Input.Lines[nextPosition.Y][nextPosition.X] != letter)
            {
                // Out of bounds -> Side found
                sides++;
                direction = (Direction) ((int) (direction + 1) % 4);

                // Okay, no blockers here. Before we advance, check if there is a valid left leaning position
                Point? leftLeaningPosition = direction switch
                {
                    Direction.Left => position with {X = position.X + 1},
                    Direction.Right => position with {X = position.X - 1},
                    _ => null
                };

                if (leftLeaningPosition.HasValue && !(leftLeaningPosition.Value.Y < 0 || leftLeaningPosition.Value.X < 0 || leftLeaningPosition.Value.Y >= Input.Lines.Length ||
                                                     leftLeaningPosition.Value.X >= Input.Lines[leftLeaningPosition.Value.Y].Length ||
                                                     Input.Lines[leftLeaningPosition.Value.Y][leftLeaningPosition.Value.X] != letter))
                {
                    // Valid left leaning position, perfect - Switch directions again
                    direction = (Direction) ((int) (direction + 2) % 4);
                }

                if (direction == Direction.Up && position == startingPosition)
                {
                    if (currentDirection != Direction.Up && visitedPoints.Count > 0)
                    {
                        sides++;
                    }
                    break;
                }

                continue;
            }

            visitedPoints.Add(position);
            position = nextPosition;

            if (position == startingPosition)
            {
                if (currentDirection != Direction.Up)
                {
                    sides++;
                }
                break;
            }
        }

        return sides;
    }


    private RegionOne GetRegionOne(Point point, HashSet<Point> visitedPoints)
    {
        var letter = Input.Lines[point.Y][point.X];

        var openPointQueue = new Queue<Point>();
        openPointQueue.Enqueue(point);

        var perimeter = 0;
        var area = 0;
        while (openPointQueue.TryDequeue(out var openPoint))
        {
            if (visitedPoints.Contains(openPoint))
            {
                continue;
            }

            var adjacentPoints = FindAdjacentPointsWithLetter(openPoint, letter);

            var potentialPerimeter = 4;
            foreach (var adjacentPoint in adjacentPoints)
            {
                potentialPerimeter--;
                openPointQueue.Enqueue(adjacentPoint);
            }

            perimeter += potentialPerimeter;
            area++;
            visitedPoints.Add(openPoint);
        }


        return new RegionOne(area, perimeter);
    }

    private IEnumerable<Point> FindAdjacentPointsWithLetter(Point pos, char letter)
    {
        if (pos.X + 1 < Input.Lines[pos.Y].Length && Input.Lines[pos.Y][pos.X + 1] == letter)
        {
            yield return pos with {X = pos.X + 1};
        }

        if (pos.X - 1 >= 0 && Input.Lines[pos.Y][pos.X - 1] == letter)
        {
            yield return pos with {X = pos.X - 1};
        }

        if (pos.Y + 1 < Input.Lines.Length && Input.Lines[pos.Y + 1][pos.X] == letter)
        {
            yield return pos with {Y = pos.Y + 1};
        }

        if (pos.Y - 1 >= 0 && Input.Lines[pos.Y - 1][pos.X] == letter)
        {
            yield return pos with {Y = pos.Y - 1};
        }
    }


    private void Print(HashSet<Point> visitedPositions, Point position, Direction direction, char letter)
    {
        for (var y = 0; y < Input.Lines.Length; y++)
        {
            for (var x = 0; x < Input.Lines[y].Length; x++)
            {
                var chr = '.';

                if (visitedPositions.Contains(new Point(x, y)))
                {
                    chr = letter;
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