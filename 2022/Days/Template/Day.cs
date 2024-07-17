using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2022.Days.Template
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
            Console.WriteLine("---Day XX---");
            Operation();
            Console.WriteLine("------------");
        }

        public static void Operation()
        {
            var input = General.GetInput("./Days/Day/input.txt");
            Console.WriteLine(input);
        }
    }
}
