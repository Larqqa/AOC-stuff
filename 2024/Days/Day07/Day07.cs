using _2024.Days.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2024.Days.Day07;

[TestClass]
public class Day07Tests
{
    [TestMethod]
    public void Test()
    {
        var d = new Day07()
        {
            Input = "190: 10 19\r\n3267: 81 40 27\r\n83: 17 5\r\n156: 15 6\r\n7290: 6 8 6 15\r\n161011: 16 10 13\r\n192: 17 8 14\r\n21037: 9 7 18 13\r\n292: 11 6 16 20"
        };

        d.ParseInput();

        Assert.AreEqual("3749", d.PartOne());
    }

    [TestMethod]
    public void Test2()
    {
        var d = new Day07()
        {
            Input = "190: 10 19\r\n3267: 81 40 27\r\n83: 17 5\r\n156: 15 6\r\n7290: 6 8 6 15\r\n161011: 16 10 13\r\n192: 17 8 14\r\n21037: 9 7 18 13\r\n292: 11 6 16 20"
        };

        d.ParseInput();

        Assert.AreEqual("11387", d.PartTwo());
    }
}

public class Day07() : Day("07")
{
    public List<(long, long[])> calculations;

    public override void ParseInput()
    {
        calculations = Input
            .Split("\r\n")
            .Select(x => {
                var y = x.Split(": ");
                return (long.Parse(y[0]), y[1].Split(" ").Select(z => long.Parse(z)).ToArray());
            })
            .ToList();
    }

    private static bool Recurse(long target, long acc, Queue<long> queue, bool p2 = false)
    {
        if (queue.Count == 0) return target == acc;

        var cur = queue.Dequeue();

        if (acc + cur <= target && Recurse(target, acc + cur, new Queue<long>(queue), p2)) return true;
        if (acc * cur <= target && Recurse(target, acc == 0 ? 1 : acc * cur, new Queue<long>(queue), p2)) return true;
        if (p2)
        {
            var next = long.Parse(string.Concat(acc.ToString(), cur.ToString()));
            if (next <= target && Recurse(target, next, new Queue<long>(queue), p2)) return true;
        }

        return false;
    }

    public override string PartOne()
    {
        return calculations.Sum(x => Recurse(x.Item1, 0, new Queue<long>(x.Item2)) ? x.Item1 : 0).ToString();
    }

    public override string PartTwo()
    {
        return calculations.Sum(x => Recurse(x.Item1, 0, new Queue<long>(x.Item2), true) ? x.Item1 : 0).ToString();
    }
}
