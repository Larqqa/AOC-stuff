using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2022.Days.Day17
{
    [TestClass]
    public class ChamberTests
    {
        [TestMethod]
        public void GetTileAndChar()
        {
            var c = new Chamber();
            c.Draw();
            Assert.AreEqual(Chamber.Tile.Wall, c.GetTile(new Point(0,0)));
            Assert.AreEqual(Chamber.Tile.Floor, c.GetTile(new Point(5, 0)));
            Assert.AreEqual(Chamber.Tile.Wall, c.GetTile(new Point(8, 0)));

            Assert.AreEqual('|', c.GetTileChar(Chamber.Tile.Wall));
            Assert.AreEqual('-', c.GetTileChar(Chamber.Tile.Floor));
        }

        [TestMethod]
        public void AddLayer()
        {
            var c = new Chamber();
            c.AddLayer(3);
            c.Draw();

            Assert.AreEqual(9, c.Width);
            Assert.AreEqual(4, c.Height);
            Assert.AreEqual(9*4, c.Map.Length);
        }
    }
    public class Chamber
    {
        public enum Tile
        {
            Rock,
            Floor,
            Wall,
            Empty
        }
        public Dictionary<char, Tile> TileMap = new()
        {
            {'@', Tile.Rock },
            {'-', Tile.Floor },
            {'|', Tile.Wall },
            {'.', Tile.Empty},
        };
        private const string _floor = "|-------|";
        private const string _row = "|.......|";
        public char[] Map;
        public int Width = 9;
        public int Height = 1;

        public Chamber(char[] map, int width, int height)
        {
            Map = map;
            Width = width;
            Height = height;
        }

        public Chamber()
        {
            Map = _floor.ToCharArray();
        }

        public Tile GetTile(Point p)
        {
            char tile = Map[p.ToIndex(Width)];
            try
            {
                return TileMap[tile];
            }
            catch
            {
                throw new Exception($"Could not find tile {tile}");
            }
        }

        public char GetTileChar(Tile t)
        {
            return TileMap.FirstOrDefault(x => x.Value == t).Key;
        }

        public void Draw()
        {
            Point p = new ();
            for (int y = 0; y < Height; y++)
            {
                p.Y = y;
                for (int x = 0; x < Width; x++)
                {
                    p.X = x;
                    Console.Write(Map[p.ToIndex(Width)]);
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public void Draw(Rock r)
        {
            var copy = new string(Map);
            var c = new Chamber(copy.ToCharArray(), Width, Height);
            r.DrawToChamber(c);
            c.Draw();
        }

        public void AddLayer(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                Map = _row.ToCharArray().Concat(Map).ToArray();
                Height += 1;
            }
        }

        public Point GetLocationOfTopMostRock()
        {
            for (var i = 0; i < Map.Length; i++)
            {
                var map = Map[i];
                if (TileMap[map] == Tile.Rock)
                {
                    return Point.FromIndex(i, Width);
                }
            }

            // If no rocks, can just return 0,0 as that is floor coords
            return new Point(0, Height - 1);
        }
    }
}
