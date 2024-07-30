using _2023.Days.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2023.Days.Day01
{
    [TestClass]
    public class Day01Tests
    {
        [TestMethod]
        public void Test()
        {
            var d = new Day01()
            {
                Input = "two1nine\r\neightwothree\r\nabcone2threexyz\r\nxtwone3four\r\n4nineeightseven2\r\nzoneight234\r\n7pqrstsixteen"
            };
            d.ParseInput();
           
            Assert.AreEqual("281", d.PartTwo());
        }

        [TestMethod]
        public void Inputs()
        {
            var d = new Day01();

            d.Input = "1twothreefour5";
            d.ParseInput();
            Assert.AreEqual("15", d.PartTwo());

            d.Input = "two1threefour5";
            d.ParseInput();
            Assert.AreEqual("25", d.PartTwo());

            d.Input = "two1three5four";
            d.ParseInput();
            Assert.AreEqual("24", d.PartTwo());

            d.Input = "aa1twothreefour5";
            d.ParseInput();
            Assert.AreEqual("15", d.PartTwo());

            d.Input = "aatwo1threefour5";
            d.ParseInput();
            Assert.AreEqual("25", d.PartTwo());

            d.Input = "aatwo1three5four";
            d.ParseInput();
            Assert.AreEqual("24", d.PartTwo());

            d.Input = "1twothreefour5aa";
            d.ParseInput();
            Assert.AreEqual("15", d.PartTwo());

            d.Input = "two1threefour5aa";
            d.ParseInput();
            Assert.AreEqual("25", d.PartTwo());

            d.Input = "two1three5fouraa";
            d.ParseInput();
            Assert.AreEqual("24", d.PartTwo());

            d.Input = "aa1twothreefour5bb";
            d.ParseInput();
            Assert.AreEqual("15", d.PartTwo());

            d.Input = "aatwo1threefour5bb";
            d.ParseInput();
            Assert.AreEqual("25", d.PartTwo());

            d.Input = "aatwo1three5fourbb";
            d.ParseInput();
            Assert.AreEqual("24", d.PartTwo());

            d.Input = "abcone1";
            d.ParseInput();
            Assert.AreEqual("11", d.PartTwo());

            d.Input = "abcone";
            d.ParseInput();
            Assert.AreEqual("11", d.PartTwo());

            d.Input = "abc1";
            d.ParseInput();
            Assert.AreEqual("11", d.PartTwo());

            d.Input = "12";
            d.ParseInput();
            Assert.AreEqual("12", d.PartTwo());

            d.Input = "1two";
            d.ParseInput();
            Assert.AreEqual("12", d.PartTwo());

            d.Input = "one2";
            d.ParseInput();
            Assert.AreEqual("12", d.PartTwo());

            d.Input = "onetwo";
            d.ParseInput();
            Assert.AreEqual("12", d.PartTwo());

            d.Input = "ab8ab";
            d.ParseInput();
            Assert.AreEqual("88", d.PartTwo());
        }

        [TestMethod]
        public void TestingBroken()
        {
            var d = new Day01();
            d.Input = "7one718onegfqtdbtxfcmd";
            d.ParseInput();
            Assert.AreEqual("71", d.PartTwo());

        }
    }

    public class Day01() : Day("01")
    {
        private List<char[]> _rows = [];

        public override void ParseInput()
        {
            _rows = Input.Split("\r\n").Aggregate(new List<char[]>(), (a, b) =>
            {
                a.Add(b.ToCharArray());
                return a;
            });
        }

        public override string PartOne()
        {
            return _rows.Aggregate(0, (a, row) =>
            {
                var l = row.ToList();
                var first = l.First(x => int.TryParse(x.ToString(), out _));
                var last = l.Last(x => int.TryParse(x.ToString(), out _));
                a += int.Parse($"{first}{last}");
                return a;
            }).ToString();
        }

        private static readonly List<string> Numlist =
        [
            "one", "two", "three", "four", "five",
            "six", "seven", "eight", "nine"
        ];

        public override string PartTwo()
        {
            return _rows.Aggregate(0, (a, row) =>
            {
                var rowString = string.Join("", row);
                if (string.IsNullOrEmpty(rowString)) return a;

                var chars = row.ToList();
                var first = chars.FirstOrDefault(x => int.TryParse(x.ToString(), out _));
                var firstOccurrence = chars.IndexOf(first);
                if (firstOccurrence == -1) firstOccurrence = row.Length; // not found

                foreach (var num in Numlist)
                {
                    var index = rowString.IndexOf(num, StringComparison.Ordinal);
                    if (index < 0 || firstOccurrence <= index) continue;

                    first = char.Parse((Numlist.IndexOf(num) + 1).ToString());
                    firstOccurrence = index;
                }

                chars = row.Reverse().ToList();
                var last = chars.FirstOrDefault(x => int.TryParse(x.ToString(), out _));
                var lastOccurrence = row.Length - 1 - chars.IndexOf(last);
                if (lastOccurrence == row.Length) lastOccurrence = 0; // not found

                foreach (var num in Numlist)
                {
                    var index = rowString.LastIndexOf(num, StringComparison.Ordinal);
                    if (index < 0 || lastOccurrence >= index) continue;

                    last = char.Parse((Numlist.IndexOf(num) + 1).ToString());
                    lastOccurrence = index;
                }

                a += int.Parse($"{first}{last}");
                return a;
            }).ToString();
        }
    }
}
