using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;

namespace _2021.Days.Day23
{
    [TestClass]
    public class Day23Tests
    {
        readonly Day23 _d = new();
        const string input = $"#############\r\n#...........#\r\n###B#C#B#D###\r\n  #A#D#C#A#  \r\n  #########  ";

        [TestMethod]
        public void TestParsing()
        {
            _d.ParseInput(input);
            _d.PrintMap(_d.burrow, _d.amphipods);

            var map = _d.burrow.Map;
            Assert.AreEqual('#', map[0]);
            Assert.AreEqual('#', map[12]);
            Assert.AreEqual('#', map[13]);
            Assert.AreEqual('#', map[25]);
            Assert.AreEqual('#', map[55]);
            Assert.AreEqual('#', map[62]);

            Assert.AreEqual('.', map[14]);
            Assert.AreEqual('.', map[24]);

            Assert.AreEqual('&', map[16]);
            Assert.AreEqual('&', map[22]);

            Assert.AreEqual('x', map[42]);
            Assert.AreEqual('x', map[48]);
        }

        [TestMethod]
        public void TestRoomCheck()
        {
            _d.ParseInput(input);
            {
                var res = _d.CheckIfAmphipodsTargetRoomIsValid(_d.amphipods[7]);
                Assert.IsFalse(res);
            }
            {
                _d.amphipods[0].Location = new Point(1, 1);
                var res = _d.CheckIfAmphipodsTargetRoomIsValid(_d.amphipods[7]);
                Assert.IsTrue(res);
            }
            {
                _d.amphipods[4].Location = new Point(2, 1);
                var res = _d.CheckIfAmphipodsTargetRoomIsValid(_d.amphipods[7]);
                Assert.IsTrue(res);
            }
        }

        [TestMethod]
        public void TestCompletionCheck()
        {
            {
                var d = new Day23();
                d.ParseInput(input);
                Assert.IsFalse(d.CheckIfBurrowResolved());
            }
            {
                var d = new Day23();
                d.ParseInput($"#############\r\n#...........#\r\n###A#C#B#D###\r\n  #A#B#C#D#  \r\n  #########  ");
                Assert.IsFalse(d.CheckIfBurrowResolved());
            }
            {
                var d = new Day23();
                d.ParseInput($"#############\r\n#...........#\r\n###A#B#C#D###\r\n  #A#B#C#D#  \r\n  #########  ");
                Assert.IsTrue(d.CheckIfBurrowResolved());
            }
        }

