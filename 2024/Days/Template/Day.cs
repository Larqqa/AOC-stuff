﻿using Library;

namespace _2024.Days.Template;

/*
[TestClass]
public class DayTests
{
    [TestMethod]
    public void TestP1()
    {
        var d = new Day()
        {
            Input = ""
        };

        d.ParseInput();

        Assert.AreEqual("1", d.PartOne());
    }
}
*/

public abstract class Day
{
    private readonly string _date;
    public string Input;

    protected Day(string date)
    {
        _date = date;
        Input = General.GetInput($"./Days/Day{_date}/input.txt");
    }

    public void Run()
    {
        ParseInput();
        Console.WriteLine($"---Day {_date}---");
        Console.WriteLine($"Part 1: {PartOne()}");
        Console.WriteLine($"Part 2: {PartTwo()}");
        Console.WriteLine("------------");
    }

    /// <summary>
    /// Parse the input string into some internal data structures
    /// </summary>
    public abstract void ParseInput();

    /// <summary>
    /// Solve part one, return as string
    /// </summary>
    public abstract string PartOne();

    /// <summary>
    /// Solve part two, return as string
    /// </summary>
    public abstract string PartTwo();
}
