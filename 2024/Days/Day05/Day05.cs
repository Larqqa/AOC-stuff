using System.Security;
using _2024.Days.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2024.Days.Day05
{
    [TestClass]
    public class Day05Tests
    {
        [TestMethod]
        public void Test()
        {
            var d = new Day05()
            {
                Input = "47|53\r\n97|13\r\n97|61\r\n97|47\r\n75|29\r\n61|13\r\n75|53\r\n29|13\r\n97|29\r\n53|29\r\n61|53\r\n97|53\r\n61|29\r\n47|13\r\n75|47\r\n97|75\r\n47|61\r\n75|61\r\n47|29\r\n75|13\r\n53|13\r\n\r\n75,47,61,53,29\r\n97,61,53,29,13\r\n75,29,13\r\n75,97,47,61,53\r\n61,13,29\r\n97,13,75,29,47"
            };

            d.ParseInput();

            Assert.AreEqual("143", d.PartOne());
        }

        [TestMethod]
        public void Test2()
        {
            var d = new Day05()
            {
                Input = "47|53\r\n97|13\r\n97|61\r\n97|47\r\n75|29\r\n61|13\r\n75|53\r\n29|13\r\n97|29\r\n53|29\r\n61|53\r\n97|53\r\n61|29\r\n47|13\r\n75|47\r\n97|75\r\n47|61\r\n75|61\r\n47|29\r\n75|13\r\n53|13\r\n\r\n75,97,47,61,53\r\n61,13,29\r\n97,13,75,29,47"
            };

            d.ParseInput();

            Assert.AreEqual("123", d.PartTwo());
        }
    }

    public class Day05() : Day("05")
    {
        public Dictionary<int, List<int>> rules = [];
        public int[][] pages = [];

        public override void ParseInput()
        {
            var sets = Input.Split("\r\n\r\n");

            (int left, int right)[] rs = sets[0]
                .Split("\r\n")
                .Select(x =>
                {
                    var n = x.Split('|');
                    return (int.Parse(n[0]), int.Parse(n[1]));
                })
                .ToArray();

            foreach (var (left, right) in rs)
            {
                if (rules.TryGetValue(left, out var found))
                {
                    found.Add(right);
                }
                else
                {
                    rules.Add(left, []);
                    rules[left].Add(right);
                }
            }

            pages = sets[1]
                .Split("\r\n")
                .Select((x) => x
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray())
                .ToArray();
        }

        private (int[][], int[][]) GetCorrectRules()
        {
            var correctPages = new List<int[]>();
            var incorrectPages = new List<int[]>();

            foreach(var page in pages)
            {
                var matches = true;
                for (var i = 0; i < page.Length; i++)
                {
                    var p = page[i];
                    var rest = page.Skip(i + 1);

                    if (!rules.TryGetValue(p, out var rs))
                    {
                        if (rest.Any()) matches = false;
                        continue;
                    }

                    if (rest.All(rs.Contains)) continue;

                    matches = false;
                }

                if (matches) correctPages.Add(page);
                else incorrectPages.Add(page);
            }

            return (correctPages.ToArray(), incorrectPages.ToArray());
        }

        public override string PartOne()
        {
            var (correctPages, _) = GetCorrectRules();

            var total = correctPages.Sum(x => x[x.Length / 2]);
            return total.ToString();
        }

        private static List<int> Recurse(int key, Dictionary<int, List<int>> rulesDict)
        {
            // if key has no rules or is last rule, it is the last page
            if (!rulesDict.TryGetValue(key, out var rules) || rules.Count == 0) return [key];

            var nextRules = new List<int>();
            foreach(var rKey in rules)
            {
                rulesDict.TryGetValue(rKey, out var r);
                if (r != null) nextRules = [.. nextRules, .. r];
            }

            // if some rule for current page is not found in any other rules list, it is the next page
            var nextPage = rules.First(rules => !nextRules.Contains(rules));
            return [key, ..Recurse(nextPage, rulesDict)];
        }

        public override string PartTwo()
        {
            var (_, incorrectPages) = GetCorrectRules();

            return incorrectPages.Sum(page => {
                var subRules = rules
                    .Where(rule => page.Contains(rule.Key))
                    .Select(rule => {
                        var subRules = rule.Value.Where(ruleKey => page.Contains(ruleKey)).ToList();
                        return new KeyValuePair<int, List<int>>(rule.Key, subRules);
                    })
                    .ToDictionary();

                var firstKey = subRules
                    .Select(rule => rule.Key)
                    .First(key => !subRules.Values.Any(rules => rules.Contains(key)));

                var sortedUpdate = Recurse(firstKey, subRules);
                return sortedUpdate[sortedUpdate.Count / 2];
            }).ToString();
        }
    }
}
