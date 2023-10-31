using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static _2021.Days.Day23.Amphipod;
using static System.Net.Mime.MediaTypeNames;

namespace _2021.Days.Day23
{

    [TestClass]
    public class BurrowTests
    {
        [TestMethod]
        public void TestParsing()
        {
            var input = $"#############\r\n#...........#\r\n###B#C#B#D###\r\n  #A#D#C#A#  \r\n  #########  ";
            var b = new Burrow(input);
            b.PrintMap();

            Assert.AreEqual("32Bronze52Copper72Bronze92Desert33Amber53Desert73Copper93Amber", b.ToString());
        }

        [TestMethod]
        public void TestClone()
        {
            var input = $"#############\r\n#...........#\r\n###B#C#B#D###\r\n  #A#D#C#A#  \r\n  #########  ";
            var b = new Burrow(input);
            var c = b.Clone();
            Assert.AreEqual(b, c);

            c.Amphipods[0].MakeMove(c, Direction.Up);

            Assert.AreNotEqual(b, c);
        }

        [TestMethod]
        public void TestRoomCheck()
        {
            var input = $"#############\r\n#...........#\r\n###B#C#B#D###\r\n  #A#D#C#A#  \r\n  #########  ";
            var b = new Burrow(input);
            var (pods, offset) = b.GetAmphipodsInRoomByDoor(new Point(3, 1));
            Assert.AreEqual(2, offset);
            Assert.AreEqual(2, pods.Count);
            CollectionAssert.AreEqual(new List<Types>() { Types.Bronze, Types.Amber }, pods.Select(x => x.Type).ToList());
            CollectionAssert.AreNotEqual(new List<Types>() { Types.Copper, Types.Desert }, pods.Select(x => x.Type).ToList());
        }

        [TestMethod]
        public void TestGenerateAllMoves()
        {
            var input = $"#############\r\n#...........#\r\n###B#C#B#D###\r\n  #A#D#C#A#  \r\n  #########  ";
            var b = new Burrow(input);
            var moves = b.GenerateAllNextStates();
            Assert.AreEqual(28, moves.Count);
        }
        
        [TestMethod]
        public void TestGenerateMoves()
        {
            var input = $"#############\r\n#...........#\r\n###B#C#B#D###\r\n  #A#D#C#A#  \r\n  #########  ";
            var b = new Burrow(input);
            var moves = b.GenerateAllMovesForAmphipod(0);
            Assert.AreEqual(7, moves.Count);

            var moves2  = b.GenerateAllMovesForAmphipod(3);
            Assert.AreEqual(7, moves2.Count);

            b.Amphipods[3].MakeMove(b, Direction.Up);
            b.Amphipods[3].MakeMove(b, Direction.Right);

            var moves3 = b.GenerateAllMovesForAmphipod(0);
            Assert.AreEqual(5, moves3.Count);

            var moves4 = b.GenerateAllMovesForAmphipod(3);
            Assert.AreEqual(6, moves4.Count);

            b.Amphipods[7].MakeMove(b, Direction.Up);
            b.Amphipods[7].MakeMove(b, Direction.Up);
            b.Amphipods[7].MakeMove(b, Direction.Left);
            b.PrintMap();

            var moves5 = b.GenerateAllMovesForAmphipod(7);
            Assert.AreEqual(4, moves5.Count);

            var moves6 = b.GenerateAllMovesForAmphipod(3);
            Assert.AreEqual(3, moves6.Count);
        }

        [TestMethod]
        public void runP1()
        {
            var input = $"#############\r\n#...........#\r\n###D#D#C#B###\r\n  #B#A#A#C#  \r\n  #########  ";
            var b = new Burrow(input);
            b.PrintMap();
            Assert.AreEqual(16157, Burrow.SolveBurrow(b));
        }

