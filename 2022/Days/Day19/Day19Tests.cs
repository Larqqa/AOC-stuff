using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2022.Days.Day19
{
    [TestClass]
    public class Day19Tests
    {
        private const string bp1 =
            "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.";
        private const string bp2 =
            "Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.";

        private readonly Day19 _d = new();

        [TestMethod]
        public void TestP1Bp1()
        {
            var f = Day19.ParseInput(bp1);
            var res = f.GetBestResult();
            Console.Write(res);

            Assert.AreEqual(9, res.GetGeodes());
        }

        [TestMethod]
        public void TestP2Bp1()
        {
            var f = Day19.ParseInput(bp1, 32);
            var res = f.GetBestResult();


            Assert.AreEqual(56, res.GetGeodes());
        }

        [TestMethod]
        public void TestP1Bp2()
        {
            var f = Day19.ParseInput(bp2);
            var res = f.GetBestResult();
            Console.Write(res);

            Assert.AreEqual(12, res.GetGeodes());
        }


        [TestMethod]
        public void TestP2Bp2()
        {
            var f = Day19.ParseInput(bp2, 32);
            var res = f.GetBestResult();
            Console.Write(res);

            Assert.AreEqual(62, res.GetGeodes());
        }

        [TestMethod]
        public void TestWait()
        {
            {
                var f = Day19.ParseInput(bp1);
                Assert.AreEqual(4, f.WaitForResources(RobotType.Ore));
                Assert.AreEqual(2, f.WaitForResources(RobotType.Clay));
            }
            {
                var f = Day19.ParseInput(bp1);
                f._time = 12;
                f._bots = new Resources
                {
                    Ore = 1,
                    Clay = 4,
                    Obsidian = 1,
                    Geode = 0
                };
                f._resources = new Resources
                {
                    Ore = 1,
                    Clay = 7,
                    Obsidian = 1,
                    Geode = 0
                };
                Assert.AreEqual(3, f.WaitForResources(RobotType.Ore));
                Assert.AreEqual(1, f.WaitForResources(RobotType.Clay));
                Assert.AreEqual(2, f.WaitForResources(RobotType.Obsidian));
                Assert.AreEqual(6, f.WaitForResources(RobotType.Geode));

                Console.Write(f);
            }
        }


        [TestMethod]
        public void TestStates()
        {
            {
                var f = Day19.ParseInput(bp1);
                f._time = 12;
                f._bots = new Resources
                {
                    Ore = 1,
                    Clay = 4,
                    Obsidian = 1,
                    Geode = 0
                };
                f._resources = new Resources
                {
                    Ore = 1,
                    Clay = 7,
                    Obsidian = 1,
                    Geode = 0
                };
                //Console.Write(f);
                //var news = f.NextStates();
                //foreach (var factory in news)
                //{
                //    Console.Write(factory);
                //}
            }

            {
                var f = Day19.ParseInput(bp1);
                f._time = 18;
                f._bots = new Resources
                {
                    Ore = 1,
                    Clay = 3,
                    Obsidian = 0,
                    Geode = 0
                };
                f._resources = new Resources
                {
                    Ore = 1,
                    Clay = 6,
                    Obsidian = 0,
                    Geode = 0
                };
                Console.Write(f);
                var news = f.NextStates();
                foreach (var factory in news)
                {
                    Console.Write(factory);
                }
            }
        }

        [TestMethod]
        public void TestMath()
        {
            // (cost - resources) / production
            Assert.AreEqual(0, (4 - 4) / 1);
            Assert.AreEqual(2, (2 - 0) / 1);
            Assert.AreEqual(1, (2 - 0) / 2);
            Assert.AreEqual(2, (4 - 1 - 1) / 2 + 1);
            Assert.AreEqual(1, (4 - 1) / 2);
            Assert.AreEqual(0, (4 - 2) / 3);
            Assert.AreEqual(1, (4 - 2 - 1) / 3 + 1);

        }
    }
}
