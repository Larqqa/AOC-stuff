using _2024.Days.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.CompilerServices;

namespace _2024.Days.Day03
{
    [TestClass]
    public class Day03Tests
    {
        [TestMethod]
        public void Test()
        {
            var d = new Day03()
            {
                Input = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))"
            };

            d.ParseInput();

            Assert.AreEqual("161", d.PartOne());
        }

        [TestMethod]
        public void Test2()
        {
            var d = new Day03()
            {
                Input = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))"
            };

            d.ParseInput();

            Assert.AreEqual("48", d.PartTwo());
        }
    }

    public class Day03() : Day("03")
    {
        private const string MatchStr = @"mul\(([0-9,;]*)\)";
        public List<string> Rows = [];

        public override void ParseInput()
        {
            Rows = Input.Split("\r\n").ToList();
        }

        public static int Mull(string str) => str.Replace("mul(", "").Replace(")", "").Split(",").Select(int.Parse).Aggregate(1, (a, b) => a * b);

        public override string PartOne() => Rows.Aggregate(0, (sum, row) => sum + Regex.Matches(row, MatchStr).Aggregate(0, (agg, match) => agg + Mull(match.Value))).ToString();

        public override string PartTwo()
        {
            var doMul = true;
            return Rows.Aggregate(0, (sum, row) =>
            {
                var res = Regex
                    .Matches(row, MatchStr + @"|don't\(\)|do\(\)")
                    .Aggregate(0, (agg, match) =>
                    {
                        var str = match.Value;

                        if (str is not ("do()" or "don't()"))
                            return doMul ? agg + Mull(str) : agg;

                        doMul = str is "do()";
                        return agg;
                    });

                return sum + res;
            }).ToString();
        }
    }
}
