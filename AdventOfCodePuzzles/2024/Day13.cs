namespace AdventOfCodePuzzles._2024;

internal sealed class Day13 : BenchmarkableBase
{
    private readonly record struct Button(
        int XIncrement,
        int YIncrement);

    private readonly record struct Prize(
        long X,
        long Y);

    private readonly record struct Instruction(
        Button A,
        Button B,
        Prize Prize);

    private readonly record struct PressCount(
        long A,
        long B);

    private readonly List<Instruction> _instructions = [];

    protected override void InternalOnLoad()
    {
        for (var i = 0; i < Input.Lines.Length; i++)
        {
            var aLine = Input.Lines[i].Split(':')[1].Split(',').Select(x => x.Trim()).ToArray();
            var buttonA = new Button(int.Parse(aLine[0][(aLine[0].IndexOf('+') + 1)..]), int.Parse(aLine[1][(aLine[1].IndexOf('+') + 1)..]));
            var bLine = Input.Lines[i + 1].Split(':')[1].Split(',').Select(x => x.Trim()).ToArray();
            var buttonB = new Button(int.Parse(bLine[0][(bLine[0].IndexOf('+') + 1)..]), int.Parse(bLine[1][(bLine[1].IndexOf('+') + 1)..]));
            var prizeLine = Input.Lines[i + 2].Split(':')[1].Split(',').Select(x => x.Trim()).ToArray();
            var prize = new Prize(long.Parse(prizeLine[0][(prizeLine[0].IndexOf('=') + 1)..]), long.Parse(prizeLine[1][(prizeLine[1].IndexOf('=') + 1)..]));
            
            _instructions.Add(new Instruction(buttonA, buttonB, prize with { Y = prize.Y + 10000000000000, X = prize.X + 10000000000000 }));
            i += 3;
        }
    }

    protected override object InternalPart1()
    {
        var tokenCount = 0L;
        
        foreach (var instruction in _instructions)
        {
            var explorationSet = new HashSet<PressCount>();
            var smallestSolution = FindEquationSolution(instruction, new(0, 0), explorationSet);

            if (!smallestSolution.HasValue)
            {
                continue;
            }
            
            tokenCount += smallestSolution.Value.A * 3 + smallestSolution.Value.B;
        }
        
        return tokenCount;
    }

    protected override object InternalPart2()
    {
        var tokenCount = 0L;
        
        foreach (var instruction in _instructions)
        {
            var smallestSolution = SolveWithCramersRule(instruction);

            if (!smallestSolution.HasValue)
            {
                continue;
            }
            
            tokenCount += smallestSolution.Value.A * 3 + smallestSolution.Value.B;
        }
        
        return tokenCount;
    }

    private static PressCount? SolveWithCramersRule(Instruction instruction)
    {
        var ax = instruction.A.XIncrement;
        var ay = instruction.A.YIncrement;
        var bx = instruction.B.XIncrement;
        var by = instruction.B.YIncrement;
        var px = instruction.Prize.X;
        var py = instruction.Prize.Y;

        var determinant = ax * by - bx * ay;
        if (determinant == 0)
        {
            return null;
        }

        var aDeterminant = px * by - py * bx;
        var bDeterminant = ax * py - ay * px;
        var aPress = (double)aDeterminant / determinant;
        var bPress = (double)bDeterminant / determinant;
        if (aPress % 1 == 0 && bPress % 1 == 0 && aPress >= 0 && bPress >= 0)
        {
            return new PressCount((long)aPress, (long)bPress);
        }

        return null;
    }

    private static PressCount? FindEquationSolution(Instruction instruction, PressCount presses, HashSet<PressCount> explorationSet)
    {
        if (!explorationSet.Add(presses))
        {
            return null;
        }
        
        var xLevel = instruction.A.XIncrement * presses.A + instruction.B.XIncrement * presses.B;
        var yLevel = instruction.A.YIncrement * presses.A + instruction.B.YIncrement * presses.B;

        if (xLevel == instruction.Prize.X && yLevel == instruction.Prize.Y)
        {
            return presses;
        }
        else if (xLevel > instruction.Prize.X || yLevel > instruction.Prize.Y)
        {
            return null;
        }

        var aPressResult = FindEquationSolution(instruction, presses with {A = presses.A + 1}, explorationSet);
        var bPressResult = FindEquationSolution(instruction, presses with {B = presses.B + 1}, explorationSet);

        if (aPressResult is null && bPressResult is null)
        {
            return null;
        }
        else if (aPressResult is null)
        {
            return bPressResult;
        }
        else if (bPressResult is null)
        {
            return aPressResult;
        }
        
        var aPressCount = aPressResult.Value.A + aPressResult.Value.B;
        var bPressCount = bPressResult.Value.A + bPressResult.Value.B;
        
        return aPressCount > bPressCount ? bPressResult : aPressResult;
    }
}