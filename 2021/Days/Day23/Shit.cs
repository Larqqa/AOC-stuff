namespace _2021.Days.Day23
{
    internal class Shit
    {
        public enum Tile
        {
            Wall,
            Room,
            Hallway,
            Amphipod,
            Empty,
        }

        public Dictionary<char, Tile> TilesMap = new() {
            { '#', Tile.Wall },
            { '.', Tile.Hallway },
            { ' ', Tile.Empty },
        };

        public enum Amphipod
        {
            Amber = 1,
            Bronze = 10,
            Copper = 100,
            Desert = 1000
        };

        public static readonly Dictionary<char, Amphipod> AmphipodMap = new() {
            { 'A', Amphipod.Amber  },
            { 'B', Amphipod.Bronze },
            { 'C', Amphipod.Copper },
            { 'D', Amphipod.Desert },
        };

        public static readonly Dictionary<Amphipod, int> TargetDoors = new() {
            { Amphipod.Amber,  16 },
            { Amphipod.Bronze, 18 },
            { Amphipod.Copper, 20 },
            { Amphipod.Desert, 22 },
        };

        public int Width { get; set; }
        public int Height { get; set; }
        public char[] Map { get; set; }

        public Shit(string map, int width, int height)
        {
            Map = map.ToCharArray();
            Width = width;
            Height = height;
        }

        public Shit(Shit s)
        {
            // Cloning shit
            Map = new string(s.Map).ToCharArray();
            Width = s.Width;
            Height = s.Height;
        }

        public char GetMapKey(Tile tile)
        {
            return TilesMap.FirstOrDefault(x => x.Value == tile).Key;
        }

        public char GetMapKey(Amphipod tile)
        {
            return AmphipodMap.FirstOrDefault(x => x.Value == tile).Key;
        }

        public Tile GetTileFromMap(char c)
        {
            if (AmphipodMap.ContainsKey(c)) return Tile.Amphipod;
            if (!TilesMap.TryGetValue(c, out var tile)) throw new Exception("No tiles like that");
            return tile;
        }

        public Amphipod GetPodFromMap(char c)
        {
            if (!AmphipodMap.TryGetValue(c, out var pod)) throw new Exception("No pods like that");
            return pod;
        }

        public int GetTargetDoor(Amphipod pod)
        {
            if (!TargetDoors.TryGetValue(pod, out var door)) throw new Exception("No doors like that");
            return door;
        }

        public bool IsDoor(int index)
        {
            return TargetDoors.Values.Contains(index);
        }

        public (int, int) GetCoords(int index)
        {
            return (index % Width, index / Width);
        }

        public int GetIndex(int x, int y)
        {
            return y * Width + x;
        }

        public bool RoomStateIsValid(int door)
        {
            var index = door;
            var state = string.Empty;
            var tile = TilesMap[Map[index]];
            while (tile != Tile.Wall)
            {
                if (string.IsNullOrEmpty(state) && tile == Tile.Hallway) continue;
                state += Map[index];
                index += Width; // Go to next row
                tile = TilesMap[Map[index]];
            }

            // Check all tiles are same and no hallway in middle of pods
            return state.All(x => state[0] == x);
        }

        public bool ShouldMove(int index)
        {
            var pod = GetPodFromMap(Map[index]);
            var (x, y) = GetCoords(index);
            var belowY = y + 1;
            while (GetTileFromMap(Map[GetIndex(x, belowY)]) != Tile.Wall)
            {
                // Not same pods below
                if (GetPodFromMap(Map[GetIndex(x, belowY)]) != pod) return false;
                belowY += 1;
            }

            var aboveY = y - 1;
            while (GetTileFromMap(Map[GetIndex(x, aboveY)]) != Tile.Wall)
            {
                // Not same pods above
                if (GetPodFromMap(Map[GetIndex(x, aboveY)]) != pod) return false;
                aboveY += 1;
            }

            return true;
        }

        public bool IsInHallway(int index)
        {
            var pod = GetPodFromMap(Map[index]);
            var door = GetTargetDoor(pod);

            // Y coods match with doors
            return index / Width == door / Width;
        }

        public bool CanMoveToHallway(int index)
        {
            if (IsInHallway(index)) return false;

            var pod = GetPodFromMap(Map[index]);
            var door = GetTargetDoor(pod);
            var (x, y) = GetCoords(index);

            var doorL = door - 1;
            var doorR = door + 1;

            // spaces adjacent to door blocked
            if (GetTileFromMap(Map[doorL]) != Tile.Hallway && GetTileFromMap(Map[doorR]) != Tile.Hallway) return false;

            var aboveY = y - 1;
            var above = GetIndex(x, aboveY);
            while (!IsDoor(above))
            {
                // Path out of room blocked
                if (GetTileFromMap(Map[above]) != Tile.Hallway) return false;
                aboveY -= 1;
                above = GetIndex(x, aboveY);
            }

            return true;
        }

        public HashSet<(int, int)> MoveToOpenHallwayTiles(int index)
        {
            var moves = new HashSet<(int, int)>();

            if (!CanMoveToHallway(index)) return moves;
            var pod = GetPodFromMap(Map[index]);
            var door = GetTargetDoor(pod);
            var (_, doorY) = GetCoords(door);
            var (x, y) = GetCoords(index);
            var offsetY = Math.Abs(doorY - y);
            y = doorY;

            var leftX = x - 1;
            while (GetTileFromMap(Map[GetIndex(leftX, y)]) == Tile.Hallway)
            {
                moves.Add((GetIndex(leftX, y), (offsetY + leftX) * (int)pod));
                leftX -= 1;
            }

            var rightX = x + 1;
            while (GetTileFromMap(Map[GetIndex(rightX, y)]) == Tile.Hallway)
            {
                moves.Add((GetIndex(rightX, y), (offsetY + rightX) * (int)pod));
                rightX += 1;
            }

            return moves;
        }

        public int MoveToRoom(int index)
        {
            var pod = GetPodFromMap(Map[index]);
            var door = GetTargetDoor(pod);

            // can't move into room, as room is not valid
            if (!RoomStateIsValid(door)) return 0;

            var (doorX, doorY) = GetCoords(door);
            var (x, y) = GetCoords(index);
            var xDirection = doorX < x ? -1 : 1;

            var offsetX = Math.Abs(doorX - x);
            var offsetY = Math.Abs(doorY - y);

            // can't move out of initial room
            if (offsetY != 0 && !CanMoveToHallway(index)) return 0;
            if (offsetY != 0 && CanMoveToHallway(index))
            {
                y = doorY;
            }

            while (x != doorX)
            {
                // horizontal path blocked
                if (GetTileFromMap(Map[GetIndex(x, y)]) != Tile.Hallway) return 0;
                x += xDirection;
            }

            // Move as far in to room as can
            var belowY = y + 1;
            var below = GetTileFromMap(Map[GetIndex(x, belowY)]);
            var roomMoves = 0;
            while (below != Tile.Hallway)
            {
                y += belowY;
                belowY = y + 1;
                below = GetTileFromMap(Map[GetIndex(x, belowY)]);
                roomMoves += 1;
            }

            // Move pod
            Map[GetIndex(x, y)] = Map[index];
            Map[index] = GetMapKey(Tile.Hallway);

            // Return cost of move
            return (offsetY + offsetX + roomMoves) * (int)pod;
        }

        public void GenerateMoves(int index)
        {
            var moves = new Dictionary<Shit, int>();

            if (!ShouldMove(index)) return;

            if (IsInHallway(index))
            {
                var c = new Shit(this);
                var cost = c.MoveToRoom(index);
                if (cost > 0) moves.Add(c, cost);
            }

            if (CanMoveToHallway(index))
            {
                foreach (var (newIndex, cost) in MoveToOpenHallwayTiles(index))
                {
                    if (cost == 0) continue;

                    var c = new Shit(this);
                    c.Map[newIndex] = c.Map[index];
                    c.Map[index] = GetMapKey(Tile.Hallway);
                    moves.Add(c, cost);
                }
            }
        }

        public int Solve(string input)
        {
            var burrow = new Shit(input, 13, 5);

            return 0;
        }
    }
}
