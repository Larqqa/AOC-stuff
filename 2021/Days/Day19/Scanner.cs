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

        public void CompareToScanner(Scanner compare)
        {
            List<Beacon> matches;
            foreach(var b in compare.Beacons)
            {
                matches = new List<Beacon>();
                var loc = b.Location;

                foreach(var rotation in ROTATIONS)
                {
                    this.TransformDegrees = rotation;
                    RotateScanner(this.TransformDegrees);

                    foreach (var beacon in Beacons)
                    {
                        if (beacon.TransposedLocation.Equals(loc))
                        {
                            matches.Add(beacon);
                        }
                    }

                    if (matches.Count == 12) break; // 12 is target
                }
            }
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

    public class Point
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
    }
}
