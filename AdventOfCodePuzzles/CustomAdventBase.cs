using AdventOfCodeSupport;

namespace AdventOfCodePuzzles;

public abstract class BenchmarkableBase : AdventBase
{
    public object BenchmarkOne() => InternalOptimizedPart1();

    protected abstract object InternalOptimizedPart1();
    
    public object BenchmarkTwo() => InternalOptimizedPart2();
    
    protected abstract object InternalOptimizedPart2();
}