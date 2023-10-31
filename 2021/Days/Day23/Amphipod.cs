using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace _2021.Days.Day23
{
    [TestClass]
    public class AmphipodTests
    {
        [TestMethod]
        public void TestStateFinished()
        {
            var input = $"#############\r\n#...........#\r\n###A#B#D#C###\r\n  #A#B#C#D#  \r\n  #########  ";
            var b = new Burrow(input);
            b.PrintMap();
            Assert.IsTrue(b.Amphipods[0].IsAmphipodStateFinished(b));
            Assert.IsTrue(b.Amphipods[1].IsAmphipodStateFinished(b));
            Assert.IsFalse(b.Amphipods[2].IsAmphipodStateFinished(b));
            Assert.IsFalse(b.Amphipods[3].IsAmphipodStateFinished(b));
            Assert.IsTrue(b.Amphipods[4].IsAmphipodStateFinished(b));
            Assert.IsTrue(b.Amphipods[5].IsAmphipodStateFinished(b));
            Assert.IsTrue(b.Amphipods[6].IsAmphipodStateFinished(b));
            Assert.IsTrue(b.Amphipods[7].IsAmphipodStateFinished(b));

            var pod = b.Amphipods[0];
            pod.MakeMove(b, Amphipod.Direction.Up);
            pod.MakeMove(b, Amphipod.Direction.Right);
            Assert.IsFalse(pod.IsAmphipodStateFinished(b));
            Assert.IsTrue(b.Amphipods[1].IsAmphipodStateFinished(b));
        }

        [TestMethod]
        public void TestPathing()
        {
            var input = $"#############\r\n#...........#\r\n###A#B#D#C###\r\n  #A#B#C#D#  \r\n  #########  ";
            var b = new Burrow(input);
            var pod = b.Amphipods[0];
            pod.MakeMove(b, Amphipod.Direction.Up);
            pod.MakeMove(b, Amphipod.Direction.Right);
            pod.MakeMove(b, Amphipod.Direction.Right);
            var pod2 = b.Amphipods[4];
            pod2.MakeMove(b, Amphipod.Direction.Up);
            pod2.MakeMove(b, Amphipod.Direction.Up);
            pod2.MakeMove(b, Amphipod.Direction.Left);
            b.PrintMap();

            Assert.IsTrue(pod2.PathToRoomIsClear(b));
            var (_, cost) = pod2.MoveToRoom(b);
            Assert.AreEqual(3, cost);
        }
    }

    public class Amphipod
    {
        public enum Types
        {
            Amber  = 1,
            Bronze = 10,
            Copper = 100,
            Desert = 1000
        };

        public static readonly Dictionary<char, Types> TypeMap = new() {
            { 'A', Types.Amber  },
            { 'B', Types.Bronze },
            { 'C', Types.Copper },
            { 'D', Types.Desert },
        };

        public static readonly Dictionary<Types, Point> TargetDoors = new() {
            { Types.Amber,  new Point(3,1) },
            { Types.Bronze, new Point(5,1) },
            { Types.Copper, new Point(7,1) },
            { Types.Desert, new Point(9,1) },
        };

        public enum Direction { Up, Down, Left, Right };

        public Point Location { get; set; }
        public Types Type { get; set; }
        public Point TargetDoor { get; set; }

        [JsonConstructor]
        public Amphipod(Point loc, Point tar, Types t) {
            Location = loc;
            TargetDoor = tar;
            Type = t;
        }

        public Amphipod(Point loc, char type)
        {
            if(!TypeMap.TryGetValue(type, out var t))
                throw new Exception($"No type found for {type}");
            if (!TargetDoors.TryGetValue(t, out var door))
                throw new Exception($"No target door found for {t}");

            Location = loc;
            Type = t;
            TargetDoor = door;
        }
       
        public int GetMovementValue()
        {
            return (int)Type;
        }

        public bool IsAmphipodInTargetRoom()
        {
            return Location.X == TargetDoor.X && Location.Y > TargetDoor.Y;
        }

        public bool IsAmphipodAtVeryEndOfRoom(Burrow b)
        {
            if (!IsAmphipodInTargetRoom()) return false;

            var below = new Point(Location.X, Location.Y + 1);
            var belowTile = b.Map[below.ToIndex(b.Width)];
            return belowTile == Burrow.Tiles.Wall;
        }

        public bool IsAmphipodStateFinished(Burrow b)
        {
            if (!IsAmphipodInTargetRoom()) return false;
            if (IsAmphipodAtVeryEndOfRoom(b)) return true;

            var below = new Point(Location.X, Location.Y + 1);
            return b.Amphipods.Any(x => x.Location.Equals(below)) && TargetRoomIsInValidState(b);
        }

        public bool TargetRoomIsInValidState(Burrow b)
        {
            var (amphipodsInRoom, roomLength) = b.GetAmphipodsInRoomByDoor(TargetDoor);
            return amphipodsInRoom.Count == 0 || (amphipodsInRoom.All(pod => pod.Type == Type));
        }

        public (bool, Point) IsMoveValid(Burrow b, Direction dir)
        {
            var targetLocation = new Point();
            switch (dir)
            {
                case Direction.Up:
                    targetLocation.X = Location.X;
                    targetLocation.Y = Location.Y - 1;
                    break;
                case Direction.Down:
                    targetLocation.X = Location.X;
                    targetLocation.Y = Location.Y + 1;
                    break;
                case Direction.Left:
                    targetLocation.X = Location.X - 1;
                    targetLocation.Y = Location.Y;
                    break;
                case Direction.Right:
                    targetLocation.X = Location.X + 1;
                    targetLocation.Y = Location.Y;
                    break;
            }
            
            // If target is another Amphipod
            var state = b.GetMapState();
            var targetIndex = targetLocation.ToIndex(b.Width);
            if (TypeMap.ContainsKey(state[targetIndex]))
            {
                return (false, targetLocation);
            }

            // If target is not our own room
            if (dir == Direction.Down && TargetDoor.X != targetLocation.X)
            {
                return (false, targetLocation);
            }

            // If is our own room, but room is not in a valid state
            if (dir == Direction.Down && TargetDoor.X == targetLocation.X && !TargetRoomIsInValidState(b))
            {
                return (false, targetLocation);
            }

            // If tile is a valid target
            var tile = b.Map[targetIndex];
            if (tile == Burrow.Tiles.Hallway ||
                tile == Burrow.Tiles.Door ||
                tile == Burrow.Tiles.Room)
            {
                return (true, targetLocation);
            }

            // Target was not valid
            return (false, targetLocation);
        }

        public bool MakeMove(Burrow b, Direction d)
        {
            var (valid, target) = IsMoveValid(b, d);
            if (valid) Location = target;
            return valid;
        }

        public bool PathToRoomIsClear(Burrow b)
        {
            var loc = new Point();
   
            loc.X = Location.X;
            loc.Y = Location.Y;
            var offsetToHallway = TargetDoor.Y - Location.Y;
            if (offsetToHallway < 0)
            {
                while (TargetDoor.Y != loc.Y)
                {
                    if (b.Amphipods.Any(x => x.Location.Equals(loc))) return false;
                    loc.Y -= 1;
                }
            }

            var offsetToDoor = TargetDoor.X - Location.X;
            var direction = offsetToDoor < 0 ? -1: 1;
            loc.X = Location.X;
            loc.Y = TargetDoor.Y;
            while (TargetDoor.X != loc.X)
            {
                if (b.Amphipods.Any(x => !x.Location.Equals(Location) && x.Location.Equals(loc))) return false;
                loc.X += direction;
            }

            return true;
        }

        public (Burrow, int) MoveToRoom(Burrow b)
        {
            var toHallway = Math.Abs(TargetDoor.Y - Location.Y);
            var toDoor = Math.Abs(TargetDoor.X - Location.X);
            var steps = 0;
            var loc = new Point(TargetDoor.X, TargetDoor.Y);
            var below = new Point(TargetDoor.X, TargetDoor.Y + 1);
            while (b.Map[below.ToIndex(b.Width)] != Burrow.Tiles.Wall && !b.Amphipods.Any(x => x.Location.Equals(below)))
            {
                loc.Y += 1;
                below.Y += 1;
                steps += 1;
            }

            Location = loc;
            return (b, (toHallway + toDoor + steps) * GetMovementValue());
        }

        public int EstimatePathCostToTarget()
        {
            var dist = 0;
            dist += Math.Abs(TargetDoor.Y - Location.Y);
            dist += Math.Abs(TargetDoor.X - Location.X);
            dist += 1; // Avg movement in room

            return dist * GetMovementValue();
        }
    }
}

