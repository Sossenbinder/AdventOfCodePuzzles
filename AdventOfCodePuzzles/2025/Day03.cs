namespace AdventOfCodePuzzles._2025;

internal sealed class Day03 : BenchmarkableBase
{
    public Day03()
    {
        // SetTestInput(
        //     """
        //     987654321111111
        //     811111111111119
        //     234234234234278
        //     818181911112111
        //     """
        // );
    }
    
    protected override object InternalPart1()
    {
        return Input.Lines.Select(GetPermutations).Select(permutations => permutations.Max()).Sum();
    }

    private static IEnumerable<int> GetPermutations(string line)
    {
        for (var i = 0; i < line.Length - 1; i++)
        {
            for (var j = i; j < line.Length; ++j)
            {
                if (i == j)
                {
                    continue;
                }

                yield return int.Parse($"{line[i]}{line[j]}");
            }
        }
    }

    protected override object InternalPart2()
    {
        long joltageSum = 0;

        foreach (var line in Input.Lines)
        {
            joltageSum += GetMaxJoltageOfLine(line);
        }
        
        return joltageSum;
    }

    private static long GetMaxJoltageOfLine(string line)
    {
        const int targetLength = 12;
        var removalsAllowed = line.Length - targetLength;
        
        var stack = new Stack<char>(line.Length);
        foreach (var digit in line)
        {
            while (removalsAllowed > 0 && stack.Count > 0 && stack.Peek() < digit)
            {
                stack.Pop();
                removalsAllowed--;
            }

            stack.Push(digit);
        }

        while (stack.Count > targetLength)
        {
            stack.Pop();
        }

        var resultBuffer = new char[targetLength];
        for (var i = targetLength - 1; i >= 0; i--)
        {
            resultBuffer[i] = stack.Pop();
        }

        return long.Parse(resultBuffer);
    }
}