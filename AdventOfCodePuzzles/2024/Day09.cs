using System.Text;

namespace AdventOfCodePuzzles._2024;

public sealed class Day09 : BenchmarkableBase
{
    protected override object InternalPart1()
    {
        var result = new List<double>();

        var recipe = Input.Text;

        var counter = 0;
        for (var i = 0; i < recipe.Length; ++i)
        {
            var number = recipe[i] - '0'; 
            var isFile = i % 2 == 0;

            for (var x = 0; x < number; ++x)
            {
                result.Add(isFile ? counter : -1);
            }

            if (isFile)
            {
                counter++;
            }
        }

        var left = 0;
        var right = result.Count - 1;

        while (left < right)
        {
            var leftItem = result[left];
            var rightItem = result[right];

            if (leftItem == -1 && rightItem != -1)
            {
                // Swap
                (result[left], result[right]) = (result[right], result[left]);
                left++;
                right--;
                continue;
            }

            if (leftItem != -1)
            {
                left++;
            }

            if (rightItem == -1)
            {
                right--;
            }
        }

        var sum = 0D;
        for (var i = 0; i < result.Count; ++i)
        {
            if (result[i] == -1)
            {
                return sum;
            }
            
            sum += i * result[i];
        }
        
        return sum;
    }

    private readonly record struct Segment(
        int Start,
        int Stop);
    protected override object InternalPart2()
    {
        var result = new List<double>();

        var recipe = Input.Text;

        var freeSpaces = new List<Segment>();
        
        var counter = 0;
        for (var i = 0; i < recipe.Length; ++i)
        {
            var number = recipe[i] - '0'; 
            var isFile = i % 2 == 0;

            if (!isFile)
            {
                freeSpaces.Add(new Segment(result.Count,result.Count + number));
            }
            
            for (var x = 0; x < number; ++x)
            {
                result.Add(isFile ? counter : -1);
            }

            if (isFile)
            {
                counter++;
            }
        }

        var segmentsToMoveAhead = new Queue<Segment>();
        for (var i = result.Count - 1; i >= 0; --i)
        {
            var number = result[i];
            
            if (number == -1)
            {
                continue;
            }

            var j = i;
            for (; j >= 1; --j)
            {
                if (result[j - 1] != number)
                {
                    break;
                }
            }

            var segmentStart = j;
            var segmentSize = i - segmentStart;
            
            segmentsToMoveAhead.Enqueue(new Segment(segmentStart, segmentStart + segmentSize));
            
            i = j;
        }

        while (segmentsToMoveAhead.TryDequeue(out var segmentToMoveAhead))
        {
            var segmentToMoveAheadSize = segmentToMoveAhead.Stop - segmentToMoveAhead.Start + 1;
            for (var i = 0; i < freeSpaces.Count; i++)
            {
                var freeSpaceSegment = freeSpaces[i];
                var freeSpaceSegmentSize = freeSpaceSegment.Stop - freeSpaceSegment.Start;
                if (freeSpaceSegmentSize == 0)
                {
                    continue;
                }

                if (segmentToMoveAheadSize <= freeSpaceSegmentSize && freeSpaceSegment.Start < segmentToMoveAhead.Start)
                {
                    for (var l = 0; l < segmentToMoveAheadSize; ++l)
                    {
                        (result[segmentToMoveAhead.Start + l], result[freeSpaceSegment.Start + l]) = (result[freeSpaceSegment.Start + l], result[segmentToMoveAhead.Start + l]);
                    }

                    freeSpaces[i] = freeSpaceSegment with {Start = freeSpaceSegment.Start  + segmentToMoveAheadSize};
                    break;
                }
            }
        }
            
        var sum = 0D;
        for (var i = 0; i < result.Count; ++i)
        {
            if (result[i] == -1)
            {
                continue;
            }
            
            sum += i * result[i];
        }

        return sum;
    }
}