        [TestMethod]
        public void runP2()
        {
            var input = $"#############\r\n#...........#\r\n###D#D#C#B###\r\n  #D#C#B#A#  \r\n  #D#B#A#C#  \r\n  #B#A#A#C#  \r\n  #########  ";
            //var input = $"#############\r\n#...........#\r\n###B#C#B#D###\r\n  #D#C#B#A#  \r\n  #D#B#A#C#  \r\n  #A#D#C#A#  \r\n  #########  ";
            var b = new Burrow(input);
            b.PrintMap();
            Assert.AreEqual(44169, Burrow.SolveBurrow(b));
        }
    }

    public class Burrow : IEquatable<Burrow>
    {
        public Tiles[] Map { get; set; }
        public List<Amphipod> Amphipods { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public enum Tiles
        {
            Wall,
            Room,
            Hallway,
            Door,
            Empty
        }

        public Dictionary<char, Tiles> TilesMap = new() {
            { '#', Tiles.Wall },
            { 'x', Tiles.Room },
            { '.', Tiles.Hallway },
            { '&', Tiles.Door },
            { ' ', Tiles.Empty },
        };

        [JsonConstructor]
        public Burrow(Tiles[] map, List<Amphipod> amphis, int width, int height)
        {
            Map = map;
            Amphipods = amphis;
            Width = width;
            Height = height;
        }

        public Burrow(string input)
		{
            var (map, width, height, amphis) = ParseInput(input);
			Map = map;
			Amphipods = amphis;
			Width = width;
			Height = height;
		}

        public (Tiles[], int, int, List<Amphipod>) ParseInput(string input)
        {
            var charMap = input.Replace("\n", "").Replace("\r", "").ToCharArray();
            var height = input.Count(c => c == '\n') + 1;
            var width = charMap.Length / height;
            var amphis = new List<Amphipod>();
            var map = new List<Tiles>();

            for (var i = 0; i < charMap.Length; i++)
            {
                var c = charMap[i];
                var isAmphipod = Amphipod.TypeMap.ContainsKey(c);

                if (!TilesMap.TryGetValue(c, out var tile) && !isAmphipod)
                {
                    throw new Exception($"No such tile {c} exists!");
                }

                if (isAmphipod)
                {
                    amphis.Add(new Amphipod(Point.FromIndex(i, width), c));
                    map.Add(Tiles.Room);
                } else
                {
                    map.Add(tile);
                }
            }

            // Doors are in fixed locations
            map[16] = Tiles.Door;
            map[18] = Tiles.Door;
            map[20] = Tiles.Door;
            map[22] = Tiles.Door;

            return (map.ToArray(), width, height, amphis);
        }

        public void PrintMap()
        {
            var m = GetMapState();
            for(var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    Console.Write(m[new Point(x, y).ToIndex(Width)]);
                }
                Console.Write('\n');
            }
        }

        public Dictionary<Burrow, int> GenerateAllNextStates()
        {
            var nexts = new Dictionary<Burrow, int>();
            Amphipod pod;
            Tiles tile;
            Burrow c;
            for (var i = 0; i < Amphipods.Count; i++)
            {
                pod = Amphipods[i];
                if (pod.IsAmphipodStateFinished(this)) continue;

                tile = Map[pod.Location.ToIndex(Width)];
                if (tile == Tiles.Hallway && !pod.TargetRoomIsInValidState(this)) continue;
                if (tile == Tiles.Hallway && pod.TargetRoomIsInValidState(this) && pod.PathToRoomIsClear(this))
                {
                    c = this.Clone();
                    var (next, cost) = c.Amphipods[i].MoveToRoom(c);
                    nexts[next] = cost;
                    continue;
                }

                foreach (var (key, val) in GenerateAllMovesForAmphipod(i))
                {
                    nexts[key] = val;
                }
            }
            return nexts;
        }

