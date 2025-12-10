namespace AdventOfCodePuzzles._2025;

internal sealed class Day05 : BenchmarkableBase
{
    override protected bool UseTestInput() => false;

    override protected string TestInput() 
        => """
        3-5
        10-14
        16-20
        12-18
        
        1
        5
        8
        11
        17
        32
        """;

    private readonly record struct IngRange(
        long From,
        long To)
    {
        public bool Contains(long ingredientId)
            => ingredientId >= From && ingredientId <= To;
    }
    
    protected override object InternalPart1()
    {
        var parsedInput = ParseInput();
        
        return parsedInput.AvailableIngredients.Count(availableIngredient => parsedInput.FreshRanges.Any(freshRange => freshRange.Contains(availableIngredient)));
    }

    protected override object InternalPart2()
    {
        var parsedInput = ParseInput();

        var sortedRanges = parsedInput.FreshRanges.OrderBy(x => x.From).ToList();
        
        long fresh = 0;
        long position = sortedRanges[0].From;

        foreach (var range in sortedRanges)
        {
            if (range.From > position)
            {
                position = range.From;
            }
            if (range.To < position)
            {
                continue;
            }
            fresh += (range.To - position) + 1;

            position = range.To + 1;
        }

        return fresh;
    }

    private (List<IngRange> FreshRanges, List<long> AvailableIngredients) ParseInput()
    {
        var freshRanges = new List<IngRange>();

        var lineIndex = 0;
        while (Input.Lines[lineIndex] != "")
        {
            var line = Input.Lines[lineIndex];
            var split = line.Split('-');
            var from = long.Parse(split[0]);
            var to = long.Parse(split[1]);
            
            freshRanges.Add(new (from, to));
            lineIndex++;
        }
        lineIndex++;
        
        var availableIngredients = new List<long>();

        while (lineIndex < Input.Lines.Length)
        {
            var line = Input.Lines[lineIndex];
            availableIngredients.Add(long.Parse(line));
            lineIndex++;
        }
        
        return (freshRanges, availableIngredients);
    }
}