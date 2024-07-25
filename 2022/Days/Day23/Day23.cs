using Library;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2022.Days.Day23
{
    [TestClass]
    public class Day23Tests
    {
        public string Input = ".....\r\n..##.\r\n..#..\r\n.....\r\n..##.\r\n.....";
        public string BiggerInput = "....#..\r\n..###.#\r\n#...#.#\r\n.#...##\r\n#.###..\r\n##.#.##\r\n.#..#..";

        [TestMethod]
        public void TestSmallInput()
        {
            var elves = Day23.ParseInput(Input);
            Day23.PrintMap(elves);

            var (res, _) = Day23.DoMoves(elves);
            
            Console.WriteLine();
            Day23.PrintMap(res);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestBigInput()
        {
            var elves = Day23.ParseInput(BiggerInput);
            Day23.PrintMap(elves);

            var (res, _) = Day23.DoMoves(elves);
            
            Console.WriteLine();
            Day23.PrintMap(res);

            Assert.AreEqual(110, Day23.GetResult(res));
        }

        [TestMethod]
        public void TestFullBigInput()
        {
            var elves = Day23.ParseInput(BiggerInput);
            Day23.PrintMap(elves);

            var (res, rounds) = Day23.DoMoves(elves, 1000);

            Console.WriteLine();
            Day23.PrintMap(res);

            Assert.AreEqual(146, Day23.GetResult(res));
            Assert.AreEqual(20, rounds);
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
            {
                var elves = ParseInput(input);
                var (res, _) = DoMoves(elves);
                Console.WriteLine($"p1: {GetResult(res)}");
            }
            {
                var elves = ParseInput(input);
                var (_, rounds) = DoMoves(elves, 10000);
                Console.WriteLine($"p2: {rounds}");
            }
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

        public static (Dictionary<Point, Elf> elves, int rounds) DoMoves(Dictionary<Point, Elf> elves, int maxRounds = 10)
        {
            var rounds = 1;
            var offset = 0;
            while (rounds <= maxRounds)
            {
                var props = GetPropositions(elves, offset);
                if (props.Count == 0) break;

                DoMoves(props);
                props.Clear();

                elves = UpdateElves(elves);

                offset++;

                if (offset > (int)Direction.East)
                    offset = 0;

                rounds++;
            }

            return (elves, rounds);
        }

        public static Dictionary<Point, List<Elf>> GetPropositions(Dictionary<Point, Elf> elves, int directionOffset = 0)
        {
            var propositions = new Dictionary<Point, List<Elf>>();
            var dirs = (int[]) Enum.GetValues(typeof(Direction));

            foreach (var elf in elves.Values)
            {
                var noNeighbours = true;
                var newLocation = elf.Location;

                foreach (var dir in dirs)
                {
                    var actualDir = (Direction) ((dir + directionOffset) % ((int)Direction.East + 1));
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

        public static int GetResult(Dictionary<Point, Elf> elves)
        {
            var minX = elves.Keys.Min(x => x.X);
            var maxX = elves.Keys.Max(x => x.X);

            var minY = elves.Keys.Min(y => y.Y);
            var maxY = elves.Keys.Max(y => y.Y);

            var rx = maxX - minX + 1;
            var ry = maxY - minY + 1;

            var tiles = rx * ry;

            return tiles - elves.Count;
        }

        public static void PrintMap(Dictionary<Point, Elf> elves)
        {
            var minX = elves.Keys.Min(x => x.X);
            var maxX = elves.Keys.Max(x => x.X);

            var minY = elves.Keys.Min(y => y.Y);
            var maxY = elves.Keys.Max(y => y.Y);

            var rx = maxX - minX;
            var ry = maxY - minY;


            for (int y = 0; y <= ry; y++)
            {
                for (int x = 0; x <= rx; x++)
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
