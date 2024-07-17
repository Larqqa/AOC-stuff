using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static _2022.Days.Day20.Pointer;

namespace _2022.Days.Day20
{
    [TestClass]
    public class Day20Tests
    {
        private const string Input = "1\r\n2\r\n-3\r\n3\r\n-2\r\n0\r\n4";

        [TestMethod]
        public void TestParsing()
        {
            var l = Day20.ParseInput(Input);
            Day20.PrintList(l);
        }

        [TestMethod]
        public void TestStep()
        {
            var l = Day20.ParseInput(Input);
            Day20.PrintList(l);
            Console.WriteLine();

            l[0].Step(Direction.Next);
            Day20.PrintList(l);
            Console.WriteLine();

            l[0].Step(Direction.Previous);
            Day20.PrintList(l);
        }

        [TestMethod]
        public void TestMixing()
        {
            var l = Day20.ParseInput(Input);
            Day20.Mix(l);
            Day20.PrintList(l);
        }

        [TestMethod]
        public void TestCoordinates()
        {
            var l = Day20.ParseInput(Input);
            Day20.Mix(l);
            var res = Day20.FindGroveCoordinates(l);
            Assert.AreEqual(3, res);
        }

        [TestMethod]
        public void TestCoordinatesP2()
        {
            var l = Day20.ParseInput(Input);
            Day20.AddDecryption(811589153, l);
            for (var i = 0; i < 10; i++)
            {
                Day20.Mix(l);
            }
            var res = Day20.FindGroveCoordinates(l);
            Assert.AreEqual(1623178306, res);
        }
    }

    public class Day20
    {
        public static void Run()
        {
            Console.WriteLine("---Day 20---");
            Operation();
            Console.WriteLine("------------");
        }

        public static void Operation()
        {
            var input = General.GetInput("./Days/Day20/input.txt");
            var l = ParseInput(input);
            Mix(l);
            var res = FindGroveCoordinates(l);
            Console.WriteLine($"Result p1: {res}");

            var l2 = ParseInput(input);
            AddDecryption(811589153, l2);
            for (var i = 0; i < 10; i++)
            {
                Mix(l2);
            }
            var res2 = FindGroveCoordinates(l2);
            Console.WriteLine($"Result p2: {res2}");
        }

        public static List<Pointer> ParseInput(string input)
        {
            var list = input
                .Split("\r\n")
                .Aggregate(new List<Pointer>(), (acc, x) =>
                {
                    var p = new Pointer()
                    {
                        Value = int.Parse(x)
                    };
                    
                    p.SetNeighbor(Direction.Next, acc.LastOrDefault());

                    acc.Add(p);
                    return acc;
                });

            list.First().SetNeighbor(Direction.Next, list.Last());

            return list;
        }

        public static void AddDecryption(int dec, List<Pointer> pl)
        {
            foreach (var pointer in pl)
            {
                pointer.Value *= dec;
            }
        }

        public static void Mix(List<Pointer> pl)
        {
            foreach (var pointer in pl)
            {
                if (pointer.Value == 0) continue;

                var dir = pointer.Value > 0 ? Direction.Next : Direction.Previous;
                var steps = Math.Abs(pointer.Value) % (pl.Count - 1);

                for (var i = 0; i < steps; i++)
                {
                    pointer.Step(dir);
                }
            }
        }

        public static long FindGroveCoordinates(List<Pointer> pl)
        {
            long result = 0;
            var zero = pl.Find(x => x.Value == 0) ?? throw new Exception("Element of value 0 must exist.");

            for (var mod = 1; mod <= 3; mod++)
            {
                var offset = 1000 * mod % pl.Count;
                var current = zero;

                for (var i = 0; i < offset; i++)
                {
                    current = current.Next ?? throw new Exception("Next pointer must exist in chain.");
                }

                result += current.Value;
            }

            return result;
        }

        public static void PrintList(List<Pointer> list)
        {
            foreach (var pointer in list)
            {
                Console.WriteLine(pointer);
            }
        }
    }
}
