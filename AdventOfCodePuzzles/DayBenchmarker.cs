using AdventOfCodePuzzles._2024;
using BenchmarkDotNet.Attributes;
using Day07 = AdventOfCodePuzzles._2025.Day07;

namespace AdventOfCodePuzzles;

[MemoryDiagnoser]
public class DayBenchmarker
{
    private readonly Day07 _day = new();

    public DayBenchmarker()
    {
        _day.OnLoad();
    }

    [Benchmark]
    public void PartOne() => _day.OptimizedPartOne();

    [Benchmark]
    public void PartTwo() => _day.OptimizedPartTwo();
}