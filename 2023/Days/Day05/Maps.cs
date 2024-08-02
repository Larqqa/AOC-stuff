namespace _2023.Days.Day05;

public class Maps
{
    public List<(long DestinationStart, long SourceStart, long Length)>? SeedToSoil = [];
    public List<(long DestinationStart, long SourceStart, long Length)>? SoilToFertilizer = [];
    public List<(long DestinationStart, long SourceStart, long Length)>? FertilizerToWater = [];
    public List<(long DestinationStart, long SourceStart, long Length)>? WaterToLight = [];
    public List<(long DestinationStart, long SourceStart, long Length)>? LightToTemperature = [];
    public List<(long DestinationStart, long SourceStart, long Length)>? TemperatureToHumidity = [];
    public List<(long DestinationStart, long SourceStart, long Length)>? HumidityToLocation = [];
    private static readonly MapType LastMapType = Enum.GetValues(typeof(MapType)).Cast<MapType>().Last();

    public static long GetValueFromRange(long seed, (long DestinationStart, long SourceStart, long Length) map)
    {
        var (start, end) = GetSourceBounds(map);
        if (seed < start || end < seed) return seed;

        return seed + map.DestinationStart - map.SourceStart;
    }

    public static long GetValueFromMap(long seed, List<(long DestinationStart, long SourceStart, long Length)>? map)
    {
        if (map == null) return seed;

        foreach (var range in map)
        {
            var newVal = GetValueFromRange(seed, range);
            if (seed != newVal) return newVal;
        }

        return seed;
    }

    public static List<(long DestinationStart, long SourceStart, long Length)>? GetMapForType(MapType type, Maps map)
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
            var map = GetMapForType(current, maps);
            var value = GetValueFromMap(seed, map);

            current++;
            if (current == target) return value;
            if (current > LastMapType) throw new Exception("Outside of map type range!");

            seed = value;
        }
    }

    public static (long start, long end) GetSourceBounds((long DestinationStart, long SourceStart, long Length) range)
    {
        return (range.SourceStart, range.SourceStart + range.Length - 1);
    }

    public static List<(long start, long end)> SplitRange((long start, long end) seed, List<(long DestinationStart, long SourceStart, long Length)>? map)
    {
        var ranges = new List<(long start, long end)>();
        if (map == null) return ranges;

        foreach (var range in map)
        {
            var bounds = GetSourceBounds(range);
            var offset = range.DestinationStart - range.SourceStart;

            // Completely inside
            if (bounds.start <= seed.start && seed.end <= bounds.end)
            {
                ranges.Add((seed.start + offset, seed.end + offset));
                return ranges;
            };

            // Completely outside of range
            if (bounds.end < seed.start || seed.end < bounds.start) continue;

            // Starts outside, ends inside
            if (seed.start < bounds.start && seed.end <= bounds.end)
            {
                ranges.Add((bounds.start + offset, seed.end + offset));
                seed.end = bounds.start - 1;
            }
            
            // Starts inside, ends outside
            if (bounds.start <= seed.start && bounds.end < seed.end)
            {
                ranges.Add((seed.start + offset, bounds.end + offset));
                seed.start = bounds.end + 1;
            }
        }

        ranges.Add(seed); // Unmodified seeds outside of map ranges
        return ranges;
    }

    public static List<(long start, long end)> GetMappedRanges((long start, long end) seedRanges, MapType type, Maps? maps)
    {
        var mappedRanges = new List<(long start, long end)>();
        if (maps == null) return mappedRanges;

        var map = GetMapForType(type, maps);
        if (map == null) return mappedRanges;

        return SplitRange(seedRanges, map);
    }

    public static List<(long start, long end)> GetValueForType((long start, long end) seed, Maps maps, MapType target, MapType current = MapType.Seed)
    {
        var ranges = new List<(long start, long end)> { seed };

        while (true)
        {
            var temp = new List<(long start, long end)>();
            foreach (var range in ranges)
            {
                var mappedRanges = GetMappedRanges(range, current, maps);
                temp = [.. temp, .. mappedRanges];
            }

            ranges = temp;

            current++;
            if (current == target) return ranges;
            if (current > LastMapType) throw new Exception("Outside of map type range!");
        }
    }

    public enum MapType
    {
        Seed, Soil, Fertilizer, Water, Light, Temperature, Humidity, Location
    }
}