        [TestMethod]
        public void TestMovements()
        {
            _d.ParseInput(input);
            var pod = _d.amphipods[0];

            Assert.IsFalse(_d.MoveAmphipod(pod, Day23.Direction.Down));
            Assert.IsFalse(_d.MoveAmphipod(pod, Day23.Direction.Left));
            Assert.IsFalse(_d.MoveAmphipod(pod, Day23.Direction.Right));
            Assert.AreEqual(new Point(3, 2), pod.Location);

            Assert.IsTrue(_d.MoveAmphipod(pod, Day23.Direction.Up));
            Assert.AreEqual(new Point(3, 1), pod.Location);

            Assert.IsTrue(_d.MoveAmphipod(pod, Day23.Direction.Left));
            Assert.AreEqual(new Point(2, 1), pod.Location);

            Assert.IsTrue(_d.MoveAmphipod(pod, Day23.Direction.Right));
            Assert.AreEqual(new Point(3, 1), pod.Location);

            Assert.IsFalse(_d.MoveAmphipod(pod, Day23.Direction.Down));
            Assert.AreEqual(new Point(3, 1), pod.Location);

            var pod2 = _d.amphipods[1];
            _d.MoveAmphipod(pod2, Day23.Direction.Up);
            _d.MoveAmphipod(pod2, Day23.Direction.Right);
            _d.MoveAmphipod(pod2, Day23.Direction.Right);
            _d.MoveAmphipod(pod2, Day23.Direction.Right);
            Assert.AreEqual(new Point(8, 1), pod2.Location);

            Assert.IsTrue(_d.MoveAmphipod(pod, Day23.Direction.Right));
            Assert.IsTrue(_d.MoveAmphipod(pod, Day23.Direction.Right));
            Assert.AreEqual(new Point(5, 1), pod.Location);

            Assert.IsFalse(_d.MoveAmphipod(pod, Day23.Direction.Down));
            Assert.AreEqual(new Point(5, 1), pod.Location);

            Assert.IsTrue(_d.MoveAmphipod(pod, Day23.Direction.Left));
            Assert.IsTrue(_d.MoveAmphipod(pod, Day23.Direction.Left));

            var pod3 = _d.amphipods[5];
            _d.MoveAmphipod(pod3, Day23.Direction.Up);
            _d.MoveAmphipod(pod3, Day23.Direction.Up);
            _d.MoveAmphipod(pod3, Day23.Direction.Right);
            Assert.AreEqual(new Point(6, 1), pod3.Location);

            Assert.IsTrue(_d.MoveAmphipod(pod, Day23.Direction.Right));
            Assert.IsTrue(_d.MoveAmphipod(pod, Day23.Direction.Right));
            Assert.IsTrue(_d.MoveAmphipod(pod, Day23.Direction.Down));
            Assert.IsTrue(_d.MoveAmphipod(pod, Day23.Direction.Down));
            Assert.AreEqual(new Point(5, 3), pod.Location);

            _d.PrintMap(_d.burrow, _d.amphipods);
        }

        [TestMethod]
        public void TestDoorCheck()
        {
            _d.ParseInput(input);
            var pod = _d.amphipods[0];

            Assert.IsTrue(_d.MoveAmphipod(pod, Day23.Direction.Up));
            Assert.IsTrue(_d.IsAmphipodStoppedOnDoor(pod));
            Assert.IsTrue(_d.MoveAmphipod(pod, Day23.Direction.Right));
            Assert.IsFalse(_d.IsAmphipodStoppedOnDoor(pod));
            Assert.IsTrue(_d.MoveAmphipod(pod, Day23.Direction.Right));
            Assert.IsTrue(_d.IsAmphipodStoppedOnDoor(pod));
        }

        [TestMethod]
        public void TestRoomDistanceCalculation()
        {
            {
                var d = new Day23();
                d.ParseInput($"#############\r\n#...........#\r\n###x#x#x#x###\r\n  #x#x#x#A#  \r\n  #########  ");
                var pod = d.amphipods[0];
                var res = d.EstimatePathCostToTarget(pod);
                Assert.AreEqual(9, res);
            }
            {
                var d = new Day23();
                d.ParseInput($"#############\r\n#.A.........#\r\n###x#x#x#x###\r\n  #x#x#x#x#  \r\n  #########  ");
                var pod = d.amphipods[0];
                var res = d.EstimatePathCostToTarget(pod);
                Assert.AreEqual(2, res);
            }
            {
                var d = new Day23();
                d.ParseInput($"#############\r\n#A..........#\r\n###x#x#x#x###\r\n  #x#x#x#x#  \r\n  #########  ");
                var pod = d.amphipods[0];
                var res = d.EstimatePathCostToTarget(pod);
                Assert.AreEqual(3, res);
            }
            {
                var d = new Day23();
                d.ParseInput($"#############\r\n#B..........#\r\n###x#x#x#x###\r\n  #x#x#x#x#  \r\n  #########  ");
                var pod = d.amphipods[0];
                var res = d.EstimatePathCostToTarget(pod);
                Assert.AreEqual(50, res);
            }
        }

