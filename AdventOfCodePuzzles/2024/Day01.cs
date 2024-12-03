using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AdventOfCodePuzzles._2024;

internal sealed class Day01 : BenchmarkableBase
{
    private readonly record struct Pair(
        uint Left,
        uint Right);
    
    private Pair[] _pairs = null!;
    
    protected override void InternalOnLoad()
    {
        _pairs = Input.Lines
            .Select(x => x.Split("  "))
            .Select(x => new Pair(uint.Parse(x[0]), uint.Parse(x[1])))
            .ToArray();
    }

    protected override object InternalPart1()
    {
        var leftOrdered = _pairs
            .Select(x => x.Left)
            .Order()
            .ToArray()
            .AsSpan();

        var orderedRight = _pairs
            .Select(x => x.Right)
            .Order()
            .ToArray();

        uint sum = 0;
        ref var leftSpace = ref MemoryMarshal.GetReference(leftOrdered);
        for (var i = 0; i < leftOrdered.Length; ++i)
        {
            var item = Unsafe.Add(ref leftSpace, i);
            sum += item > orderedRight[i] ? item - orderedRight[i] : orderedRight[i] - item;
        }
        
        return sum;
    }

    public override object OptimizedPartOne()
    {
        var leftOrdered = _pairs
            .Select(x => x.Left)
            .Order()
            .Zip(_pairs.Select(x => x.Right).Order())
            .Sum(x => Math.Abs(x.First - x.Second));
        
        return leftOrdered;
    }

    protected override object InternalPart2()
    {
        var occurenceMap = _pairs
            .CountBy(x => x.Right)
            .ToDictionary();

        return _pairs
            .Select(x => x.Left * occurenceMap.GetValueOrDefault(x.Left, 0))
            .Sum();
    }

    public override object OptimizedPartTwo()
    {
        var occurenceMap = new Dictionary<uint, uint>(1000);
        
        ref var pairs = ref MemoryMarshal.GetReference<Pair>(_pairs);
        for (var i = 0; i < _pairs.Length; ++i)
        {
            var pair = Unsafe.Add(ref pairs, i);

            uint newValue = 1;
            if (occurenceMap.TryGetValue(pair.Left, out var occurences))
            {
                newValue += occurences;
            }
            occurenceMap[pair.Left] = newValue;
        }
        
        pairs = ref MemoryMarshal.GetReference<Pair>(_pairs);

        uint sum = 0;
        for (var i = 0; i < _pairs.Length; ++i)
        {
            var pair = Unsafe.Add(ref pairs, i);
            var val = pair.Left;
            sum += val * occurenceMap.GetValueOrDefault<uint, uint>(val, 0);
        }

        return sum;
    }
}