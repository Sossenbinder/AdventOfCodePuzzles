using AdventOfCodeSupport;

namespace AdventOfCodePuzzles._2025;

internal sealed class Day02 : BenchmarkableBase
{
    public Day02()
    {
        //SetTestInput("11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124");
    }

    protected override object InternalPart1()
    {
        var textSpan = Input.Text.AsSpan();
        var rangeSpans = textSpan.Split(',');

        long invalidIdCount = 0;
        foreach (var rangeSpan in rangeSpans)
        {
            var range = textSpan[rangeSpan];
            var dashIndex = range.IndexOf('-');
            var start = long.Parse(range[..dashIndex]);
            var end = long.Parse(range[(dashIndex + 1)..]);

            for (var i = start; i <= end; i++)
            {
                var str = i.ToString();

                if (str.Length % 2 != 0)
                {
                    continue;
                }
                
                if (HasTwiceRepeatedSequence(str))
                {
                    invalidIdCount += i;
                }
            }
        }

        return invalidIdCount;
    }

    protected override object InternalPart2()
    {
        var textSpan = Input.Text.AsSpan();
        var rangeSpans = textSpan.Split(',');

        long invalidIdCount = 0;
        foreach (var rangeSpan in rangeSpans)
        {
            var range = textSpan[rangeSpan];
            var dashIndex = range.IndexOf('-');
            var start = long.Parse(range[..dashIndex]);
            var end = long.Parse(range[(dashIndex + 1)..]);

            for (var i = start; i <= end; i++)
            {
                var str = i.ToString();
                
                if (HasArbitrarilyRepeatedSequence(str))
                {
                    invalidIdCount += i;
                }
            }
        }

        return invalidIdCount;
    }

    private static bool HasTwiceRepeatedSequence(string number)
    {
        var nrSpan = number.AsSpan();
        var center = number.Length / 2;
        var left = nrSpan[..center];
        var right = nrSpan[center..];
        return left.SequenceEqual(right);
    }
    
    private static bool HasArbitrarilyRepeatedSequence(string number)
    {
        var numLen = number.Length;
        var pivot = numLen / 2;
        while (pivot > 0)
        {
            if (numLen % pivot != 0)
            {
                pivot--;
                continue;
            }

            var slices = number.Chunk(pivot).ToList();
            
            if (slices.All(s => s.SequenceEqual(slices[0])))
            {
                return true;
            }
            
            pivot--;
        }

        return false;
    }
}