        [TestMethod]
        public void TestAmphipodIsSolved()
        {
            var d = new Day23();
            d.ParseInput($"#############\r\n#...........#\r\n###x#B#C#x###\r\n  #x#D#C#A#  \r\n  #########  ");
            var pod = d.amphipods[4];

            // A pod moves to correct position
            Assert.IsFalse(d.CheckIfAmphipodIsSolved(pod));
            d.MoveAmphipod(pod, Day23.Direction.Up);
            d.MoveAmphipod(pod, Day23.Direction.Up);
            Assert.IsFalse(d.CheckIfAmphipodIsSolved(pod));
            d.MoveAmphipod(pod, Day23.Direction.Left);
            d.MoveAmphipod(pod, Day23.Direction.Left);
            d.MoveAmphipod(pod, Day23.Direction.Left);
            d.MoveAmphipod(pod, Day23.Direction.Left);
            d.MoveAmphipod(pod, Day23.Direction.Left);
            d.MoveAmphipod(pod, Day23.Direction.Left);
            Assert.IsFalse(d.CheckIfAmphipodIsSolved(pod));
            d.MoveAmphipod(pod, Day23.Direction.Down);
            Assert.IsFalse(d.CheckIfAmphipodIsSolved(pod));
            d.MoveAmphipod(pod, Day23.Direction.Down);
            Assert.IsTrue(d.CheckIfAmphipodIsSolved(pod));

            // B and D pods are not solved
            Assert.IsFalse(d.CheckIfAmphipodIsSolved(d.amphipods[0]));
            Assert.IsFalse(d.CheckIfAmphipodIsSolved(d.amphipods[2]));

            // C pods both in correct room
            Assert.IsTrue(d.CheckIfAmphipodIsSolved(d.amphipods[1]));
            Assert.IsTrue(d.CheckIfAmphipodIsSolved(d.amphipods[3]));
        }

        [TestMethod]
        public void TestBurrowDistanceCalculation()
        {
            {
                var d = new Day23();
                d.ParseInput($"#############\r\n#...........#\r\n###x#x#x#x###\r\n  #x#x#x#A#  \r\n  #########  ");
                var pod = d.amphipods[0];
                var res = d.EstimateCostToResolvedBurrow();
                Assert.AreEqual(9, res);
            }
            {
                var d = new Day23();
                d.ParseInput($"#############\r\n#...........#\r\n###x#x#x#x###\r\n  #x#x#B#A#  \r\n  #########  ");
                var pod = d.amphipods[0];
                var res = d.EstimateCostToResolvedBurrow();
                Assert.AreEqual(59, res);
            }
            {
                var d = new Day23();
                d.ParseInput($"#############\r\n#...........#\r\n###x#x#x#x###\r\n  #A#x#B#A#  \r\n  #########  ");
                var pod = d.amphipods[0];
                var res = d.EstimateCostToResolvedBurrow();
                Assert.AreEqual(59, res);
            }
            {
                var d = new Day23();
                d.ParseInput($"#############\r\n#...........#\r\n###C#x#x#x###\r\n  #A#x#B#A#  \r\n  #########  ");
                var pod = d.amphipods[0];
                var res = d.EstimateCostToResolvedBurrow();
                Assert.AreEqual(659, res);
            }
        }

        [TestMethod]
        public void TestGeneratingNextMoves()
        {
            var d = new Day23();
            d.ParseInput($"#############\r\n#...........#\r\n###D#B#C#A###\r\n  #A#B#C#D#  \r\n  #########  ");
            Assert.AreEqual(14, d.GenerateNextBurrows(d).Count);
        }

