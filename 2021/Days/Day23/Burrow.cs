using System;
using Newtonsoft.Json;

namespace _2021.Days.Day23
{
    public class Burrow : IEquatable<Burrow>, ICloneable
    {
        public Tiles[] Map { get; set; }
        public Amphipod[] Amphipods { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public enum Tiles
        {
            Wall,
            Room,
            Hallway,
            Door,
            Empty
        }

        public Dictionary<char, Tiles> TilesMap = new() {
            { '#', Tiles.Wall },
            { 'x', Tiles.Room },
            { '.', Tiles.Hallway },
            { '&', Tiles.Door },
            { ' ', Tiles.Empty },
        };

		public Burrow(string input)
		{
            var (map, width, height, amphis) = ParseInput(input);
			Map = map;
			Amphipods = amphis;
			Width = width;
			Height = height;
		}

        public (Tiles[], int, int, Amphipod[]) ParseInput(string input)
        {
            var charMap = input.Replace("\n", "").Replace("\r", "").ToCharArray();
            var height = input.Count(c => c == '\n');
            var width = charMap.Length / height;
            var amphis = new List<Amphipod>();
            var map = new List<Tiles>(charMap.Length);

            for (var i = 0; i < charMap.Length; i++)
            {
                var c = charMap[i];

                if (TilesMap.TryGetValue(c, out var tile)) throw new Exception($"No such tile {c} exists!");
                map[i] = tile;

                if (Amphipod.TypeMap.ContainsKey(c))
                {
                    amphis.Add(new Amphipod(Point.FromIndex(i, width), c));
                    map[i] = Tiles.Room;
                }
            }

            return(map.ToArray(), height, width, amphis.ToArray());
        }

        public void PrintMap()
        {
            var m = new List<char>(Map.Length);
            for(var i = 0;i < Map.Length; i++)
            {
                var t = Map[i];
                var c = TilesMap.FirstOrDefault(x => x.Value == t).Key;
                m[i] = c;
            }

            for(var y = 0; y < Height; y++)
            {
                for (var x = 0; y < Width; x++)
                {
                    Console.Write(m[new Point(x, y).ToIndex(Width)]);
                }
                Console.Write('\n');
            }
        }

        public override bool Equals(object? obj)
        {
            var b = obj as Burrow;
            if (b == null) return false;
            return Equals(b);
        }

        public bool Equals(Burrow? b)
        {
            if (b == null) return false;
            return ToString() == b.ToString();
        }

        public override string ToString()
        {
            var m = new string(Map);
            var a = Amphipods.Aggregate("", (acc, x) => acc + x.Location.ToString() + x.Type);
            return m + a; // Return map + all amphipods location and type
        }

        public override int GetHashCode()
        {
            var m = Map.Aggregate(0, (acc, x) => acc + x.GetHashCode());
            var a = Amphipods.Aggregate(0, (acc, x) => acc + x.Location.X + x.Location.Y + x.GetMovementValue());
            return m + a;
        }

        public object Clone()
        {
            var s = JsonConvert.SerializeObject(this);
            var clone = JsonConvert.DeserializeObject<Burrow>(s);
            if (clone != null) return clone;

            throw new Exception("Cloning a burrow failed");
        }
    }
}
