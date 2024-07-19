using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2022.Days.Day22
{
    [TestClass]
    public class MapTests
    {
        public const string Input = "#...\r\n.#..\r\n....\r\n...#\r\n\r\n10R5L5R10L4R5L5";

        [TestMethod]
        public void TestRotation()
        {
            var human = Day22.ParseInput(Input);
            Console.WriteLine(human.Map);

            human.Map.RotateSquareMap();
            Console.WriteLine(human.Map);

            human.Map.RotateSquareMap();
            Console.WriteLine(human.Map);

            human.Map.RotateSquareMap();
            Console.WriteLine(human.Map);

            human.Map.RotateSquareMap();
            Console.WriteLine(human.Map);
        }
    }

    public class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Tile[] Tiles = Array.Empty<Tile>();

        public int GetIndex(Point p)
        {
            return p.ToIndex(Width);
        }

        public Point GetCoords(int index)
        {
            return Point.FromIndex(index, Width);
        }

        public override string ToString()
        {
            var str = string.Empty;

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    str += (char)Tiles[GetIndex(new Point(x, y))];
                }
                if (y < Height - 1)
                    str += '\n';
            }

            return str;
        }

        public static Tile GetTileValue(char tile)
        {
            return tile switch
            {
                (char)Tile.Empty => Tile.Empty,
                (char)Tile.Free => Tile.Free,
                (char)Tile.Wall => Tile.Wall,
                _ => Tile.Empty
            };
        }

        public Point FindStartingLocation()
        {
            var index = 0;
            while (true)
            {
                if (Tiles[index] != Tile.Empty)
                    return GetCoords(index);
                index++;
            }
        }
        public void RotateSquareMap(bool clockwise = true)
        {
            if (Width != Height) return;

            var ret = new Tile[Width * Width];

            for (var y = 0; y < Width; ++y)
            {
                for (var x = 0; x < Width; ++x)
                {
                    var next = clockwise ? new Point(y, Width - x - 1) : new Point(Width - y - 1, x);
                    ret[GetIndex(new Point(x, y))] = Tiles[GetIndex(next)];
                }
            }

            Tiles = ret;
        }
    }

    public enum Tile
    {
        Empty = ' ',
        Free = '.',
        Wall = '#'
    }
}
