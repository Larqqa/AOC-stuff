using Library;

namespace _2022.Days.Day17
{
    public abstract class Rock
    {
        public List<Point> vertices = new();
        public Point Position { get; set; }
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
            foreach (Point p in vertices)
            {
                toCheck.X = target.X + p.X;
                toCheck.Y = target.Y + p.Y;
                var pos = c.Map[toCheck.ToIndex(c.Width)];

                if (pos != '.') return true;
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
    }
}
