using AdventOfCodeSupport;

namespace AdventOfCodePuzzles;

public abstract class BenchmarkableBase : AdventBase
{
    public virtual object OptimizedPartOne() => InternalPart1();
    
    public virtual object OptimizedPartTwo() => InternalPart2();
}