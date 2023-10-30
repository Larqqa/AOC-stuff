using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace _2021.Days.Day23
{
    [TestClass]
    public class Day23Tests
    {
        [TestMethod]
        public void TestParsing()
        {
            var input = $"#############\r\n#...........#\r\n###B#C#B#D###\r\n  #A#D#C#A#  \r\n  #########  ";
            var b = new Burrow(input);
            b.PrintMap();

            Assert.AreEqual("##############..&.&.&.&..####x#x#x#x###  #x#x#x#x#    #########  (3,2,0)Bronze(5,2,0)Copper(7,2,0)Bronze(9,2,0)Desert(3,3,0)Amber(5,3,0)Desert(7,3,0)Copper(9,3,0)Amber", b.ToString());
        }

        [TestMethod]
        public void TestSolver()
        {
            var input = $"#############\r\n#...........#\r\n###B#C#B#D###\r\n  #A#D#C#A#  \r\n  #########  ";
            var b = new Burrow(input);
            b.PrintMap();

            Assert.AreEqual(12521, Burrow.Solve(b));
        }
    }

    public class Day23
    {
        public static void Run()
        {

            Console.WriteLine("---Day 23---");
            var d = new Day23();
            var input = $"#############\r\n#...........#\r\n###D#D#C#B###\r\n  #B#A#A#C#  \r\n  #########  ";
            d.Operation(input);
            Console.WriteLine("------------");
        }
        private void Operation(string input)
        {
            var b = new Burrow(input);
            var res = Burrow.Solve(b);
            Console.WriteLine($"Least amount of moves is: {res}");
        }
    }
}
