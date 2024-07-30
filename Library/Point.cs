namespace Library;

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

    public Point(int val)
    {
        X = val;
        Y = val;
        Z = val;
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
        X = int.Parse(coords[0]);
        Y = int.Parse(coords[1]);
        Z = int.Parse(coords[2]);
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

    public override bool Equals(object? obj)
    {
        return obj is Point p && Equals(p);
    }

    public bool Equals(Point? p)
    {
        if (p == null) return false;
        return X == p.X && Y == p.Y && Z == p.Z;
    }

    public override int GetHashCode()
    {
        return X + Y + Z;
    }

    public Point Add(Point p)
    {
        return new Point(X + p.X, Y + p.Y, Z + p.Z);
    }

    public Point Sub(Point p)
    {
        return new Point(X - p.X, Y - p.Y, Z - p.Z);
    }

    public Point Mul(Point p)
    {
        return new Point(X * p.X, Y * p.Y, Z * p.Z);
    }

    public Point Div(Point p)
    {
        return new Point(X / p.X, Y / p.Y, Z / p.Z);
    }

    public double DistanceTo(Point p)
    {
        return Math.Sqrt(Math.Pow(p.X - X, 2) + Math.Pow(p.Y - Y, 2) + Math.Pow(p.Z - Z, 2));
    }

    public int ManhattanDistanceTo(Point p)
    {
        var x = Math.Abs(X - p.X);
        var y = Math.Abs(Y - p.Y);
        var z = Math.Abs(Z - p.Z);

        return x + y + z;
    }

    public bool OutOfBounds(Point maxP, Point? minP = null)
    {
        minP ??= new Point();

        return
            (X < minP.X || Y < minP.Y || Z < minP.Z) ||
            (X > maxP.X || Y > maxP.Y || Z > maxP.Z);
    }

    public static readonly List<Point> AdjacentPoints =
    [
        new Point( 0, -1), // Up
        new Point( 1,  0), // Right
        new Point( 0,  1), // Down
        new Point(-1,  0), // Left
    ];

    public static readonly List<Point> DiagonalPoints =
    [
        new Point( 1, -1), // Top right
        new Point( 1,  1), // Bottom right
        new Point( -1, 1), // Bottom left
        new Point( -1, -1), // Top left
    ];

    public static readonly List<Point> NeighborPoints = AdjacentPoints.Concat(DiagonalPoints).ToList();
}
