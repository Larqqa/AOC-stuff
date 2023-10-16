using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2021.Days.Day16
{
    [TestClass]
    public class Day16Tests
    {
        readonly Day16 _d = new();

        [TestMethod]
        public void TestConversion()
        {
            Assert.AreEqual("110100101111111000101000", _d.ConvertHexToBinary("D2FE28"));
        }

        [TestMethod]
        public void TestReading()
        {
            _d.ParsePacket("110100101111111000101000");
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestLiteral()
        {
            Assert.AreEqual(("011111100101", "000"), _d.ReadLiteralPacketValue("101111111000101000"));
        }

        [TestMethod]
        public void TextBinToDecimalConversion()
        {
            Assert.AreEqual(2021, _d.ConvertBinaryToDecimal("011111100101"));
        }
    }

    public class Day16
    {
        private readonly Dictionary<string, string> map = new Dictionary<string, string>()
            {
                { "0", "0000" },
                { "1", "0001" },
                { "2", "0010" },
                { "3", "0011" },
                { "4", "0100" },
                { "5", "0101" },
                { "6", "0110" },
                { "7", "0111" },
                { "8", "1000" },
                { "9", "1001" },
                { "A", "1010" },
                { "B", "1011" },
                { "C", "1100" },
                { "D", "1101" },
                { "E", "1110" },
                { "F", "1111" }
            };
        public static void Run()
        {
            Console.WriteLine("---Day 16---");
            Operation();
            Console.WriteLine("------------");
        }
        private static void Operation()
        {
            var input = Library.GetInput(@"./Days/Day16/input.txt");
            Console.WriteLine(input);
        }

        public string ConvertHexToBinary(string hex)
        {
            string bin = string.Empty;

            foreach (char c in hex)
            {
                try
                {
                    map.TryGetValue(c.ToString(), out var val);
                    bin = $"{bin}{val}";
                }
                catch
                {
                    throw new Exception("Invalid hex value");
                }
            }

            return bin;
        }

        public int ConvertBinaryToDecimal(string bin)
        {
            return Convert.ToInt32(bin, 2);
        }

        public void ParsePacket(string bin)
        {
            var version = bin[..3];
            var type = bin[3..5];
            var rest = bin[6..];

            if (type == "100")
            {
                var (value, remainder) = ReadLiteralPacketValue(rest);
            }
            else
            {
                var (value, remainder) = ParseOperator(rest);
            }
        }

        public (string, string) ReadLiteralPacketValue(string bin)
        {
            var end = false;
            var index = 0;
            var value = String.Empty;
            while (!end)
            {
                var val = bin.Substring(index, 5);
                if (val[0] == '0')
                {
                    end = true;
                }
                value = $"{value}{val[1..]}";
                index += 5;
            }

            return (value, bin[index..]);
        }

        public (string, string) ParseOperator(string bin)
        {
            var lengthTypeID = bin[0];
            if (lengthTypeID == '0')
            {
                var bitsLength = ConvertBinaryToDecimal(bin[1..14]);
                var subPackets = bin.Substring(16, bitsLength);
            }
            else
            {
                var packetsLength = ConvertBinaryToDecimal(bin[1..10]);
                for (var i = 0; packetsLength >= i; i++)
                {
                    var subPacket = bin;
                }
            }

            return ("", "");
        }
    }
}
