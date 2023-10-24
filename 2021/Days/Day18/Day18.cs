using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace _2021.Days.Day18
{
    [TestClass]
    public class Day18Tests
    {
        readonly Day18 _d = new();

        [TestMethod]
        public void TestParsing()
        {
            {
                var input = "[1,2]";
                var (num, _) = _d.ConvertToSnailfish(input);
                var output = Day18.ConvertToJson(num);
                var expect = "{\"LeftNumber\":1,\"RightNumber\":2,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}";
                Assert.AreEqual(expect, output);
            }
            {
                var input = "[[1,2],[3,4]]";
                var (num, _) = _d.ConvertToSnailfish(input);
                var output = Day18.ConvertToJson(num);
                var expect = "{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":1,\"RightNumber\":2,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":{\"LeftNumber\":3,\"RightNumber\":4,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}";
                Assert.AreEqual(expect, output);
            }
            {
                var input = "[[1,2],[[3,4],5]]";
                var (num, _) = _d.ConvertToSnailfish(input);
                var output = Day18.ConvertToJson(num);
                var expect = "{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":1,\"RightNumber\":2,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":5,\"LeftSnailfishNumber\":{\"LeftNumber\":3,\"RightNumber\":4,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":null}}";
                Assert.AreEqual(expect, output);
            }
            {
                var input = "[[1,[[3,4],5]],2]";
                var (num, _) = _d.ConvertToSnailfish(input);
                var output = Day18.ConvertToJson(num);
                var expect = "{\"LeftNumber\":null,\"RightNumber\":2,\"LeftSnailfishNumber\":{\"LeftNumber\":1,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":5,\"LeftSnailfishNumber\":{\"LeftNumber\":3,\"RightNumber\":4,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":null}},\"RightSnailfishNumber\":null}";
                Assert.AreEqual(expect, output);
            }
        }

        [TestMethod]
        public void TestExploding()
        {
            {
                var input = "[[[[[9, 8],1],2],3],4]";
                var (num, _) = _d.ConvertToSnailfish(input);
                var (n, _, _, _) = _d.ExplodeSnailfish(num);
                var output = Day18.ConvertToJson(n);
                var expect = "{\"LeftNumber\":null,\"RightNumber\":4,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":3,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":2,\"LeftSnailfishNumber\":{\"LeftNumber\":0,\"RightNumber\":9,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":null}";
                Assert.AreEqual(expect, output);
            }
            {
                var input = "[7,[6,[5,[4,[3,2]]]]]";
                var (num, _) = _d.ConvertToSnailfish(input);
                var (n, _, _, _) = _d.ExplodeSnailfish(num);
                var output = Day18.ConvertToJson(n);
                var expect = "{\"LeftNumber\":7,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":6,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":5,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":7,\"RightNumber\":0,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}}}";
                Assert.AreEqual(expect, output);
            }
            {
                var input = "[[6,[5,[4,[3,2]]]],1]";
                var (num, _) = _d.ConvertToSnailfish(input);
                var (n, _, _, _) = _d.ExplodeSnailfish(num);
                var output = Day18.ConvertToJson(n);
                var expect = "{\"LeftNumber\":null,\"RightNumber\":3,\"LeftSnailfishNumber\":{\"LeftNumber\":6,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":5,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":7,\"RightNumber\":0,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}},\"RightSnailfishNumber\":null}";
                Assert.AreEqual(expect, output);
            }
            {
                var input = "[[0,0],[[[[1,1],0],0],0]]";
                var (num, _) = _d.ConvertToSnailfish(input);
                var (n, _, _, _) = _d.ExplodeSnailfish(num);
                var output = Day18.ConvertToJson(n);
                var expect = "{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":0,\"RightNumber\":1,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":0,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":0,\"LeftSnailfishNumber\":{\"LeftNumber\":0,\"RightNumber\":1,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":null}}";
                Assert.AreEqual(expect, output);
            }
            {
                var input = "[[0,[0,[0,[1,1]]]],[0,0]]";
                var (num, _) = _d.ConvertToSnailfish(input);
                var (n, _, _, _) = _d.ExplodeSnailfish(num);
                var output = Day18.ConvertToJson(n);
                var expect = "{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":0,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":0,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":1,\"RightNumber\":0,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}},\"RightSnailfishNumber\":{\"LeftNumber\":1,\"RightNumber\":0,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}";
                Assert.AreEqual(expect, output);
            }
            {
                var input = "[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]";
                var (num, _) = _d.ConvertToSnailfish(input);
                var (n, _, _, _) = _d.ExplodeSnailfish(num);
                var output = Day18.ConvertToJson(n);
                var expect = "{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":3,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":2,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":8,\"RightNumber\":0,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}},\"RightSnailfishNumber\":{\"LeftNumber\":9,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":5,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":4,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":3,\"RightNumber\":2,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}}}}";
                Assert.AreEqual(expect, output);
            }
            {
                var input = "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]";
                var (num, _) = _d.ConvertToSnailfish(input);
                var (n, _, _, _) = _d.ExplodeSnailfish(num);
                var output = Day18.ConvertToJson(n);
                var expect = "{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":3,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":2,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":8,\"RightNumber\":0,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}},\"RightSnailfishNumber\":{\"LeftNumber\":9,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":5,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":7,\"RightNumber\":0,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}}}";
                Assert.AreEqual(expect, output);
            }
            {
                var input = "[[0,[0,[0,0]]],[[[[1,1],0],0],0]]";
                var (num, _) = _d.ConvertToSnailfish(input);
                var (n, _, _, _) = _d.ExplodeSnailfish(num);
                var output = Day18.ConvertToJson(n);
                var expect = "{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":0,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":0,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":0,\"RightNumber\":1,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}},\"RightSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":0,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":0,\"LeftSnailfishNumber\":{\"LeftNumber\":0,\"RightNumber\":1,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":null}}";
                Assert.AreEqual(expect, output);
            }
            {
                var input = "[[0,[0,[0,[1,1]]]],[[[0,0],0],0]]";
                var (num, _) = _d.ConvertToSnailfish(input);
                var (n, _, _, _) = _d.ExplodeSnailfish(num);
                var output = Day18.ConvertToJson(n);
                var expect = "{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":0,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":0,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":1,\"RightNumber\":0,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}},\"RightSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":0,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":0,\"LeftSnailfishNumber\":{\"LeftNumber\":1,\"RightNumber\":0,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":null}}";
                Assert.AreEqual(expect, output);
            }
        }
    
        [TestMethod]
        public void TestExplodeContinue()
        {
            {
                var input = "[0,0]";
                var (num, _) = _d.ConvertToSnailfish(input);
                var (_, _, _, more) = _d.ExplodeSnailfish(num);
                Assert.IsFalse(more);
            }
            {
                var input = "[[[[[9,8],1],2],3],4]";
                var (num, _) = _d.ConvertToSnailfish(input);
                var (n, _, _, m) = _d.ExplodeSnailfish(num);
                Assert.IsTrue(m);
                var (n2, _, _, m2) = _d.ExplodeSnailfish(n);
                Assert.IsFalse(m2);
            }
            {
                var input = "[[[[[1,1],[2,2]],[3,3]],[4,4]],[5,5]]";
                var (num, _) = _d.ConvertToSnailfish(input);
                var (n, _, _, _) = _d.ExplodeSnailfish(num);
                var output = Day18.ConvertToJson(n);
                var expect = "{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":0,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":3,\"RightNumber\":2,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}},\"RightSnailfishNumber\":{\"LeftNumber\":3,\"RightNumber\":3,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}},\"RightSnailfishNumber\":{\"LeftNumber\":4,\"RightNumber\":4,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}},\"RightSnailfishNumber\":{\"LeftNumber\":5,\"RightNumber\":5,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}";
                var (n2, _, _, _) = _d.ExplodeSnailfish(n);
                var output2 = Day18.ConvertToJson(n2);
                var expect2 = "{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":3,\"RightNumber\":0,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":{\"LeftNumber\":5,\"RightNumber\":3,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}},\"RightSnailfishNumber\":{\"LeftNumber\":4,\"RightNumber\":4,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}},\"RightSnailfishNumber\":{\"LeftNumber\":5,\"RightNumber\":5,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}";
                Assert.AreEqual(expect2, output2);
            }
        }

        [TestMethod]
        public void TestSplitting()
        {
            var input = "[15,[0,13]]";
            var (num, _) = _d.ConvertToSnailfish(input);
            var (n, m) = _d.SplitSnailfish(num);
            Assert.IsTrue(m);
            var output = Day18.ConvertToJson(n);
            var expect = "{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":7,\"RightNumber\":8,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":{\"LeftNumber\":0,\"RightNumber\":13,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}";
            Assert.AreEqual(expect, output);

            var (n2, m2) = _d.SplitSnailfish(n);
            Assert.IsTrue(m2);
            var output2 = Day18.ConvertToJson(n2);
            var expect2 = "{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":7,\"RightNumber\":8,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":{\"LeftNumber\":0,\"RightNumber\":null,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":{\"LeftNumber\":6,\"RightNumber\":7,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}}";
            Assert.AreEqual(expect2, output2);

            var (_, m3) = _d.SplitSnailfish(n2);
            Assert.IsFalse(m3);
        }

        [TestMethod]
        public void TestReducing()
        {
            {
                var input = "[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]";
                var (num, _) = _d.ConvertToSnailfish(input);
                var n = _d.ReduceSnailfish(num);
                var output = Day18.ConvertToJson(n);
                var expect = "{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":4,\"LeftSnailfishNumber\":{\"LeftNumber\":0,\"RightNumber\":7,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":7,\"RightNumber\":8,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":{\"LeftNumber\":6,\"RightNumber\":0,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}},\"RightSnailfishNumber\":{\"LeftNumber\":8,\"RightNumber\":1,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}";
                Assert.AreEqual(expect, output);
            }
            {
                var input = "[[[[[1,1],[2,2]],[3,3]],[4,4]],[5,5]]";
                var (num, _) = _d.ConvertToSnailfish(input);
                var n = _d.ReduceSnailfish(num);
                var output = Day18.ConvertToJson(n);
                var expect = "{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":3,\"RightNumber\":0,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":{\"LeftNumber\":5,\"RightNumber\":3,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}},\"RightSnailfishNumber\":{\"LeftNumber\":4,\"RightNumber\":4,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}},\"RightSnailfishNumber\":{\"LeftNumber\":5,\"RightNumber\":5,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}";
                Assert.AreEqual(expect, output);
            }
        }

        [TestMethod]
        public void TestAddition()
        {
            {
                var input = new List<string>()
                {
                    "[1,1]",
                    "[2,2]",
                    "[3,3]",
                    "[4,4]",
                };
                var list = input.Select(x =>
                {
                    var (n, _) = _d.ConvertToSnailfish(x);
                    return n;
                }).ToArray();
                var res = _d.DoSnailfishAddition(list);
                var output = Day18.ConvertToJson(res);
                var expect = "{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":1,\"RightNumber\":1,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":{\"LeftNumber\":2,\"RightNumber\":2,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}},\"RightSnailfishNumber\":{\"LeftNumber\":3,\"RightNumber\":3,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}},\"RightSnailfishNumber\":{\"LeftNumber\":4,\"RightNumber\":4,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}";
                Assert.AreEqual(expect, output);
            }
            {
                var input = new List<string>()
                {
                    "[1,1]",
                    "[2,2]",
                    "[3,3]",
                    "[4,4]",
                    "[5,5]",
                };
                var list = input.Select(x =>
                {
                    var (n, _) = _d.ConvertToSnailfish(x);
                    return n;
                }).ToArray();
                var res = _d.DoSnailfishAddition(list);
                var output = Day18.ConvertToJson(res);
                var expect = "{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":3,\"RightNumber\":0,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":{\"LeftNumber\":5,\"RightNumber\":3,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}},\"RightSnailfishNumber\":{\"LeftNumber\":4,\"RightNumber\":4,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}},\"RightSnailfishNumber\":{\"LeftNumber\":5,\"RightNumber\":5,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}";
                Assert.AreEqual(expect, output);
            }
            {
                var input = new List<string>()
                {
                    "[1,1]",
                    "[2,2]",
                    "[3,3]",
                    "[4,4]",
                    "[5,5]",
                    "[6,6]",
                };
                var list = input.Select(x =>
                {
                    var (n, _) = _d.ConvertToSnailfish(x);
                    return n;
                }).ToArray();
                var res = _d.DoSnailfishAddition(list);
                var output = Day18.ConvertToJson(res);
                var expect = "{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":5,\"RightNumber\":0,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":{\"LeftNumber\":7,\"RightNumber\":4,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}},\"RightSnailfishNumber\":{\"LeftNumber\":5,\"RightNumber\":5,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}},\"RightSnailfishNumber\":{\"LeftNumber\":6,\"RightNumber\":6,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}";
                Assert.AreEqual(expect, output);
            }
            {
                var input = new List<string>()
                {
                    "[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]",
                    "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]",
                    "[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]",
                    "[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]",
                    "[7,[5,[[3,8],[1,4]]]]",
                    "[[2,[2,2]],[8,[8,1]]]",
                    "[2,9]",
                    "[1,[[[9,3],9],[[9,0],[0,7]]]]",
                    "[[[5,[7,4]],7],1]",
                    "[[[[4,2],2],6],[8,7]]",
                };
                var list = input.Select(x =>
                {
                    var (n, _) = _d.ConvertToSnailfish(x);
                    return n;
                }).ToArray();
                var res = _d.DoSnailfishAddition(list);
                var output = Day18.ConvertToJson(res);
                var expect = "{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":8,\"RightNumber\":7,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":{\"LeftNumber\":7,\"RightNumber\":7,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}},\"RightSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":8,\"RightNumber\":6,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":{\"LeftNumber\":7,\"RightNumber\":7,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}},\"RightSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":0,\"RightNumber\":7,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":{\"LeftNumber\":6,\"RightNumber\":6,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}},\"RightSnailfishNumber\":{\"LeftNumber\":8,\"RightNumber\":7,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}}";
                Assert.AreEqual(expect, output);
            }
        }

        [TestMethod]
        public void TestMagnitude()
        {
            {
                var input = "[9,1]";
                var (n, _) = _d.ConvertToSnailfish(input);
                var num = _d.GetMagnitude(n);
                Assert.AreEqual(29, num);
            }
            {
                var input = "[[9,1],[1,9]]";
                var (n, _) = _d.ConvertToSnailfish(input);
                var num = _d.GetMagnitude(n);
                Assert.AreEqual(129, num);
            }
            {
                var input = "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]";
                var (n, _) = _d.ConvertToSnailfish(input);
                var num = _d.GetMagnitude(n);
                Assert.AreEqual(3488, num);
            }
        }

        [TestMethod]
        public void TestPart1Operation()
        {
            var input = new List<string>()
            {
                "[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]",
                "[[[5,[2,8]],4],[5,[[9,9],0]]]",
                "[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]",
                "[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]",
                "[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]",
                "[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]",
                "[[[[5,4],[7,7]],8],[[8,3],8]]",
                "[[9,3],[[9,9],[6,[4,9]]]]",
                "[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]",
                "[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]"
            };
            var list = input.Select(x =>
            {
                var (n, _) = _d.ConvertToSnailfish(x);
                return n;
            }).ToArray();
            var res = _d.DoSnailfishAddition(list);

            var output = Day18.ConvertToJson(res);
            var expect = "{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":6,\"RightNumber\":6,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":{\"LeftNumber\":7,\"RightNumber\":6,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}},\"RightSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":7,\"RightNumber\":7,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":{\"LeftNumber\":7,\"RightNumber\":0,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}},\"RightSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":7,\"RightNumber\":7,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":{\"LeftNumber\":7,\"RightNumber\":7,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}},\"RightSnailfishNumber\":{\"LeftNumber\":null,\"RightNumber\":null,\"LeftSnailfishNumber\":{\"LeftNumber\":7,\"RightNumber\":8,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null},\"RightSnailfishNumber\":{\"LeftNumber\":9,\"RightNumber\":9,\"LeftSnailfishNumber\":null,\"RightSnailfishNumber\":null}}}}";
            Assert.AreEqual(expect, output);

            var num = _d.GetMagnitude(res);
            Assert.AreEqual(4140, num);
        }

        [TestMethod]
        public void TestLargestMagnitude()
        {
            var input = new List<string>()
            {
                "[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]",
                "[[[5,[2,8]],4],[5,[[9,9],0]]]",
                "[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]",
                "[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]",
                "[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]",
                "[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]",
                "[[[[5,4],[7,7]],8],[[8,3],8]]",
                "[[9,3],[[9,9],[6,[4,9]]]]",
                "[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]",
                "[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]"
            };
            var list = input.Select(x =>
            {
                var (n, _) = _d.ConvertToSnailfish(x);
                return n;
            }).ToArray();
            var res = _d.FindLargestMagnitude(list);
            Assert.AreEqual(3993, res);
        }
    }
    
    public class Day18
	{
        public static void Run()
        {
            Console.WriteLine("---Day 18---");
            var d = new Day18();
            d.Operation();
            Console.WriteLine("------------");
        }
        private void Operation()
        {
            var input = Library.GetInput(@"./Days/Day18/input.txt").Split("\n");
            var list = input.Select(x =>
            {
                var (n, _) = ConvertToSnailfish(x);
                return n;
            });
            
            var res = DoSnailfishAddition(list.ToArray());
            var num = GetMagnitude(res);
            Console.WriteLine($"Magnitude of number: {num}");

            var largest = FindLargestMagnitude(list.ToArray());
            Console.WriteLine($"Largest magnitude of two numbers: {largest}");
        }

        public static string ConvertToJson(SnailfishNumber num)
        {
            return JsonConvert.SerializeObject(num);
        }

        public (SnailfishNumber, int) ConvertToSnailfish(string input)
        {
            SnailfishNumber num = new SnailfishNumber();
            var stack = String.Empty;
            var index = 0;
            while (index < input.Length)
            {
                var c = input[index];

                if (c == '[')
                {
                    var nextIndex = index + 1;
                    if (input[nextIndex] == '[') // If next is nested
                    {
                        var (sNum, rIndex) = ConvertToSnailfish(input[nextIndex..]);
                        num.LeftSnailfishNumber = sNum;
                        index = nextIndex + rIndex;
                        continue;
                    }
                }
                else if (c == ']')
                {
                    if (int.TryParse(stack, out var number))
                    {
                        num.RightNumber = number;
                    }

                    return (num, index + 1);
                }
                else if (c == ',')
                {
                    if (int.TryParse(stack, out var number))
                    {
                        num.LeftNumber = number;
                        stack = String.Empty;
                    }

                    var nextIndex = index + 1;
                    if (input[nextIndex] == '[')
                    {
                        var (sNum, rIndex) = ConvertToSnailfish(input[nextIndex..]);
                        num.RightSnailfishNumber = sNum;
                        index = nextIndex + rIndex;
                        continue;
                    }
                }
                else if (Char.IsNumber(c))
                {
                    stack = $"{stack}{c}";
                }

                index++;
            }

            throw new Exception("Shouldn't hit here ever!");
        }

        public SnailfishNumber FindLeftMostNumber(SnailfishNumber num)
        {
            if (num.LeftSnailfishNumber == null) return num;
            return FindLeftMostNumber(num.LeftSnailfishNumber);
        }

        public SnailfishNumber FindRightMostNumber(SnailfishNumber num)
        {
            if (num.RightSnailfishNumber == null) return num;
            return FindRightMostNumber(num.RightSnailfishNumber);
        }
        public SnailfishNumber ReduceSnailfish(SnailfishNumber num)
        {
            var canReduce = true;
            while (canReduce)
            {
                {
                    var (n, _, _, more) = ExplodeSnailfish(num);
                    if (n == null) throw new Exception("There should be a number!");
                    num = n;
                    if (more) continue;
                }
                {
                    var (n, more) = SplitSnailfish(num);
                    if (n == null) throw new Exception("There should be a number!");
                    num = n;
                    if (more) continue;
                }
                canReduce = false;
            }
            return num;
        }

        public (SnailfishNumber?, int?, int?, bool) ExplodeSnailfish(SnailfishNumber num, int nesting = 0, bool exploded = false)
        {
            // Maximum nesting allowed, explode number unless exploded
            if (!exploded && nesting == 4)
            {
                return (null, num.LeftNumber, num.RightNumber, true);
            }

            bool moreToExplode = false;
            int? explodeLeft = null;
            int? explodeRight = null;

            if (num.LeftSnailfishNumber != null)
            {
                var (n,l,r,m) = ExplodeSnailfish(num.LeftSnailfishNumber, nesting + 1, moreToExplode);

                if (n == null)
                {
                    num.LeftSnailfishNumber = null;
                    num.LeftNumber = 0;
                }

                if (moreToExplode && r != null && num.LeftSnailfishNumber != null && num.RightSnailfishNumber != null)
                {
                    var ln = FindLeftMostNumber(num.RightSnailfishNumber);
                    ln.LeftNumber += r;
                    return (num, null, null, true);
                }

                moreToExplode = m;
                explodeRight = r;
                explodeLeft = l;

                if (num.RightNumber != null && r != null)
                {
                    num.RightNumber += r;
                    explodeRight = null;
                }
                else if (num.RightSnailfishNumber != null && r != null)
                {
                    var ln = FindLeftMostNumber(num.RightSnailfishNumber);
                    ln.LeftNumber += r;
                    explodeRight = null;
                }

                if (moreToExplode) return (num, explodeLeft, explodeRight, moreToExplode);
            }

            if (num.RightSnailfishNumber != null)
            {
                var (n, l, r, m) = ExplodeSnailfish(num.RightSnailfishNumber, nesting + 1, moreToExplode);

                if (n == null)
                {
                    num.RightSnailfishNumber = null;
                    num.RightNumber = 0;
                }

                if (moreToExplode && l != null && num.LeftSnailfishNumber != null && num.RightSnailfishNumber != null)
                {
                    var rn = FindRightMostNumber(num.LeftSnailfishNumber);
                    rn.RightNumber += explodeLeft;
                    return (num, null, null, true);
                }

                moreToExplode = m;
                explodeRight = r;
                explodeLeft = l;

                if (num.LeftNumber != null && l != null)
                {
                    num.LeftNumber += l;
                    explodeLeft = null;
                }
                else if (num.LeftSnailfishNumber != null && l != null)
                {
                    var ln = FindRightMostNumber(num.LeftSnailfishNumber);
                    ln.RightNumber += l;
                    explodeLeft = null;
                }

                if (moreToExplode) return (num, explodeLeft, explodeRight, moreToExplode);
            }

            return (num, explodeLeft, explodeRight, moreToExplode);
        }

        public (SnailfishNumber?, bool) SplitSnailfish(SnailfishNumber num)
        {
            var sn = new SnailfishNumber();
            double d;
            var didSplit = false;

            if (num.LeftSnailfishNumber != null)
            {
                var (n, did) = SplitSnailfish(num.LeftSnailfishNumber);
                if (did)
                {
                    num.LeftSnailfishNumber = n;
                    return (num, did);
                }
            }

            if (num.RightSnailfishNumber != null && (num.LeftNumber == null || num.LeftNumber < 10))
            {
                var (n, did) = SplitSnailfish(num.RightSnailfishNumber);
                if (did)
                {
                    num.RightSnailfishNumber = n;
                    return (num, did);
                }
            }

            if (num.LeftNumber != null && num.LeftNumber >= 10)
            {
                d = (double)num.LeftNumber / 2.0;
                sn.LeftNumber = (int?)Math.Floor(d);
                sn.RightNumber = (int?)Math.Ceiling(d);

                num.LeftNumber = null;
                num.LeftSnailfishNumber = sn;
                didSplit = true;
            }
            else if (num.RightNumber != null && num.RightNumber >= 10)
            {
                d = (double)num.RightNumber / 2.0;
                sn.LeftNumber = (int?)Math.Floor(d);
                sn.RightNumber = (int?)Math.Ceiling(d);

                num.RightNumber = null;
                num.RightSnailfishNumber = sn;
                didSplit = true;
            }

            return (num, didSplit);
        }

        public SnailfishNumber DoSnailfishAddition(SnailfishNumber[] values)
        {
            var temp = values[0];
            for (var i = 1; i < values.Length; i++)
            {
                var newFish = new SnailfishNumber()
                {
                    LeftSnailfishNumber = temp,
                    RightSnailfishNumber = values[i]
                };
                temp = ReduceSnailfish(newFish);
            }

            return ReduceSnailfish(temp);
        }

        public int GetMagnitude(SnailfishNumber num)
        {
            if (num.LeftSnailfishNumber != null)
            {
                num.LeftNumber = GetMagnitude(num.LeftSnailfishNumber);
                num.LeftSnailfishNumber = null;

            }

            if (num.RightSnailfishNumber != null)
            {
                num.RightNumber = GetMagnitude(num.RightSnailfishNumber);
                num.RightSnailfishNumber = null;
            }

            if (num.LeftNumber != null && num.RightNumber != null)
            {
                return (int)(3 * num.LeftNumber + 2 * num.RightNumber);

            }

            throw new Exception("No magnitudes!");
        }

        public int FindLargestMagnitude(SnailfishNumber[] values)
        {
            var largest = 0;
            for (var i = values.Length - 1; i > 0; i--)
            {
                for (var j = i - 1; j >= 0; j--)
                {
                    var s1 = DoSnailfishAddition(new SnailfishNumber[] { values[i].Clone(), values[j].Clone() });
                    var v1 = GetMagnitude(s1);

                    var s2 = DoSnailfishAddition(new SnailfishNumber[] { values[j].Clone(), values[i].Clone() });
                    var v2 = GetMagnitude(s2);

                    var larger = v1 > v2 ? v1 : v2;
                    if (larger > largest) largest = larger;
                }
            }

            return largest;
        }
    }
}
