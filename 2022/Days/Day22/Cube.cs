using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2022.Days.Day22
{
    [TestClass]
    public class CubeTests
    {
        public static List<(Point, Point, int)> TransformTestMap = new()
        {
            (new Point(8,0), new Point(12,4), 0),
            (new Point(8,8), new Point(12,12), 0),

            (new Point(4,4), new Point(8,8), 1),
            (new Point(12,8), new Point(16,12), 2),

            (new Point(0,4), new Point(4,8), 2),
            (new Point(8,4), new Point(12,8), 0),
        };

        [TestMethod]
        public void TestParsing()
        {
            var c = Cube.ParseCube(Day22Tests.Input, TransformTestMap);
            Console.WriteLine(c);
        }
    }

    public class Cube
    {
        public Map Front;
        public Map Back;
        public Map Left;
        public Map Right;
        public Map Top;
        public Map Bottom;

        public Cube(
            Map front,
            Map back,
            Map left,
            Map right,
            Map top,
            Map bottom
        )
        {
            Front = front;
            Back = back;
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        /// <summary>
        /// Read and map cube faces from string input.
        /// All faces should be perfect squares.
        /// Give the face position matrix in this order: front,back, left,right, top,bottom
        /// </summary>
        public static Cube ParseCube(string input, List<(Point start, Point end, int rotations)> positions)
        {
            var inp = input.Split("\r\n\r\n");
            var rows = inp[0].Split("\r\n");

            var maps = new List<Map>();
            foreach (var (start, end, rotations) in positions)
            {
                var actualRows = rows[start.Y..end.Y];

                var tileChars = new List<char>();
                foreach (var row in actualRows)
                {
                    tileChars = tileChars
                        .Concat(row[start.X..end.X].ToCharArray())
                        .ToList();
                }

                var map = new Map
                {
                    Width = end.X - start.X,
                    Height = end.Y - start.Y,
                    Tiles = tileChars.Select(Map.GetTileValue).ToArray()
                };

                var direction = rotations > 0;
                for (var i = 0; i < Math.Abs(rotations); i++)
                {
                    map.RotateSquareMap(direction);
                }

                maps.Add(map);
            }

            // Mapped cube face order used here, so position order is important!
            return new Cube(maps[0], maps[1], maps[2], maps[3], maps[4], maps[5]);
        }

        public Map GetFace(Face f)
        {
            return f switch
            {
                Face.Front => Front,
                Face.Back => Back,
                Face.Left => Left,
                Face.Right => Right,
                Face.Top => Top,
                Face.Bottom => Bottom,
                _ => throw new ArgumentOutOfRangeException(nameof(f), f, null)
            };
        }

        public static Face GetNextFace(Direction d, Face currentFace)
        {
            return currentFace switch
            {
                Face.Front=> d switch
                {
                    Direction.Right => Face.Right,
                    Direction.Down => Face.Bottom,
                    Direction.Left => Face.Left,
                    Direction.Up => Face.Top,
                    _ => throw new Exception("Not a valid direction!")
                },
                Face.Back => d switch
                {
                    Direction.Right => Face.Right,
                    Direction.Down => Face.Top,
                    Direction.Left => Face.Left,
                    Direction.Up => Face.Bottom,
                    _ => throw new Exception("Not a valid direction!")
                },
                Face.Left => d switch
                {
                    Direction.Right => Face.Front,
                    Direction.Down => Face.Bottom,
                    Direction.Left => Face.Back,
                    Direction.Up => Face.Top,
                    _ => throw new Exception("Not a valid direction!")
                },
                Face.Right => d switch
                {
                    Direction.Right => Face.Back,
                    Direction.Down => Face.Bottom,
                    Direction.Left => Face.Front,
                    Direction.Up => Face.Top,
                    _ => throw new Exception("Not a valid direction!")
                },
                Face.Top => d switch
                {
                    Direction.Right => Face.Right,
                    Direction.Down => Face.Front,
                    Direction.Left => Face.Left,
                    Direction.Up => Face.Back,
                    _ => throw new Exception("Not a valid direction!")
                },
                Face.Bottom => d switch
                {
                    Direction.Right => Face.Right,
                    Direction.Down => Face.Back,
                    Direction.Left => Face.Left,
                    Direction.Up => Face.Front,
                    _ => throw new Exception("Not a valid direction!")
                },
                _ => throw new ArgumentOutOfRangeException(nameof(currentFace), currentFace, null)
            };
        }

        public override string ToString()
        {
            var padding = new string(' ', Front.Width);
            var spacing = $"{padding} {padding} {padding}\n";
            var str = string.Empty;

            str += string.Join("", Back.ToString().Split('\n').Select(x => $"{padding} {x} {padding}\n"));
            str += spacing;

            str += string.Join("", Top.ToString().Split('\n').Select(x => $"{padding} {x} {padding}\n"));
            str += spacing;

            var left = Left.ToString().Split('\n');
            var front = Front.ToString().Split('\n');
            var right = Right.ToString().Split('\n');

            for (var i = 0; i < front.Length; i++)
            {
                str += string.Join("", $"{left[i]} {front[i]} {right[i]}\n");
            }
            str += spacing;

            str += string.Join("", Bottom.ToString().Split('\n').Select(x => $"{padding} {x} {padding}\n"));

            return str;
        }
    }

    public enum Face
    {
        Front, Back, Left, Right, Top, Bottom
    }
}
