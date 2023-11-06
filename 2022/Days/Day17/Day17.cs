using _2022.Days.Day17;
using _2022.Days.Day17.Rocks;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.Metrics;

namespace _2022.Days.Day
{
    [TestClass]
    public class Day17Tests
    {
        [TestMethod]
        public void Test()
        {
            Assert.IsTrue(true);
        }
    }

    public class Day17
    {
        public static void Run()
        {
            Console.WriteLine("---Day 17---");
            var d = new Day17();
            d.Operation();
            Console.WriteLine("------------");
        }

        public void Operation()
        {
            var input = General.GetInput(@"./Days/Day/input.txt");
            Console.WriteLine(input);
        }

        public Stack<char> GetMoveStack(string input)
        {
            Stack<char> stack = new();
            foreach (char c in input)
            {
                stack.Push(c);
            }
            return stack;
        }

        public Rock SpawnNextRock(int index, Point location)
        {
            return new Square(location);

            //switch(index)
            //{
            //    case 0:
            //        return new Dash(new());
            //    case 1:
            //        return new Cross(new());
            //    case 2:
            //        return new Corner(new());
            //    case 3:
            //        return new Line(new());
            //    case 4:
            //        return new Square(new());
            //}

            //throw new Exception("Rock shape index out of bounds!");
        }

        public void RunLoop(string input, int maxRocks)
        {
            var c = new Chamber("".ToCharArray(), 0);
            var queue = GetMoveStack(input);
            var rockInTransit = false;
            Rock currentRock = SpawnNextRock(0, new());
            var rocks = 0;
            Rock.Direction nextMove = Rock.Direction.Empty;
            while(maxRocks < rocks)
            {
                if (queue.Count == 0)
                {
                    queue = GetMoveStack(input);
                }
                if (!rockInTransit)
                {
                    currentRock = SpawnNextRock(rocks % 5, new());
                    rocks++;
                }
                if (nextMove == Rock.Direction.Empty)
                {
                    var move = queue.Pop();
                    nextMove = move == '<' ? Rock.Direction.Left : Rock.Direction.Right;
                } else
                {
                    currentRock.Move(nextMove, c);
                    currentRock.Move(Rock.Direction.Down, c);
                }
            }
        }
    }
}
