using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                var expect = "{\"Location\":{\"X\":0,\"Y\":0,\"Z\":0},\"TransformDegrees\":{\"Item1\":0,\"Item2\":0,\"Item3\":0},\"Beacons\":[{\"Location\":{\"X\":404,\"Y\":-588,\"Z\":-901},\"TransposedLocation\":{\"X\":404,\"Y\":-588,\"Z\":-901}},{\"Location\":{\"X\":528,\"Y\":-643,\"Z\":409},\"TransposedLocation\":{\"X\":528,\"Y\":-643,\"Z\":409}},{\"Location\":{\"X\":-838,\"Y\":591,\"Z\":734},\"TransposedLocation\":{\"X\":-838,\"Y\":591,\"Z\":734}},{\"Location\":{\"X\":390,\"Y\":-675,\"Z\":-793},\"TransposedLocation\":{\"X\":390,\"Y\":-675,\"Z\":-793}},{\"Location\":{\"X\":-537,\"Y\":-823,\"Z\":-458},\"TransposedLocation\":{\"X\":-537,\"Y\":-823,\"Z\":-458}},{\"Location\":{\"X\":-485,\"Y\":-357,\"Z\":347},\"TransposedLocation\":{\"X\":-485,\"Y\":-357,\"Z\":347}},{\"Location\":{\"X\":-345,\"Y\":-311,\"Z\":381},\"TransposedLocation\":{\"X\":-345,\"Y\":-311,\"Z\":381}},{\"Location\":{\"X\":-661,\"Y\":-816,\"Z\":-575},\"TransposedLocation\":{\"X\":-661,\"Y\":-816,\"Z\":-575}},{\"Location\":{\"X\":-876,\"Y\":649,\"Z\":763},\"TransposedLocation\":{\"X\":-876,\"Y\":649,\"Z\":763}},{\"Location\":{\"X\":-618,\"Y\":-824,\"Z\":-621},\"TransposedLocation\":{\"X\":-618,\"Y\":-824,\"Z\":-621}},{\"Location\":{\"X\":553,\"Y\":345,\"Z\":-567},\"TransposedLocation\":{\"X\":553,\"Y\":345,\"Z\":-567}},{\"Location\":{\"X\":474,\"Y\":580,\"Z\":667},\"TransposedLocation\":{\"X\":474,\"Y\":580,\"Z\":667}},{\"Location\":{\"X\":-447,\"Y\":-329,\"Z\":318},\"TransposedLocation\":{\"X\":-447,\"Y\":-329,\"Z\":318}},{\"Location\":{\"X\":-584,\"Y\":868,\"Z\":-557},\"TransposedLocation\":{\"X\":-584,\"Y\":868,\"Z\":-557}},{\"Location\":{\"X\":544,\"Y\":-627,\"Z\":-890},\"TransposedLocation\":{\"X\":544,\"Y\":-627,\"Z\":-890}},{\"Location\":{\"X\":564,\"Y\":392,\"Z\":-477},\"TransposedLocation\":{\"X\":564,\"Y\":392,\"Z\":-477}},{\"Location\":{\"X\":455,\"Y\":729,\"Z\":728},\"TransposedLocation\":{\"X\":455,\"Y\":729,\"Z\":728}},{\"Location\":{\"X\":-892,\"Y\":524,\"Z\":684},\"TransposedLocation\":{\"X\":-892,\"Y\":524,\"Z\":684}},{\"Location\":{\"X\":-689,\"Y\":845,\"Z\":-530},\"TransposedLocation\":{\"X\":-689,\"Y\":845,\"Z\":-530}},{\"Location\":{\"X\":423,\"Y\":-701,\"Z\":434},\"TransposedLocation\":{\"X\":423,\"Y\":-701,\"Z\":434}},{\"Location\":{\"X\":7,\"Y\":-33,\"Z\":-71},\"TransposedLocation\":{\"X\":7,\"Y\":-33,\"Z\":-71}},{\"Location\":{\"X\":630,\"Y\":319,\"Z\":-379},\"TransposedLocation\":{\"X\":630,\"Y\":319,\"Z\":-379}},{\"Location\":{\"X\":443,\"Y\":580,\"Z\":662},\"TransposedLocation\":{\"X\":443,\"Y\":580,\"Z\":662}},{\"Location\":{\"X\":-789,\"Y\":900,\"Z\":-551},\"TransposedLocation\":{\"X\":-789,\"Y\":900,\"Z\":-551}},{\"Location\":{\"X\":459,\"Y\":-707,\"Z\":401},\"TransposedLocation\":{\"X\":459,\"Y\":-707,\"Z\":401}}]}";
                Assert.AreEqual(expect, output);
            }
        }

        [TestMethod]
        public void TestComparison()
        {
            var input0 = "--- scanner 0 ---\n404,-588,-901\n528,-643,409\n-838,591,734\n390,-675,-793\n-537,-823,-458\n-485,-357,347\n-345,-311,381\n-661,-816,-575\n-876,649,763\n-618,-824,-621\n553,345,-567\n474,580,667\n-447,-329,318\n-584,868,-557\n544,-627,-890\n564,392,-477\n455,729,728\n-892,524,684\n-689,845,-530\n423,-701,434\n7,-33,-71\n630,319,-379\n443,580,662\n-789,900,-551\n459,-707,401";
            var input1 = "--- scanner 1 ---\n686,422,578\n605,423,415\n515,917,-361\n-336,658,858\n95,138,22\n-476,619,847\n-340,-569,-846\n567,-361,727\n-460,603,-452\n669,-402,600\n729,430,532\n-500,-761,534\n-322,571,750\n-466,-666,-811\n-429,-592,574\n-355,545,-477\n703,-491,-529\n-328,-685,520\n413,935,-424\n-391,539,-444\n586,-435,557\n-364,-763,-893\n807,-499,-711\n755,-354,-619\n553,889,-390";
            var input2 = "--- scanner 2 ---\n649,640,665\n682,-795,504\n-784,533,-524\n-644,584,-595\n-588,-843,648\n-30,6,44\n-674,560,763\n500,723,-460\n609,671,-379\n-555,-800,653\n-675,-892,-343\n697,-426,-610\n578,704,681\n493,664,-388\n-671,-858,530\n-667,343,800\n571,-461,-707\n-138,-166,112\n-889,563,-600\n646,-828,498\n640,759,510\n-630,509,768\n-681,-892,-333\n673,-379,-804\n-742,-814,-386\n577,-820,562";
            var input3 = "--- scanner 3 ---\n-589,542,597\n605,-692,669\n-500,565,-823\n-660,373,557\n-458,-679,-417\n-488,449,543\n-626,468,-788\n338,-750,-386\n528,-832,-391\n562,-778,733\n-938,-730,414\n543,643,-506\n-524,371,-870\n407,773,750\n-104,29,83\n378,-903,-323\n-778,-728,485\n426,699,580\n-438,-605,-362\n-469,-447,-387\n509,732,623\n647,635,-688\n-868,-804,481\n614,-800,639\n595,780,-596";
            var input4 = "--- scanner 4 ---\n727,592,562\n-293,-554,779\n441,611,-461\n-714,465,-776\n-743,427,-804\n-660,-479,-426\n832,-632,460\n927,-485,-438\n408,393,-506\n466,436,-512\n110,16,151\n-258,-428,682\n-393,719,612\n-211,-452,876\n808,-476,-593\n-575,615,604\n-485,667,467\n-680,325,-822\n-627,-443,-432\n872,-547,-609\n833,512,582\n807,604,487\n839,-516,451\n891,-625,532\n-652,-548,-490\n30,-46,-14";

            var s0 = _d.ParseScanner(input0);
            var s1 = _d.ParseScanner(input1);
            var s2 = _d.ParseScanner(input2);
            var s3 = _d.ParseScanner(input3);
            var s4 = _d.ParseScanner(input4);

            var res = s1.CompareToScanner(s0);
            Assert.IsTrue(res);

            var res2 = s4.CompareToScanner(s0);
            Assert.IsFalse(res2);
            var res3 = s4.CompareToScanner(s1);
            Assert.IsTrue(res3);

            var res4 = s2.CompareToScanner(s4);
            Assert.IsTrue(res4);

            var res5 = s3.CompareToScanner(s1);
            Assert.IsTrue(res5);
        }

        [TestMethod]
        public void TestBeaconMapping()
        {
            var input0 = "--- scanner 0 ---\n404,-588,-901\n528,-643,409\n-838,591,734\n390,-675,-793\n-537,-823,-458\n-485,-357,347\n-345,-311,381\n-661,-816,-575\n-876,649,763\n-618,-824,-621\n553,345,-567\n474,580,667\n-447,-329,318\n-584,868,-557\n544,-627,-890\n564,392,-477\n455,729,728\n-892,524,684\n-689,845,-530\n423,-701,434\n7,-33,-71\n630,319,-379\n443,580,662\n-789,900,-551\n459,-707,401";
            var input1 = "--- scanner 1 ---\n686,422,578\n605,423,415\n515,917,-361\n-336,658,858\n95,138,22\n-476,619,847\n-340,-569,-846\n567,-361,727\n-460,603,-452\n669,-402,600\n729,430,532\n-500,-761,534\n-322,571,750\n-466,-666,-811\n-429,-592,574\n-355,545,-477\n703,-491,-529\n-328,-685,520\n413,935,-424\n-391,539,-444\n586,-435,557\n-364,-763,-893\n807,-499,-711\n755,-354,-619\n553,889,-390";
            var input2 = "--- scanner 2 ---\n649,640,665\n682,-795,504\n-784,533,-524\n-644,584,-595\n-588,-843,648\n-30,6,44\n-674,560,763\n500,723,-460\n609,671,-379\n-555,-800,653\n-675,-892,-343\n697,-426,-610\n578,704,681\n493,664,-388\n-671,-858,530\n-667,343,800\n571,-461,-707\n-138,-166,112\n-889,563,-600\n646,-828,498\n640,759,510\n-630,509,768\n-681,-892,-333\n673,-379,-804\n-742,-814,-386\n577,-820,562";
            var input3 = "--- scanner 3 ---\n-589,542,597\n605,-692,669\n-500,565,-823\n-660,373,557\n-458,-679,-417\n-488,449,543\n-626,468,-788\n338,-750,-386\n528,-832,-391\n562,-778,733\n-938,-730,414\n543,643,-506\n-524,371,-870\n407,773,750\n-104,29,83\n378,-903,-323\n-778,-728,485\n426,699,580\n-438,-605,-362\n-469,-447,-387\n509,732,623\n647,635,-688\n-868,-804,481\n614,-800,639\n595,780,-596";
            var input4 = "--- scanner 4 ---\n727,592,562\n-293,-554,779\n441,611,-461\n-714,465,-776\n-743,427,-804\n-660,-479,-426\n832,-632,460\n927,-485,-438\n408,393,-506\n466,436,-512\n110,16,151\n-258,-428,682\n-393,719,612\n-211,-452,876\n808,-476,-593\n-575,615,604\n-485,667,467\n-680,325,-822\n-627,-443,-432\n872,-547,-609\n833,512,582\n807,604,487\n839,-516,451\n891,-625,532\n-652,-548,-490\n30,-46,-14";
            
            var rotatedScanners = new List<Scanner>()
            {
                _d.ParseScanner(input0),
            };
            
            var restOfScanners = new List<Scanner>() {
                _d.ParseScanner(input1),
                _d.ParseScanner(input2),
                _d.ParseScanner(input3),
                _d.ParseScanner(input4),
            };

            while (true)
            {
                var hitOtherScanner = false;
                for (var i =  restOfScanners.Count - 1; i >= 0; i--)
                {
                    foreach(var j in rotatedScanners)
                    {
                        var s = restOfScanners[i];
                        var res = s.CompareToScanner(j);
                        if (res)
                        {
                            rotatedScanners.Add(s);
                            restOfScanners.Remove(s);
                            hitOtherScanner = true;
                            break;
                        }
                    }
                }

                if (restOfScanners.Count == 0 || !hitOtherScanner)
                {
                    break;
                }
            }

            Assert.AreEqual(0, restOfScanners.Count);
            Assert.AreEqual(5, rotatedScanners.Count);

            var beacons = new HashSet<Point>();
            foreach (var scanner in rotatedScanners)
            {
                foreach (var beacon in scanner.Beacons)
                {
                    var x = beacon.TransposedLocation.X + scanner.Location.X;
                    var y = beacon.TransposedLocation.Y + scanner.Location.Y;
                    var z = beacon.TransposedLocation.Z + scanner.Location.Z;
                    beacons.Add(new Point(x,y,z));
                }
            }
            Assert.AreEqual(79, beacons.Count);
        }

        [TestMethod]
        public void TestManhattanDistance()
        {
            var input0 = "--- scanner 0 ---\n404,-588,-901\n528,-643,409\n-838,591,734\n390,-675,-793\n-537,-823,-458\n-485,-357,347\n-345,-311,381\n-661,-816,-575\n-876,649,763\n-618,-824,-621\n553,345,-567\n474,580,667\n-447,-329,318\n-584,868,-557\n544,-627,-890\n564,392,-477\n455,729,728\n-892,524,684\n-689,845,-530\n423,-701,434\n7,-33,-71\n630,319,-379\n443,580,662\n-789,900,-551\n459,-707,401";
            var input1 = "--- scanner 1 ---\n686,422,578\n605,423,415\n515,917,-361\n-336,658,858\n95,138,22\n-476,619,847\n-340,-569,-846\n567,-361,727\n-460,603,-452\n669,-402,600\n729,430,532\n-500,-761,534\n-322,571,750\n-466,-666,-811\n-429,-592,574\n-355,545,-477\n703,-491,-529\n-328,-685,520\n413,935,-424\n-391,539,-444\n586,-435,557\n-364,-763,-893\n807,-499,-711\n755,-354,-619\n553,889,-390";
            var input2 = "--- scanner 2 ---\n649,640,665\n682,-795,504\n-784,533,-524\n-644,584,-595\n-588,-843,648\n-30,6,44\n-674,560,763\n500,723,-460\n609,671,-379\n-555,-800,653\n-675,-892,-343\n697,-426,-610\n578,704,681\n493,664,-388\n-671,-858,530\n-667,343,800\n571,-461,-707\n-138,-166,112\n-889,563,-600\n646,-828,498\n640,759,510\n-630,509,768\n-681,-892,-333\n673,-379,-804\n-742,-814,-386\n577,-820,562";
            var input3 = "--- scanner 3 ---\n-589,542,597\n605,-692,669\n-500,565,-823\n-660,373,557\n-458,-679,-417\n-488,449,543\n-626,468,-788\n338,-750,-386\n528,-832,-391\n562,-778,733\n-938,-730,414\n543,643,-506\n-524,371,-870\n407,773,750\n-104,29,83\n378,-903,-323\n-778,-728,485\n426,699,580\n-438,-605,-362\n-469,-447,-387\n509,732,623\n647,635,-688\n-868,-804,481\n614,-800,639\n595,780,-596";
            var input4 = "--- scanner 4 ---\n727,592,562\n-293,-554,779\n441,611,-461\n-714,465,-776\n-743,427,-804\n-660,-479,-426\n832,-632,460\n927,-485,-438\n408,393,-506\n466,436,-512\n110,16,151\n-258,-428,682\n-393,719,612\n-211,-452,876\n808,-476,-593\n-575,615,604\n-485,667,467\n-680,325,-822\n-627,-443,-432\n872,-547,-609\n833,512,582\n807,604,487\n839,-516,451\n891,-625,532\n-652,-548,-490\n30,-46,-14";

            var rotatedScanners = new List<Scanner>()
            {
                _d.ParseScanner(input0),
            };

            var restOfScanners = new List<Scanner>() {
                _d.ParseScanner(input1),
                _d.ParseScanner(input2),
                _d.ParseScanner(input3),
                _d.ParseScanner(input4),
            };

            while (true)
            {
                var hitOtherScanner = false;
                for (var i = restOfScanners.Count - 1; i >= 0; i--)
                {
                    foreach (var j in rotatedScanners)
                    {
                        var s = restOfScanners[i];
                        var res = s.CompareToScanner(j);
                        if (res)
                        {
                            rotatedScanners.Add(s);
                            restOfScanners.Remove(s);
                            hitOtherScanner = true;
                            break;
                        }
                    }
                }

                if (restOfScanners.Count == 0 || !hitOtherScanner)
                {
                    break;
                }
            }

            var maxDistance = 0;
            for (int i = rotatedScanners.Count - 1; i >= 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    var res = rotatedScanners[i].GetManhattanDistance(rotatedScanners[j]);
                    if (res > maxDistance) maxDistance = res;
                }
            }

            Assert.AreEqual(3621, maxDistance);
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
            var scanners = input.Split("\r\n\r\n").Select(x => ParseScanner(x)).ToList();

            // First is always correct
            var scannerZero = scanners[0];
            var rotatedScanners = new List<Scanner>() { scannerZero };

            // Rest need to find rotations
            scanners.Remove(scannerZero);
            var restOfScanners = scanners;

            while (true)
            {
                var hitOtherScanner = false;
                for (var i = restOfScanners.Count - 1; i >= 0; i--)
                {
                    foreach (var j in rotatedScanners)
                    {
                        var s = restOfScanners[i];
                        var res = s.CompareToScanner(j);
                        if (res)
                        {
                            rotatedScanners.Add(s);
                            restOfScanners.Remove(s);
                            hitOtherScanner = true;
                            break;
                        }
                    }
                }

                if (restOfScanners.Count == 0 || !hitOtherScanner)
                {
                    break;
                }
            }

            var beacons = new HashSet<Point>();
            foreach (var scanner in rotatedScanners)
            {
                foreach (var beacon in scanner.Beacons)
                {
                    var x = beacon.TransposedLocation.X + scanner.Location.X;
                    var y = beacon.TransposedLocation.Y + scanner.Location.Y;
                    var z = beacon.TransposedLocation.Z + scanner.Location.Z;
                    beacons.Add(new Point(x, y, z));
                }
            }

            Console.WriteLine($"Beacons: {beacons.Count}");

            var maxDistance = 0;
            for (int i = rotatedScanners.Count - 1; i >= 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    var res = rotatedScanners[i].GetManhattanDistance(rotatedScanners[j]);
                    if (res > maxDistance) maxDistance = res;
                }
            }
            
            Console.WriteLine($"Max distance: {maxDistance}");
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
