namespace _2022.Days.Day19
{
    public class Resources
    {
        public int Ore { get; set; }
        public int Clay { get; set; }
        public int Obsidian { get; set; }
        public int Geode { get; set; }

        public Resources Clone()
        {
            return new Resources()
            {
                Ore = Ore,
                Clay = Clay,
                Obsidian = Obsidian,
                Geode = Geode
            };
        }

        public void Add(Resources resource)
        {
            Ore += resource.Ore;
            Clay += resource.Clay;
            Obsidian += resource.Obsidian;
            Geode += resource.Geode;
        }

        public override string ToString()
        {
            return $"Ore: {Ore}, Clay: {Clay}, Obsidian: {Obsidian}, Geode: {Geode}";
        }
    }
}
