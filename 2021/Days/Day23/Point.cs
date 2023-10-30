namespace _2021.Days.Day23
{
    public class Point : IEquatable<Point>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Point(int x = 0, int y = 0, int z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Point ParsePointFromString(string loc)
        {
            var coords = loc.Split(',');
            return new Point(
                int.Parse(coords[0]),
                int.Parse(coords[1]),
                int.Parse(coords[2])
            );
        }

        public static Point FromIndex(int index, int width)
        {
            var x = index % width;
            var y = index / width;
            return new Point(x, y);
        }

        public int ToIndex(int width)
        {
            return Y * width + X;
        }

        public override string ToString()
        {
            return $"({X},{Y},{Z})";
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
            return this.X == p.X && this.Y == p.Y && this.Z == p.Z;
        }

        public override int GetHashCode()
        {
            return X + Y + Z;
        }
    }
}
