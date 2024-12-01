using _2024.Days.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2024.Days.Day01
{
    [TestClass]
    public class Day01Tests
    {
        [TestMethod]
        public void Test()
        {
            var d = new Day01()
            {
                Input = "3   4\r\n4   3\r\n2   5\r\n1   3\r\n3   9\r\n3   3"
            };

            d.ParseInput();

            Assert.AreEqual("11", d.PartOne());
            Assert.AreEqual("31", d.PartTwo());
        }
    }

    public class Day01() : Day("01")
    {
        public readonly List<int> _left = [];
        public readonly List<int> _right = [];

        public override void ParseInput()
        {
            var rows = Input.Split("\r\n");

            foreach (var row in rows) { 
                var pair = row.Split("   ");
                _left.Add(int.Parse(pair[0]));
                _right.Add(int.Parse(pair[1]));
            }

            _left.Sort();
            _right.Sort();
        }

        public override string PartOne()
        {
            var sum = 0;
            for (int i = 0; i < _left.Count; i++)
            {
                sum += Math.Abs(_left[i] - _right[i]);
            }

            return sum.ToString();
        }

        public override string PartTwo()
        {
            var occurs = _right.GroupBy(i => i).ToDictionary(g => g.Key, g => g.Count());

            return _left.Aggregate(0, (a, item) =>
            {
                occurs.TryGetValue(item, out var mult);
                return a + mult * item;
            }).ToString();
        }
    }
}
