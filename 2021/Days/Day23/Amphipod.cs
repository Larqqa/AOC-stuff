namespace _2021.Days.Day23
{
    public class Amphipod
    {
        public enum Types
        {
            Amber = 1,
            Bronze = 10,
            Copper = 100,
            Desert = 1000
        };

        public static readonly Dictionary<char, Types> TypeMap = new() {
            {'A', Types.Amber },
            {'B', Types.Bronze},
            {'C', Types.Copper },
            {'D', Types.Desert },
        };

        public Point Location { get; set; }
        public Types Type { get; set; }

        public Amphipod(Point loc, char type)
        {
            if(!TypeMap.TryGetValue(type, out var t))
                throw new Exception($"No type found for {type}");

            Location = loc;
            Type = t;
        }

        public int GetMovementValue()
        {
            return (int)Type;
        }
    }
}

