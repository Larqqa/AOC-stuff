using _2024.Days.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2024.Days.Day02
{
    [TestClass]
    public class Day02Tests
    {
        [TestMethod]
        public void Test()
        {
            var d = new Day02()
            {
                Input = "7 6 4 2 1\r\n1 2 7 8 9\r\n9 7 6 2 1\r\n1 3 2 4 5\r\n8 6 4 4 1\r\n1 3 6 7 9"
            };

            d.ParseInput();

            Assert.AreEqual("2", d.PartOne());
            Assert.AreEqual("1", d.PartTwo());
        }
    }

    public class Day02() : Day("02")
    {
        public List<List<int>> Rows = [];
        public List<List<int>> Valid = [];


        public override void ParseInput()
        {
            Rows = Input
                .Split("\r\n")
                .Select(x => x.Split(" ").Select(int.Parse).ToList()).ToList();
        }

        private static bool CheckNumber(int prev, int num, bool increasing)
        {
            if (num == prev || Math.Abs(num - prev) > 3) return false;

            return increasing switch
            {
                true when prev < num => true,
                false when prev > num => true,
                _ => false
            };
        }

        private static bool FindValidRow(List<int> row)
        {
            var curr = row[0];
            var increasing = row[0] < row[1];

            foreach (var next in row[1..])
            {
                if (!CheckNumber(curr, next, increasing)) return false;
                curr = next;
            }

            return true;
        }

        public override string PartOne()
        {
            Valid = Rows.Where(FindValidRow).ToList();

            return Valid.Count.ToString();
        }

        public override string PartTwo()
        {
            Valid = Rows.Where(row =>
            {
                for (var i = 0; i < row.Count; i++)
                {
                    List<int> clone = [..row];
                    clone.RemoveAt(i);
                    if (FindValidRow(clone)) return true;
                }

                return false;
            }).ToList();

            return Valid.Count.ToString();
        }
    }
}
