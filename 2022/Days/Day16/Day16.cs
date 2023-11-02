﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library;
using System.Runtime.ConstrainedExecution;
using System.Linq;

namespace _2022.Days.Day16
{
    [TestClass]
    public class Day16Tests
    {
        private Day16 _d = new Day16();
        string testInput = $"Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II";

        [TestMethod]
        public void ParseInput()
        {
            var inputs = _d.ParseTunnels(testInput);
            Assert.AreEqual(10, inputs.Count);
        }

        [TestMethod]
        public void ShortestPathToTarget()
        {
            _d.tunnels = _d.ParseTunnels(testInput);
            {
                var list = _d.FindShortestPathToTarget("AA", "CC");
                Assert.AreEqual(3, list.Split(',').Length);
            }
            {
                var list = _d.FindShortestPathToTarget("AA", "HH");
                Assert.AreEqual(6, list.Split(',').Length);
            }

            // Optimial test path
            {
                var list = _d.FindShortestPathToTarget("AA", "DD");
                Assert.AreEqual(2, list.Split(',').Length);
            }
            {
                var list = _d.FindShortestPathToTarget("DD", "BB");
                Assert.AreEqual(3, list.Split(',').Length);
            }
            {
                var list = _d.FindShortestPathToTarget("BB", "JJ");
                Assert.AreEqual(4, list.Split(',').Length);
            }
            {
                var list = _d.FindShortestPathToTarget("JJ", "HH");
                Assert.AreEqual(8, list.Split(',').Length);
            }
            {
                var list = _d.FindShortestPathToTarget("HH", "EE");
                Assert.AreEqual(4, list.Split(',').Length);
            }
            {
                var list = _d.FindShortestPathToTarget("EE", "CC");
                Assert.AreEqual(3, list.Split(',').Length);
            }
        }

        [TestMethod]
        public void FindtargetList()
        {
            _d.tunnels = _d.ParseTunnels(testInput);
            {
                var list = _d.FindtargetListFor("AA");
                Assert.AreEqual("AA,DD", list["DD"]);
                Assert.AreEqual("AA,BB,CC", list["CC"]);
                Assert.AreEqual("AA,DD,EE,FF,GG,HH", list["HH"]);
            }
        }

        [TestMethod]
        public void MapAllPossibleTargets()
        {
            //_d.tunnels = _d.ParseTunnels(testInput);
            _d.tunnels = _d.ParseTunnels(General.GetInput(@"./Days/Day16/input.txt"));
            var t = _d.MapAllPossibleTargets();
            Assert.AreEqual(57, t.Count);
        }

        [TestMethod]
        public void VerifyPaths()
        {
            string testInput = $"Valve AA has flow rate=0; tunnels lead to valves DD, CC, BB\r\nValve BB has flow rate=10; tunnels lead to valves CC, AA\r\nValve CC has flow rate=1; tunnels lead to valves AA, BB\r\nValve DD has flow rate=5; tunnels lead to valves AA";
            _d.tunnels = _d.ParseTunnels(testInput);
            _d.targets = _d.MapAllPossibleTargets();
            var (score, _) = _d.FindPaths("AA", string.Empty, 10);
            Assert.AreEqual(107, score);
        }

        [TestMethod]
        public void FindPaths()
        {
            _d.tunnels = _d.ParseTunnels(testInput);
            _d.targets = _d.MapAllPossibleTargets();
            var (score, path) = _d.FindPaths("AA");
            Assert.AreEqual(1651, score);
            Console.WriteLine($"Best pressure: {score} path {string.Join("->", path)}");
        }

        [TestMethod]
        public void testP1()
        {
            var input = General.GetInput(@"./Days/Day16/input.txt");
            _d.tunnels = _d.ParseTunnels(input);
            _d.targets = _d.MapAllPossibleTargets();
            var (score, path) = _d.FindPaths("AA");
            Console.WriteLine($"Best pressure: {score} path {string.Join("->", path)}");
        }

        [TestMethod]
        public void testP2()
        {
            _d.tunnels = _d.ParseTunnels(testInput);
            _d.targets = _d.MapAllPossibleTargets();
            var (score, path) = _d.FindPaths("AA", string.Empty, 26);
            Assert.AreEqual(1707, score);
        }
    }

