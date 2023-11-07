using _2022.Days.Day17.Rocks;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2022.Days.Day17
{
    [TestClass]
    public class RockTests
    {
        [TestMethod]
        public void Collides()
        {
            var c = new Chamber();
            c.AddLayer(5);
            var r = new Square(new Point(2, 3));
            Assert.IsTrue(r.Collides(new Point(0, 1), c));
            Assert.IsTrue(r.Collides(new Point(7, 1), c));
            Assert.IsFalse(r.Collides(new Point(3, 3), c));
            Assert.IsFalse(r.Collides(new Point(6, 2), c));
        }

        [TestMethod]
        public void Move()
        {
            var c = new Chamber();
            c.AddLayer(5);
            var r = new Square(new Point(2, 3));
            Assert.IsTrue(r.Move(Rock.Direction.Left, c));
            Assert.IsFalse(r.Move(Rock.Direction.Left, c));
            Assert.AreEqual(new Point(1,3), r.Position);

            Assert.IsTrue(r.Move(Rock.Direction.Right, c));
            Assert.IsTrue(r.Move(Rock.Direction.Right, c));
            Assert.IsTrue(r.Move(Rock.Direction.Right, c));
            Assert.IsTrue(r.Move(Rock.Direction.Right, c));
            Assert.IsTrue(r.Move(Rock.Direction.Right, c));
            Assert.IsFalse(r.Move(Rock.Direction.Right, c));
            Assert.AreEqual(new Point(6, 3), r.Position);

            Assert.IsTrue(r.Move(Rock.Direction.Down, c));
            Assert.IsFalse(r.Move(Rock.Direction.Down, c));
            Assert.AreEqual(new Point(6, 4), r.Position);
        }

        [TestMethod]
        public void DrawToChamber()
        {
            var c = new Chamber();
            c.AddLayer(5);
            var r = new Square(new Point(2, 3));
            r.DrawToChamber(c);
            c.Draw();
        }
    }

    public abstract class Rock
    {
        public List<Point> Vertices = new();
        public Point Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Shapes Shape { get; set; }


        public enum Shapes
        {
            Dash, Cross, Corner, Line, Square
        }

        public enum Direction
        {
            Left, Right, Down, Empty
        }

        public Rock(Point position)
        {
            Position = position;
        }

        public bool Collides(Point target, Chamber c)
        {
            var toCheck = new Point();
            foreach (Point p in Vertices)
            {
                toCheck.X = target.X + p.X;
                toCheck.Y = target.Y + p.Y;
                var tile = c.GetTile(toCheck);
                if (tile != Chamber.Tile.Empty) return true;
            }

            return false;
        }

        public bool Move(Direction d, Chamber c)
        {
            Point target = new(Position.X, Position.Y);
            switch(d)
            {
                case Direction.Left:
                    target.X = Position.X - 1;
                    break;
                case Direction.Right:
                    target.X = Position.X + 1;
                    break;
                case Direction.Down:
                    target.Y = Position.Y + 1;
                    break;
            }
            if (Collides(target, c)) return false;

            Position = target;
            return true;
        }

        public void DrawToChamber(Chamber c)
        {
            var point = new Point();
            foreach (Point p in Vertices)
            {
                point.X = Position.X + p.X;
                point.Y = Position.Y + p.Y;
                c.Map[point.ToIndex(c.Width)] = c.GetTileChar(Chamber.Tile.Rock);
            }
        }
    }
}