        [TestMethod]
        public void TestAllPossibleMovements()
        {
            var d = new Day23();
            d.ParseInput($"#############\r\n#...........#\r\n###D#B#C#A###\r\n  #A#B#C#D#  \r\n  #########  ");
            {
                d.MoveAmphipod(d.amphipods[3], Day23.Direction.Up);
                d.MoveAmphipod(d.amphipods[3], Day23.Direction.Right);
                d.MoveAmphipod(d.amphipods[3], Day23.Direction.Right);
                d.PrintMap(d.burrow, d.amphipods);
                Assert.AreEqual(7, d.GetAllPossibleMovements(0, d).Count());
                Console.WriteLine();

                d.MoveAmphipod(d.amphipods[0], Day23.Direction.Up);
                d.MoveAmphipod(d.amphipods[0], Day23.Direction.Right);
                d.MoveAmphipod(d.amphipods[0], Day23.Direction.Right);
                d.MoveAmphipod(d.amphipods[0], Day23.Direction.Right);
                d.PrintMap(d.burrow, d.amphipods);
                Assert.AreEqual(2, d.GetAllPossibleMovements(3, d).Count());
                Console.WriteLine();

                d.MoveAmphipod(d.amphipods[0], Day23.Direction.Right);
                d.MoveAmphipod(d.amphipods[0], Day23.Direction.Right);
                d.PrintMap(d.burrow, d.amphipods);
                Assert.AreEqual(1, d.GetAllPossibleMovements(3, d).Count());
                Console.WriteLine();

                d.MoveAmphipod(d.amphipods[0], Day23.Direction.Right);
                d.MoveAmphipod(d.amphipods[0], Day23.Direction.Down);
                d.PrintMap(d.burrow, d.amphipods);
                Assert.AreEqual(7, d.GetAllPossibleMovements(3, d).Count());
                Console.WriteLine();

                d.MoveAmphipod(d.amphipods[1], Day23.Direction.Up);
                d.MoveAmphipod(d.amphipods[1], Day23.Direction.Left);
                d.PrintMap(d.burrow, d.amphipods);
                Assert.AreEqual(3, d.GetAllPossibleMovements(3, d).Count());
            }
        }

        [TestMethod]
        public void TestSolving()
        {
            //var d = new Day23();
            //d.ParseInput($"#############\r\n#...........#\r\n###B#A#C#D###\r\n  #A#B#C#D#  \r\n  #########  ");
            //d.PrintMap(d.burrow, d.amphipods);

            //var res = d.SolveBurrow();
            //Assert.AreEqual(46, res);

            //_d.ParseInput(input);
            _d.ParseInput($"#############\r\n#...........#\r\n###D#D#C#B###\r\n  #B#A#A#C#  \r\n  #########  ");
            _d.PrintMap(_d.burrow, _d.amphipods);
            Assert.AreEqual(12521, _d.SolveBurrow());
        }
    }

    public class Day23 : IEquatable<Day23>
    {
        const char Hallway = '.';
        const char Doorway = '&';
        const char Room = 'x';
        const char Wall = '#';
        const char Empty = 'o';
        private int[] Doors = {16, 18, 20, 22};
        public enum Direction { Up, Down, Left, Right };

        public List<Amphipod> amphipods = new();
        public Burrow burrow = new Burrow(Array.Empty<char>(), 0, 0);

        public static void Run()
        {
            Console.WriteLine("---Day 23---");
            var d = new Day23();
            // 16161 too high
            d.Operation();
            Console.WriteLine("------------");
        }

        private void Operation()
        {

        }

        public void ParseInput(string input)
        {
            var height = input.Count(x => x == '\n') + 1;
            var map = input
                .Replace("\r", "")
                .Replace("\n", "")
                .Replace(' ', Empty)
                .ToCharArray();
            var width = (map.Length / height);
            burrow = new Burrow(map, width, height);
            ParseAmphipodsFromBurrow();

            for (var i = 0; i < map.Length; i++)
            {
                if (map[i] != Hallway && map[i] != Wall && map[i] != Empty)
                {
                    map[i] = Room;
                }
            }

            foreach(var door in Doors)
            {
                map[door] = Doorway;
            }

            burrow.Map = map;
        }

