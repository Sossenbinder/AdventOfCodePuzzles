using AdventOfCodePuzzles._2024;
using BenchmarkDotNet.Attributes;

namespace AdventOfCodePuzzles;

[MemoryDiagnoser]
public class DayBenchmarker
{
    private readonly Day02 _day;

    public DayBenchmarker()
    {
        _day = new Day02();
        _day.OnLoad();
    }

    [Benchmark]
    public void PartOne() => _day.BenchmarkOne();

    [Benchmark]
    public void PartTwo() => _day.BenchmarkTwo();
}