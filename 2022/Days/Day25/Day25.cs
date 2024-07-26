using System.Linq;
using System.Numerics;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2022.Days.Day25
{
    [TestClass]
    public class Day25Tests
    {
        public static string Input =
            "1=-0-2\r\n12111\r\n2=0=\r\n21\r\n2=01\r\n111\r\n20012\r\n112\r\n1=-1=\r\n1-12\r\n12\r\n1=\r\n122";

        [TestMethod]
        public void TestSnafuToNumber()
        {
            Assert.AreEqual(976, Day25.SnafuToNumber("2=-01"));
            Assert.AreEqual(2022, Day25.SnafuToNumber("1=11-2"));
            Assert.AreEqual(12345, Day25.SnafuToNumber("1-0---0"));
            Assert.AreEqual(314159265, Day25.SnafuToNumber("1121-1110-1=0"));
        }

        [TestMethod]
        public void TestNumberToSnafu()
        {
            //  2 * 625 =  1250
            // -2 * 125 = -250
            // -1 * 25  = -25
            //  0 * 5   =  0
            //  1 * 1   =  1
            // ----------------
            //          =  976

            //   1 = 976 ~ 976  = 1
            //   5 = 195 ~ 975  = 0
            //  25 = 39  ~ 975  = -1
            // 125 = 8   ~ 1000 = -2
            // 625 = 2   ~ 1250 = 2

            // 1=11-2 == 2022

            //   1 = 2022 ~ 2022 = 2022 - 2020 =  2    = 2
            //   5 = 404  ~ 2020 = 2020 - 2025 = -5    = -1
            //  25 = 81   ~ 2025 = 2025 - 2000 = 25    = 1
            // 125 = 16   ~ 2000 = 2000 - 1875 = 125   = 1
            // 625 = 3    ~ 1875 = 1875 - 3125 = -1250 = -2
            // 3125 = 1   ~ 3125                       = 1

            // Formula, check how many times the base fits longo the previous total
            // Round the divisions of previous totals, works like a charm < .5 floor >.5 Ceil
            // Example:
            // 5 fits longo 2022 404 times,  this means that base 1   covers 2022 - 2020 =  2  =  2
            // 25 fits longo 2020 81 times,  this means that base 5   covers 2020 - 2025 = -5  = -1
            // 125 fits longo 2025 16 times, this means that base 25  covers 2025 - 2000 = 25  =  1
            // 625 fits longo 2000 3 times,  this means that base 125 covers 2000 - 1875 = 125 =  1
            // 625 is over limit still, but 3125 does not fit longo 1875 3125 becomes 1
            //                              this means that base 625 covers 1875 - 3125 = -1250 = -2

            // 3125 + (625 * -2) + (125 * 1) + (25 * 1) + (5 * -1) + (1 * 2)
            // = 2022

            // 1-0---0 == 12345

            //     1 = 12345 ~ 12345 = 12345 - 12345 =  0    = 0
            //     5 = 2469  ~ 12345 = 12345 - 12350 = -5    = -
            //    25 = 494   ~ 12350 = 12350 - 12375 = -25   = -
            //   125 = 99    ~ 12375 = 12375 - 12500 = -125  = -
            //   625 = 20    ~ 12500 = 12500 - 12500 = 0     = 0
            //  3125 = 4     ~ 12500 = 12500 - 15625 = -3125 = -
            // 15625 = 1     ~ 15625                         = 1

            // 15625 + (3125 * -1) + (625 * 0) + (125 * -1) + (25 * -1) + (5 * -1) + (1 * 0)
            // = 12345

            // 12 == 7

            // 1 = 7 ~ 7 - 5 = 2 = 2
            // 5 = 1 ~ 5     = 5 = 1

            // (5 * 1) + (1 * 2)
            // 7

            // 2= == 8

            // 1 = 8 ~ 8 - 10 = -2 = =
            // 5 = 2 ~ 10     = 2  = 2

            // (5 * 2) + (1 * -2)
            // 8

            // 1=0 == 15

            //  1 = 15 ~ 15 - 15 =  0  = 0
            //  5 = 3 ~ 15 - 25  = -10 = = 
            // 25 = 1            = 25  = 1

            Assert.AreEqual("2=-01", Day25.NumberToSnafu(976));
            Assert.AreEqual("1=11-2", Day25.NumberToSnafu(2022));
            Assert.AreEqual("1-0---0", Day25.NumberToSnafu(12345));
            Assert.AreEqual("1121-1110-1=0", Day25.NumberToSnafu(314159265));
        }

        [TestMethod]
        public void TestSum()
        {
            Assert.AreEqual(4890, Day25.GetSumOfSnafus(Input));
        }
    }

    public class Day25
    {
        public static void Run()
        {
            Console.WriteLine("---Day 25---");
            Operation();
            Console.WriteLine("------------");
        }

        public static void Operation()
        {
            var input = General.GetInput("./Days/Day25/input.txt");
            var sum = GetSumOfSnafus(input);
            Console.WriteLine($"p1: sum: {sum} snafu: {NumberToSnafu(sum)}");
        }

        public static long GetSumOfSnafus(string input)
        {
            return input
                .Split("\r\n")
                .Aggregate(new List<long>(), (a, b) =>
                {
                    a.Add(SnafuToNumber(b));
                    return a;
                })
                .Sum();
        }

        public static long SnafuToNumber(string input)
        {
            long num = 0;
            long offset = 1;
            foreach (var c in input.ToCharArray().Reverse())
            {
                num += MapNumber(c) * offset;
                offset *= 5;
            }

            return num;
        }

        public static string NumberToSnafu(long number)
        {
            var snafus = new List<long>();
            var prevTotal = number;
            long offset = 5;

            while (true)
            {
                var fits = (long) Math.Round((double) number / offset);
                var total = offset * fits;
                snafus.Add((prevTotal - total) / (offset / 5));

                if (number < offset)
                {
                    if (snafus.Last() < 0) snafus.Add(1);
                    break;
                }

                prevTotal = total;
                offset *= 5;
            }

            snafus.Reverse();
            return snafus
                .Select(MapNumber)
                .Aggregate(string.Empty, (a, b) => $"{a}{b}");
        }

        public static long MapNumber(char c)
        {
            return c switch
            {
                '2' => 2,
                '1' => 1,
                '0' => 0,
                '-' => -1,
                '=' => -2,
                _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
            };
        }

        public static char MapNumber(long n)
        {
            return (int)n switch
            {
                2 => '2',
                1 => '1',
                0 => '0',
                -1 => '-',
                -2 => '=',
                _ => throw new ArgumentOutOfRangeException(nameof(n), n, null)
            };
        }
    }
}
