using AdventOfCodeSupport;

namespace AdventOfCodePuzzles._2025;

internal sealed class Day08 : TestableBenchmarkableBase
{
    private readonly record struct Point(int X, int Y, int Z);

    private record Circuit(Guid Id);
    
    protected override object InternalPart1()
    {
        var points = ParsePoints();

        var pointPermutations = GetPointPermutations(points);
        
        var pointPermutationDistances = new Dictionary<(Point From, Point To), double>();
        foreach (var (from, to) in pointPermutations)
        {
            var distance = Math.Sqrt(
                Math.Pow(to.X - from.X, 2) +
                Math.Pow(to.Y - from.Y, 2) +
                Math.Pow(to.Z - from.Z, 2)
            );
            pointPermutationDistances[(from, to)] = distance;
        }

        var ascendingDistances = pointPermutationDistances
            .DistinctBy(x => x.Value)
            .OrderBy(x => x.Value).ToList();

        var circuitLookup = ascendingDistances
            .SelectMany(x => new List<Point>(){ x.Key.From, x.Key.To})
            .Distinct()
            .ToDictionary(x => x, x => new Circuit(Guid.NewGuid()));

        var ctr = 0;
        const int connections = 10;
        for (var i = connections; i > 0; i--)
        {
            var point = ascendingDistances[ctr];
            var (from, to) = point.Key;

            var fromCircuit = circuitLookup[from];
            var toCircuit = circuitLookup[to];
            
            if (fromCircuit.Id == toCircuit.Id)
            {
                ctr++;
                continue;
            }

            var mergedCircuit = fromCircuit;
            
            circuitLookup[from] = mergedCircuit;
            circuitLookup[to] = mergedCircuit;
            
            foreach (var circuit in circuitLookup.Where(x => x.Value.Id == toCircuit.Id).ToList())
            {
                circuitLookup[circuit.Key] = mergedCircuit;
            }
            
            ctr++;
        }

        var occurences = circuitLookup
            .GroupBy(x => x.Value.Id)
            .Select(x => x.Count());
        
        return occurences.OrderByDescending(x => x).Take(3).Aggregate(1, (current, next) => current * next);
    }

    protected override object InternalPart2()
    {
        var points = ParsePoints();

        var pointPermutations = GetPointPermutations(points);
        
        var pointPermutationDistances = new Dictionary<(Point From, Point To), double>();
        foreach (var (from, to) in pointPermutations)
        {
            var distance = Math.Sqrt(
                Math.Pow(to.X - from.X, 2) +
                Math.Pow(to.Y - from.Y, 2) +
                Math.Pow(to.Z - from.Z, 2)
            );
            pointPermutationDistances[(from, to)] = distance;
        }

        var ascendingDistances = pointPermutationDistances
            .DistinctBy(x => x.Value)
            .OrderBy(x => x.Value).ToList();

        var circuitLookup = ascendingDistances
            .SelectMany(x => new List<Point>(){ x.Key.From, x.Key.To})
            .Distinct()
            .ToDictionary(x => x, x => new Circuit(Guid.NewGuid()));

        var ctr = 0;
        while(true)
        {
            var point = ascendingDistances[ctr];
            var (from, to) = point.Key;

            var fromCircuit = circuitLookup[from];
            var toCircuit = circuitLookup[to];
            
            if (fromCircuit.Id == toCircuit.Id)
            {
                ctr++;
                continue;
            }

            var mergedCircuit = fromCircuit;
            
            circuitLookup[from] = mergedCircuit;
            circuitLookup[to] = mergedCircuit;
            
            foreach (var circuit in circuitLookup.Where(x => x.Value.Id == toCircuit.Id).ToList())
            {
                circuitLookup[circuit.Key] = mergedCircuit;
            }
            
            if (circuitLookup.All(x => x.Value.Id == mergedCircuit.Id))
            {
                return from.X * to.X;
            }
            
            ctr++;
        }
    }

    private List<Point> ParsePoints()
    {
        return Input.Lines.Select(x => x.Split(',')).Select(x => new Point(
            int.Parse(x[0]),
            int.Parse(x[1]),
            int.Parse(x[2])
        )).ToList();
    }

    private static List<(Point From, Point To)> GetPointPermutations(List<Point> points)
    {
        var permutations = new List<(Point From, Point To)>();

        for (var i = 0; i < points.Count; ++i)
        {
            for (var j = 0; j < points.Count; ++j)
            {
                if (i == j)
                {
                    continue;
                }

                permutations.Add((points[i], points[j]));
            }
        }

        return permutations;
    }
    

    protected override bool UseDayTestInput => false;
    protected override string DayTestInput =>
        """
        162,817,812
        57,618,57
        906,360,560
        592,479,940
        352,342,300
        466,668,158
        542,29,236
        431,825,988
        739,650,466
        52,470,668
        216,146,977
        819,987,18
        117,168,530
        805,96,715
        346,949,466
        970,615,88
        941,993,340
        862,61,35
        984,92,344
        425,690,689
        """;
}