namespace AdventOfCodePuzzles._2025;

internal sealed class Day10 : TestableBenchmarkableBase
{
    private readonly record struct IndicatorLightDiagram(List<bool> Lights);

    private readonly record struct WiringSchematics(List<int> Schematics);

    private readonly record struct JoltageRequirements(List<int> Requirements);
    
    private readonly record struct Machine(IndicatorLightDiagram IndicatorLightDiagram, List<WiringSchematics> WiringSchematics, JoltageRequirements JoltageRequirements);
    
    protected override object InternalPart1()
    {
        var machines = ParseMachines();

        var fewestInstructions = 0;

        foreach (var machine in machines)
        {
            fewestInstructions = FindFewestSolution(machine);
        }
        
        return fewestInstructions;
    }

    private int FindFewestSolution(Machine machine)
    {
        throw new NotImplementedException();
    }

    protected override object InternalPart2()
    {
        return 0;
    }

    private List<Machine> ParseMachines()
    {
        var machines = new List<Machine>();

        foreach (var line in Input.Lines)
        {
            var split = line.Split(' ');

            var indicatorLightDiagram = new IndicatorLightDiagram(split[0][1..^1].Select(x => x == '#').ToList());
            var joltageRequirements = new JoltageRequirements(split[^1][1..^1].Split(',').Select(int.Parse).ToList());
            var wiringSchematics = split[1..^1]
                .Select(schematic => new WiringSchematics(schematic[1..^1].Split(',').Select(int.Parse).ToList()))
                .ToList();
            
            machines.Add(new Machine(indicatorLightDiagram, wiringSchematics, joltageRequirements));
        }

        return machines;
    }

    protected override bool UseDayTestInput => true;
    
    protected override string DayTestInput =>
        """
        [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
        [...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}
        [.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}
        """;
}