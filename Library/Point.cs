namespace Library
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
        public Point(Point p)
        {
            X = p.X;
            Y = p.Y;
            Z = p.Z;
        }

        public Point(string loc)
        {
            var coords = loc.Split(',');
            X = int.Parse(coords[0] ?? "0");
            Y = int.Parse(coords[1] ?? "0");
            Z = int.Parse(coords[2] ?? "0");
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
            Point? p = obj as Point;
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

        public Point Add(Point p)
        {
            return new Point(X + p.X, Y + p.Y, Z + p.Z);
        }

        public bool OutOfBounds(int maxX, int maxY, int maxZ = 1)
        {
            return
                (X < 0 || Y < 0 || Z < 0) ||
                (X >= maxX || Y >= maxY || Z >= maxZ);
        }
    }
}
