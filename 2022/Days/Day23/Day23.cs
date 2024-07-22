using _2022.Days.Day19;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2022.Days.Day23
{
    [TestClass]
    public class Day23Tests
    {
        public string Input = ".....\r\n..##.\r\n..#..\r\n.....\r\n..##.\r\n.....";

        [TestMethod]
        public void Test()
        {
            var elves = Day23.ParseInput(Input);
            Day23.DoMoves(elves);
            Day23.PrintMap(elves);
            Assert.IsTrue(true);
        }
    }

    public class Day23
    {
        public static void Run()
        {
            Console.WriteLine("---Day 23---");
            Operation();
            Console.WriteLine("------------");
        }

        public static void Operation()
        {
            var input = General.GetInput("./Days/Day23/input.txt");
            var elves = ParseInput(input);
            DoMoves(elves);
        }

        public static Dictionary<Point, Elf> ParseInput(string input)
        {
            var elves = new Dictionary<Point, Elf>();

            var lines = input.Split("\n");
            var width = lines[0].Length - 1;
            var height = lines.Length;

            for (var y = 0; y < height; y++)
            {
                var chars = lines[y].ToCharArray();

                for (var x = 0; x < width; x++)
                {
                    if (chars[x] == '#')
                    {
                        var location = new Point(x, y);
                        elves.Add(location, new Elf(location));
                    }
                }
            }

            return elves;
        }

        public static Dictionary<Point, Elf> DoMoves(Dictionary<Point, Elf> elves)
        {
            var index = 0;
            var offset = 0;
            while (index < 10)
            {
                var props = GetPropositions(elves, offset);
                DoMoves(props);
                elves = UpdateElves(elves);

                offset++;

                if (offset > (int)Direction.East)
                    offset = 0;

                index++;
            }

            return elves;
        }

        public static Dictionary<Point, List<Elf>> GetPropositions(Dictionary<Point, Elf> elves, int directionOffset = 0)
        {
            var propositions = new Dictionary<Point, List<Elf>>();

            foreach (var elf in elves.Values)
            {
                var dirs = Enum.GetValues(typeof(Direction));
                var noNeighbours = true;
                var newLocation = elf.Location;

                foreach (int dir in dirs)
                {
                    var actualDir = (Direction) ((dir + directionOffset) % (int)Direction.East);
                    if (elf.CheckAdjacentPointsAreEmpty(actualDir, elves))
                    {
                        // Get first actual different empty location
                        if (newLocation.Equals(elf.Location))
                            newLocation = elf.GetAdjacent(actualDir);
                    }
                    else
                    {
                        noNeighbours = false;
                    }
                }

                // No one around, so we can stay here
                if (noNeighbours) continue;

                if (!propositions.TryAdd(newLocation, new List<Elf> { elf }))
                {
                    propositions[newLocation].Add(elf);
                }
            }

            return propositions;
        }

        public static void DoMoves(Dictionary<Point, List<Elf>> propositions)
        {
            foreach (var (location, movingElves) in propositions)
            {
                if (movingElves.Count > 1)
                {
                    propositions.Remove(location);
                    continue;
                }

                movingElves.First().Location = location;
            }
        }

        public static Dictionary<Point, Elf> UpdateElves(Dictionary<Point, Elf> elves)
        {
            return elves.Values.ToDictionary(elf => elf.Location);
        }

        public static void PrintMap(Dictionary<Point, Elf> elves)
        {
            var minX = elves.Keys.Min(x => x.X);
            var maxX = elves.Keys.Max(x => x.X);

            var minY = elves.Keys.Min(y => y.Y);
            var maxY  = elves.Keys.Max(y => y.Y);

            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    if (elves.TryGetValue(new Point(x + minX, y + minY), out var elf))
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.Write('\n');
            }
        }
    }

    public enum Direction
    {
        North, South, West, East
    }
}
