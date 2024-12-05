namespace AdventOfCodePuzzles._2024;


internal sealed class Day05 : BenchmarkableBase
{
    private readonly record struct OrderingRule(
        uint Left,
        uint Right);

    private readonly record struct Update(List<uint> Sequence);
    
    private readonly List<OrderingRule> _orderingRules = [];
    private readonly List<Update> _updates = [];
    
    protected override void InternalOnLoad()
    {
        var i = 0;
        for (; i < Input.Lines.Length; i++)
        {
            var line = Input.Lines[i];
            if (line.Length == 0)
            {
                ++i;
                break;
            }

            var rawOrdering = line.AsSpan();
            var splitEnumerator = rawOrdering.Split('|');

            splitEnumerator.MoveNext();
            var left = uint.Parse(rawOrdering[splitEnumerator.Current]);
            splitEnumerator.MoveNext();
            var right = uint.Parse(rawOrdering[splitEnumerator.Current]);
            _orderingRules.Add(new OrderingRule(left, right));
        }

        for (; i < Input.Lines.Length; i++)
        {
            var list = new List<uint>();

            var line = Input.Lines[i];
            foreach (var segment in line.AsSpan().Split(','))
            {
                list.Add(uint.Parse(line[segment]));
            }

            _updates.Add(new Update(list));
        }
    }

    protected override object InternalPart1()
    {
        var validUpdates = new List<Update>();

        foreach (var update in _updates)
        {
            var occurences = update.Sequence.ToHashSet();

            var activatedRules = _orderingRules.Where(r => occurences.Contains(r.Left) && occurences.Contains(r.Right))
                .GroupBy(x => x.Right)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Left).ToHashSet());

            var valid = true;
            for (var i = update.Sequence.Count - 1; i >= 0; i--)
            {
                var item = update.Sequence[i];
                var beforeItems = update.Sequence[..i].ToHashSet();

                if (!activatedRules.TryGetValue(item, out var ruleDictatedBeforeSet))
                {
                    continue;
                }
                
                valid = ruleDictatedBeforeSet.IsSubsetOf(beforeItems);

                if (!valid)
                {
                    break;
                }
            }

            if (!valid)
            {
                continue;
            }
            
            validUpdates.Add(update);
        }
        
        return validUpdates
            .Select(x =>
            {
                var index = x.Sequence.Count / 2;
                return x.Sequence[index];
            })
            .Sum(x => x);
    }

    protected override object InternalPart2()
    {
        
        var invalidUpdates = new List<Update>();

        foreach (var update in _updates)
        {
            var sequence = update.Sequence;
            var occurences = sequence.ToHashSet();

            var activeRules = _orderingRules.Where(r => occurences.Contains(r.Left) && occurences.Contains(r.Right))
                .GroupBy(x => x.Right)
                .ToDictionary(x => x.Key, x => x.ToList());

            var valid = OrderUpdate(sequence, activeRules);

            if (valid)
            {
                continue;
            }
            
            invalidUpdates.Add(update);
            while (!valid)
            {
                valid = OrderUpdate(sequence, activeRules);
            }
        }
        
        return invalidUpdates
            .Select(x =>
            {
                var index = x.Sequence.Count / 2;
                return x.Sequence[index];
            })
            .Sum(x => x);
    }

    private static bool OrderUpdate(List<uint> sequence, Dictionary<uint, List<OrderingRule>> activeRules)
    {
        var valid = true;
        for (var sequenceIndex = sequence.Count - 1; sequenceIndex >= 0; sequenceIndex--)
        {
            var item = sequence[sequenceIndex];
            var beforeItems = sequence[..sequenceIndex].ToHashSet();

            if (!activeRules.TryGetValue(item, out var indexSpecificBeforeRules))
            {
                continue;
            }

            foreach (var rule in indexSpecificBeforeRules)
            {
                var requiredBeforeItem = rule.Left;

                if (beforeItems.Contains(requiredBeforeItem))
                {
                    continue;
                }
                        
                // Broken rule, now fix it
                if (valid)
                {
                    valid = false;
                }
                    
                var requiredIndex = sequence.IndexOf(requiredBeforeItem);
                (sequence[requiredIndex], sequence[sequenceIndex]) = (sequence[sequenceIndex], sequence[requiredIndex]);
            }
        }

        return valid;
    }
}