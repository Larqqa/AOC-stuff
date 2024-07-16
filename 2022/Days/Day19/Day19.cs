using Library;

namespace _2022.Days.Day19
{
    public class Day19
    {
        public static void Run()
        {
            Console.WriteLine("---Day 19---");
            Operation();
            Console.WriteLine("------------");
        }

        private static void Operation()
        {
            var input = General.GetInput("./Days/Day19/input.txt");

            var result = input
                .Split("\r\n")
                .Aggregate(0, (result, line) =>
                {
                    var f = ParseInput(line);
                    var res = f.GetBestResult();
                    result += f._blueprintId * res.GetGeodes();
                    return result;
                });

            Console.WriteLine($"P1 quality score: {result}");

            var result2 = input
                .Split("\r\n")
                .Take(3)
                .Aggregate(1, (result2, line) =>
                {
                    var f = ParseInput(line, 32);
                    var res = f.GetBestResult();
                    result2 *= res.GetGeodes();
                    return result2;
                });

            Console.WriteLine($"P2 product: {result2}");

        }

        public static Factory ParseInput(string input, int time = 24)
        {
            var a = input.Split(": ");

            var bp = int.Parse(a[0].Replace("Blueprint ", string.Empty));
            var fac = new Factory(bp, time);

            var b = a[1].Split(". ");
            fac.AddRobotBlueprint(new Robot(RobotType.Ore, b[0]));
            fac.AddRobotBlueprint(new Robot(RobotType.Clay, b[1]));
            fac.AddRobotBlueprint(new Robot(RobotType.Obsidian, b[2]));
            fac.AddRobotBlueprint(new Robot(RobotType.Geode, b[3]));

            return fac;
        }
    }
}