    public class Day16
    {
        public Dictionary<string, Tunnel> tunnels = new();
        public Dictionary<string, Dictionary<string, string>> targets = new();
        Dictionary<string, int> BestMoves = new();

        public static void Run()
        {
            Console.WriteLine("---Day 16---");
            var d = new Day16();
            d.Operation();
            Console.WriteLine("------------");
        }

        public void Operation()
        {
            var input = General.GetInput(@"./Days/Day16/input.txt");
            tunnels = ParseTunnels(input);
            targets = MapAllPossibleTargets();
            var (score, path) = FindPaths("AA");
            Console.WriteLine($"Best pressure: {score} path {string.Join("->", path)}");
        }

        public Dictionary<string, Tunnel> ParseTunnels(string input)
        {
            var tunnels = new Dictionary<string, Tunnel>();
            foreach(var row in input.Replace("\r", "").Split('\n'))
            {
                var test = row.Contains("tunnels") ? "tunnels lead to valves" : "tunnel leads to valve";
                var rowSplit = row.Split($"; {test} ");
                var left = rowSplit[0].Split(" has flow rate=");

                var key = left[0].Replace("Valve ", "");
                var flowRate = int.Parse(left[1]);
                var connections = rowSplit[1].Split(", ");
                
                tunnels.Add(key, new Tunnel(key, flowRate, connections));
            }

            return tunnels;
        }

        public string FindShortestPathToTarget(string current, string target)
        {
            var heap = new PriorityQueue<(string, string), int>();
            heap.Enqueue((current, current), 0);
            var seen = new HashSet<string>();
            while (heap.Count > 0)
            {
                var (c, path) = heap.Dequeue();
                if (c == target) return path;
                if (seen.Contains(c)) continue;
                var cur = tunnels[c];
                seen.Add(c);

                foreach (var t in cur.Connections)
                {
                    heap.Enqueue((t, $"{path},{t}"), path.Length);
                }
            }

            throw new Exception($"No path found from {current} to {target}");
        }

        public Dictionary<string, string> FindtargetListFor(string current)
        {
            var paths = new Dictionary<string, string>();
            foreach (var location in tunnels.Keys)
            {
                if (location == current) continue;
                if (tunnels[location].FlowRate == 0) continue;
                paths.Add(location, FindShortestPathToTarget(current, location));
            }
            return paths;
        }

        public Dictionary<string, Dictionary<string, string>> MapAllPossibleTargets()
        {
            var targets = new Dictionary<string, Dictionary<string, string>>();
            var keys = tunnels.Keys.ToArray();
            foreach(var current in keys)
            {
                targets.Add(current, FindtargetListFor(current));
            }
            return targets;
        }

        public (int, List<string>) FindPaths(string current, string open = "", int timeLeft = 30)
        {
            var bestPath = new List<string>() { current };
            var bestPressure = 0;

            var targetNodes = targets[current];
            foreach (var (key, path) in targetNodes)
            {
                if (open.Contains(key)) continue;
                var length = path.Split(',').Length - 1;
                var t = timeLeft - length - 1;
                if (t <= 0) continue;

                var node = tunnels[key];
                var pressure = t * node.FlowRate;

                var (nextPressure, nextPath) = FindPaths(key, $"{open},{key}", t);
                var newPressure = pressure + nextPressure;

                if (newPressure > bestPressure)
                {
                    bestPressure = newPressure;
                    bestPath = nextPath.Prepend(current).ToList();
                }
            }

            return (bestPressure, bestPath);
        }
    }

    public class State
    {
        public int Time { get; set; }
        public string Position { get; set; }
        public string Target { get; set; }
        public int Pressure { get; set; }
        public List<string> MoveStack { get; set; }
        public HashSet<string> OpenValves { get; set; }

        public State(int time, string position, string target, int pressure, List<string> moveStack, HashSet<string> openValves)
        {
            Time = time;
            Position = position;
            Target = target;
            Pressure = pressure;
            MoveStack = moveStack;
            OpenValves = openValves;
        }
    }

    public class IntMaxCompare: IComparer<int>
    {
        public int Compare(int x, int y) => y.CompareTo(x);
    }
}
