namespace AdventOfCodePuzzles._2024;

internal sealed class Day17 : BenchmarkableBase
{
    protected override object InternalPart1()
    {
        int[] rawInstructions = [2,4,1,5,7,5,0,3,4,1,1,6,5,5,3,0];
        const int registerA = 47719761;
        var output = new List<int>();
        foreach (var solution in Solve(registerA, rawInstructions.Length, rawInstructions))
        {
            output.Add(solution);
        }

        return string.Join(',', output);
    }

    protected override object InternalPart2()
    {
        int[] rawInstructions = [2,4,1,5,7,5,0,3,4,1,1,6,5,5,3,0];
        const int registerA = 47719761;
        
        // for (var i = 2300000000; true; i++)
        // {
        //     if (i % 100_000_000 == 0)
        //     {
        //         Console.WriteLine(i);
        //     }
        //
        //     if (SolveTwo(i, rawInstructions.Length, rawInstructions))
        //     {
        //         return i;
        //     }
        // }
        
        const int rangeCount = 8;
        const long rangeSize = 999_999_999_999_999L / rangeCount;

        var chunks = new List<(long Start, long End)>();
        
        for (var i = 0; i < rangeCount; i++)
        {
            var start = i * rangeSize;
            var end = (i == rangeCount - 1) ? long.MaxValue : (start + rangeSize - 1);

            chunks.Add((start, end));
        }

        var cts = new CancellationTokenSource();

        var result = 0L;
        Parallel.ForEach(chunks, new ParallelOptions(){
            CancellationToken = cts.Token,
        },(range) =>
        {
            var counter = range.Start;
            for (var i = range.Start; i < range.End; i++)
            {        
                if (counter % 100_000_000 == 0)
                {
                    Console.WriteLine(counter);
                }
                
                if (SolveTwo(i, rawInstructions.Length, rawInstructions) && i != registerA)
                {
                    result = i;
                    cts.Cancel();
                }

                counter++;
            }
        });

        Console.WriteLine("Done");
        return result;
    }
    
    private static bool SolveTwo(long registerA, int instructionLength, int[] rawInstructions)
    {
        var counter = 0;
        var instructionPointer = 0;
        var registerB = 0;
        var registerC = 0;
        while (true)
        {
            if (instructionPointer >= instructionLength)
            {
                break;
            }
            
            var rawOpCode = rawInstructions[instructionPointer];
            
            var literalOperand = rawInstructions[instructionPointer + 1];
            var comboOperand = literalOperand switch
            {
                <= 3 => literalOperand,
                4 => registerA,
                5 => registerB,
                6 => registerC,
            };

            switch (rawOpCode)
            {
                //adv
                case 0:
                    registerA = (int) (registerA / Math.Pow(2, comboOperand));
                    break;
                //bxl
                case 1:
                    registerB ^= literalOperand;
                    break;
                //bst
                case 2:
                    registerB = (int)(comboOperand % 8);
                    break;
                //jnz
                case 3:
                    if (registerA == 0)
                    {
                        break;
                    }
                    instructionPointer = literalOperand;
                    continue;
                //bxc
                case 4:
                    registerB ^= registerC;
                    break;
                //out
                case 5:
                    var instruction = (int)comboOperand % 8;
                    if (rawInstructions[counter] != instruction)
                    {
                        return false;
                    }

                    counter++;

                    if (counter == instructionLength)
                    {
                        return true;
                    }
                    break;
                //bdv
                case 6:
                    registerB = (int) (registerA / Math.Pow(2, comboOperand));
                    break;
                //cdv
                case 7:
                    registerC = (int) (registerA / Math.Pow(2, comboOperand));
                    break;
            }
            
            instructionPointer += 2;
        }

        return false;
    }

    private static IEnumerable<int> Solve(long registerA, int instructionLength, int[] rawInstructions)
    {
        var instructionPointer = 0;
        var registerB = 0;
        var registerC = 0;
        while (true)
        {
            if (instructionPointer >= instructionLength)
            {
                break;
            }
            
            var rawOpCode = rawInstructions[instructionPointer];
            
            var literalOperand = rawInstructions[instructionPointer + 1];
            var comboOperand = literalOperand switch
            {
                <= 3 => literalOperand,
                4 => registerA,
                5 => registerB,
                6 => registerC,
            };

            switch (rawOpCode)
            {
                //adv
                case 0:
                    registerA = (int) (registerA / Math.Pow(2, comboOperand));
                    break;
                //bxl
                case 1:
                    registerB ^= literalOperand;
                    break;
                //bst
                case 2:
                    registerB = (int)(comboOperand % 8);
                    break;
                //jnz
                case 3:
                    if (registerA == 0)
                    {
                        break;
                    }
                    instructionPointer = literalOperand;
                    continue;
                //bxc
                case 4:
                    registerB ^= registerC;
                    break;
                //out
                case 5:
                    yield return (int)comboOperand % 8;
                    break;
                //bdv
                case 6:
                    registerB = (int) (registerA / Math.Pow(2, comboOperand));
                    break;
                //cdv
                case 7:
                    registerC = (int) (registerA / Math.Pow(2, comboOperand));
                    break;
            }
            
            instructionPointer += 2;
        }
    }
}