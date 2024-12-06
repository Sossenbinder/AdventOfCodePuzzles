using AdventOfCodePuzzles._2024;
using BenchmarkDotNet.Attributes;

namespace AdventOfCodePuzzles;

[MemoryDiagnoser]
public class DayBenchmarker
{
    private readonly Day06 _day;

    public DayBenchmarker()
    {
        _day = new Day06();
        _day.OnLoad();
    }

    [Benchmark]
    public void PartOne() => _day.OptimizedPartOne();

    [Benchmark]
    public void PartTwo() => _day.OptimizedPartTwo();
}