using _2022.Days.Day17;
using _2022.Days.Day17.Rocks;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2022.Days.Day17
{
    [TestClass]
    public class Day17Tests
    {
        [TestMethod]
        public void SpawnNextRock()
        {
            var d = new Day17();
            Assert.AreEqual(Rock.Shapes.Dash, d.SpawnNextRock(0 % 5, new()).Shape);
            Assert.AreEqual(Rock.Shapes.Cross, d.SpawnNextRock(1 % 5, new()).Shape);
            Assert.AreEqual(Rock.Shapes.Corner, d.SpawnNextRock(2 % 5, new()).Shape);
            Assert.AreEqual(Rock.Shapes.Line, d.SpawnNextRock(3 % 5, new()).Shape);
            Assert.AreEqual(Rock.Shapes.Square, d.SpawnNextRock(4 % 5, new()).Shape);
            Assert.AreEqual(Rock.Shapes.Dash, d.SpawnNextRock(5 % 5, new()).Shape);
            Assert.AreEqual(Rock.Shapes.Cross, d.SpawnNextRock(6 % 5, new()).Shape);
            Assert.AreEqual(Rock.Shapes.Corner, d.SpawnNextRock(7 % 5, new()).Shape);
            Assert.AreEqual(Rock.Shapes.Line, d.SpawnNextRock(8 % 5, new()).Shape);
            Assert.AreEqual(Rock.Shapes.Square, d.SpawnNextRock(9 % 5, new()).Shape);
        }

        [TestMethod]
        public void TestLoop()
        {
            var d = new Day17();
            var c = new Chamber();
            d.RunLoop(c, ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>", 2022, false);
            Assert.AreEqual(3068, c.Height - c.GetLocationOfTopMostRock().Y - 1);
        }


        [TestMethod]
        public void TestLoopP2()
        {
            var d = new Day17();
            var c = new Chamber();
            var (_, keys) = d.RunLoop(c, ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>", 5000, false);
            Assert.AreEqual(1514285714288, d.FindHeightAtRock(1000000000000, keys));
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
            var input = General.GetInput(@"./Days/Day17/input.txt");
            var d = new Day17();
            var c = new Chamber();
            var(_, keys) = d.RunLoop(c, input, 10000);

            var p1 = d.FindHeightAtRock(2022, keys);
            Console.WriteLine($"Height at 2022 rocks is: {p1}");
            
            var p2 = d.FindHeightAtRock(1000000000000, keys);
            Console.WriteLine($"Height 1000000000000 rocks is: {p2}");
        }

        public Stack<char> GetMoveStack(string input)
        {
            Stack<char> stack = new();
            foreach (char c in input.Reverse())
            {
                stack.Push(c);
            }
            return stack;
        }

        public Rock SpawnNextRock(int index, Point location)
        {
            switch (index)
            {
                case 0:
                    return new Dash(location);
                case 1:
                    return new Cross(location);
                case 2:
                    return new Corner(location);
                case 3:
                    return new Line(location);
                case 4:
                    return new Square(location);
            }

            throw new Exception("Rock shape index out of bounds!");
        }

        public (Chamber, List<(string, long)>)RunLoop(Chamber chamber, string input, int maxRocks, bool draw = false)
        {
            var keys = new List<(string, long)>();
            var queue = GetMoveStack(input);
            var rockInTransit = false;
            Rock currentRock = new Dash(new()); // Placeholder rock
            var rocks = 0;
            Rock.Direction nextMove = Rock.Direction.Empty;
            while(rocks <= maxRocks)
            {
                if (queue.Count == 0)
                {
                    queue = GetMoveStack(input);
                }

                if (!rockInTransit)
                {
                    currentRock = SpawnNextRock(rocks % 5, new(3, 0));
                    var topRock = chamber.GetLocationOfTopMostRock();
                    var topOffset = topRock.Y - currentRock.Height - 3;
                    if (topOffset < 0)
                    {
                        chamber.AddLayer(Math.Abs(topOffset));
                        topRock = chamber.GetLocationOfTopMostRock();
                    }
                    currentRock.Position.Y = topRock.Y - 4;
                    rockInTransit = true;
                    rocks++;

                    keys.Add(($"{(int)currentRock.Shape}{input.Length - queue.Count}{FindPeaks(chamber)}", chamber.Height - topRock.Y - 1));
                }

                if (nextMove == Rock.Direction.Empty)
                {
                    var move = queue.Pop();
                    nextMove = move == '<' ? Rock.Direction.Left : Rock.Direction.Right;
                }
                else
                {
                    currentRock.Move(nextMove, chamber);
                    nextMove = Rock.Direction.Empty;

                    if (!currentRock.Move(Rock.Direction.Down, chamber))
                    {
                        currentRock.DrawToChamber(chamber);
                        rockInTransit = false;
                    }
                }
            }

            if (draw)
                chamber.Draw();

            return (chamber, keys);
        }

        public string FindPeaks(Chamber c)
        {
            var peaks = new List<int>();
            for (int x = 1; x < c.Width - 1; x++)
            {
                var y = 0;
                var p = new Point(x, y);
                var t = c.GetTile(p);
                while (t != Chamber.Tile.Rock && t != Chamber.Tile.Floor)
                {
                    y++;
                    p.Y = y;
                    t = c.GetTile(p);
                }

                peaks.Add(p.ToIndex(c.Width));
            }

            return peaks.Aggregate("", (acc, p) => acc + p);
        }

        public (int, int, long) FindCycle(List<(string, long)> keys)
        {
            var match = false;
            var tortoise = 0;
            var hare = 1;
            while (!match)
            {
                while (keys[tortoise].Item1 != keys[hare].Item1)
                {
                    tortoise += 1;
                    hare += 2;
                }
                match = true;

                var offset = hare - tortoise;
                var t = tortoise + 1;
                var h = hare + 1;
                while (offset > 0)
                {
                    if (keys[t].Item1 != keys[h].Item1)
                    {
                        match = false;
                        tortoise += 1;
                        hare += 2;
                        break;
                    }
                    t += 1;
                    h += 1;
                    offset--;
                }
            }

            var lam = 1;
            hare = tortoise + 1;
            while (keys[tortoise].Item1 != keys[hare].Item1)
            {
                hare++;
                lam++;
            }

            // (starting index of cycle, length of cycle, height of cycle)
            return (tortoise, lam, keys[hare].Item2 - keys[tortoise].Item2);
        }

        public long FindHeightAtRock(long rockAmount, List<(string, long)> keys)
        {
            var (start, length, height) = FindCycle(keys);
            var leftOver = rockAmount - start;
            var overflow = leftOver % length;
            var nonCyclicalHeight = keys[start + (int)overflow].Item2;
            var cyclicalHeight = (leftOver - overflow) / length * height;
            return nonCyclicalHeight + cyclicalHeight;
        }
    }
}
