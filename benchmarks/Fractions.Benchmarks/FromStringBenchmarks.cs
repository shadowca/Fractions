﻿using System.Globalization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Fractions.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net90)]
public class FromStringBenchmarks {
    [Params(true, false)]
    public bool Normalize { get; set; }
    public static IEnumerable<string> ValidStrings => ["0", "1", "-1", "10242048", "1/5", "-1/5", "3.5", "-3.5", "1.2345678901234567890"];
    public static IEnumerable<string> InvalidStrings => ["", "invalid", "-10242048/", "1.2345678901234567890f"];

    [Benchmark]
    [ArgumentsSource(nameof(ValidStrings))]
    public bool TryParseValidString(string validString) {
        return Fraction.TryParse(validString, NumberStyles.Number, CultureInfo.InvariantCulture, Normalize, out _);
    }
    
    [Benchmark]
    [ArgumentsSource(nameof(InvalidStrings))]
    public bool TryParseInvalidString(string invalidString) {
        return Fraction.TryParse(invalidString, NumberStyles.Number, CultureInfo.InvariantCulture, Normalize, out _);
    }
}
