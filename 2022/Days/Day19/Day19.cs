using Library;
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
            var f = _d.ParseInput(bp1);
            var res = f.GetBestResult();
            Console.Write(res);

            Assert.AreEqual(9, res.GetGeodes());
        }

        [TestMethod]
        public void TestP2Bp1()
        {
            var f = _d.ParseInput(bp1, 32);
            var res = f.GetBestResult();
            Console.Write(res);

            Assert.AreEqual(56, res.GetGeodes());
        }


        [TestMethod]
        public void TestWait()
        {
            {
                var f = _d.ParseInput(bp1);
                Assert.AreEqual(4, f.WaitForResources(RobotType.Ore));
                Assert.AreEqual(2, f.WaitForResources(RobotType.Clay));
            }
            {
                var f = _d.ParseInput(bp1);
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
                var f = _d.ParseInput(bp1);
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
                var f = _d.ParseInput(bp1);
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
        public void TestP1Bp2()
        {
            var f = _d.ParseInput(bp2);
            var res = f.GetBestResult();

            Assert.AreEqual(12, res.GetGeodes());
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
            var input = General.GetInput("./Days/Day19/input.txt");

            var result = 0;
            foreach (var line in input.Split("\r\n"))
            {
                var f = ParseInput(line);
                var res = f.GetBestResult();
                result += f._blueprintId * res.GetGeodes();
            }
            Console.WriteLine($"Quality score: {result}");

            var result2 = 1;
            foreach (var line in input.Split("\r\n").Take(3))
            {
                var f = ParseInput(line, 32);
                var res = f.GetBestResult();
                result2 *= res.GetGeodes();
            }
            Console.WriteLine($"Geodes: {result2}");

        }

        public Factory ParseInput(string input, int time = 24)
        {
            var a = input.Split(": ");

            var bp = int.Parse(a[0].Replace("Blueprint ", string.Empty));
            var fac = new Factory(bp, time);

            var b = a[1].Split(". ");
            fac.AddRobotBlueprint(new Robot(RobotType.Ore, b[0]));
            fac.AddRobotBlueprint(new Robot(RobotType.Clay, b[1]));
            fac.AddRobotBlueprint(new Robot(RobotType.Obsidian, b[2]));
            fac.AddRobotBlueprint(new Robot(RobotType.Geode, b[3]));

            //Console.WriteLine(fac);
            return fac;
        }
    }

    public class Robot
    {
        public RobotType Type { get; set; }
        public int Ore { get; set; }
        public int Clay { get; set; }
        public int Obsidian { get; set; }

        public Robot(RobotType type, string? blueprint)
        {
            if (string.IsNullOrEmpty(blueprint)) return;

            Type = type;
            Ore = 0;
            Clay = 0;
            Obsidian = 0;

            // Assume format of "Each geode robot costs 4 ore and 9 obsidian"
            var bp = blueprint.Split(" ");
            switch (type)
            {
                case RobotType.Ore:
                    Ore = int.Parse(bp[4]);
                    break;
                case RobotType.Clay:
                    Ore = int.Parse(bp[4]);
                    break;
                case RobotType.Obsidian:
                    Ore = int.Parse(bp[4]);
                    Clay = int.Parse(bp[7]);
                    break;
                case RobotType.Geode:
                    Ore = int.Parse(bp[4]);
                    Obsidian = int.Parse(bp[7]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

        }

        public override string ToString()
        {
            return $"Type: {Type}, Ore: {Ore}, Clay: {Clay}, Obsidian: {Obsidian}";
        }
    }

    public enum RobotType
    {
        Ore, Clay, Obsidian, Geode
    }

    public class Resources
    {
        public int Ore { get; set; }
        public int Clay { get; set; }
        public int Obsidian { get; set; }
        public int Geode { get; set; }

        public Resources Clone()
        {
            return new Resources()
            {
                Ore = Ore,
                Clay = Clay,
                Obsidian = Obsidian,
                Geode = Geode
            };
        }

        public void Add(Resources resource)
        {
            Ore += resource.Ore;
            Clay += resource.Clay;
            Obsidian += resource.Obsidian;
            Geode += resource.Geode;
        }

        public override string ToString()
        {
            return $"Ore: {Ore}, Clay: {Clay}, Obsidian: {Obsidian}, Geode: {Geode}";
        }
    }

    public class Factory
    {
        public int _blueprintId;
        public int _time;
        private Dictionary<RobotType, Robot> _botBlueprint = new();
        public Resources _resources = new Resources();
        public Resources _maxMap = new Resources();
        public Resources _bots = new Resources() { Ore = 1};

        public Factory(int blueprint, int time = 24)
        {
            _blueprintId = blueprint;
            _time = time;
        }

        public Factory(int blueprint, Dictionary<RobotType, Robot> botBlueprint, Resources resources, Resources maxMap, Resources bots, int time = 24)
        {
            _blueprintId = blueprint;
            _time = time;
            _botBlueprint = botBlueprint;
            _resources = resources;
            _maxMap = maxMap;
            _bots = bots;
        }

        public Factory Clone()
        {
            return new Factory(
                _blueprintId,
                _botBlueprint,
                _resources.Clone(),
                _maxMap,
                _bots.Clone(),
                _time
            );
        }

        public int GetGeodes()
        {
            return _resources.Geode + _bots.Geode * _time;
        }

        public void AddRobotBlueprint(Robot bot)
        {
            if (!_botBlueprint.TryAdd(bot.Type, bot))
            {
                _botBlueprint[bot.Type] = bot;
            }

            if (_maxMap.Ore < bot.Ore)
            {
                _maxMap.Ore = bot.Ore;
            }

            if (_maxMap.Clay < bot.Clay)
            {
                _maxMap.Clay = bot.Clay;
            }

            if (_maxMap.Obsidian < bot.Obsidian)
            {
                _maxMap.Obsidian = bot.Obsidian;
            }
        }

        public override string ToString()
        {
            var str = $"--Factory id {_blueprintId}--\nTime: {_time}\n";
            str = _botBlueprint.Aggregate($"{str}Blueprints:\n", (current, bot) => $"{current}    {bot.Value}\n");
            str = $"{str}Bots:\n    {_bots}\n";
            str = $"{str}Maxes:\n    {_maxMap}\n";
            str = $"{str}Resources:\n    {_resources}\n";
            return str;
        }

        public Factory GetBestResult()
        {
            var stack = new Queue<Factory>();
            stack.Enqueue(this);
            var best = this;

            while (stack.Count > 0)
            {
                var current = stack.Dequeue();

                if (current._time < 0) continue; // Time has run out
                
                if (best.GetGeodes() < current.GetGeodes())
                    best = current;

                foreach (var factory in current.NextStates())
                {
                    if (((factory._time - 1) * factory._time) / 2 + factory.GetGeodes() < best.GetGeodes())
                        continue;

                    stack.Enqueue(factory);
                }
            }

            return best;
        }

        public List<Factory> NextStates()
        {
            var factories = new List<Factory>();
            foreach (RobotType type in Enum.GetValues(typeof(RobotType)))
            {
                if (ProducingEnough(type) || !ProducingRequired(type)) continue;

                var fac = Clone();

                var waitTime = fac.WaitForResources(type);
                if (waitTime >= fac._time) continue;

                fac._time -= waitTime;
                fac._resources.Ore += _bots.Ore * waitTime;
                fac._resources.Clay += _bots.Clay * waitTime;
                fac._resources.Obsidian += _bots.Obsidian * waitTime;
                fac._resources.Geode += _bots.Geode * waitTime;

                if (!fac.TryBuild(type)) continue;

                fac._time--;
                fac._resources.Add(_bots);
                factories.Add(fac);
            }

            return factories;
        }

        private bool TryBuild(RobotType type)
        {
            var bb = _botBlueprint[type];

            if (!EnoughResources(bb)) return false;
            
            _resources.Ore -= bb.Ore;
            _resources.Clay -= bb.Clay;
            _resources.Obsidian -= bb.Obsidian;

            switch (type)
            {
                case RobotType.Ore:
                    _bots.Ore += 1;
                    break;
                case RobotType.Clay:
                    _bots.Clay += 1;
                    break;
                case RobotType.Obsidian:
                    _bots.Obsidian += 1;
                    break;
                case RobotType.Geode:
                    _bots.Geode += 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return true;
        }

        public int WaitForResources(RobotType type)
        {
            var bb = _botBlueprint[type];
            var maxWait = 0;

            if (bb.Ore > 0)
            {
                var waitForOre = _bots.Ore == 0 ? _time + 1 : (bb.Ore - _resources.Ore + _bots.Ore - 1) / _bots.Ore;
                if (maxWait < waitForOre) maxWait = waitForOre;
            }

            if (bb.Clay > 0)
            {
                var waitForClay = _bots.Clay == 0 ? _time + 1 : (bb.Clay - _resources.Clay + _bots.Clay - 1) / _bots.Clay;
                if (maxWait < waitForClay) maxWait = waitForClay;
            }

            if (bb.Obsidian > 0)
            {
                var waitForObsidian = _bots.Obsidian == 0 ? _time + 1 : (bb.Obsidian - _resources.Obsidian + _bots.Obsidian - 1) / _bots.Obsidian;
                if (maxWait < waitForObsidian) maxWait = waitForObsidian;
            }

            return maxWait;
        }

        private bool EnoughResources(Robot bot)
        {
            return
                _resources.Ore >= bot.Ore &&
                _resources.Clay >= bot.Clay &&
                _resources.Obsidian >= bot.Obsidian;
        }

        private bool ProducingEnough(RobotType type)
        {
            return type switch
            {
                RobotType.Ore => _bots.Ore >= _maxMap.Ore,
                RobotType.Clay => _bots.Clay >= _maxMap.Clay,
                RobotType.Obsidian => _bots.Obsidian >= _maxMap.Obsidian,
                RobotType.Geode => false,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };
        }

        private bool ProducingRequired(RobotType type)
        {
            var producingOre = _bots.Ore > 0;
            return type switch
            {
                RobotType.Ore => producingOre,
                RobotType.Clay => producingOre,
                RobotType.Obsidian => producingOre && _bots.Clay > 0,
                RobotType.Geode => producingOre && _bots.Obsidian > 0,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };
        }
    }
}
