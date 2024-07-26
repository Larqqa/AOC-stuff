using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2022.Days.Day24
{
    [TestClass]
    public class Day24Tests
    {
        public string Input = "#.#####\r\n#.....#\r\n#>....#\r\n#.....#\r\n#...v.#\r\n#.....#\r\n#####.#";
        public string Complex = "#.######\r\n#>>.<^<#\r\n#.<..<<#\r\n#>v.><>#\r\n#<^v^^>#\r\n######.#";

        [TestMethod]
        public void Test()
        {
            //var (m, b) = Day24.ParseInput(Input);
            var (m, b) = Day24.ParseInput(Complex);
            var cache = Day24.GenerateBlizzardCache(b, m);

            Day24.PrintCacheMinute(m, cache, 0);

            Day24.PrintCacheMinute(m, cache, 11);

            //var max = cache.Keys.Max();

            //for (var i = 0; i <= max; i++)
            //{
            //    Day24.PrintCacheMinute(m, cache, i);
            //}

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestPathing()
        {
            var (m, b) = Day24.ParseInput(Complex);
            var cache = Day24.GenerateBlizzardCache(b, m);
            var elapsed = Day24.FindShortestPath(m, cache);

            Assert.AreEqual(18, elapsed);
        }

        [TestMethod]
        public void TestPathingBackAndForth()
        {
            var (m, b) = Day24.ParseInput(Complex);
            var cache = Day24.GenerateBlizzardCache(b, m);
            var elapsed = Day24.FindShortestBackAndForthPath(m, cache);

            Assert.AreEqual(54, elapsed);
        }
    }

    public class Day24
    {
        public static void Run()
        {
            Console.WriteLine("---Day 24---");
            Operation();
            Console.WriteLine("------------");
        }

        public static void Operation()
        {
            var input = General.GetInput("./Days/Day24/input.txt");
            var (m, b) = ParseInput(input);
            var cache = GenerateBlizzardCache(b, m);

            Console.WriteLine($"p1: {FindShortestPath(m, cache)}");
            Console.WriteLine($"p2: {FindShortestBackAndForthPath(m, cache)}");
        }

        public static (Map map, List<Blizzard> blizzards) ParseInput(string input)
        {
            var rows = input.Split("\r\n");
            var map = new Map(rows[0].Length, rows.Length);

            var blizzards = new List<Blizzard>();
            for (var y = 0; y < map.Height; y++)
            {
                for (var x = 0; x < map.Width; x++)
                {
                    var tile = rows[y][x];
                    var direction = Blizzard.GetDirection(tile);
                    if (direction != null)
                    {
                        blizzards.Add(new Blizzard(new Point(x, y), (Direction)direction));
                    }
                }
            }

            return (map, blizzards);
        }

        public static Dictionary<int, HashSet<Point>> GenerateBlizzardCache(List<Blizzard> blizzards, Map map)
        {
            var cache = new Dictionary<int, HashSet<Point>>();
            var minute = 0;

            while (true)
            {
                var slice = new HashSet<Point>();
                foreach (var blizzard in blizzards)
                {
                    slice.Add(blizzard.Location);
                }

                // Check if blizzard positions loop, if so we found the end
                if (cache.ContainsKey(0) && cache[0].All(point => slice.Contains(point))) break;

                cache.Add(minute, slice);

                blizzards = blizzards.Aggregate(new List<Blizzard>(),
                    (acc, b) =>
                    {
                        acc.Add(b.NextLocation(map.Width, map.Height));
                        return acc;
                    });

                minute++;
            }

            return cache;
        }

        public static void PrintCacheMinute(Map map, Dictionary<int, HashSet<Point>> cache, int minute)
        {
            minute %= cache.Count;

            if (!cache.ContainsKey(minute)) return;

            var str = map.ToString().ToCharArray();
            var slice = cache[minute];

            foreach (var point in slice)
            {
                str[point.X + point.Y * (map.Width + 1)] = '*';
            }

            Console.WriteLine(str);
        }


        private static readonly List<Point> PositionMatrix = new()
        {
            new Point( 0,  0), // Wait
            new Point( 0, -1), // Up
            new Point( 1,  0), // Right
            new Point( 0,  1), // Down
            new Point(-1,  0), // Left
        };

        public static List<Point> GetNextPositions(Point p, Dictionary<int, HashSet<Point>> cache, Map map, int minute)
        {
            if (!cache.ContainsKey(minute)) return new List<Point>();

            var slice = cache[minute];
            var nexts = new List<Point>();

            foreach (var point in PositionMatrix)
            {
                var newLocation = p.Add(point);

                // If we are going to the end position from some direction, we don't have to check other options
                // as those would be longer routes
                if (newLocation.Equals(map.End)) return new List<Point> { newLocation };

                // Start and end are only locations we can go to that are "inside" the walls
                if (!newLocation.Equals(map.Start))
                {
                    if (newLocation.X <= 0 || newLocation.X >= map.Width - 1) continue;
                    if (newLocation.Y <= 0 || newLocation.Y >= map.Height - 1) continue;
                }

                if (slice.Contains(newLocation)) continue;
                nexts.Add(newLocation);
            }

            return nexts;
        }

        public static int FindShortestPath(Map map, Dictionary<int, HashSet<Point>> cache, int startingMinute = 0)
        {
            var queue = new PriorityQueue<(Point location, int minute), int>();
            var visited = new HashSet<(Point location, int minute)>();
            var shortest = int.MaxValue;

            // Starting position
            queue.Enqueue((map.Start, startingMinute), int.MaxValue);

            while (queue.Count > 0)
            {
                var (current, minute) = queue.Dequeue();

                if (minute > shortest) continue; // Escape if shorter path already found

                if (current.Equals(map.End))
                {
                    if (minute < shortest) shortest = minute;
                    continue; // Found the goal!
                }

                var nextMinute = minute + 1;
                var nexts = GetNextPositions(current, cache, map, nextMinute % cache.Count);
                foreach (var point in nexts)
                {
                    if (visited.Contains((point, nextMinute))) continue;

                    var heuristic = nextMinute + (int)Math.Sqrt(Math.Pow(map.End.X - point.X, 2) + Math.Pow(map.End.Y - point.Y, 2));
                    queue.Enqueue((point, nextMinute), heuristic);

                    visited.Add((point, nextMinute));
                }
            }

            return shortest;
        }

        public static int FindShortestBackAndForthPath(Map map, Dictionary<int, HashSet<Point>> cache)
        {
            var forth = FindShortestPath(map, cache);

            (map.End, map.Start) = (map.Start, map.End);
            var back = FindShortestPath(map, cache, forth);

            (map.End, map.Start) = (map.Start, map.End);
            return FindShortestPath(map, cache, back);
        }
    }
}
