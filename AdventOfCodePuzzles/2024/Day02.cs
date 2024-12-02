using AdventOfCodeSupport;

namespace AdventOfCodePuzzles._2024;

internal sealed class Day02 : BenchmarkableBase
{
    private readonly record struct Pair(
        int Left,
        int Right);
    
    protected override object InternalPart1()
    {
        return Input
            .Lines
            .Select(line =>
            {
                var lineItems = line.Split(" ").Select(int.Parse).ToArray();
                return CheckPairs(lineItems.Zip(lineItems[1..], (a, b) => new Pair(a, b)));
            })
            .Sum(x => x ? 1 : 0);
    }

    protected override object InternalPart2()
    {
        return Input
            .Lines
            .Select(line =>
            {
                var lineItems = line.Split(" ").Select(int.Parse).ToArray();
                var pairs = lineItems.Zip(lineItems[1..], (a, b) => new Pair(a, b)).ToList();

                if (CheckPairs(pairs))
                {
                    return true;
                }

                return lineItems
                    .Select((_, i) =>
                    {
                        List<int> shortenedLineItems = [..lineItems[..i], ..lineItems[(i + 1)..]];
                        return shortenedLineItems.Zip(shortenedLineItems[1..], (a, b) => new Pair(a, b)).ToList();
                    })
                    .Any(CheckPairs);
            })
            .Sum(x => x ? 1 : 0);
    }

    private static bool CheckPairs(IEnumerable<Pair> pairs)
    {
        var deltas = pairs.Select(x => x.Right - x.Left).ToArray();
                
        return deltas.All(x => Math.Abs(x) is < 4 and > 0) && (deltas.All(x => x > 0) || deltas.All(x => x < 0));
    }

    protected override object InternalOptimizedPart1() => InternalPart1();

    protected override object InternalOptimizedPart2() => InternalPart2();
}