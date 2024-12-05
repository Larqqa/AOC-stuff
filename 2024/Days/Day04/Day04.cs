using _2024.Days.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.CompilerServices;

namespace _2024.Days.Day04
{
    [TestClass]
    public class Day04Tests
    {
        [TestMethod]
        public void Test()
        {
            var d = new Day04()
            {
                Input = "MMMSXXMASM\r\nMSAMXMSMSA\r\nAMXSXMAAMM\r\nMSAMASMSMX\r\nXMASAMXAMM\r\nXXAMMXXAMA\r\nSMSMSASXSS\r\nSAXAMASAAA\r\nMAMMMXMMMM\r\nMXMXAXMASX"
            };

            d.ParseInput();

            Assert.AreEqual("18", d.PartOne());
        }

        [TestMethod]
        public void Test2()
        {
            var d = new Day04()
            {
                Input = "MMMSXXMASM\r\nMSAMXMSMSA\r\nAMXSXMAAMM\r\nMSAMASMSMX\r\nXMASAMXAMM\r\nXXAMMXXAMA\r\nSMSMSASXSS\r\nSAXAMASAAA\r\nMAMMMXMMMM\r\nMXMXAXMASX"
            };

            d.ParseInput();

            Assert.AreEqual("48", d.PartTwo());
        }
    }

    public class Day04() : Day("04")
    {
        public List<string> Rows = [];

        public override void ParseInput()
        {
            Rows = Input.Split("\r\n").ToList();
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
