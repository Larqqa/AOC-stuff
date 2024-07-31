using _2023.Days.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2023.Days.Day04;

[TestClass]
public class Day04Tests
{
    [TestMethod]
    public void TestP1()
    {
        var d = new Day04();
        d.Input = "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53\r\nCard 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19\r\nCard 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1\r\nCard 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83\r\nCard 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36\r\nCard 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11";

        d.ParseInput();
        Assert.AreEqual("13", d.PartOne());
    }

    [TestMethod]
    public void TestP2()
    {
        var d = new Day04();
        d.Input = "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53\r\nCard 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19\r\nCard 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1\r\nCard 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83\r\nCard 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36\r\nCard 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11";

        d.ParseInput();
        Assert.AreEqual("30", d.PartTwo());
    }
}

public class Day04() : Day("04")
{
    public readonly Dictionary<int, (HashSet<int> winning, HashSet<int> numbers)> Cards = [];

    public override void ParseInput()
    {
        var index = 1;
        foreach (var row in Input.Split("\r\n"))
        {
            var r = row.Split(": ");

            var ns = r[1].Split(" | ").Select(n => n.Split(' ').Aggregate(new HashSet<int>(), (a, b) =>
            {
                if (!string.IsNullOrEmpty(b))
                    a.Add(int.Parse(b));

                return a;
            })).ToList();

            Cards.Add(index, (ns[0], ns[1]));
            index++;
        }
    }

    private readonly Dictionary<int, int> _fibs = new ()
    {
        {0, 0},
        {1, 1},
    };

    private int GetFibonacci(int number)
    {
        if (_fibs.TryGetValue(number, out var num))
            return num;

        var i = 1;
        for (var j = 0; j < number - 1; j++)
        {
            i *= 2;
        }
        _fibs.Add(number, i);
        return i;
    }

    public override string PartOne()
    {
        return Cards.Values.Aggregate(new List<int>(), (a, b) =>
        {
            var intersects = b.winning.Intersect(b.numbers).Count();
            a.Add(GetFibonacci(intersects));
            return a;
        }).Sum().ToString();
    }


    public override string PartTwo()
    {
        var winningNumbers = new Dictionary<int, int>();
        var total = 0;
        var cards = new Queue<int>();

        foreach (var key in Cards.Keys)
        {
            cards.Enqueue(key);
        }

        while (cards.Count > 0)
        {
            var key = cards.Dequeue();
            total++;

            if (!winningNumbers.TryGetValue(key, out var value))
            {
                var (winning, numbers) = Cards[key];
                value = winning.Intersect(numbers).Count();
                winningNumbers.Add(key, value);
            }

            foreach (var k in Enumerable.Range(key + 1, value))
            {
                if (Cards.ContainsKey(k)) cards.Enqueue(k);
            }
        }

        return total.ToString();
    }
}
