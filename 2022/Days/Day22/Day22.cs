using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2022.Days.Day22
{
    [TestClass]
    public class Day22Tests
    {
        public const string Input =
            "        ...#\r\n        .#..\r\n        #...\r\n        ....\r\n...#.......#\r\n........#...\r\n..#....#....\r\n..........#.\r\n        ...#....\r\n        .....#..\r\n        .#......\r\n        ......#.\r\n\r\n10R5L5R10L4R5L5";
        [TestMethod]
        public void Test()
        {
            var human = Day22.ParseInput(Input);
            //Console.WriteLine(human);

            while (human.Instructions.Count > 0)
            {
                human.DoNextInstruction();
                //Console.WriteLine(human);
            }

            Console.WriteLine("Last location:");
            Console.WriteLine(human);

            var password = Day22.GetPassword(human.Location, human.Direction);
            Assert.AreEqual(6032, password);
        }

        [TestMethod]
        public void Test3D()
        {
            var human = Day22.ParseInputForCube(Input, CubeTests.TransformTestMap);
            //Console.WriteLine(human);

            while (human.Instructions.Count > 0)
            {
                human.DoNextInstruction();
                //Console.WriteLine(human);
            }

            Console.WriteLine("Last location:");
            Console.WriteLine(human);

            var t = CubeTests.TransformTestMap[(int)human.Face];

            var invertRotation = t.Item3;
            var invertDirection = invertRotation > 0;
            var location = human.Location;

            for (var i = 0; i < Math.Abs(invertRotation); i++)
            {
                location = invertDirection ? new Point(location.Y, human.Map.Width - location.X - 1) : new Point(human.Map.Width - location.Y - 1, location.X);
                human.Turn(invertDirection ? Direction.Left : Direction.Right);
            }

            location = location.Add(t.Item1);

            Console.WriteLine(location);
            Console.WriteLine(human.Direction);

            var password = Day22.GetPassword(location, human.Direction);
            Assert.AreEqual(5031, password);
        }
    }

    public class Day22
    {
        public static void Run()
        {
            Console.WriteLine("---Day 22---");
            Operation();
            Console.WriteLine("------------");
        }

        public static void Operation()
        {
            var input = General.GetInput("./Days/Day22/input.txt");

            {
                var human = ParseInput(input);
                while (human.Instructions.Count > 0)
                {
                    human.DoNextInstruction();
                }
        
                var password = GetPassword(human.Location, human.Direction);
                Console.WriteLine($"p1: {password}");
            }
            {
                // First point of face, Last point of face, 90 deg rotations to left or right (left < 0 < right)
                List<(Point, Point, int)> transformMap = new()
                {
                    (new Point(50,0), new Point(100,50), 0),
                    (new Point(50,100), new Point(100,150), 0),

                    (new Point(0,100), new Point(50,150), 2),
                    (new Point(100,0), new Point(150,50), 0),

                    (new Point(0,150), new Point(50,200), -1),
                    (new Point(50,50), new Point(100,100), 0)
                };

                var human = ParseInputForCube(input, transformMap);
                while (human.Instructions.Count > 0)
                {
                    human.DoNextInstruction();
                }

                // Reverse rotation of the face we end up in to get accurate row/col values

                var (offset, _, rotation) = transformMap[(int)human.Face];
                var direction = rotation > 0;

                for (var i = 0; i < Math.Abs(rotation); i++)
                {
                    human.Location = direction
                        ? new Point(human.Location.Y, human.Map.Width - human.Location.X - 1)
                        : new Point(human.Map.Width - human.Location.Y - 1, human.Location.X);

                    human.Turn(direction ? Direction.Left : Direction.Right);
                }

                // Add face's offset, since we map location in relation to current face
                human.Location = human.Location.Add(offset);

                var password = GetPassword(human.Location, human.Direction);
                Console.WriteLine($"p2: {password}");
            }
        }

        public static Human ParseInput(string input)
        {
            var inp = input.Split("\r\n\r\n");

            var rows = inp[0].Split("\r\n");
            var width = rows.Aggregate(0, (a, b) => a > b.Length ? a : b.Length);
            var height = rows.Length;

            var tiles = rows.Aggregate(Array.Empty<Tile>(), (map, row) =>
            {
                var padding = width - row.Length;
                if (padding > 0)
                {
                    row += new string(' ', padding);
                }

                var mappedRow = row
                    .ToCharArray()
                    .Select(Map.GetTileValue);

                return map.Concat(mappedRow).ToArray();
            });

            var map = new Map
            {
                Width = width,
                Height = height,
                Tiles = tiles,
            };

            return new Human(map.FindStartingLocation(), map)
            {
                Instructions = GetInstructions(inp[1])
            };
        }

        public static Human3d ParseInputForCube(string input, List<(Point, Point, int)> positionMatrix)
        {
            var inp = input.Split("\r\n\r\n");
            var c = Cube.ParseCube(inp[0], positionMatrix);

            return new Human3d(new Point(), c)
            {
                Instructions = GetInstructions(inp[1])
            };
        }

        public static Queue<string> GetInstructions(string input)
        {
            var instructions = new Queue<string>();
            var temp = string.Empty;

            foreach (var c in input.ToCharArray())
            {
                var newStr = $"{temp}{c}";
                if (!int.TryParse(newStr, out _))
                {
                    instructions.Enqueue(temp);
                    temp = string.Empty;
                }

                temp += c;
            }

            if (!string.IsNullOrEmpty(temp))
                instructions.Enqueue(temp);

            return instructions;
        }

        public static int GetPassword(Point p, Direction facing)
        {
            return 1000 * (p.Y + 1) + 4 * (p.X + 1) + (int)facing;
        }
    }
}