        public void PrintMap(Burrow burrow, List<Amphipod> pods)
        {
            // Use clone of burrow since we write to it
            var clone = burrow.Clone();
            foreach (var pod in pods)
            {
                var i = pod.Location.Y * clone.Width + pod.Location.X;
                switch(pod.Type)
                {
                    case Amphipod.Types.Amber:
                        clone.Map[i] = 'A';
                        break;
                    case Amphipod.Types.Bronze:
                        clone.Map[i] = 'B';
                        break;
                    case Amphipod.Types.Copper:
                        clone.Map[i] = 'C';
                        break;
                    case Amphipod.Types.Desert:
                        clone.Map[i] = 'D';
                        break;
                }
            }
            
            PrintMap(clone);
        }

        public void PrintMap(Burrow burrow)
        {
            for (var y = 0; y < burrow.Height; y++)
            {
                for (var x = 0; x < burrow.Width; x++)
                {
                    var i = y * burrow.Width + x;
                    Console.Write(burrow.Map[i]);
                }
                Console.Write('\n');
            }
        }

        public void ParseAmphipodsFromBurrow()
        {
            var width = burrow.Width;
            for (var i = 0; i < burrow.Map.Length; i++)
            {
                int x = i % width;
                int y = i / width;
                switch (burrow.Map[i])
                {
                    case 'A':
                        amphipods.Add(new Amphipod(Amphipod.Types.Amber, new Point(x, y), new Point(Doors[0] % width, Doors[0] / width)));
                        break;
                    case 'B':
                        amphipods.Add(new Amphipod(Amphipod.Types.Bronze, new Point(x, y), new Point(Doors[1] % width, Doors[1] / width)));
                        break;
                    case 'C':
                        amphipods.Add(new Amphipod(Amphipod.Types.Copper, new Point(x, y), new Point(Doors[2] % width, Doors[2] / width)));
                        break;
                    case 'D':
                        amphipods.Add(new Amphipod(Amphipod.Types.Desert, new Point(x, y), new Point(Doors[3] % width, Doors[3] / width)));
                        break;
                    default: break;
                }
            }
        }

        public bool CheckIfAmphipodsTargetRoomIsValid(Amphipod pod)
        {
            var (amphipodsInRoom, roomLength) = GetAmphipodsInRoomByDoor(pod.TargetDoor);
            return amphipodsInRoom.Count == 0 || amphipodsInRoom.Count <= roomLength && amphipodsInRoom.All(tar => tar.Type == pod.Type);
        }
        
        public bool CheckIfAmphipodIsSolved(Amphipod pod)
        {
            if (pod.Location.X != pod.TargetDoor.X)
            {
               // Amphipod is not in target column
                return false;
            }

            var above = new Point(pod.Location.X, pod.Location.Y - 1);
            var below = new Point(pod.Location.X, pod.Location.Y + 1);
            if (burrow.Map[below.ConvertToIndex(burrow.Width)] == Wall)
            {
                // Amphipod is at the end of their target room
                return true;
            }

            // Check that we can't move more downward
            var belowIsPod = amphipods.Any(x => below.Equals(x.Location));

            var aboveChar = burrow.Map[above.ConvertToIndex(burrow.Width)];
            var aboveIsRoomOrDoor = aboveChar == Doorway || aboveChar == Room;

            if (aboveIsRoomOrDoor && belowIsPod && CheckIfAmphipodsTargetRoomIsValid(pod))
            {
                // Amphipod is inside target room, can't move any more downward and target room is in a valid state
                return true;
            }

            return false;
        }

        public bool CheckIfBurrowResolved()
        {
            foreach(var door in Doors)
            {
                var (amphipodsInRoom, roomLength) = GetAmphipodsInRoomByDoor(Point.ConvertIndexToPoint(door, burrow.Width));
                var targetType = amphipodsInRoom[0].Type;
                if (amphipodsInRoom.Count != roomLength || !amphipodsInRoom.All(tar => tar.Type == targetType))
                {
                    // Some room is not full or it contains a mix of amphipod types
                    return false;
                }
            }

            // All rooms are full and contain the same amphipod types
            return true;
        }

