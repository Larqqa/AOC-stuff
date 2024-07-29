using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2023.Days.Template;

[TestClass]
public class DayTests
{
    [TestMethod]
    public void Test()
    {
        Assert.IsTrue(true);
    }
}

public class Day
{
    public static void Run()
    {
        Console.WriteLine("---Day 00---");
        Operation();
        Console.WriteLine("------------");
    }

    public static void Operation()
    {
        var input = General.GetInput("./Days/Day/input.txt");
        ParseInput(input);
        Console.WriteLine(input);
    }

    public static void ParseInput(string input)
    {
    }
}
