using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2022.Days.Day18
{
    [TestClass]
    public class Day18Tests
    {
        string input = $"2,2,2\r\n1,2,2\r\n3,2,2\r\n2,1,2\r\n2,3,2\r\n2,2,1\r\n2,2,3\r\n2,2,4\r\n2,2,6\r\n1,2,5\r\n3,2,5\r\n2,1,5\r\n2,3,5";
        Day18 d = new Day18();

        [TestMethod]
        public void TestP1()
        {
            d.ParseInput(input);
            var count = 0;
            foreach (var c in d.cubes)
            {
                count += d.CountTouchingSides(c);
            }
            Assert.AreEqual(64, count);
        }

        [TestMethod]
        public void TestP2()
        {
            var input = General.GetInput(@"./Days/Day18/input.txt");
            d.ParseInput(input);
            var count = d.FindExteriorVolume();
            Assert.AreEqual(58, count);
        }
    }

    public class Day18
    {
        public HashSet<Point> cubes = new();
        public readonly Point[] matrix = new Point[6]
        {
            new Point( -1, 0, 0 ),
            new Point( 1, 0, 0 ),
            new Point( 0, -1, 0 ),
            new Point( 0, 1, 0 ),
            new Point( 0, 0, -1 ),
            new Point( 0, 0, 1 )
        };

        public readonly Point[] matrixNoZ = new Point[4]
        {
            new Point( -1, 0, 0 ),
            new Point( 1, 0, 0 ),
            new Point( 0, -1, 0 ),
            new Point( 0, 1, 0 ),
        };

        public static void Run()
        {
            Console.WriteLine("---Day 18---");
            var d = new Day18();
            d.Operation();
            Console.WriteLine("------------");
        }

        public void Operation()
        {
            var input = General.GetInput(@"./Days/Day18/input.txt");
            ParseInput(input);
            var count = 0;
            foreach(var c in cubes)
            {
                count += CountTouchingSides(c);
            }
            Console.WriteLine($"non touching sides: {count}");
        }

        public void ParseInput(string input)
        {
            cubes.Clear();
            foreach (var c in input.Replace("\r", "").Split('\n'))
            {
                cubes.Add(new Point(c));
            }
        }

        public int CountTouchingSides(Point p)
        {
            var count = 0;
            foreach(var m in matrix)
            {
                var side = p.Add(m);
                if (!cubes.Contains(side)) count++;
            }
            return count;
        }

        public int FindExteriorVolume()
        {
            // Get lists of points by same X,Y value
            var dict = new Dictionary<Point, List<Point>>();
            foreach(var c in cubes)
            {
                var xy = new Point(c.X, c.Y);
                if (dict.ContainsKey(xy))
                {
                    dict[xy].Add(c);
                }
                else
                {
                    dict.Add(xy, new() { c });
                }
            }

            // Sort each list from smallest to largest
            foreach(var e in dict)
            {
                e.Value.Sort((a, b) => a.Z.CompareTo(b.Z));
            }


            var count = 0;
            return count;
        }
    }
}
