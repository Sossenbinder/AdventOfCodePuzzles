namespace AdventOfCodePuzzles._2024;

internal sealed class Day07 : BenchmarkableBase
{
    private readonly record struct Equation(
        List<long> Operands,
        long Result);

    private enum Operation
    {
        Add,
        Multiply,
        Concatenate
    }
    
    private readonly List<Equation> _equations = [];

    protected override void InternalOnLoad()
    {
        foreach (var line in Input.Lines)
        {
            var lineSpan = line.AsSpan();
            
            var operandResultSplit = lineSpan.Split(":");
            operandResultSplit.MoveNext();
            var result = long.Parse(lineSpan[operandResultSplit.Current]);
            operandResultSplit.MoveNext();
            var operations = lineSpan[operandResultSplit.Current].Trim();

            var operandsSlice = operations.Split(' ');

            var operands = new List<long>();
            foreach (var operandRange in operandsSlice)
            {
                operands.Add(long.Parse(operations[operandRange]));
            }
            
            _equations.Add(new Equation(operands, result));
        }
    }

    protected override object InternalPart1()
    {
        var maximumCombinationSize = _equations.Max(x => x.Operands.Count) - 1;

        var combinationMap = Enumerable.Range(1, maximumCombinationSize)
            .ToDictionary(x => x, x => GetOperationCombination(x, false));

        var validEquations = new List<Equation>(); 
        foreach (var equation in _equations)
        {
            var operands = equation.Operands;
            var combinations = combinationMap[operands.Count - 1];

            foreach (var combination in combinations)
            {
                var result = operands[0];

                for (var i = 1; i < operands.Count; i++)
                {
                    result = combination[i - 1] switch
                    {
                        Operation.Add => result + operands[i],
                        Operation.Multiply => result * operands[i],
                    };

                    if (result > equation.Result)
                    {
                        break;
                    }
                }

                if (result == equation.Result)
                {
                    validEquations.Add(equation);
                    break;
                }
            }
        }

        return validEquations.Sum(x => x.Result);
    }


    protected override object InternalPart2()
    {
        var maximumCombinationSize = _equations.Max(x => x.Operands.Count) - 1;

        var combinationMap = Enumerable.Range(1, maximumCombinationSize)
            .ToDictionary(x => x, x => GetOperationCombination(x, true));

        var validEquations = new List<Equation>(); 
        foreach (var equation in _equations)
        {
            var baseOperands = equation.Operands;
            var combinations = combinationMap[baseOperands.Count - 1];

            foreach (var combination in combinations)
            {
                var openOperands = new Queue<long>(baseOperands);
                var openOperations = new Queue<Operation>(combination);
                
                var result = openOperands.Dequeue();

                while (openOperations.TryDequeue(out var operation))
                {
                    var operand = openOperands.Dequeue();
                    result = operation switch
                    {
                        Operation.Add => result + operand,
                        Operation.Multiply => result * operand,
                        Operation.Concatenate => long.Parse(result.ToString() + operand),
                    };

                    if (result > equation.Result)
                    {
                        break;
                    }
                }
                
                if (result == equation.Result)
                {
                    validEquations.Add(equation);
                    break;
                }
            }
        }

        return validEquations.Sum(x => x.Result);
    }
    
    private static List<Operation[]> GetOperationCombination(int length, bool includeConcat = false)
    {
        if (length == 1)
        {
            if (!includeConcat)
            {
                return [[Operation.Add], [Operation.Multiply]];
            }
            
            return [[Operation.Add], [Operation.Multiply], [Operation.Concatenate]];
        }
        
        var operationCombinations = new List<Operation[]>();

        GenerateCombinationsRecursive([], length, operationCombinations, includeConcat);
        
        return operationCombinations;
    }

    private static void GenerateCombinationsRecursive(Operation[] current, int remaining, List<Operation[]> results, bool includeConcat  = false)
    {
        if (remaining == 0)
        {
            results.Add(current);
            return;
        }

        GenerateCombinationsRecursive([..current, Operation.Add], remaining - 1, results, includeConcat);
        GenerateCombinationsRecursive([..current, Operation.Multiply], remaining - 1, results, includeConcat);
        if (includeConcat)
        {
            GenerateCombinationsRecursive([..current, Operation.Concatenate], remaining - 1, results, includeConcat);      
        }
    }
}