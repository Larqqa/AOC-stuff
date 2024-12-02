using _2024.Days.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static System.Net.Mime.MediaTypeNames;

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
            Assert.AreEqual("4", d.PartTwo());
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
            if (num == prev) return false;

            if (increasing)
            {
                if (prev > num || num - prev > 3) return false;
            }
            else
            {
                if (prev < num || prev - num > 3) return false;
            }

            return true;
        }

        public override string PartOne()
        {
            Valid = Rows.Where(row =>
            {
                var curr = row[0];
                var increasing = row[0] < row[1];

                foreach (var next in row[1..])
                {
                    if (!CheckNumber(curr, next, increasing)) return false;

                    curr = next;
                }

                return true;
            }).ToList();

            return Valid.Count.ToString();
        }

        public override string PartTwo()
        {
            Valid = Rows.Where(row =>
            {
                var removed = false;
                var i = 0;
                var increasing = row[0] < row[1];

                while (true)
                {
                    if (i + 2 >= row.Count) return true;

                    var curr = row[i];
                    var next = row[i + 1];

                    if (!CheckNumber(curr, next, increasing))
                    {
                        if (removed) return false;
                        removed = true;

                        if (CheckNumber(curr, row[i + 2], curr < row[i + 2]))
                        {
                            increasing = i > 1 ? curr < row[i + 2] : increasing;
                            row.RemoveAt(i + 1);
                            i++;
                            continue;
                        }

                        if (CheckNumber(next, row[i + 2], next < row[i + 2]))
                        {
                            increasing = i > 1 ? next < row[i + 2] : increasing;
                            row.RemoveAt(i);

                            if (i != 0 && !CheckNumber(row[i - 1], next, increasing)) return false;

                            continue;
                        }

                        return false;
                    }

                    i++;
                }
            }).ToList();

            // 543 too high
            // 497 too high

            return Valid.Count.ToString();
        }
    }
}
