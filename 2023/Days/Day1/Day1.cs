using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2023.Days.Day1
{
    [TestClass]
    public class DayTests
    {
        [TestMethod]
        public void Test()
        {
            Assert.IsTrue(true);
        }
    }

    public class Day1
    {
        public static void Run()
        {
            Console.WriteLine("---Day 01---");
            var d = new Day1();
            d.Operation();
            Console.WriteLine("------------");
        }

        public void Operation()
        {
            var input = General.GetInput(@"./Days/Day1/input.txt");
            Console.WriteLine(input);
        }
    }
}
