namespace AdventOfCodePuzzles._2024;

internal sealed class Day19 : BenchmarkableBase
{
    private readonly List<string> _towels = [];

    private readonly List<string> _patterns = [];
    
    protected override void InternalOnLoad()
    {
        _towels.AddRange(Input.Lines[0].Split(',').Select(x => x.Trim()));
        
        for (var i = 2; i < Input.Lines.Length; ++i)
        {
            _patterns.Add(Input.Lines[i]);
        }
    }

    protected override object InternalPart1()
    {
        var result = 0;
        
        foreach (var pattern in _patterns)
        {
            result += SolvePatternPart1("", pattern) ? 1 : 0;
        }

        return result;
    }

    protected override object InternalPart2()
    {
        var result = 0L;
        
        foreach (var pattern in _patterns)
        {
            var patternLookup = new Dictionary<string, long>();
            var patternPaths = SolvePatternPart2("", pattern, patternLookup);
            result += patternPaths;
        }

        return result;
    }
    
    private long SolvePatternPart2(string current, string pattern, Dictionary<string, long> patternLookup)
    {
        if (patternLookup.TryGetValue(current, out var count))
        {
            return count;
        }
        
        if (!pattern.StartsWith(current) || current.Length > pattern.Length)
        {
            return 0;
        }

        if (current.Length == pattern.Length && current == pattern)
        {
            return 1;
        }

        var counter = 0L;
        foreach (var towel in _towels)
        {
            counter += SolvePatternPart2(current + towel, pattern, patternLookup);
        }

        patternLookup.Add(current, counter);
        return counter;
    }

    private bool SolvePatternPart1(string current, string pattern)
    {
        if (!pattern.StartsWith(current) || current.Length > pattern.Length)
        {
            return false;
        }

        if (current.Length == pattern.Length)
        {
            return current == pattern;
        }

        foreach (var towel in _towels)
        {
            if (SolvePatternPart1(current + towel, pattern))
            {
                return true;
            }
        }
        
        return false;
    }
}