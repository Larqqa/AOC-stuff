using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace _2022.Days.Day22
{
    [TestClass]
    public class HumanTests
    {
        private string input =
            "        ....\r\n        ....\r\n        ....\r\n        ....\r\n............\r\n............\r\n............\r\n............\r\n        ........\r\n        ........\r\n        ........\r\n        ........\r\n\r\n";

        private string inputWithWalls =
            "        ....\r\n        ....\r\n        ....\r\n        ....\r\n############\r\n............\r\n............\r\n............\r\n        .......#\r\n        .......#\r\n        .......#\r\n        .......#\r\n\r\n";

        [TestMethod]
        public void TestMovingRight()
        {
            input += "4LR4LR4LR4R1L4LR4LR4LR4R2L4LR4LR4LR4";
            var human = Day22.ParseInputForCube(input, CubeTests.TransformTestMap);
            while (human.Instructions.Count > 0)
            {
                human.DoNextInstruction();
                Console.WriteLine(human);
            }
        }

        [TestMethod]
        public void TestMovingLeft()
        {
            input += "1LL5LR4LR4LR4L1R4LR4LR4LR4L2R4LR4LR4LR4";
            var human = Day22.ParseInputForCube(input, CubeTests.TransformTestMap);
            while (human.Instructions.Count > 0)
            {
                human.DoNextInstruction();
                Console.WriteLine(human);
            }
        }

        [TestMethod]
        public void TestMovingUp()
        {
            input += "1LL1R4LR4LR4LR4R1L4LR4LR4LR4R2L4LR4LR4LR4";
            var human = Day22.ParseInputForCube(input, CubeTests.TransformTestMap);
            while (human.Instructions.Count > 0)
            {
                human.DoNextInstruction();
                Console.WriteLine(human);
            }
        }

        [TestMethod]
        public void TestMovingDown()
        {
            input += "1LL1L4LR4LR4LR4L1R4LR4LR4LR4L2R4LR4LR4LR4";
            var human = Day22.ParseInputForCube(input, CubeTests.TransformTestMap);
            while (human.Instructions.Count > 0)
            {
                human.DoNextInstruction();
                Console.WriteLine(human);
            }
        }

        [TestMethod]
        public void TestMovingUpLeftFace()
        {
            input += "4L4LR4LR4LR4R1L4LR4LR4LR4R2L4LR4LR4LR4";
            var human = Day22.ParseInputForCube(input, CubeTests.TransformTestMap);
            while (human.Instructions.Count > 0)
            {
                human.DoNextInstruction();
                Console.WriteLine(human);
            }
        }

        [TestMethod]
        public void TestMovingDownLeftFace()
        {
            input += "4R4LR4LR4LR4L1R4LR4LR4LR4L2R4LR4LR4LR4";
            var human = Day22.ParseInputForCube(input, CubeTests.TransformTestMap);
            while (human.Instructions.Count > 0)
            {
                human.DoNextInstruction();
                Console.WriteLine(human);
            }
        }

        [TestMethod]
        public void TestMovingAgainstWalls()
        {
            inputWithWalls += "5R5R5R5LL5L5L5L5";
            var human = Day22.ParseInputForCube(inputWithWalls, CubeTests.TransformTestMap);
            while (human.Instructions.Count > 0)
            {
                human.DoNextInstruction();
                Console.WriteLine(human);
            }
        }

        [TestMethod]
        public void TestMovingDownToWall()
        {
            inputWithWalls += "1LL1L5LL5LL5";
            var human = Day22.ParseInputForCube(inputWithWalls, CubeTests.TransformTestMap);
            while (human.Instructions.Count > 0)
            {
                human.DoNextInstruction();
                Console.WriteLine(human);
            }
        }
    }

    public class Human
    {
        public Point Location { get; set; }
        public Direction Direction { get; set; } = Direction.Right;
        public Queue<string> Instructions = new ();
        public Map Map;

        public Human(Point location, Map map)
        {
            Location = location;
            Map = map;
        }

        public void Turn(Direction d)
        {
            if (d != Direction.Right && d != Direction.Left) return;

            var newDirection = d switch
            {
                Direction.Right => (int)Direction + 1,
                Direction.Left => (int)Direction - 1,
                _ => throw new Exception("Not allowed to turn there!")
            };

            if (newDirection > (int)Direction.Up)
            {
                newDirection = (int)Direction.Right;
            }
            else if (newDirection < (int)Direction.Right)
            {
                newDirection = (int)Direction.Up;
            }

            Direction = (Direction)newDirection;
        }

        public virtual void Move(int steps)
        {
            var lastLocation = new Point(Location.X, Location.Y);
            while (steps > 0)
            {
                _ = Direction switch
                {
                    Direction.Right => Location.X++,
                    Direction.Down => Location.Y++,
                    Direction.Left => Location.X--,
                    Direction.Up => Location.Y--,
                    _ => throw new ArgumentOutOfRangeException()
                };

                if (Location.X < 0)
                {
                    Location.X = Map.Width - 1;
                }
                else if (Location.X >= Map.Width)
                {
                    Location.X = 0;
                }

                if (Location.Y < 0)
                {
                    Location.Y = Map.Height - 1;
                }
                else if (Location.Y >= Map.Height)
                {
                    Location.Y = 0;
                }

                var index = Map.GetIndex(Location);
                var nextLocation = Map.Tiles[index];

                if (nextLocation == Tile.Free)
                    lastLocation = new Point(Location.X, Location.Y);

                if (nextLocation == Tile.Wall)
                {
                    Location = lastLocation;
                    break;
                }

                if (nextLocation != Tile.Empty)
                    steps--;
            }
        }

        public void DoNextInstruction()
        {
            var instruction = Instructions.Dequeue();
            if (int.TryParse(instruction, out var steps))
            {
                Move(steps);
                return;
            }

            var turnDirection = instruction switch
            {
                "R" => Direction.Right,
                "L" => Direction.Left,
                _ => throw new Exception($"{instruction}: Not allowed instruction")
            };

            Turn(turnDirection);
        }

        public override string ToString()
        {
            var str = string.Empty;

            str += $"Location: {Location}\n";
            str += $"Direction: {Direction}\n";
            str += "Instructions: ";
            str = Instructions.Aggregate(str, (str, x) => str += $"{x}, ");
            str += "\n\n";

            var dirChar = Direction switch
            {
                Direction.Right => '>',
                Direction.Down => 'v',
                Direction.Left => '<',
                Direction.Up => '^',
                _ => throw new ArgumentOutOfRangeException()
            };

            str += new StringBuilder(Map.ToString())
            {
                [Map.GetIndex(Location) + Location.Y] = dirChar
            };

            return str;
        }
    }

    public class Human3d : Human
    {
        public Face Face { get; set; } = Face.Front;
        public Cube Cube;

        public Human3d(Point location, Cube cube) : base(location, cube.Front)
        {
            Location = location;
            Cube = cube;
        }

        public override void Move(int steps)
        {
            var lastLocation = new Point(Location.X, Location.Y);
            var lastFace = Face;
            var lastDirection = Direction;

            while (steps > 0)
            {
                _ = Direction switch
                {
                    Direction.Right => Location.X++,
                    Direction.Down => Location.Y++,
                    Direction.Left => Location.X--,
                    Direction.Up => Location.Y--,
                    _ => throw new ArgumentOutOfRangeException()
                };

                var newLocation = new Point(Location.X, Location.Y);
                var newFace = Face;
                var newMap = Map;
                var newDirection = Direction;

                // If we go over the edge, change map to next face
                if (Location.X < 0 || Location.Y < 0 || Location.X >= Map.Width || Location.Y >= Map.Height)
                {
                    newFace = Cube.GetNextFace(Direction, Face);
                    newMap = Cube.GetFace(newFace);
                }

                // If we go over an edge, wrap around to next face
                if (Location.X < 0)
                {
                    newLocation.X = Map.Width - 1;
                }

                if (Location.X >= Map.Width)
                {
                    newLocation.X = 0;
                }

                if (Location.Y < 0)
                {
                    newLocation.Y = Map.Height - 1;
                }

                if (Location.Y >= Map.Height)
                {
                    newLocation.Y = 0;
                }

                // Handle special cases where direction does not line up directly

                if (newFace != Face && Face == Face.Back)
                {
                    if (Direction == Direction.Left)
                    {
                        newLocation.X = 0;
                        newLocation.Y = Map.Width - 1 - Location.Y;
                        newDirection = Direction.Right;
                    }
                    else if (Direction == Direction.Right)
                    {
                        newLocation.X = Map.Width - 1;
                        newLocation.Y = Map.Width - 1 - Location.Y;
                        newDirection = Direction.Left;
                    }
                }

                if (newFace != Face && Face == Face.Left)
                {
                    if (Location.Y < 0 && Direction == Direction.Up)
                    {
                        newLocation.X = 0;
                        newLocation.Y = Location.X;
                        newDirection = Direction.Right;
                    }

                    if (Location.Y >= Map.Height && Direction == Direction.Down)
                    {
                        newLocation.X = 0;
                        newLocation.Y = Map.Width - 1 - Location.X;
                        newDirection = Direction.Right;
                    }

                    if (Direction == Direction.Left)
                    {
                        newLocation.X = 0;
                        newLocation.Y = Map.Width - 1 - Location.Y;
                        newDirection = Direction.Right;
                    }
                }

                if (newFace != Face && Face == Face.Right)
                {
                    if (Direction == Direction.Up)
                    {
                        newLocation.X = Map.Width - 1;
                        newLocation.Y = Map.Width - 1 - Location.X;
                        newDirection = Direction.Left;
                    }

                    if (Direction == Direction.Down)
                    {
                        newLocation.X = Map.Width - 1;
                        newLocation.Y = Location.X;
                        newDirection = Direction.Left;
                    }

                    if (Direction == Direction.Right)
                    {
                        newLocation.X = Map.Width - 1;
                        newLocation.Y = Map.Width - 1 - Location.Y;
                        newDirection = Direction.Left;
                    }
                }

                if (newFace != Face && Face == Face.Top)
                {
                    if (Direction == Direction.Right)
                    {
                        newLocation.X = Map.Width - 1 - Location.Y;
                        newLocation.Y = 0;
                        newDirection = Direction.Down;
                    }

                    if (Direction == Direction.Left)
                    {
                        newLocation.X = Location.Y;
                        newLocation.Y = 0;
                        newDirection = Direction.Down;
                    }
                }

                if (newFace != Face && Face == Face.Bottom)
                {
                    if (Location.X >= Map.Width && Direction == Direction.Right)
                    {
                        newLocation.X = Location.Y;
                        newLocation.Y = Map.Width - 1;
                        newDirection = Direction.Up;
                    }

                    if (Location.X < 0 && Direction == Direction.Left)
                    {
                        newLocation.X = Map.Width - 1 - Location.Y;
                        newLocation.Y = Map.Width - 1;
                        newDirection = Direction.Up;
                    }
                }

                // Move to new location
                Location = newLocation;
                Face = newFace;
                Map = newMap;
                Direction = newDirection;

                var index = Map.GetIndex(Location);
                var nextLocation = Map.Tiles[index];

                if (nextLocation == Tile.Free)
                {
                    lastLocation = new Point(Location.X, Location.Y);
                    lastFace = Face;
                    lastDirection = Direction;
                }
                else if (nextLocation == Tile.Wall)
                {
                    Location = lastLocation;
                    Direction = lastDirection;
                    Face = lastFace;
                    Map = Cube.GetFace(Face);
                    break; // Skip rest of moves, since we hit a wall
                }

                steps--;
            }
        }

        public override string ToString()
        {
            var str = string.Empty;

            str += $"Location: {Location}\n";
            str += $"Direction: {Direction}\n";
            str += $"Face: {Face}\n";
            str += "Instructions: ";
            str = Instructions.Aggregate(str, (str, x) => str += $"{x}, ");
            str += "\n\n";

            var dirChar = Direction switch
            {
                Direction.Right => '>',
                Direction.Down => 'v',
                Direction.Left => '<',
                Direction.Up => '^',
                _ => throw new ArgumentOutOfRangeException()
            };

            var padding = Map.Width + 1;
            var offset = Face switch
            {
                Face.Front => new Point(padding, padding * 2),
                Face.Back => new Point(padding, 0),
                Face.Left => new Point(0, padding * 2),
                Face.Right => new Point(padding * 2, padding * 2),
                Face.Top => new Point(padding, padding),
                Face.Bottom => new Point(padding, padding * 3),
                _ => throw new ArgumentOutOfRangeException()
            };

            var index = Location.Add(offset);
            str += new StringBuilder(Cube.ToString())
            {
                [index.ToIndex(padding * 3)] = dirChar
            };

            return str;
        }
    }

    public enum Direction
    {
        Right, Down, Left, Up
    }
}
