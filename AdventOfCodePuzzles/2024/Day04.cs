namespace AdventOfCodePuzzles._2024;

enum Direction
{
    NW,
    NE,
    SE,
    SW
}

internal sealed class Day04 : BenchmarkableBase
{
    private string[] Lines => Input.Lines;
    
    protected override object InternalPart1()
    {
        var count = 0;
        for (var y = 0; y < Lines.Length; y++)
        {
            for (var x = 0; x < Lines[y].Length; x++)
            {
                var occurences = FindXmasOccurences(y, x);
                count += occurences;
            }
        }

        return count;
    }

    protected override object InternalPart2()
    {
        var count = 0;
        for (var y = 0; y < Lines.Length; y++)
        {
            for (var x = 0; x < Lines[y].Length; x++)
            {
                if (IsCrossMasOccurences(y, x))
                {
                    count++;
                }
            }
        }

        return count;
    }

    private bool IsCrossMasOccurences(int y, int x)
    {
        if (Lines[y][x] != 'A')
        {
            return false;
        }

        var lines = new List<string>();

        try
        {
            lines.Add(Lines[y - 1][(x - 1)..(x + 2)]);
            lines.Add(Lines[y + 1][(x - 1)..(x + 2)]);
        }
        catch (Exception exc) when (exc is (IndexOutOfRangeException or ArgumentOutOfRangeException))
        {
            return false;
        }
        
        return (lines[0] is ['M', _, 'S'] && lines[1] is ['M', _, 'S'])
            || (lines[0] is ['M', _, 'M'] && lines[1] is ['S', _, 'S'])
            || (lines[0] is ['S', _, 'M'] && lines[1] is ['S', _, 'M'])
            || (lines[0] is ['S', _, 'S'] && lines[1] is ['M', _, 'M']);
    }

    private int FindXmasOccurences(int y, int x)
    {
        if (Lines[y][x] != 'X')
        {
            return 0;
        }

        var starLines = new List<string>();

        void RunIndexSafe(Action action)
        {
            try
            {
                action();
            }
            catch (IndexOutOfRangeException)
            {
            }
        }
        
        // Horizontal
        RunIndexSafe(() => starLines.Add(new string(Lines[y][..x].Reverse().ToArray())));
        RunIndexSafe(() => starLines.Add(Lines[y][(x + 1)..]));
            
        // Vertical
        RunIndexSafe(() => starLines.Add(new string(Lines[..y].Select(line => line[x]).Reverse().ToArray())));
        RunIndexSafe(() => starLines.Add(new string(Lines[(y + 1)..].Select(line => line[x]).ToArray())));

        HashSet<Direction> openDiagDirs = [Direction.NW, Direction.NE, Direction.SE, Direction.SW];

        for (var distance = 1; distance < 4; ++distance)
        {
            var searchedChar = distance switch
            {
                1 => 'M',
                2 => 'A',
                3 => 'S',
            };

            foreach (var dir in openDiagDirs)
            {
                try
                {
                    var item = dir switch
                    {
                        Direction.NW => Lines[y - distance][x - distance],
                        Direction.NE => Lines[y - distance][x + distance],
                        Direction.SE => Lines[y + distance][x + distance],
                        Direction.SW => Lines[y + distance][x - distance],
                    };

                    if (item != searchedChar)
                    {
                        openDiagDirs.Remove(dir);
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    openDiagDirs.Remove(dir);
                }
            }
        }

        return starLines.Count(str => str is ['M', 'A', 'S', ..]) + openDiagDirs.Count;
    }
}