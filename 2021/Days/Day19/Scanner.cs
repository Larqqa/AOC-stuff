namespace _2021.Days.Day19
{
    public class Scanner
    {
        readonly (int,int,int)[] ROTATIONS = new (int,int,int)[] {
            (0, 0,   0),    (90, 0,   0),   (180, 0,   0),   (270, 0,   0),
            (0, 90,  0),    (90, 90,  0),   (180, 90,  0),   (270, 90,  0),
            (0, 180, 0),    (90, 180, 0),   (180, 180, 0),   (270, 180, 0),
            (0, 270, 0),    (90, 270, 0),   (180, 270, 0),   (270, 270, 0),
            (0, 0,   90),   (90, 0,   90),  (180, 0,   90),  (270, 0,   90),
            (0, 0,   270),  (90, 0,   270), (180, 0,   270), (270, 0,   270)
        };

        readonly Dictionary<int, int> COS = new Dictionary<int, int>()
        {
            { 0, 1 },
            { 90, 0 },
            { 180, -1 },
            { 270, 0 },
        };

        readonly Dictionary<int, int> SIN = new Dictionary<int, int>()
        {
            { 0, 0 },
            { 90, 1 },
            { 180, 0 },
            { 270, -1 },
        };

        public Point Location { get; set; }
        public (int,int,int) TransformDegrees { get; set; }
        public List<Beacon> Beacons { get; set; }

        public Scanner()
        {
            Location = new Point();
            TransformDegrees = ROTATIONS[0];
            Beacons = new List<Beacon>();
        }

        public int GetCos(int delta)
        {
            return COS.GetValueOrDefault(delta);
        }

        public int GetSin(int delta)
        {
            return SIN.GetValueOrDefault(delta);
        }

        public Point RotateAroundX(Point p, int delta)
        {
            int x = p.X;
            int y = p.Y;
            int z = p.Z;
            return new Point(x, y * GetCos(delta) - z * GetSin(delta), y * GetSin(delta) + z * GetCos(delta));
        }
        public Point RotateAroundY(Point p, int delta)
        {
            int x = p.X;
            int y = p.Y;
            int z = p.Z;
            return new Point(x * GetCos(delta) + z * GetSin(delta), y, z * GetCos(delta) - x * GetSin(delta));
        }
        public Point RotateAroundZ(Point p, int delta)
        {
            int x = p.X;
            int y = p.Y;
            int z = p.Z;
            return new Point(x * GetCos(delta) - y * GetSin(delta), x * GetSin(delta) + y * GetCos(delta), z);
        }

        public HashSet<Point> DoAllRotations(Point p)
        {
            var set = new HashSet<Point>();
            Point tempP;
            foreach (var (x,y,z) in ROTATIONS)
            {
                tempP = RotateAroundX(p, x);
                tempP = RotateAroundY(tempP, y);
                tempP = RotateAroundZ(tempP, z);
                set.Add(tempP);
            }
            return set;
        }

        public Scanner RotateScanner((int,int,int) rotationMatrix)
        {
            var (x, y, z) = rotationMatrix;
            Point tempP;
            foreach(var beacon in Beacons)
            {
                tempP = RotateAroundX(beacon.Location, x);
                tempP = RotateAroundY(tempP, y);
                tempP = RotateAroundZ(tempP, z);
                beacon.TransposedLocation = tempP;
            }

            return this;
        }

        public bool CompareToScanner(Scanner compare)
        {
            Dictionary<string, int> matches;
            foreach (var rotation in ROTATIONS)
            {
                matches = new Dictionary<string, int>();
                this.TransformDegrees = rotation;
                RotateScanner(this.TransformDegrees);

                foreach (var b in compare.Beacons)
                {
                    foreach (var beacon in Beacons)
                    {
                        var x = b.TransposedLocation.X - beacon.TransposedLocation.X;
                        var y = b.TransposedLocation.Y - beacon.TransposedLocation.Y;
                        var z = b.TransposedLocation.Z - beacon.TransposedLocation.Z;
                        var sLoc = $"{x},{y},{z}";

                        if (matches.ContainsKey(sLoc))
                        {
                            matches[sLoc] += 1;
                        }
                        else
                        {
                            matches.Add(sLoc, 1);
                        }
                    }

                    // 12 is target for correct matching rotation
                    var m = matches.Where(x => x.Value == 12);
                    if (m.Count() == 1)
                    {
                        var sLoc = m.FirstOrDefault().Key.Split(',');
                        var x = int.Parse(sLoc[0]) + compare.Location.X;
                        var y = int.Parse(sLoc[1]) + compare.Location.Y;
                        var z = int.Parse(sLoc[2]) + compare.Location.Z;
                        this.Location = new Point(x, y, z);
                        return true;
                    }
                }
            }
            return false;
        }

        public int GetManhattanDistance(Scanner compare)
        {
            var x = Math.Abs(this.Location.X - compare.Location.X);
            var y = Math.Abs(this.Location.Y - compare.Location.Y);
            var z = Math.Abs(this.Location.Z - compare.Location.Z);

            return x + y + z;
        }
    }

    public class Beacon
    {
        public Point Location { get; set; }
        public Point TransposedLocation { get; set; }
        public Beacon(Point location)
        {
            Location = location;
            TransposedLocation = location;
        }
    }

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
