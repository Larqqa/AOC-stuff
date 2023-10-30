namespace _2021.Days.Day23
{
    public class Amphipod
    {
        public enum Types
        {
            Amber = 1,
            Bronze = 10,
            Copper = 100,
            Desert = 1000
        };
        public Types Type { get; set; }
        public Point Location { get; set; }
        public Point TargetDoor { get; set; }

        public Amphipod(Types type, Point loc, Point target)
        {
            Type = type;
            Location = loc;
            TargetDoor = target;
        }

        public int GetMovementCost()
        {
            return (int)Type;
        }
    }


    public class Point : IEquatable<Point>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public override bool Equals(Object? obj)
        {
            var p = obj as Point;
            if (p == null) return false;
            return Equals(p);
        }

        public bool Equals(Point? p)
        {
            if (p == null) return false;
            return this.X == p.X && this.Y == p.Y;
        }

        public override int GetHashCode()
        {
            return X + Y;
        }

        public static Point ConvertIndexToPoint(int index, int width)
        {
            return new Point(index % width, index / width);
        }

        public int ConvertToIndex(int width)
        {
            return Y * width + X;
        }
    }
}
