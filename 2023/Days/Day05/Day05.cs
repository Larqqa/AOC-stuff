using _2023.Days.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2023.Days.Day05;

[TestClass]
public class Day05Tests
{
    [TestMethod]
    public void TestP1()
    {
        var d = new Day05()
        {
            Input =
                "seeds: 79 14 55 13\r\n\r\nseed-to-soil map:\r\n50 98 2\r\n52 50 48\r\n\r\nsoil-to-fertilizer map:\r\n0 15 37\r\n37 52 2\r\n39 0 15\r\n\r\nfertilizer-to-water map:\r\n49 53 8\r\n0 11 42\r\n42 0 7\r\n57 7 4\r\n\r\nwater-to-light map:\r\n88 18 7\r\n18 25 70\r\n\r\nlight-to-temperature map:\r\n45 77 23\r\n81 45 19\r\n68 64 13\r\n\r\ntemperature-to-humidity map:\r\n0 69 1\r\n1 0 69\r\n\r\nhumidity-to-location map:\r\n60 56 37\r\n56 93 4"
        };

        d.ParseInput();

        Assert.AreEqual("35", d.PartOne());
    }

    [TestMethod]
    public void TestP2()
    {
        var d = new Day05()
        {
            Input =
                "seeds: 79 14 55 13\r\n\r\nseed-to-soil map:\r\n50 98 2\r\n52 50 48\r\n\r\nsoil-to-fertilizer map:\r\n0 15 37\r\n37 52 2\r\n39 0 15\r\n\r\nfertilizer-to-water map:\r\n49 53 8\r\n0 11 42\r\n42 0 7\r\n57 7 4\r\n\r\nwater-to-light map:\r\n88 18 7\r\n18 25 70\r\n\r\nlight-to-temperature map:\r\n45 77 23\r\n81 45 19\r\n68 64 13\r\n\r\ntemperature-to-humidity map:\r\n0 69 1\r\n1 0 69\r\n\r\nhumidity-to-location map:\r\n60 56 37\r\n56 93 4"
        };

        d.ParseInput();

        Assert.AreEqual("46", d.PartTwo());
    }
}

public class Day05() : Day("05")
{
    public List<long> Seeds = [];
    public Maps Maps = new ();

    public override void ParseInput()
    {
        var rows = Input.Split("\r\n\r\n");
        Seeds = rows[0].Replace("seeds: ", "").Split(' ').Select(long.Parse).ToList();

        var maps = new Maps();
        foreach (var row in rows[1..])
        {
            var r = row.Split(":\r\n");

            var map = r[1].Split("\r\n").Select(x =>
            {
                var xs = x.Split(' ').Select(long.Parse).ToList();
                return (xs[0], xs[1], xs[2]);
            }).ToList();

            switch (r[0])
            {
                case "seed-to-soil map":
                    maps.SeedToSoil = map;
                    break;
                case "soil-to-fertilizer map":
                    maps.SoilToFertilizer = map;
                    break;
                case "fertilizer-to-water map":
                    maps.FertilizerToWater = map;
                    break;
                case "water-to-light map":
                    maps.WaterToLight = map;
                    break;
                case "light-to-temperature map":
                    maps.LightToTemperature = map;
                    break;
                case "temperature-to-humidity map":
                    maps.TemperatureToHumidity = map;
                    break;
                case "humidity-to-location map":
                    maps.HumidityToLocation = map;
                    break;
            }
        }

        Maps = maps;
    }

    public override string PartOne()
    {
        return Seeds
            .Select(seed => Maps.GetValueForType(seed, Maps, Maps.MapType.Location))
            .Min()
            .ToString();
    }

    public override string PartTwo()
    {
        var lowest = long.MaxValue;
        for (var i = 0; i < Seeds.Count; i += 2)
        {
            var seed = Seeds[i];
            var amount = Seeds[i + 1] - 1;
            var mapped = Maps.GetValueForType((seed, seed + amount), Maps, Maps.MapType.Location);

            var lowestLocation = mapped.Min(x => x.start);
            if (lowestLocation < lowest) lowest = lowestLocation;
        }

        return lowest.ToString();
    }
}
