using AdventOfCodeSupport;

namespace AdventOfCodePuzzles;

public abstract class BenchmarkableBase : AdventBase
{
    protected BenchmarkableBase()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        if (UseTestInput())
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            SetTestInput(TestInput());
        }
    }

    protected virtual string TestInput() => "";

    protected virtual bool UseTestInput() => false;
    
    public virtual object OptimizedPartOne() => InternalPart1();
    
    public virtual object OptimizedPartTwo() => InternalPart2();
}

public abstract class TestableBenchmarkableBase : BenchmarkableBase
{
    protected override bool UseTestInput() => UseDayTestInput;
    
    protected override string TestInput() => DayTestInput;
    
    protected abstract bool UseDayTestInput { get; }
    
    protected abstract string DayTestInput { get; }
}