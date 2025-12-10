namespace AdventOfCodePuzzles._2025;

internal sealed class Day04 : BenchmarkableBase
{
    protected override bool UseTestInput() => false;

    protected override string TestInput()
    {
        return 
            """
            ..@@.@@@@.
            @@@.@.@.@@
            @@@@@.@.@@
            @.@@@@..@.
            @@.@@@@.@@
            .@@@@@@@.@
            .@.@.@.@@@
            @.@@@.@@@@
            .@@@@@@@@.
            @.@.@@@.@.
            """;
    }

    private readonly record struct Point(int X, int Y);

    private static bool IsPaper(char[][] input, Point point)
    {
        if (point.Y < 0 || point.Y >= input.Length || point.X < 0 || point.X >= input[point.Y].Length)
        {
            return false;
        }
        
        return input[point.Y][point.X] == '@';
    }

    private static bool CheckIfAccessible(char[][] input, int x, int y)
    {
        Span<Point> adjacentPoints = [
            new(x - 1, y - 1),
            new(x - 1, y),
            new(x - 1, y + 1),
            new(x, y + 1),
            new(x + 1, y + 1),
            new(x + 1, y),
            new(x + 1, y - 1),
            new(x, y - 1),
        ];

        var adjacentPaperCount = 0;

        foreach (var adjacentPoint in adjacentPoints)
        {
            if (IsPaper(input, adjacentPoint))
            {
                adjacentPaperCount++;
            }
            
            if (adjacentPaperCount >= 4)
            {
                return false;
            }
        }

        return true;
    }
    
    protected override object InternalPart1()
    {
        var rolls = 0;
        var input = Input.Lines.Select(x => x.ToCharArray()).ToArray();
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] != '@')
                {
                    continue;
                }
                
                if (CheckIfAccessible(input, x, y))
                {
                    rolls++;
                }
            }
        }

        return rolls;
    }

    protected override object InternalPart2()
    {
        var rolls = 0;

        var input = Input.Lines.Select(x => x.ToCharArray()).ToArray();
        bool didCleanUp;
        do
        {
            didCleanUp = false;
            for (var y = 0; y < input.Length; y++)
            {
                for (var x = 0; x < input[y].Length; x++)
                {
                    if (input[y][x] != '@')
                    {
                        continue;
                    }

                    if (CheckIfAccessible(input, x, y))
                    {
                        didCleanUp = true;
                        rolls++;
                        input[y][x] = '.';
                    }
                }
            }
        } while (didCleanUp);

        return rolls;
    }
}