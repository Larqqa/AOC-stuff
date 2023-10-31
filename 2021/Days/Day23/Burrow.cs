using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2021.Days.Day23
{
    [TestClass]
    public class BurrowTests
    {
        [TestMethod]
        public void RoomStateIsValid()
        {
            {
                var input = $"#############\r\n#...........#\r\n###B#C#B#D###\r\n  #A#D#C#A#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                Assert.IsFalse(b.RoomStateIsValid(16));
                Assert.IsFalse(b.RoomStateIsValid(16, true));
            }
            {
                var input = $"#############\r\n#...........#\r\n###.#C#B#D###\r\n  #B#D#C#A#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                Assert.IsFalse(b.RoomStateIsValid(16));
                Assert.IsFalse(b.RoomStateIsValid(16, true));
            }
            {
                var input = $"#############\r\n#...........#\r\n###A#C#B#D###\r\n  #.#D#C#A#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                Assert.IsFalse(b.RoomStateIsValid(16));
                Assert.IsFalse(b.RoomStateIsValid(16, true));
            }
            {
                var input = $"#############\r\n#...........#\r\n###A#C#B#D###\r\n  #A#D#C#A#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                Assert.IsTrue(b.RoomStateIsValid(16));
                Assert.IsTrue(b.RoomStateIsValid(16, true));
            }
            {
                var input = $"#############\r\n#...........#\r\n###.#C#B#D###\r\n  #A#D#C#A#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                Assert.IsTrue(b.RoomStateIsValid(16));
                Assert.IsFalse(b.RoomStateIsValid(16, true));
            }
        }

        [TestMethod]
        public void ShouldMove()
        {
            {
                var input = $"#############\r\n#.........D.#\r\n###B#C#B#.###\r\n  #A#D#C#A#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                Assert.IsTrue(b.ShouldMove(23));

                Assert.IsTrue(b.ShouldMove(29));
                Assert.IsTrue(b.ShouldMove(31));
                Assert.IsTrue(b.ShouldMove(33));

                Assert.IsFalse(b.ShouldMove(42));
                Assert.IsTrue(b.ShouldMove(44));
                Assert.IsFalse(b.ShouldMove(46));
                Assert.IsTrue(b.ShouldMove(48));
            }
        }

        [TestMethod]
        public void IsInHallway()
        {
            var input = $"#############\r\n#.........D.#\r\n###B#C#B#.###\r\n  #A#D#C#A#  \r\n  #########  ";
            var b = new Burrow(input, 13, 5);
            Assert.IsTrue(b.IsInHallway(23));
            Assert.IsFalse(b.IsInHallway(29));
        }

        [TestMethod]
        public void CanMoveToHallway()
        {
            {
                var input = $"#############\r\n#.C.......D.#\r\n###B#B#.#.###\r\n  #A#D#C#A#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                Assert.IsFalse(b.CanMoveToHallway(15));
                Assert.IsFalse(b.CanMoveToHallway(23));

                Assert.IsTrue(b.CanMoveToHallway(29));
                Assert.IsTrue(b.CanMoveToHallway(31));

                Assert.IsFalse(b.CanMoveToHallway(42));
                Assert.IsFalse(b.CanMoveToHallway(44));
                Assert.IsFalse(b.CanMoveToHallway(46));
                Assert.IsTrue(b.CanMoveToHallway(48));

            }
        }

        [TestMethod]
        public void MoveToOpenHallwayTiles()
        {
            {
                var input = $"#############\r\n#...A.....C.#\r\n###B#.#D#.###\r\n  #A#B#C#D#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                {
                    var moves = b.MoveToOpenHallwayTiles(17);
                    Assert.AreEqual(0, moves.Count);
                }
                {
                    var moves = b.MoveToOpenHallwayTiles(29);
                    Assert.AreEqual(2, moves.Count);
                }
                {
                    var moves = b.MoveToOpenHallwayTiles(33);
                    Assert.AreEqual(2, moves.Count);
                }
            }

            {
                var input = $"#############\r\n#...........#\r\n###B#A#D#C###\r\n  #A#B#C#D#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                var moves = b.MoveToOpenHallwayTiles(29);
                Assert.AreEqual(7, moves.Count);
            }
        }

        [TestMethod]
        public void MoveToRoom()
        {
            {
                var input = $"#############\r\n#.........D.#\r\n###B#C#B#.###\r\n  #A#D#C#A#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                Assert.AreEqual(0, b.MoveToRoom(23));
            }
            {
                var input = $"#############\r\n#.........D.#\r\n###B#C#B#.###\r\n  #A#A#C#D#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                Assert.AreEqual(2000, b.MoveToRoom(23));
            }
            {
                var input = $"#############\r\n#.D.......D.#\r\n###B#C#B#.###\r\n  #A#A#C#.#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                Assert.AreEqual(3000, b.MoveToRoom(23));
                Assert.AreEqual(8000, b.MoveToRoom(15));
            }
            {
                var input = $"#############\r\n#.........B.#\r\n###B#C#D#.###\r\n  #A#A#C#D#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                Assert.AreEqual(4000, b.MoveToRoom(33));
            }
            {
                var input = $"#############\r\n#.........BC#\r\n###B#C#.#.###\r\n  #A#A#D#D#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                Assert.AreEqual(5000, b.MoveToRoom(46));
            }
        }

        [TestMethod]
        public void GenerateMoves()
        {
            {
                var input = $"#############\r\n#.........D.#\r\n###B#C#B#.###\r\n  #A#D#C#A#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                var moves = b.GenerateMoves(23);
                Assert.AreEqual(0, moves.Count);
            }
            {
                var input = $"#############\r\n#.........D.#\r\n###B#C#B#.###\r\n  #A#D#C#D#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                var moves = b.GenerateMoves(23);
                Assert.AreEqual(1, moves.Count);
            }
            {
                var input = $"#############\r\n#.........D.#\r\n###B#C#B#.###\r\n  #A#D#C#A#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                var moves = b.GenerateMoves(48);
                Assert.AreEqual(5, moves.Count);
            }
            {
                var input = $"#############\r\n#.B.......D.#\r\n###.#C#B#.###\r\n  #A#D#C#A#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                var moves = b.GenerateMoves(48);
                Assert.AreEqual(1, moves.Count);
            }

            {
                var input = $"#############\r\n#AA........A#\r\n###.#A#C#D###\r\n  #.#B#C#D#  \r\n  #.#B#C#D#  \r\n  #B#B#C#D#  \r\n  #########  ";
                var b = new Burrow(input, 13, 7);
                var moves = b.GenerateMoves(68);
                Assert.AreEqual(4, moves.Count);
            }
            {
                var input = $"#############\r\n#..........A#\r\n###.#A#C#D###\r\n  #.#B#C#D#  \r\n  #.#B#C#D#  \r\n  #.#B#C#D#  \r\n  #########  ";
                var b = new Burrow(input, 13, 7);
                var moves = b.GenerateMoves(24);
                Assert.AreEqual(1, moves.Count);
                var moves2 = b.GenerateMoves(31);
                Assert.AreEqual(1, moves2.Count);
            }
            {
                var input = $"#############\r\n#C.........A#\r\n###.#C#.#D###\r\n  #.#B#.#D#  \r\n  #.#B#.#D#  \r\n  #.#B#.#D#  \r\n  #########  ";
                var b = new Burrow(input, 13, 7);
                var moves = b.GenerateMoves(14);
                Assert.AreEqual(1, moves.Count);
                var moves2 = b.GenerateMoves(31);
                Assert.AreEqual(1, moves2.Count);
            }
        }

        [TestMethod]
        public void IsSolved()
        {
            {
                var input = $"#############\r\n#...........#\r\n###A#B#C#D###\r\n  #A#B#C#D#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                Assert.IsTrue(b.IsSolved());
            }
            {
                var input = $"#############\r\n#...........#\r\n###B#A#C#D###\r\n  #A#B#C#D#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                Assert.IsFalse(b.IsSolved());
            }
            {
                var input = $"#############\r\n#A..........#\r\n###.#B#C#D###\r\n  #A#B#C#D#  \r\n  #########  ";
                var b = new Burrow(input, 13, 5);
                Assert.IsFalse(b.IsSolved());
            }
        }

        [TestMethod]
        public void TestSolving()
        {
            var input = $"#############\r\n#...........#\r\n###B#C#B#D###\r\n  #A#D#C#A#  \r\n  #########  ";
            Assert.AreEqual(12521, Burrow.Solve(input));
        }

        [TestMethod]
        public void TestSolvingP2()
        {
            var input = $"#############\r\n#...........#\r\n###B#C#B#D###\r\n  #D#C#B#A#  \r\n  #D#B#A#C#  \r\n  #A#D#C#A#  \r\n  #########  ";
            Assert.AreEqual(44169, Burrow.Solve(input, 13, 7));
        }
    }

    public class Burrow
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

        public Burrow(string map, int width, int height)
        {
            Map = map.Replace("\r", "").Replace("\n", "").ToCharArray();
            Width = width;
            Height = height;
        }

        public Burrow(Burrow s)
        {
            // Do Cloning
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

        public Amphipod GetMapKey(int door)
        {
            return TargetDoors.FirstOrDefault(x => x.Value == door).Key;
        }

        public Tile GetTileFromMap(char c)
        {
            if (AmphipodMap.ContainsKey(c)) return Tile.Amphipod;
            if (!TilesMap.TryGetValue(c, out var tile)) throw new Exception($"No tiles like that {c}");
            return tile;
        }

        public Amphipod GetPodFromMap(char c)
        {
            if (!AmphipodMap.TryGetValue(c, out var pod)) throw new Exception($"No pods like that {c}");
            return pod;
        }

        public int GetTargetDoor(Amphipod pod)
        {
            if (!TargetDoors.TryGetValue(pod, out var door)) throw new Exception($"No doors like that {pod}");
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

        public bool RoomStateIsValid(int door, bool solve = false)
        {
            var index = door + Width;
            var state = string.Empty;
            var tile = GetTileFromMap(Map[index]);
            var count = 0;
            while (tile != Tile.Wall)
            {
                state += Map[index];
                count += 1;
                index += Width; // Go to next row
                tile = GetTileFromMap(Map[index]);
            }

            // Check all tiles in room are same and an amphipod
            if (solve) return AmphipodMap.ContainsKey(state[0]) && GetPodFromMap(state[0]) == GetMapKey(door) && state.All(x => state[0] == x);

            Amphipod? pod = null;
            foreach(var c in state)
            {
                if (pod == null && AmphipodMap.ContainsKey(c)) pod = GetPodFromMap(c);
                if (pod == null && GetTileFromMap(c) == Tile.Hallway) continue; // skip empties at start of room
                if (AmphipodMap.ContainsKey(c) && GetPodFromMap(c) != GetMapKey(door)) return false; // some pod is not in correct room
                if (pod != null && !AmphipodMap.ContainsKey(c) || GetPodFromMap(c) != pod) return false; // some pod was not same as others
            }

            return true;
        }

        public bool ShouldMove(int index)
        {
            var pod = GetPodFromMap(Map[index]);
            var (x, y) = GetCoords(index);

            var door = GetTargetDoor(pod);
            var (_, doorY) = GetCoords(door);

            var dIndex = GetIndex(x, doorY);

            // x position is not in room
            if (!TargetDoors.Values.Contains(dIndex)) return true;
            // not in own room
            else if (GetTargetDoor(pod) != dIndex) return true;

            var belowY = y + 1;

            // At end of own room
            if (GetTileFromMap(Map[GetIndex(x, belowY)]) == Tile.Wall) return false;

            while (GetTileFromMap(Map[GetIndex(x, belowY)]) != Tile.Wall)
            {
                // Not same pods below
                if (GetPodFromMap(Map[GetIndex(x, belowY)]) != pod) return true;
                belowY += 1;
            }

            var aboveY = y - 1;
            while (GetTileFromMap(Map[GetIndex(x, aboveY)]) != Tile.Wall)
            {
                // Not same pods above
                var c = Map[GetIndex(x, aboveY)];
                if (AmphipodMap.ContainsKey(c) && GetPodFromMap(c) != pod) return true;
                aboveY += 1;
            }

            return false;
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
            if (IsInHallway(index) || !ShouldMove(index)) return false;

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
                if (!TargetDoors.Values.Contains(GetIndex(leftX, y)))
                    moves.Add((GetIndex(leftX, y), (offsetY + Math.Abs(x - leftX)) * (int)pod));
                leftX -= 1;
            }

            var rightX = x + 1;
            while (GetTileFromMap(Map[GetIndex(rightX, y)]) == Tile.Hallway)
            {
                if (!TargetDoors.Values.Contains(GetIndex(rightX, y)))
                    moves.Add((GetIndex(rightX, y), (offsetY + Math.Abs(x - rightX)) * (int)pod));
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
                x += xDirection;
                // horizontal path blocked
                if (GetTileFromMap(Map[GetIndex(x, y)]) != Tile.Hallway) return 0;
            }

            // Move as far in to room as can
            var belowY = y + 1;
            var below = GetTileFromMap(Map[GetIndex(x, belowY)]);
            var roomMoves = 0;
            while (below == Tile.Hallway)
            {
                y = belowY;
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

        public Dictionary<Burrow, int> GenerateMoves(int index)
        {
            var moves = new Dictionary<Burrow, int>();

            if (!ShouldMove(index)) return moves;

            if (IsInHallway(index))
            {
                var c = new Burrow(this);
                var cost = c.MoveToRoom(index);
                if (cost > 0) moves.Add(c, cost);
            }

            if (CanMoveToHallway(index))
            {
                // Check if can move directly to target room
                var c = new Burrow(this);
                var roomMove = c.MoveToRoom(index);
                if (roomMove > 0)
                {
                    moves.Add(c, roomMove);
                    return moves;
                }

                // Move to hallway spaces
                foreach (var (newIndex, cost) in MoveToOpenHallwayTiles(index))
                {
                    if (cost == 0) continue;

                    c = new Burrow(this);
                    c.Map[newIndex] = c.Map[index];
                    c.Map[index] = GetMapKey(Tile.Hallway);
                    moves.Add(c, cost);
                }
            }

            return moves;
        }

        public bool IsSolved()
        {
            return TargetDoors.Values.All(x => RoomStateIsValid(x, true));
        }

        public static int Solve(string input, int width = 13, int height = 5)
        {
            var burrow = new Burrow(input, width, height);
            var queue = new PriorityQueue<Burrow, int>();
            queue.Enqueue(burrow, 0);
            var visited = new HashSet<string>();
            while(queue.Count > 0)
            {
                if (!queue.TryDequeue(out var current, out var cost)) throw new Exception("Dequing failed!");
                if (visited.Contains(new string(current.Map))) continue;
                if (current.IsSolved()) return cost;
                visited.Add(new string(current.Map));

                for (var i = 0; i < current.Map.Length; i++)
                {
                    var c = current.Map[i];
                    if (AmphipodMap.ContainsKey(c))
                    {
                        foreach(var (next, nextCost) in current.GenerateMoves(i))
                        {
                            queue.Enqueue(next, cost + nextCost);
                        }
                    }
                }
            }

            throw new Exception("No solution found!");
        }
    }
}
