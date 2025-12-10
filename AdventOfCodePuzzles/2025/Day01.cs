namespace AdventOfCodePuzzles._2025;

public sealed class Day01 : BenchmarkableBase
{
    protected override object InternalPart1()
    {
        const int maxRotation = 100;
        
        var zeroes = 0;
        var currentPosition = 50;
        
        foreach (var line in Input.Lines)
        {
            var span = line.AsSpan();
            var value = int.Parse(span[1..]);
            
            var delta = span[0] == 'R' ? 1 : -1;
            for (var i = value; i > 0; i -= 1)
            {
                currentPosition += delta;
                
                if (currentPosition == -1)
                {
                    currentPosition = 99;
                }
                else if (currentPosition == maxRotation)
                {
                    currentPosition = 0;
                }
            }
            
            if (currentPosition == 0)
            {
                zeroes++;
            }
        }

        return zeroes;
    }


    protected override object InternalPart2()
    {
        const int maxRotation = 100;
        
        var zeroes = 0;
        var currentPosition = 50;
        
        foreach (var line in Input.Lines)
        {
            var span = line.AsSpan();
            var value = int.Parse(span[1..]);
            
            var delta = span[0] == 'R' ? 1 : -1;
            for (var i = value; i > 0; i -= 1)
            {
                currentPosition += delta;
                
                if (currentPosition == -1)
                {
                    currentPosition = 99;
                }
                else if (currentPosition == maxRotation)
                {
                    currentPosition = 0;
                }
                
                if (currentPosition == 0)
                {
                    zeroes++;
                }
            }
        }

        return zeroes;
    }
}