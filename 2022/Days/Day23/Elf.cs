using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2022.Days.Day23
{
    [TestClass]
    public class ElfTests
    {

        [TestMethod]
        public void TestCheckingAdjacentPoints()
        {
            var dict = new Dictionary<Point, Elf>()
            {
                {new Point(3, 0), new Elf(new Point(3,0))},
                {new Point(0, 1), new Elf(new Point(0,1))},
            };

            var e = new Elf(new Point());

            Assert.IsTrue(e.CheckAdjacentPointsAreEmpty(Direction.East, dict));
            Assert.IsFalse(e.CheckAdjacentPointsAreEmpty(Direction.South, dict));
        }
    }

    public class Elf
    {
        public Point Location { get; set; }

        public Elf(Point location)
        {
            Location = location;
        }

        public Point GetAdjacent(ElfDirection d)
        {
            return d switch
            {
                ElfDirection.N => new Point(Location.X, Location.Y - 1),
                ElfDirection.NE => new Point(Location.X + 1, Location.Y - 1),
                ElfDirection.E => new Point(Location.X + 1, Location.Y),
                ElfDirection.SE => new Point(Location.X + 1, Location.Y + 1),
                ElfDirection.S => new Point(Location.X, Location.Y + 1),
                ElfDirection.SW => new Point(Location.X - 1, Location.Y + 1),
                ElfDirection.W => new Point(Location.X - 1, Location.Y),
                ElfDirection.NW => new Point(Location.X - 1, Location.Y - 1),
                _ => throw new ArgumentOutOfRangeException(nameof(d), d, null)
            };
        }

        public Point GetAdjacent(Direction d)
        {
            return d switch
            {
                Direction.North => GetAdjacent(ElfDirection.N),
                Direction.East => GetAdjacent(ElfDirection.E),
                Direction.South => GetAdjacent(ElfDirection.S),
                Direction.West => GetAdjacent(ElfDirection.W),
                _ => throw new ArgumentOutOfRangeException(nameof(d), d, null)
            };
        }

        public bool CheckAdjacentPointsAreEmpty(Direction d, Dictionary<Point, Elf> elves)
        {
            var points = d switch
            {
                Direction.North => new List<Point> { GetAdjacent(ElfDirection.NW), GetAdjacent(ElfDirection.N), GetAdjacent(ElfDirection.NE) },
                Direction.East => new List<Point> { GetAdjacent(ElfDirection.NE), GetAdjacent(ElfDirection.E), GetAdjacent(ElfDirection.SE) },
                Direction.South => new List<Point> { GetAdjacent(ElfDirection.SE), GetAdjacent(ElfDirection.S), GetAdjacent(ElfDirection.SW) },
                Direction.West => new List<Point> { GetAdjacent(ElfDirection.SW), GetAdjacent(ElfDirection.W), GetAdjacent(ElfDirection.NW) },
                _ => throw new ArgumentOutOfRangeException(nameof(d), d, null)
            };

            var res = points.All(point => !elves.ContainsKey(point));
            points.Clear();
            return res;
        }

        public void Move(Direction d)
        {
            Location = GetAdjacent(d);
        }

        public enum ElfDirection
        {
            N, NE, E, SE, S, SW, W, NW
        }
    }
}
