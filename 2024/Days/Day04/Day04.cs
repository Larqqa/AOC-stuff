using _2024.Days.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library;

namespace _2024.Days.Day04
{
    [TestClass]
    public class Day04Tests
    {
        [TestMethod]
        public void Test()
        {
            var d = new Day04()
            {
                Input = "MMMSXXMASM\r\nMSAMXMSMSA\r\nAMXSXMAAMM\r\nMSAMASMSMX\r\nXMASAMXAMM\r\nXXAMMXXAMA\r\nSMSMSASXSS\r\nSAXAMASAAA\r\nMAMMMXMMMM\r\nMXMXAXMASX"
            };

            d.ParseInput();

            Assert.AreEqual("18", d.PartOne());
        }

        [TestMethod]
        public void Test2()
        {
            var d = new Day04()
            {
                Input = "MMMSXXMASM\r\nMSAMXMSMSA\r\nAMXSXMAAMM\r\nMSAMASMSMX\r\nXMASAMXAMM\r\nXXAMMXXAMA\r\nSMSMSASXSS\r\nSAXAMASAAA\r\nMAMMMXMMMM\r\nMXMXAXMASX"
            };

            d.ParseInput();

            Assert.AreEqual("9", d.PartTwo());
        }
    }

    public class Day04() : Day("04")
    {
        public int width;
        public int height;
        public List<List<char>> map;

        public override void ParseInput()
        {
            var rows = Input.Split("\r\n");
            width = rows[0].Length;
            height = rows.Length;
            map = rows.Select(x => x.ToCharArray().ToList()).ToList();
        }

        private List<Point> GetLocations(char target)
        {
            List<Point> locations = [];
            var matches = new List<string> { "XMAS", "SAMX" };

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (map[y][x] == target) locations.Add(new Point(x, y));
                }
            }

            return locations;
        }

        public override string PartOne()
        {
            var results = new List<Point>();
            var matches = new List<string> { "XMAS", "SAMX" };
            List<Point> locations = GetLocations('X');

            foreach (var p in locations) {
                foreach(var dir in Point.NeighborPoints)
                {
                    var str = map[p.Y][p.X].ToString();
                    var next = new Point(p);
                    for(int i = 0; i < 3; i++)
                    {
                        next = next.Add(dir);

                        if (next.X >= width || next.Y >= height ||
                            next.X < 0 || next.Y < 0 ||
                            !matches.Any(x => x.Contains(str))
                        ) break;

                        str += map[next.Y][next.X];
                    }

                    if (matches.Contains(str)) results.Add(p);
                }
            }

            return results.Count.ToString();
        }

        public override string PartTwo()
        {
            var results = new List<Point>();
            var matches = new List<string> { "MAS", "SAM" };
            List<Point> locations = GetLocations('A');

            foreach (var p in locations)
            {
                var chars = new List<char>();

                foreach (var dir in Point.DiagonalPoints)
                {
                    var next = new Point(p).Add(dir);
                    if (next.X >= width || next.Y >= height || next.X < 0 || next.Y < 0) chars.Add('X');
                    else chars.Add(map[next.Y][next.X]);
                }

                var foo = $"{chars[2]}A{chars[0]}";
                var bar = $"{chars[1]}A{chars[3]}";

                if (matches.Contains(foo.ToString()) && matches.Contains(bar.ToString())) results.Add(p);
            }

            return results.Count.ToString();
        }
    }
}
