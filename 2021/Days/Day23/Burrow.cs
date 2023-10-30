using Newtonsoft.Json;
using static _2021.Days.Day23.Amphipod;
using static System.Net.Mime.MediaTypeNames;

namespace _2021.Days.Day23
{
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

        public HashSet<(Burrow, int)> GenerateAllNextStates()
        {
            var nexts = new HashSet<(Burrow, int)>();
            for (var i = 0; i < Amphipods.Count; i++)
            {
                if (Amphipods[i].IsAmphipodHome(this)) continue;
                var moves = GenerateAllMovesForAmphipod(i);
                foreach (var move in moves)
                {
                    nexts.Add(move);
                }
            }
            return nexts;
        }

        public HashSet<(Burrow, int)> GenerateAllMovesForAmphipod(int index)
        {
            var queue = new Queue<(int, Burrow)>();
            queue.Enqueue((0, this.Clone()));
            var visited = new HashSet<Point>();
            var moves = new HashSet<(Burrow, int)>();
            while (queue.Count > 0)
            {
                var (cost, current) = queue.Dequeue();
                var pod = current.Amphipods[index];
                foreach (var move in Enum.GetValues<Direction>())
                {
                    if (visited.Contains(pod.Location)) continue;

                    var c = current.Clone();
                    var p = c.Amphipods[index];
                    if (!p.MakeMove(c, move)) continue;

                    var nextCost = cost + p.GetMovementValue();
                    queue.Enqueue((nextCost, c));

                    // Amphipods can't stop at doors
                    if (IsPositionADoor(p.Location)) continue;
                    moves.Add((c, nextCost));
                }
                visited.Add(pod.Location);
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
                if (!pod.IsAmphipodHome(this))
                {
                    cost += pod.EstimatePathCostToTarget();
                }
            }

            return cost;
        }

        //public static int Solve(Burrow b)
        //{
        //    var c = b.Clone();
        //    var queue = new PriorityQueue<Burrow, int>();
        //    queue.Enqueue(c, c.EstimateCostToResolvedBurrow());
        //    HashSet<Burrow> open = new() { c };
        //    var costs = new Dictionary<Burrow, int>() {{ c, 0 }};

        //    while(queue.Count > 0)
        //    {
        //        if (!queue.TryDequeue(out var current, out int currentCost))
        //            throw new Exception("Queuing borked");
        //        if (currentCost - costs[current] == 0) return costs[current];
        //        open.Remove(current);

        //        foreach (var (next, nextCost) in current.GenerateAllNextStates())
        //        {
        //            var foundCost = costs[current] + nextCost;
        //            if (foundCost >= costs.GetValueOrDefault(next, int.MaxValue)) continue;

        //            costs[next] = foundCost;
        //            if (open.Contains(next)) continue;
        //            open.Add(next);
        //            queue.Enqueue(next, next.EstimateCostToResolvedBurrow() + foundCost);
        //        }
        //    }

        //    throw new Exception("No solution was found!");
        //}

        public static int Solve(Burrow b)
        {
            var queue = new PriorityQueue<Burrow, int>();
            queue.Enqueue(b.Clone(), 0);
            HashSet<Burrow> open = new();

            while (queue.Count > 0)
            {
                if (!queue.TryDequeue(out var current, out int currentCost))
                    throw new Exception("Queuing borked");
                if (open.Contains(current)) continue;
                if (current.IsDone()) return currentCost;
                open.Add(current);

                foreach (var (next, nextCost) in current.GenerateAllNextStates())
                {
                    queue.Enqueue(next, currentCost + nextCost);
                }
            }

            throw new Exception("No solution was found!");
        }

        public bool IsDone()
        {
            return Amphipods.All(x => x.IsAmphipodHome(this));
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
            var m = Map.Aggregate("", (acc, t) => acc + TilesMap.FirstOrDefault(x => x.Value == t).Key);
            var a = Amphipods.Aggregate("", (acc, a) => acc + a.Location.ToString() + a.Type);
            return m + a;
        }

        public override int GetHashCode()
        {
            var m = Map.Aggregate(0, (acc, x) => acc + (int)x);
            var a = Amphipods.Aggregate(0, (acc, x) => acc + x.Location.X + x.Location.Y + (int)x.Type);
            return m + a;
        }

        public Burrow Clone()
        {
            var strBr = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<Burrow>(strBr) ?? throw new NullReferenceException("Burrow was null!");
        }
    }
}
