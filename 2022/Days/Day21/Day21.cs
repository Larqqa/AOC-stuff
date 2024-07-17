using System.Numerics;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2022.Days.Day21
{
    [TestClass]
    public class Day21Tests
    {
        private const string Input =
            "root: pppw + sjmn\r\ndbpl: 5\r\ncczh: sllz + lgvd\r\nzczc: 2\r\nptdq: humn - dvpt\r\ndvpt: 3\r\nlfqf: 4\r\nhumn: 5\r\nljgn: 2\r\nsjmn: drzm * dbpl\r\nsllz: 4\r\npppw: cczh / lfqf\r\nlgvd: ljgn * ptdq\r\ndrzm: hmdt - zczc\r\nhmdt: 32";

        [TestMethod]
        public void Test()
        {
            var (number, math) = Day21.ParseInput(Input);
            var root = Day21.FindMonkey("root", number, math);

            Assert.AreEqual(152, root);
        }

        [TestMethod]
        public void TestP2()
        {
            var (number, math) = Day21.ParseInput(Input);
            Assert.AreEqual(301, Day21.FindHumanNumber(number, math));
        }
    }

    public class Day21
    {
        public static void Run()
        {
            Console.WriteLine("---Day 21---");
            Operation();
            Console.WriteLine("------------");
        }

        public static void Operation()
        {
            var input = General.GetInput("./Days/Day21/input.txt");

            {
                var (number, math) = ParseInput(input);
                Console.WriteLine($"p1: { FindMonkey("root", number, math) }");
            }
            {
                var (number, math) = ParseInput(input);
                Console.WriteLine($"p2: {FindHumanNumber(number, math)}");
            }
        }

        public static (Dictionary<string, BigInteger> number, Dictionary<string, string> math) ParseInput(string input)
        {
            var numberMonkeys = new Dictionary<string, BigInteger>();
            var mathMonkeys = new Dictionary<string, string>();

            foreach (var line in input.Split("\r\n"))
            {
                var kv = line.Split(": ");

                if (long.TryParse(kv[1], out var val))
                {
                    numberMonkeys.Add(kv[0], val);
                }
                else
                {
                    mathMonkeys.Add(kv[0], kv[1]);
                }
            }

            return (numberMonkeys, mathMonkeys);
        }

        public static BigInteger FindHumanNumber(Dictionary<string, BigInteger> number, Dictionary<string, string> math)
        {
            var tempNum = number
                .ToDictionary(entry => entry.Key, entry => entry.Value);

            BigInteger mid = long.MaxValue / 2;

            (BigInteger key, BigInteger value) maxBound = (long.MaxValue, long.MaxValue);
            (BigInteger key, BigInteger value) minBound = (0, long.MaxValue);

            while (true)
            {
                var minRes = GetDifference(minBound.key, number, math);
                if (minRes == 0) return minBound.key;

                var midRes = GetDifference(mid, number, math);
                if (midRes == 0) return mid;

                var maxRes = GetDifference(maxBound.key, number, math);
                if (maxRes == 0) return maxBound.key;

                //Console.WriteLine($"{minBound.key} {mid} {maxBound.key}");
                //Console.WriteLine($"{minRes} {midRes} {maxRes}");

                if (minRes < maxRes)
                    maxBound = (mid, midRes);

                if(maxRes < minRes)
                    minBound = (mid, midRes);

                mid = minBound.key + (maxBound.key - minBound.key) / 2;
            }
        }

        public static BigInteger GetDifference(BigInteger human, Dictionary<string, BigInteger> number, Dictionary<string, string> math)
        {
            var numberCopy = number.ToDictionary(entry => entry.Key, entry => entry.Value);
            numberCopy["humn"] = human;
            FindMonkey("root", numberCopy, math); // Calculate root values
            var (first, _, last) = ReadMonkey(math["root"]);
            return BigInteger.Abs(numberCopy[first] - numberCopy[last]);
        }

        public static BigInteger FindMonkey(string monkey, Dictionary<string, BigInteger> numberMonkeys, Dictionary<string, string> mathMonkeys)
        {
            var m = mathMonkeys[monkey];
            var (first, operand, last) = ReadMonkey(m);

            var f = numberMonkeys.TryGetValue(first, out var firstMonkey) ? firstMonkey : FindMonkey(first, numberMonkeys, mathMonkeys);
            var s = numberMonkeys.TryGetValue(last, out var secondMonkey) ? secondMonkey : FindMonkey(last, numberMonkeys, mathMonkeys);

            var result  = operand switch
            {
                '+' => f + s,
                '-' => f - s,
                '/' => f / s,
                '*' => f * s,
                '=' => f == s ? 1 : 0,
                _ => throw new Exception("No such operand!")
            };
            
            numberMonkeys.Add(monkey, result);

            return result;
        }

        public static (string first, char operand, string last) ReadMonkey(string monkey)
        {
            var first = monkey[..4];
            var second = monkey[7..11];
            var operand = monkey[5];
            return (first, operand, second);
        }
    }
}
