namespace _2022.Days.Day19
{
    public class Robot
    {
        public RobotType Type { get; set; }
        public int Ore { get; set; }
        public int Clay { get; set; }
        public int Obsidian { get; set; }

        public Robot(RobotType type, string? blueprint)
        {
            if (string.IsNullOrEmpty(blueprint)) return;

            Type = type;
            Ore = 0;
            Clay = 0;
            Obsidian = 0;

            // Assume format of "Each geode robot costs 4 ore and 9 obsidian"
            var bp = blueprint.Split(" ");
            switch (type)
            {
                case RobotType.Ore:
                    Ore = int.Parse(bp[4]);
                    break;
                case RobotType.Clay:
                    Ore = int.Parse(bp[4]);
                    break;
                case RobotType.Obsidian:
                    Ore = int.Parse(bp[4]);
                    Clay = int.Parse(bp[7]);
                    break;
                case RobotType.Geode:
                    Ore = int.Parse(bp[4]);
                    Obsidian = int.Parse(bp[7]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

        }

        public override string ToString()
        {
            return $"Type: {Type}, Ore: {Ore}, Clay: {Clay}, Obsidian: {Obsidian}";
        }
    }

    public enum RobotType
    {
        Ore, Clay, Obsidian, Geode
    }
}