        public Dictionary<Burrow, int> GenerateAllMovesForAmphipod(int index)
        {
            var queue = new Queue<(Burrow, int)>();
            queue.Enqueue((this.Clone(), 0));
            var visited = new HashSet<Point>();
            var moves = new Dictionary<Burrow, int>();
            Amphipod p;
            Burrow c;
            int nextCost;
            while (queue.Count > 0)
            {
                var (current, cost) = queue.Dequeue();
                foreach (var move in Enum.GetValues<Direction>())
                {
                    c = current.Clone();
                    p = c.Amphipods[index];
                    if (!p.MakeMove(c, move)) continue;
                    if (visited.Contains(p.Location)) continue;

                    nextCost = cost + p.GetMovementValue();
                    queue.Enqueue((c, nextCost));

                    if (IsPositionADoor(p.Location)) continue;
                    moves[c] = nextCost;
                }

                visited.Add(current.Amphipods[index].Location);
            }
            return moves;
        }

        public bool IsPositionADoor(Point location)
        {
            return Map[location.ToIndex(Width)] == Tiles.Door;
        }

        public (List<Amphipod>, int) GetAmphipodsInRoomByDoor(Point door)
        {
            List<Amphipod> amphipodsInRoom = new();
            var offset = 1;
            var roomTile = new Point(door.X, door.Y + offset);
            while (Map[roomTile.ToIndex(Width)] == Tiles.Room)
            {
                foreach (var tarPod in Amphipods)
                {
                    if (tarPod.Location.Equals(roomTile))
                    {
                        amphipodsInRoom.Add(tarPod);
                    }
                }

                offset += 1;
                roomTile.Y = door.Y + offset;
            }

            // Offset is length of room
            return (amphipodsInRoom, offset - 1);
        }

        public int EstimateCostToResolvedBurrow()
        {
            var cost = 0;
            foreach (var pod in Amphipods)
            {
                if (!pod.IsAmphipodStateFinished(this))
                {
                    cost += pod.EstimatePathCostToTarget();
                }
            }
            return cost;
        }

        public static int SolveBurrow(Burrow b)
        {
            var heap = new PriorityQueue<Burrow, int>();
            heap.Enqueue(b, b.EstimateCostToResolvedBurrow());
            HashSet<string> open = new() { b.ToString() };
            Dictionary<string, int> costs = new() { { b.ToString(), 0 } };

            int foundCost;
            while (heap.Count > 0)
            {
                if (!heap.TryDequeue(out var current, out int cost))
                    throw new Exception("Queuing borked");

                if (cost - costs[current.ToString()] == 0) return costs[current.ToString()];
                open.Remove(current.ToString());

                foreach (var (next, nextCost) in current.GenerateAllNextStates())
                {
                    foundCost = costs[current.ToString()] + nextCost;
                    if (foundCost >= costs.GetValueOrDefault(next.ToString(), int.MaxValue)) continue;

                    costs[next.ToString()] = foundCost;
                    if (open.Contains(next.ToString())) continue;
                    open.Add(next.ToString());
                    heap.Enqueue(next, next.EstimateCostToResolvedBurrow() + foundCost);
                }
            }

            throw new Exception("No solution was found!");
        }

        public bool IsDone()
        {
            return Amphipods.All(x => x.IsAmphipodStateFinished(this));
        }

        public override bool Equals(object? obj)
        {
            var b = obj as Burrow;
            if (b == null) return false;
            return Equals(b);
        }

        public bool Equals(Burrow? b)
        {
            if (b == null) return false;
            return ToString() == b.ToString();
        }

        public string GetMapState()
        {
            var m = Map.Aggregate("", (acc, t) => acc + TilesMap.FirstOrDefault(x => x.Value == t).Key).ToCharArray();
            foreach(var a in Amphipods)
            {
                m[a.Location.ToIndex(Width)] = Amphipod.TypeMap.FirstOrDefault(x => x.Value == a.Type).Key;
            }

            return new string(m);
        }

        public override string ToString()
        {
            return Amphipods.Aggregate("", (acc, a) => acc + a.Location.X + a.Location.Y + a.Type);
        }

        public override int GetHashCode()
        {
            return Amphipods.Aggregate(0, (acc, x) => acc + x.Location.X + x.Location.Y + (int)x.Type);
        }

        public Burrow Clone()
        {
            var strBr = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<Burrow>(strBr) ?? throw new NullReferenceException("Burrow was null!");
        }
    }
}
