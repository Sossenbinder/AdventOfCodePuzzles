namespace AdventOfCodePuzzles._2024;

internal sealed class Day11 : BenchmarkableBase
{
    private List<ulong> _stones;
    
    protected override void InternalOnLoad()
    {
        _stones = Input.Text.Split(' ').Select(ulong.Parse).ToList();
    }

    protected override object InternalPart1()
    {
        var stones = _stones;
        
        const int blinks = 25;

        foreach (var _ in Enumerable.Range(0, blinks))
        {
            var newStones = new List<ulong>();
            foreach (var stoneValue in stones)
            {
                if (stoneValue == 0)
                {
                    newStones.Add(1);
                    continue;
                }
                
                var digits = stoneValue.ToString().ToCharArray();

                if (digits.Length % 2 == 0)
                {
                    newStones.Add(ulong.Parse(digits.AsSpan()[..(digits.Length / 2)]));
                    newStones.Add(ulong.Parse(digits.AsSpan()[(digits.Length / 2)..]));
                    continue;
                }
                
                newStones.Add(stoneValue * 2024);
            }
            stones = newStones;
        }

        return stones.Count;
    }

    protected override object InternalPart2()
    {
        const int blinks = 75;
        
        var constellation = new Dictionary<ulong, ulong>();
        
        foreach (var stoneValue in _stones)
        {
            constellation[stoneValue] = 1;
        }

        foreach (var _ in Enumerable.Range(0, blinks))
        {
            var newConstellation = new Dictionary<ulong, ulong>();

            foreach (var (value, ocurrences) in constellation)
            {
                if (value == 0)
                {
                    newConstellation.TryAdd(1, 0);
                    newConstellation[1] += ocurrences;
                    continue;
                }

                var digits = value.ToString();
                if (digits.Length % 2 == 0)
                {
                    var left = ulong.Parse(digits.AsSpan()[..(digits.Length / 2)]);
                    var right = ulong.Parse(digits.AsSpan()[(digits.Length / 2)..]);

                    newConstellation.TryAdd(left, 0);
                    newConstellation[left] += ocurrences;

                    newConstellation.TryAdd(right, 0);
                    newConstellation[right] += ocurrences;
                }
                else
                {
                    var newValue = value * 2024;
                    newConstellation.TryAdd(newValue, 0);
                    newConstellation[newValue] += ocurrences;
                }
            }
            constellation = newConstellation;
        }

        return constellation.Values.Aggregate(0UL, (current, val) => current + val);
    }
}