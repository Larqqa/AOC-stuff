using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2022.Days.Day18
{
    [TestClass]
    public class Day18Tests
    {
        private const string Input = "2,2,2\r\n1,2,2\r\n3,2,2\r\n2,1,2\r\n2,3,2\r\n2,2,1\r\n2,2,3\r\n2,2,4\r\n2,2,6\r\n1,2,5\r\n3,2,5\r\n2,1,5\r\n2,3,5";
        private readonly Day18 _d = new();

        [TestMethod]
        public void TestP1()
        {
            _d.ParseInput(Input);
            Assert.AreEqual(64, _d.FindTotalVolume());
        }

        [TestMethod]
        public void TestP2()
        {
            _d.ParseInput(Input);
            Assert.AreEqual(58, _d.FindExteriorVolume());
        }
    }

    public class Day18
    {
        public HashSet<Point> Cubes = new();
        public readonly Point[] Matrix = new Point[6]
        {
            new( -1,  0,  0 ),
            new(  1,  0,  0 ),
            new(  0, -1,  0 ),
            new(  0,  1,  0 ),
            new(  0,  0, -1 ),
            new(  0,  0,  1 )
        };

        public static void Run()
        {
            Console.WriteLine("---Day 18---");
            var d = new Day18();
            d.Operation();
            Console.WriteLine("------------");
        }

        private void Operation()
        {
            var input = General.GetInput("./Days/Day18/input.txt");
            ParseInput(input);
            Console.WriteLine($"non touching sides: {FindTotalVolume()}");
            Console.WriteLine($"exterior volume: {FindExteriorVolume()}");
        }

        public void ParseInput(string input)
        {
            Cubes.Clear();
            foreach (var c in input.Replace("\r", "").Split('\n'))
            {
                Cubes.Add(new Point(c));
            }
        }

        public int FindTotalVolume()
        {
            var count = 0;
            foreach (var c in Cubes)
            {
                count += NonTouchingSides(c);
            }

            return count;
        }

        private int NonTouchingSides(Point p)
        {
            var count = 0;
            foreach(var m in Matrix)
            {
                var side = p.Add(m);
                if (!Cubes.Contains(side)) count++;
            }
            return count;
        }

        public int FindExteriorVolume()
        {
            // Cube is confined between 0-19, add 1 step of padding on all sides
            const int size = 22;
            var cube = new List<string[]>(size);
            const string rock = "#";
            const string outside = " ";

            for (var i = 0; i < size; i++)
            {
                cube.Add(new string[size * size]);
            }

            foreach (var point in Cubes)
            {
                var p = point.Add(new Point(1,1,1));
                cube[p.Z][p.ToIndex(size)] = rock;
            }

            var visited = new HashSet<Point>();
            var toCheck = new Queue<Point>();
            toCheck.Enqueue(new Point());
            var count = 0;

            while (toCheck.Count > 0)
            {
                var p = toCheck.Dequeue();
                if (!visited.Add(p)) continue;

                foreach (var neighbor in Matrix)
                {
                    var n = p.Add(neighbor);

                    if (n.OutOfBounds(new Point(size - 1))) continue;

                    var nv = cube[n.Z][n.ToIndex(size)];
                    if (nv == rock)
                    {
                        count++;
                        continue;
                    }

                    cube[n.Z][n.ToIndex(size)] = outside;
                    toCheck.Enqueue(n);
                }

            }

            //PrintCube(cube, size);
            return count;
        }

        private static void PrintCube(IReadOnlyList<string[]> cube, int size)
        {
            for (var z = 0; z < size; z++)
            {
                for (var x = 0; x < size; x++)
                {
                    for (var y = 0; y < size; y++)
                    {
                        var point = cube[z][y * size + x];

                        Console.Write(string.IsNullOrEmpty(point) ? "o" : point);
                    }

                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
    }
}
