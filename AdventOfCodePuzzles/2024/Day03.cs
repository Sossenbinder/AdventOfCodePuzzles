using System.Text.RegularExpressions;

namespace AdventOfCodePuzzles._2024;

internal sealed partial class Day03 : BenchmarkableBase
{
    protected override object InternalPart1()
    {
        var multiplications = Part1Regex().Matches(Input.Text);

        var sum = 0u;

        foreach (Match match in multiplications)
        {
            var item = match.Groups[0].Value;

            var subset = item[4..^1].Split(',');
            sum += subset.Select(uint.Parse).Aggregate(1u, (curr, prev) => curr * prev);
        }

        return sum;
    }

    protected override object InternalPart2()
    {
        var multiplications = Part2Regex().Matches(Input.Text);

        var sum = 0u;

        var enabled = true;
        foreach (Match match in multiplications)
        {
            var item = match.Groups[0].Value;

            if (item == "do()")
            {
                enabled = true;
            }
            else if (item == "don't()")
            {
                enabled = false;
            }
            else
            {
                if (!enabled)
                {
                    continue;
                }
                
                var subset = item[4..^1].Split(',');
                sum += subset.Select(uint.Parse).Aggregate(1u, (curr, prev) => curr * prev);
            }
        }

        return sum;
    }
    
    [GeneratedRegex(@"mul\([0-9]*,[0-9]*\)")]
    private static partial Regex Part1Regex();
    
    [GeneratedRegex(@"mul\([0-9]*,[0-9]*\)|don't\(\)|do\(\)")]
    private static partial Regex Part2Regex();
}