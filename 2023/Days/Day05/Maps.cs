namespace _2023.Days.Day05;

public class Maps
{
    public List<(long destinationStart, long SourceStart, long Length)> SeedToSoil = [];
    public List<(long destinationStart, long SourceStart, long Length)> SoilToFertilizer = [];
    public List<(long destinationStart, long SourceStart, long Length)> FertilizerToWater = [];
    public List<(long destinationStart, long SourceStart, long Length)> WaterToLight = [];
    public List<(long destinationStart, long SourceStart, long Length)> LightToTemperature = [];
    public List<(long destinationStart, long SourceStart, long Length)> TemperatureToHumidity = [];
    public List<(long destinationStart, long SourceStart, long Length)> HumidityToLocation = [];

    public static long GetValueFromRange(long val, (long destinationStart, long SourceStart, long Length) map)
    {
        if (val < map.SourceStart || val > map.SourceStart + map.Length - 1) return val;

        return val + map.destinationStart - map.SourceStart;
    }

    public static long GetValueFromMap(long val, List<(long destinationStart, long SourceStart, long Length)>? map)
    {
        if (map == null) return val;

        var res = val;
        foreach (var range in map)
        {
            var newVal = GetValueFromRange(val, range);
            if (val != newVal) res = newVal;
        }

        return res;
    }

    public static List<(long destinationStart, long SourceStart, long Length)>? GetMapForType(MapType type, Maps map)
    {
        return type switch
        {
            MapType.Seed => map.SeedToSoil,
            MapType.Soil => map.SoilToFertilizer,
            MapType.Fertilizer => map.FertilizerToWater,
            MapType.Water => map.WaterToLight,
            MapType.Light => map.LightToTemperature,
            MapType.Temperature => map.TemperatureToHumidity,
            MapType.Humidity => map.HumidityToLocation,
            MapType.Location => null,
            _ => null
        };
    }

    public static long GetValueForType(long seed, Maps maps, MapType target, MapType current = MapType.Seed)
    {
        while (true)
        {
            var m = GetMapForType(current, maps);
            var val = GetValueFromMap(seed, m);

            if (current == target) return val;

            seed = val;
            current++;
        }
    }

    public enum MapType
    {
        Seed, Soil, Fertilizer, Water, Light, Temperature, Humidity, Location
    }
}
