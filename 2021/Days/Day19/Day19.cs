using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace _2021.Days.Day19
{
    [TestClass]
    public class Day19Tests
    {
        readonly Day19 _d = new();

        [TestMethod]
        public void TestRotation()
        {
            HashSet<Point> rots = new HashSet<Point>() {
                new Point(1,2,3),
                new Point(1,-3,2),
                new Point(1,-2,-3),
                new Point(1,3,-2),

                new Point(3,2,-1),
                new Point(2,-3,-1),
                new Point(-3,-2,-1),
                new Point(-2,3,-1),

                new Point(-1,2,-3),
                new Point(-1,-3,-2),
                new Point(-1,-2,3),
                new Point(-1,3,2),

                new Point(-3,2,1),
                new Point(-2,-3,1),
                new Point(3,-2,1),
                new Point(2,3,1),

                new Point(-2,1,3),
                new Point(3,1,2),
                new Point(2,1,-3),
                new Point(-3,1,-2),

                new Point(2,-1,3),
                new Point(-3,-1,2),
                new Point(-2,-1,-3),
                new Point(3,-1,-2),
            };

            var sc = new Scanner();
            var p = new Point(1, 2, 3);
            var bc = new Beacon(p);
            sc.Beacons.Add(bc);
            var r = sc.DoAllRotations(sc.Beacons[0].Location).ToArray();

            var res = rots.ToArray().Where(x => r.Where(y => y.X == x.X && y.Y == x.Y && y.Z == x.Z).Count() > 0);
            Assert.AreEqual(rots.Count, res.Count());
        }

        [TestMethod]
        public void TestParsing()
        {
            var input = "--- scanner 0 ---\n404,-588,-901\n528,-643,409\n-838,591,734\n390,-675,-793\n-537,-823,-458\n-485,-357,347\n-345,-311,381\n-661,-816,-575\n-876,649,763\n-618,-824,-621\n553,345,-567\n474,580,667\n-447,-329,318\n-584,868,-557\n544,-627,-890\n564,392,-477\n455,729,728\n-892,524,684\n-689,845,-530\n423,-701,434\n7,-33,-71\n630,319,-379\n443,580,662\n-789,900,-551\n459,-707,401";

            var s = _d.ParseScanner(input);
            Assert.AreEqual(25, s.Beacons.Count);
            if (s != null)
            {
                var output = Library.ConvertToJson(s);
                var expect = "{\"Location\":{\"X\":0,\"Y\":0,\"Z\":0},\"TransformDegrees\":{\"X\":0,\"Y\":0,\"Z\":0},\"Beacons\":[{\"Location\":{\"X\":404,\"Y\":-588,\"Z\":-901}},{\"Location\":{\"X\":528,\"Y\":-643,\"Z\":409}},{\"Location\":{\"X\":-838,\"Y\":591,\"Z\":734}},{\"Location\":{\"X\":390,\"Y\":-675,\"Z\":-793}},{\"Location\":{\"X\":-537,\"Y\":-823,\"Z\":-458}},{\"Location\":{\"X\":-485,\"Y\":-357,\"Z\":347}},{\"Location\":{\"X\":-345,\"Y\":-311,\"Z\":381}},{\"Location\":{\"X\":-661,\"Y\":-816,\"Z\":-575}},{\"Location\":{\"X\":-876,\"Y\":649,\"Z\":763}},{\"Location\":{\"X\":-618,\"Y\":-824,\"Z\":-621}},{\"Location\":{\"X\":553,\"Y\":345,\"Z\":-567}},{\"Location\":{\"X\":474,\"Y\":580,\"Z\":667}},{\"Location\":{\"X\":-447,\"Y\":-329,\"Z\":318}},{\"Location\":{\"X\":-584,\"Y\":868,\"Z\":-557}},{\"Location\":{\"X\":544,\"Y\":-627,\"Z\":-890}},{\"Location\":{\"X\":564,\"Y\":392,\"Z\":-477}},{\"Location\":{\"X\":455,\"Y\":729,\"Z\":728}},{\"Location\":{\"X\":-892,\"Y\":524,\"Z\":684}},{\"Location\":{\"X\":-689,\"Y\":845,\"Z\":-530}},{\"Location\":{\"X\":423,\"Y\":-701,\"Z\":434}},{\"Location\":{\"X\":7,\"Y\":-33,\"Z\":-71}},{\"Location\":{\"X\":630,\"Y\":319,\"Z\":-379}},{\"Location\":{\"X\":443,\"Y\":580,\"Z\":662}},{\"Location\":{\"X\":-789,\"Y\":900,\"Z\":-551}},{\"Location\":{\"X\":459,\"Y\":-707,\"Z\":401}}]}";
                Assert.AreEqual(expect, output);
            }
        }

        [TestMethod]
        public void TestComparison()
        {
            var input0 = "--- scanner 0 ---\n404,-588,-901\n528,-643,409\n-838,591,734\n390,-675,-793\n-537,-823,-458\n-485,-357,347\n-345,-311,381\n-661,-816,-575\n-876,649,763\n-618,-824,-621\n553,345,-567\n474,580,667\n-447,-329,318\n-584,868,-557\n544,-627,-890\n564,392,-477\n455,729,728\n-892,524,684\n-689,845,-530\n423,-701,434\n7,-33,-71\n630,319,-379\n443,580,662\n-789,900,-551\n459,-707,401";
            var input1 = "--- scanner 1 ---\n686,422,578\n605,423,415\n515,917,-361\n-336,658,858\n95,138,22\n-476,619,847\n-340,-569,-846\n567,-361,727\n-460,603,-452\n669,-402,600\n729,430,532\n-500,-761,534\n-322,571,750\n-466,-666,-811\n-429,-592,574\n-355,545,-477\n703,-491,-529\n-328,-685,520\n413,935,-424\n-391,539,-444\n586,-435,557\n-364,-763,-893\n807,-499,-711\n755,-354,-619\n553,889,-390";

            var s0 = _d.ParseScanner(input0);
            var s1 = _d.ParseScanner(input1);

            s1.CompareToScanner(s0);
        }
    }

    public class Day19
    {
        public static void Run()
        {
            Console.WriteLine("---Day 19---");
            var d = new Day19();
            d.Operation();
            Console.WriteLine("------------");
        }
        private void Operation()
        {
            var input = Library.GetInput(@"./Days/Day19/input.txt");
            Console.WriteLine(input);
        }

        public Scanner ParseScanner(string input)
        {
            var s = new Scanner();
            var rows = input.Split('\n');
            // var scannerId = rows[0].Split("--- scanner ")[1].Split(" ---")[0];
            var beacons = rows[1..];
            foreach(var beacon in beacons)
            {
                s.Beacons.Add(new Beacon(Point.ParsePointFromString(beacon)));
            }
            return s;
        }
    }
}
