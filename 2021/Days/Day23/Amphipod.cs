using Newtonsoft.Json;

namespace _2021.Days.Day23
{
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

        public int EstimatePathCostToTarget()

        {
            var dist = 0;
            dist += Math.Abs(TargetDoor.Y - Location.Y);
            dist += Math.Abs(TargetDoor.X - Location.X);
            dist += 2;

            return dist * GetMovementValue();
        }
       
        public int GetMovementValue()
        {
            return (int)Type;
        }

        public bool IsAmphipodHome(Burrow b)
        {
            if (Location.X != TargetDoor.X)
            {
                return false;
            }

            var above = new Point(Location.X, Location.Y - 1);
            var below = new Point(Location.X, Location.Y + 1);
            var aboveTile = b.Map[above.ToIndex(b.Width)];

            // If pod is at the end of the room
            var aboveIsRoom = aboveTile == Burrow.Tiles.Room;
            var aboveIsOtherPod = b.Amphipods.Any(x => above.Equals(x.Location));
            var belowIsWall = b.Map[below.ToIndex(b.Width)] == Burrow.Tiles.Wall;
            if ((aboveIsRoom || aboveIsOtherPod) && belowIsWall && TargetRoomIsInValidState(b))
            {
                return true;
            }

            // If pod is in the room
            var aboveIsRoomOrDoor = aboveTile == Burrow.Tiles.Door || aboveIsRoom;
            var belowIsOtherPod = b.Amphipods.Any(x => below.Equals(x.Location));
            if (aboveIsRoomOrDoor && belowIsOtherPod && TargetRoomIsInValidState(b))
            {
                return true;
            }

            return false;
        }

        public bool TargetRoomIsInValidState(Burrow b)
        {
            var (amphipodsInRoom, roomLength) = b.GetAmphipodsInRoomByDoor(TargetDoor);
            return amphipodsInRoom.Count == 0 || amphipodsInRoom.Count <= roomLength && amphipodsInRoom.All(tar => tar.Type == Type);
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
    }
}

