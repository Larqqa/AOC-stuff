using _2024.Days.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2024.Days.Day05
{
    [TestClass]
    public class Day05Tests
    {
        [TestMethod]
        public void Test()
        {
            var d = new Day05()
            {
                Input = "47|53\r\n97|13\r\n97|61\r\n97|47\r\n75|29\r\n61|13\r\n75|53\r\n29|13\r\n97|29\r\n53|29\r\n61|53\r\n97|53\r\n61|29\r\n47|13\r\n75|47\r\n97|75\r\n47|61\r\n75|61\r\n47|29\r\n75|13\r\n53|13\r\n\r\n75,47,61,53,29\r\n97,61,53,29,13\r\n75,29,13\r\n75,97,47,61,53\r\n61,13,29\r\n97,13,75,29,47"
            };

            d.ParseInput();

            Assert.AreEqual("143", d.PartOne());
        }
    }

    public class Day05() : Day("05")
    {
        public override void ParseInput()
        {
        }

        public override string PartOne()
        {
            throw new NotImplementedException();
        }

        public override string PartTwo()
        {
            throw new NotImplementedException();
        }
    }
}
