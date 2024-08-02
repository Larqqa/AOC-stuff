using _2023.Days.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2023.Days.Day06;

[TestClass]
public class Day06Tests
{
    [TestMethod]
    public void TestP1()
    {
        var d = new Day06()
        {
            Input = "Time:      7  15   30\r\nDistance:  9  40  200"
        };

        // 0 -> 2 - 5 -> 7
        // 2 - 3 - 2

        // 0 -> 4 - 11 -> 15
        // 4 - 7 - 4

        // 0 -> 11 - 19 -> 30
        // 11 - 8 - 11

        // Edges are always even!
        // We can find the lowest edge under half point of total time
        // and subtract that from total time to get the higher edge

        d.ParseInput();

        Assert.AreEqual("288", d.PartOne());
    }

    [TestMethod]
    public void TestP2()
    {
        var d = new Day06()
        {
            Input = "Time:      7  15   30\r\nDistance:  9  40  200"
        };

        d.ParseInput();

        Assert.AreEqual("71503", d.PartTwo());
    }
}

public class Day06() : Day("06")
{
    public readonly Dictionary<long, long> Races = [];

    public override void ParseInput()
    {
        var input = Input.Split("\r\n");

        var times = input[0]
            .Replace("Time:    ", "")
            .Split(' ')
            .Aggregate(new List<long>(), (a, b) =>
                {
                    if (long.TryParse(b, out var c)) a.Add(c);
                    return a;
                }).ToList();

        var distances = input[1]
            .Replace("Distance:", "")
            .Split(' ')
            .Aggregate(new List<long>(), (a, b) =>
                {
                    if (long.TryParse(b, out var c)) a.Add(c);
                    return a;
                }).ToList();

        for (var i = 0; i < times.Count; i++)
        {
            Races.Add(times[i], distances[i]);
        }
    }

    public static (long lowest, long highest) FindEdges(long time, long target)
    {
        long left = 0;
        var right = time / 2;

        while (true)
        {
            var middle = left + (right - left) / 2;
            var score = middle * (time - middle);

            if (score <= target)
            {
                left = middle;
            }
            else
            {
                right = middle;
            }

            if (right - left == 1 && left * (time - left) <= target && right * (time - right) > target)
            {
                return (right, time - right);
            }
        }
    }

    public override string PartOne()
    {
        return Races
            .Aggregate((long) 1, (total, race) =>
            {
                var (low, high) = FindEdges(race.Key, race.Value);
                total *= high - low + 1;
                return total;
            })
            .ToString();
    }

    public override string PartTwo()
    {
        var time = long.Parse(string.Join("", Races.Keys));
        var distance = long.Parse(string.Join("", Races.Values));
        var (low, high) = FindEdges(time, distance);
        return (high - low + 1).ToString();
    }
}
