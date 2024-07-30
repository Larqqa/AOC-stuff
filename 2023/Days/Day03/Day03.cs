using _2023.Days.Template;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2023.Days.Day03;

[TestClass]
public class Day3Tests
{
    [TestMethod]
    public void Test()
    {
        var d = new Day03()
        {
            Input =
                "467..114..\r\n...*......\r\n..35..633.\r\n......#...\r\n617*......\r\n.....+.58.\r\n..592.....\r\n......755.\r\n...$.*....\r\n.664.598.."
        };

        d.ParseInput();

        Assert.AreEqual(467, d.Numbers[new Point()]);
        Assert.AreEqual(467, d.Numbers[new Point(1, 0)]);
        Assert.AreEqual(467, d.Numbers[new Point(2, 0)]);
        Assert.AreEqual(598, d.Numbers[new Point(5, 9)]);

        Assert.AreEqual('*', d.Symbols[new Point(3, 1)]);
        Assert.AreEqual('*', d.Symbols[new Point(3, 4)]);
        Assert.AreEqual('*', d.Symbols[new Point(5, 8)]);
    }

    [TestMethod]
    public void TestP1()
    {
        var d = new Day03();

        d.Input = "100.101\r\n102*103\r\n104.105";
        d.ParseInput();

        Assert.AreEqual("615", d.PartOne());

        d.Numbers.Clear();
        d.Symbols.Clear();

        d.Input = "..100..\r\n101*102\r\n..104..";
        d.ParseInput();

        Assert.AreEqual("407", d.PartOne());

        d.Numbers.Clear();
        d.Symbols.Clear();

        d.Input = ".10.11.\r\n.12*13.\r\n.14.15.";
        d.ParseInput();

        Assert.AreEqual("75", d.PartOne());

        d.Numbers.Clear();
        d.Symbols.Clear();

        d.Input = "..10.11\r\n.12*.13\r\n14.15..";
        d.ParseInput();

        Assert.AreEqual("37", d.PartOne());

        d.Numbers.Clear();
        d.Symbols.Clear();

        d.Input = "100.10.\r\n101*102\r\n.11.103";
        d.ParseInput();

        Assert.AreEqual("427", d.PartOne());

        d.Numbers.Clear();
        d.Symbols.Clear();

        d.Input = "..90*12..";
        d.ParseInput();

        Assert.AreEqual("102", d.PartOne());

        d.Numbers.Clear();
        d.Symbols.Clear();

        d.Input = ".....24.*23.\r\n..10........\r\n..397*.610..\r\n.......50...\r\n1*2..4......";
        d.ParseInput();

        Assert.AreEqual("423", d.PartOne());

        d.Numbers.Clear();
        d.Symbols.Clear();

        d.Input = "1...2\r\n3.*.4\r\n5...6";
        d.ParseInput();

        Assert.AreEqual("0", d.PartOne());


        d.Numbers.Clear();
        d.Symbols.Clear();

        d.Input = ".......329...........\r\n..+....*...43.671....\r\n961.848...*......*...\r\n..........735...584..";
        d.ParseInput();

        Assert.AreEqual("4171", d.PartOne());
    }

    [TestMethod]
    public void TestP2()
    {
        var d = new Day03()
        {
            Input =
                "467..114..\r\n...*......\r\n..35..633.\r\n......#...\r\n617*......\r\n.....+.58.\r\n..592.....\r\n......755.\r\n...$.*....\r\n.664.598.."
        };
        d.ParseInput();

        Assert.AreEqual("467835", d.PartTwo());
    }
}

public class Day03() : Day("03")
{
    public readonly Dictionary<Point, int> Numbers = [];
    public readonly Dictionary<Point, char> Symbols = [];

    public override void ParseInput()
    {
        var rows = Input.Split("\r\n");
        var width = rows[0].Length;
        var height = rows.Length;

        for (var y = 0; y < height; y++)
        {
            var start = -1;
            var num = string.Empty;

            for (var x = 0; x < width; x++)
            {
                var c = rows[y][x];

                if (char.IsNumber(c))
                {
                    if (start < 0)
                    {
                        start = x;
                        num += c;
                    }
                    else
                    {
                        num += c;
                    }
                }
                else if (start >= 0)
                {
                    for (var xx = start; xx < x; xx++)
                    {
                        Numbers.Add(new Point(xx, y), int.Parse(num));
                    }

                    start = -1;
                    num = string.Empty;
                }

                if (!char.IsNumber(c) && c != '.')
                {
                    Symbols.Add(new Point(x, y), c);
                }
            }

            if (start >= 0)
            {
                for (var xx = start; xx < width; xx++)
                {
                    Numbers.Add(new Point(xx, y), int.Parse(num));
                }
            }
        }
    }

    public override string PartOne()
    {
        var total = 0;
        foreach (var (key, _) in Symbols)
        {
            var values = new HashSet<(int y,int val)>();
            foreach (var point in Point.NeighborPoints)
            {
                var p = key.Add(point);

                if (Numbers.TryGetValue(p, out var number))
                {
                    values.Add((p.Y, number));
                }
            }

            total += values.Sum(a => a.val);
        }

        return total.ToString();
    }

    public override string PartTwo()
    {
        var total = 0;
        foreach (var (key, _) in Symbols.Where(a => a.Value == '*'))
        {
            var values = new HashSet<(int y, int val)>();
            foreach (var point in Point.NeighborPoints)
            {
                var p = key.Add(point);

                if (Numbers.TryGetValue(p, out var number))
                {
                    values.Add((p.Y, number));
                }
            }

            if(values.Count == 2)
                total += values.Aggregate(1, (a,b)=> a * b.val);
        }

        return total.ToString();
    }
}
