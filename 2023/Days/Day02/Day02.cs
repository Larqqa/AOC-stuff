using _2023.Days.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2023.Days.Day02;

[TestClass]
public class Day2Tests
{
    [TestMethod]
    public void Test()
    {
        var d = new Day02
        {
            Input = "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green\r\nGame 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue\r\nGame 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red\r\nGame 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red\r\nGame 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green"
        };

        d.ParseInput();
        Assert.AreEqual("8", d.PartOne());
        Assert.AreEqual("2286", d.PartTwo());
    }
}

public class Day02() : Day("02")
{
    private readonly Dictionary<int, List<Dictionary<Color, int>>> _games = [];

    public override void ParseInput()
    {
        var games = Input.Split("\r\n");
        foreach (var game in games)
        {
            var g = game.Split(": ");
            var id = int.Parse(g[0].Replace("Game ", ""));

            var sets = g[1].Split("; ");
            var s = new List<Dictionary<Color, int>>();
            foreach(var set in sets)
            {
                var cubes = set.Split(", ");

                var d = new Dictionary<Color, int>();
                foreach(var cube in cubes)
                {
                    var c = cube.Split(' ');
                    var amount = int.Parse(c[0]);
                    var color = ConvertType(c[1]);
                    d.Add(color, amount);
                }

                s.Add(d);
            }

            _games.Add(id, s);
        }
    }

    public override string PartOne()
    {
        var targets = new Dictionary<Color, int> { {Color.Red, 12}, { Color.Green, 13 }, { Color.Blue, 14 } };
        return _games.Aggregate(0, (total, game) =>
        {
            var viable = true;

            foreach (var set in game.Value)
            {
                if (viable && set.TryGetValue(Color.Red, out var red))
                {
                    viable = red <= targets[Color.Red];
                }

                if (viable && set.TryGetValue(Color.Green, out var green))
                {
                    viable = green <= targets[Color.Green];
                }


                if (viable && set.TryGetValue(Color.Blue, out var blue))
                {
                    viable = blue <= targets[Color.Blue];
                }

                if (!viable) break;
            }

            return total + (viable ? game.Key : 0);
        }).ToString();
    }

    public override string PartTwo()
    {
        return _games.Aggregate(0, (total, game) =>
        {
            var targets = new Dictionary<Color, int> { { Color.Red, 0 }, { Color.Green, 0 }, { Color.Blue, 0 } };

            foreach (var set in game.Value)
            {
                if (set.TryGetValue(Color.Red, out var red))
                {
                    if (targets[Color.Red] < red)
                        targets[Color.Red] = red;
                }

                if (set.TryGetValue(Color.Green, out var green))
                {
                    if (targets[Color.Green] < green)
                        targets[Color.Green] = green;
                }

                if (set.TryGetValue(Color.Blue, out var blue))
                {
                    if (targets[Color.Blue] < blue)
                        targets[Color.Blue] = blue;
                }
            }

            return total + targets.Values.Aggregate(1, (a, b) => a * b);
        }).ToString();
    }

    private Color ConvertType(string type)
    {
        return type switch
        {
            "blue" => Color.Blue,
            "red" => Color.Red,
            "green" => Color.Green,
            _ => throw new NotImplementedException()
        };
    } 

    private enum Color
    {
        Blue, Red, Green
    }
}
