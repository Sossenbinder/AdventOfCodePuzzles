namespace AdventOfCodePuzzles._2024;

internal sealed class Day10 : BenchmarkableBase
{
    private readonly record struct Point(int X, int Y, int Score);

    private readonly HashSet<Point> _trailHeads = [];
    
    protected override void InternalOnLoad()
    {
        for (var y = 0; y < Input.Lines.Length; ++y)
        {
            for (var x = 0; x < Input.Lines[y].Length; ++x)
            {
                if (Input.Lines[y][x] is not '0')
                {
                    continue;
                }
                
                _trailHeads.Add(new Point(x, y, 0));
            }
        }
    }

    protected override object InternalPart1()
    {
        var score = 0;

        foreach (var trailHead in _trailHeads)
        {
            var visitedPoints = new HashSet<Point>();
            var openPoints = new Queue<Point>();
            openPoints.Enqueue(trailHead);

            while (openPoints.TryDequeue(out var nextPoint))
            {
                if (nextPoint.Score == 9)
                {
                    score++;
                    continue;
                }
                
                var adjacentPoints = FindAdjacentPointsWithScore(nextPoint, nextPoint.Score + 1);

                foreach (var adjacentPoint in adjacentPoints)
                {
                    if (!visitedPoints.Add(adjacentPoint))
                    {
                        continue;
                    }
                    
                    openPoints.Enqueue(adjacentPoint);
                }
            }

        }

        return score;
    }

    protected override object InternalPart2()
    {
        var validStartToEnd = new HashSet<(Point Start, Point End, HashSet<Point> VisitedPoints)>();

        foreach (var trailHead in _trailHeads)
        {
            var visitedPoints = new HashSet<Point> {trailHead};
            var openPoints = new Queue<Point>();
            openPoints.Enqueue(trailHead);

            var finalPoints = new HashSet<Point>();
            while (openPoints.TryDequeue(out var nextPoint))
            {
                if (nextPoint.Score == 9)
                {
                    finalPoints.Add(nextPoint);
                    continue;
                }
                
                var adjacentPoints = FindAdjacentPointsWithScore(nextPoint, nextPoint.Score + 1);

                foreach (var adjacentPoint in adjacentPoints)
                {
                    if (!visitedPoints.Add(adjacentPoint))
                    {
                        continue;
                    }
                    
                    openPoints.Enqueue(adjacentPoint);
                }
            }

            foreach (var finalPoint in finalPoints)
            {
                validStartToEnd.Add((trailHead, finalPoint, visitedPoints));
            }
        }

        var validPaths = 0;
        foreach (var (start, end, visitedPoints) in validStartToEnd)
        {
            validPaths += GetValidUniquePaths(end, start, 8, visitedPoints);
        }

        return validPaths;
    }

    private int GetValidUniquePaths(Point start, Point end, int score, HashSet<Point> visitedPoints)
    {
        if (start.Score == 0)
        {
            return 1;
        }
        
        var sum = 0;
        foreach (var adjacentPoint in FindAdjacentPointsWithScore(start, score))
        {
            if (!visitedPoints.Contains(adjacentPoint))
            {
                continue;
            }

            sum += GetValidUniquePaths(adjacentPoint, end, score - 1, visitedPoints);
        }

        return sum;
    }

    private IEnumerable<Point> FindAdjacentPointsWithScore(Point pos, int score)
    {
        var scoreChar = (char)(score + '0');

        if (pos.X + 1 < Input.Lines[pos.Y].Length && Input.Lines[pos.Y][pos.X + 1] == scoreChar)
        {
            yield return pos with {X = pos.X + 1, Score = score};
        }
        
        if (pos.X - 1 >= 0 && Input.Lines[pos.Y][pos.X - 1] == scoreChar)
        {
            yield return pos with {X = pos.X - 1, Score = score};
        }
        
        if (pos.Y + 1 < Input.Lines.Length && Input.Lines[pos.Y + 1][pos.X] == scoreChar)
        {
            yield return pos with {Y = pos.Y + 1, Score = score};
        }
        
        if (pos.Y - 1 >= 0 && Input.Lines[pos.Y - 1][pos.X] == scoreChar)
        {
            yield return pos with {Y = pos.Y - 1, Score = score};
        }
    }
}