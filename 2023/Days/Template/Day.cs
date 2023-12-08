using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2023.Days.Day
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

    public class Day
    {
        public static void Run()
        {
            Console.WriteLine("---Day 00---");
            var d = new Day();
            d.Operation();
            Console.WriteLine("------------");
        }

        public void Operation()
        {
            var input = General.GetInput(@"./Days/Day/input.txt");
            Console.WriteLine(input);
        }
    }
}
