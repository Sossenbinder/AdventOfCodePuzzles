namespace AdventOfCodePuzzles._2025;

internal sealed class Day06 : BenchmarkableBase
{
    private enum Operation
    {
        Add,
        Multiply
    }

    private readonly record struct Equation(
        Operation Operation,
        List<long> Operands);
    
    protected override bool UseTestInput() => false;

    protected override string TestInput() => 
        """
        123 328  51 64 
         45 64  387 23 
          6 98  215 314
        *   +   *   +  
        """;

    protected override object InternalPart1()
    {
        var equations = ParseInstructions();

        long sum = 0;
        foreach (var equation in equations)
        {
            sum += equation.Operands[1..].Aggregate(equation.Operands[0], (aggregate, current) => equation.Operation switch
            {
                Operation.Add => aggregate + current,
                Operation.Multiply => aggregate * current,
                _ => throw new InvalidOperationException($"Unknown operation: {equation.Operation}")
            });
        }
        return sum;
    }

    protected override object InternalPart2()
    {
        var equations = ParseInstructionsPart2();

        long sum = 0;
        foreach (var equation in equations)
        {
            sum += equation.Operands[1..].Aggregate(equation.Operands[0], (aggregate, current) => equation.Operation switch
            {
                Operation.Add => aggregate + current,
                Operation.Multiply => aggregate * current,
                _ => throw new InvalidOperationException($"Unknown operation: {equation.Operation}")
            });
        }
        return sum;
    }
    
    
    private List<Equation> ParseInstructionsPart2()
    {
        var equations = new List<Equation>();

        var splits = new List<string[]>();

        var ctr = 0;
        while (ctr < Input.Lines[0].Length)
        {
            var end = Input.Lines[0].Length - 1;
            for (var i = ctr; i < Input.Lines[0].Length; i++)
            {
                if (Input.Lines.All(x => x[i] == ' '))
                {
                    end = i;
                    break;
                }
            }
            
            var segments = Input.Lines
                .Select(x => end == Input.Lines[0].Length - 1 ? x[ctr..] : x[ctr..end])
                .ToArray();
            
            splits.Add(segments);

            ctr = end + 1;
        }
        
        for (var column = 0; column < splits.Count; column++)
        {
            var operands = new List<long>();
            var operation = splits[column][^1].Trim()[0] switch
                {
                    '+' => Operation.Add,
                    '*' => Operation.Multiply,
                };

            var rawOperands = splits[column][..^1];
            
            for (var col = rawOperands[0].Length - 1; col >= 0; col--)
            {
                var nr = "";
                for (var l = 0; l < rawOperands.Length; l++)
                {
                    var cellVal = rawOperands[l][col];
                    if (cellVal != ' ')
                    {
                        nr += cellVal;
                    }
                }
                operands.Add(long.Parse(nr));
            }

            equations.Add(new Equation(operation, operands));
        }
        
        
        return equations;
    }
    
    
    private List<Equation> ParseInstructions()
    {
        var equations = new List<Equation>();
        
        var splits = Input.Lines.Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();

        for (var column = 0; column < splits[0].Length; column++)
        {
            var operands = new List<long>();
            var operation = Operation.Add;
            for (var line = 0; line < splits.Length; line++)
            {
                var val = splits[line][column];
                
                if (line == splits.Length - 1)
                {
                    operation = val[0] switch
                    {
                        '+' => Operation.Add,
                        '*' => Operation.Multiply,
                        _ => throw new InvalidOperationException($"Unknown operation: {val}")
                    };
                }
                else
                {
                    operands.Add(long.Parse(val));
                }
            }

            equations.Add(new Equation(operation, operands));
        }
        
        
        return equations;
    }
}