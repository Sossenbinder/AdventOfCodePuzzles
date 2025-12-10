using AdventOfCodePuzzles;
using AdventOfCodePuzzles._2025;
using AdventOfCodeSupport;
using BenchmarkDotNet.Running;

var solutions = new AdventSolutions();
var today = solutions.GetMostRecentDay();
// var day3 = solutions.GetDay(2024, 3);
// var day4 = solutions.First(x => x.Year == 2024 && x.Day == 4);

today.Part1().Part2();

//today.Benchmark();

//BenchmarkRunner.Run<DayBenchmarker>();