        public (List<Amphipod>, int) GetAmphipodsInRoomByDoor(Point door)
        {
            List<Amphipod> amphipodsInRoom = new();
            var offset = 1;
            var roomTile = new Point(door.X, door.Y + offset);
            while (burrow.Map[roomTile.ConvertToIndex(burrow.Width)] == Room)
            {
                foreach (var tarPod in amphipods)
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

        public bool MoveAmphipod(Amphipod pod, Direction dir, bool onlyCheckValidity = false)
        {
            var targetLocation = new Point();
            switch(dir)
            {
                case Direction.Up:
                    targetLocation.X = pod.Location.X;
                    targetLocation.Y = pod.Location.Y - 1;
                    break;
                case Direction.Down:
                    targetLocation.X = pod.Location.X;
                    targetLocation.Y = pod.Location.Y + 1;
                    break;
                case Direction.Left:
                    targetLocation.X = pod.Location.X - 1;
                    targetLocation.Y = pod.Location.Y;
                    break;
                case Direction.Right:
                    targetLocation.X = pod.Location.X + 1;
                    targetLocation.Y = pod.Location.Y;
                    break;
            }

            foreach(var tarPod in amphipods)
            {
                if (targetLocation.Equals(tarPod.Location))
                {
                    // Target is other amphipod
                    return false;
                }
            }

            if (dir == Direction.Down && pod.TargetDoor.X != targetLocation.X)
            {
                // Can't move into room that is not our target.
                return false;
            }

            if (dir == Direction.Down && pod.TargetDoor.X == targetLocation.X && !CheckIfAmphipodsTargetRoomIsValid(pod))
            {
                // Can't move into non valid target room
                return false;
            }

            var target = burrow.Map[targetLocation.ConvertToIndex(burrow.Width)];
            if (target == Hallway || target == Doorway || target == Room)
            {
                // Target is valid location, move to target
                if (!onlyCheckValidity) pod.Location = targetLocation;
                return true;
            }

            // Target was not valid
            return false;
        }

        public bool IsAmphipodStoppedOnDoor(Amphipod pod)
        {
            return burrow.Map[pod.Location.ConvertToIndex(burrow.Width)] == Doorway;
        }

        public int EstimatePathCostToTarget(Amphipod pod)
        {
            var dist = 0;

            // Distance to hallway
            dist += Math.Abs(pod.TargetDoor.Y - pod.Location.Y);

            // Distance to door
            dist += Math.Abs(pod.TargetDoor.X - pod.Location.X);

            // Average Movements to get in to a room
            //var (_, roomLength) = GetAmphipodsInRoomByDoor(pod.TargetDoor);
            //var avg = 0;
            //for (var i = 0; i < roomLength; i++)
            //{
            //    avg += roomLength - i;
            //}
            // Calculate more accurate distance, easy to flip to Ceil if that's better
            //dist += (int)Math.Floor((double)avg / (double)roomLength);
            dist += 2;

            return dist * pod.GetMovementCost();
        }

        public int EstimateCostToResolvedBurrow()
        {
            var cost = 0;
            foreach(var pod in amphipods)
            {
                if (!CheckIfAmphipodIsSolved(pod))
                {
                    cost += EstimatePathCostToTarget(pod);
                }
            }

            return cost;
        }

        public HashSet<(int, Day23)> GenerateNextBurrows(Day23 d)
        {
            var nexts = new HashSet<(int, Day23)>();

            for (var i = 0; i < d.amphipods.Count; i++)
            {
                if (d.CheckIfAmphipodIsSolved(d.amphipods[i])) continue;
                var moves = GetAllPossibleMovements(i, d);
                foreach (var (cost, move) in moves)
                {
                    nexts.Add((cost, move));
                }
            }

            return nexts;
        }

        public HashSet<(int, Day23)> GetAllPossibleMovements(int index, Day23 d)
        {
            var queue = new Queue<(int, Day23)>();
            queue.Enqueue((0, d.Clone()));
            var visited = new HashSet<Point>();
            var moves = new HashSet<(int, Day23)>();
            
            while (queue.Count > 0)
            {
                var (cost, current) = queue.Dequeue();
                var pod = current.amphipods[index];
                foreach (var move in Enum.GetValues<Direction>())
                {
                    if (visited.Contains(current.amphipods[index].Location)) continue;
                    if (!current.MoveAmphipod(current.amphipods[index], move, true)) continue;

                    var c = current.Clone();
                    var p = c.amphipods[index];
                    c.MoveAmphipod(p, move);

                    var nextCost = cost + p.GetMovementCost();
                    queue.Enqueue((nextCost, c));

                    // Amphipods can't stop at doors
                    if (IsAmphipodStoppedOnDoor(p)) continue;
                    moves.Add((nextCost, c));
                }
                visited.Add(pod.Location);
            }

            return moves;
        }

        public Day23 Clone()
        {
            var strBr = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<Day23>(strBr) ?? throw new NullReferenceException("Day was null!");
        }

        public int SolveBurrow()
        {
            var c = Clone();
            Queue<(int, Day23)> heap = new();
            heap.Enqueue((EstimateCostToResolvedBurrow(), c));
            HashSet<string> open = new() { c.GetUniq() };
            Dictionary<string, int> costs = new() { { c.GetUniq(), 0 } };

            while(heap.Count > 0)
            {
                var (cost, current) = heap.Dequeue();
                //if (current.CheckIfBurrowResolved()) return c;
                if (cost - costs[current.GetUniq()] == 0) return costs[current.GetUniq()];
                open.Remove(current.GetUniq());

                foreach (var (nextCost, next) in GenerateNextBurrows(current))
                {
                    var foundCost = costs[current.GetUniq()] + nextCost;
                    if (foundCost >= costs.GetValueOrDefault(next.GetUniq(), int.MaxValue)) continue;

                    costs[next.GetUniq()] = foundCost;
                    if (open.Contains(next.GetUniq())) continue;
                    open.Add(next.GetUniq());
                    heap.Enqueue((next.EstimateCostToResolvedBurrow() + foundCost, next));
                }

                heap = new Queue<(int, Day23)>(heap.OrderBy(x => x.Item1));
            }

            throw new Exception("No solution was found!");
        }

        public string GetUniq()
        {
            // Map state + amphipods state = magic
            return new String(burrow.Map) + amphipods.Aggregate("", (acc, x) => acc + x.Location.ToString());
        }

        public override bool Equals(object? obj)
        {
            var b = obj as Day23;
            if (b == null) return false;
            return Equals(b);
        }

        public bool Equals(Day23? p)
        {
            if (p == null) return false;
            return GetUniq() == p.GetUniq();
        }

        public override int GetHashCode()
        {
            return GetUniq().ToCharArray().Aggregate(0, (acc, x) => acc + x.GetHashCode());
        }
    }

    public class Burrow : IEquatable<Burrow>
    {
        public char[] Map { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Burrow(char[] map, int width, int height)
        {
            Map = map;
            Width = width;
            Height = height;
        }

        public Burrow Clone()
        {
            var strBr = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<Burrow>(strBr) ?? throw new NullReferenceException("Burrow was null!");
        }

        public override bool Equals(object? obj)
        {
            var b = obj as Burrow;
            if (b == null) return false;
            return Equals(b);
        }

        public bool Equals(Burrow? p)
        {
            if (p == null) return false;
            return new String(Map) == new string(p.Map);
        }

        public override int GetHashCode()
        {
            return Map.ToList().Aggregate(0, (acc, x) => acc + x.GetHashCode());
        }
    }
}
