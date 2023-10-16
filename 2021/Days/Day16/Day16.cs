using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;

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
        public void TextBinToDecimalConversion()
        {
            Assert.AreEqual(2021, _d.ConvertBinaryToDecimal("011111100101"));
        }

        [TestMethod]
        public void TestLiteral()
        {
            var (values, remainder) = _d.ReadLiteralPacketValue("101111111000101000");
            Assert.AreEqual(2021, values[0]);
            Assert.AreEqual("000", remainder);
        }

        [TestMethod]
        public void TestReadingLiteralPacket()
        {
            var (version, values, remainder) = _d.ParsePacket(_d.ConvertHexToBinary("D2FE28"));
            Assert.AreEqual(6, version);
            Assert.AreEqual(2021, values[0]);
            Assert.AreEqual("000", remainder);
        }

        [TestMethod]
        public void TestReadingOperatorZero()
        {
            var (version, values, remainder) = _d.ParsePacket(_d.ConvertHexToBinary("38006F45291200"));
            Assert.AreEqual(9, version);
            Assert.AreEqual(1, values[0]);
            Assert.AreEqual("0000000", remainder);
        }

        [TestMethod]
        public void TestReadingOperatorOne()
        {
            var (version, values, remainder) = _d.ParsePacket(_d.ConvertHexToBinary("EE00D40C823060"));
            Assert.AreEqual(14, version);
            Assert.AreEqual(3, values[0]);
            Assert.AreEqual("00000", remainder);
        }

        [TestMethod]
        public void TestReadingOperatorsVersions()
        {
            var (v1, _, _) = _d.ParsePacket(_d.ConvertHexToBinary("8A004A801A8002F478"));
            var (v2, _, _) = _d.ParsePacket(_d.ConvertHexToBinary("620080001611562C8802118E34"));
            var (v3, _, _) = _d.ParsePacket(_d.ConvertHexToBinary("C0015000016115A2E0802F182340"));
            var (v4, _, _) = _d.ParsePacket(_d.ConvertHexToBinary("A0016C880162017C3686B18A3D4780"));

            Assert.AreEqual(16, v1);
            Assert.AreEqual(12, v2);
            Assert.AreEqual(23, v3);
            Assert.AreEqual(31, v4);
        }

        [TestMethod]
        public void TestOperations()
        {
            var version = "001";
            var lengthType = "1";
            var length = "00000000011";

            var p1 = "01010000001";
            var p2 = "01110000010";
            var p3 = "10010000011";

            // 0
            {
                var (ver, val, _) = _d.ParsePacket($"{version}000{lengthType}{length}{p1}{p2}{p3}");
                Assert.AreEqual(10, ver);
                Assert.AreEqual(6, val[0]);
            }

            // 1
            {
                var (ver, val, _) = _d.ParsePacket($"{version}001{lengthType}{length}{p1}{p2}{p3}");
                Assert.AreEqual(10, ver);
                Assert.AreEqual(6, val[0]);
            }

            // 2
            {
                var (ver, val, _) = _d.ParsePacket($"{version}010{lengthType}{length}{p1}{p2}{p3}");
                Assert.AreEqual(10, ver);
                Assert.AreEqual(1, val[0]);
            }

            // 3
            {
                var (ver, val, _) = _d.ParsePacket($"{version}011{lengthType}{length}{p1}{p2}{p3}");
                Assert.AreEqual(10, ver);
                Assert.AreEqual(3, val[0]);
            }

            length = "00000000010";

            // 5
            {
                var (ver, val, _) = _d.ParsePacket($"{version}101{lengthType}{length}{p1}{p2}");
                Assert.AreEqual(6, ver);
                Assert.AreEqual(0, val[0]);
            }
            {
                var (ver, val, _) = _d.ParsePacket($"{version}101{lengthType}{length}{p2}{p1}");
                Assert.AreEqual(6, ver);
                Assert.AreEqual(1, val[0]);
            }

            // 6
            {
                var (ver, val, _) = _d.ParsePacket($"{version}110{lengthType}{length}{p1}{p2}");
                Assert.AreEqual(6, ver);
                Assert.AreEqual(1, val[0]);
            }
            {
                var (ver, val, _) = _d.ParsePacket($"{version}110{lengthType}{length}{p2}{p1}");
                Assert.AreEqual(6, ver);
                Assert.AreEqual(0, val[0]);
            }

            // 7
            {
                var (ver, val, _) = _d.ParsePacket($"{version}111{lengthType}{length}{p1}{p1}");
                Assert.AreEqual(5, ver);
                Assert.AreEqual(1, val[0]);
            }
            {
                var (ver, val, _) = _d.ParsePacket($"{version}111{lengthType}{length}{p1}{p2}");
                Assert.AreEqual(6, ver);
                Assert.AreEqual(0, val[0]);
            }
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
            var d = new Day16();
            d.Operation();
            Console.WriteLine("------------");
        }
        private void Operation()
        {
            var input = Library.GetInput(@"./Days/Day16/input.txt");
            var inputBin = ConvertHexToBinary(input);
            var (ver, val, _) = ParsePacket(inputBin);
            Console.WriteLine($"version total: {ver}");
            Console.WriteLine($"value total: {val[0]}");
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

        public long ConvertBinaryToDecimal(string bin)
        {
            return Convert.ToInt64(bin, 2);
        }

        public (long, List<long>, string) ParsePacket(string bin)
        {
            if (bin.Length < 11)
            {
                throw new Exception("Packet string too short!");
            }

            var version = ConvertBinaryToDecimal(bin[..3]);
            var type = bin[3..6];
            var rest = bin[6..];

            switch (type)
            {
                case "000":
                    {
                        var (ver, value, remainder) = ParseOperator(rest);
                        return (version + ver, new List<long>() { value.Sum() }, remainder);
                    }
                case "001":
                    {
                        var (ver, value, remainder) = ParseOperator(rest);
                        long v = 1;
                        foreach(var c in value) { v *= c; }
                        return (version + ver, new List<long>() { v }, remainder);
                    }
                case "010":
                    {
                        var (ver, value, remainder) = ParseOperator(rest);
                        return (version + ver, new List<long>() { value.Min() }, remainder);
                    }
                case "011":
                    {
                        var (ver, value, remainder) = ParseOperator(rest);
                        return (version + ver, new List<long>() { value.Max() }, remainder);
                    }
                case "100":
                    {
                        var (value, remainder) = ReadLiteralPacketValue(rest);
                        return (version, value, remainder);
                    }
                case "101":
                    {
                        var (ver, value, remainder) = ParseOperator(rest);
                        return (version + ver, new List<long>() { value[0] > value[1] ? 1 : 0 }, remainder);
                    }
                case "110":
                    {
                        var (ver, value, remainder) = ParseOperator(rest);
                        return (version + ver, new List<long>() { value[0] < value[1] ? 1 : 0 }, remainder);
                    }
                case "111":
                    {
                        var (ver, value, remainder) = ParseOperator(rest);
                        return (version + ver, new List<long>() { value[0] == value[1] ? 1 : 0 }, remainder);
                    }
                default:
                    {
                        // Shouldn't hit here ever...
                        throw new Exception("Operator not defined!");
                    }
            }

        }

        public (List<long>, string) ReadLiteralPacketValue(string bin)
        {
            var end = false;
            var index = 0;
            var value = String.Empty;
            while (!end)
            {
                var val = bin.Substring(index, 5);
                if (val[0] == '0') // last value, so end loop
                {
                    end = true;
                }
                value = $"{value}{val[1..]}";
                index += 5;
            }
            var remainder = bin[index..];
            return (new List<long>() { ConvertBinaryToDecimal(value) }, remainder);
        }

        public (long, List<long>, string) ParseOperator(string bin)
        {
            long version = 0;
            var values = new List<long>();
            string rest;
            var lengthTypeID = bin[0];
            if (lengthTypeID == '0') // read 15 bit header of length of sub packets
            {
                var bitsLength = (int)ConvertBinaryToDecimal(bin[1..16]);
                var subPackets = bin.Substring(16, bitsLength);
                rest = bin[(16 + bitsLength)..];
                var end = false;
                while (!end)
                {
                    var (ver, value, remainder) = ParsePacket(subPackets);
                    if (string.IsNullOrEmpty(remainder)) // No more packets, so end loop
                    {
                        end = true;
                    }
                    values.AddRange(value);
                    subPackets = remainder;
                    version += ver;
                }
            }
            else // read 11 bit header of number of subpackets
            {
                var packetsLength = ConvertBinaryToDecimal(bin[1..12]);
                rest = bin.Substring(12);
                for (var i = 0; packetsLength > i; i++)
                {
                    var (ver, value, remainder) = ParsePacket(rest);
                    values.AddRange(value);
                    rest = remainder;
                    version += ver;
                }
            }

            return (version, values, rest);
        }
    }
}
