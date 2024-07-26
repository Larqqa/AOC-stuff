using Library;

namespace _2022.Days.Day24
{
    public class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Point Start = new (1, 0);
        public Point End;
        public Tile[] Tiles;

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            End = new Point(Width - 2, Height - 1);

            Tiles = new Tile[Width * Height];

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                    {
                        Tiles[x + y * Width] = Tile.Wall;
                    }
                    else
                    {
                        Tiles[x + y * Width] = Tile.Empty;
                    }
                }
            }

            Tiles[Start.X + Start.Y * Width] = Tile.Empty;
            Tiles[End.X + End.Y * Width] = Tile.Empty;
        }

        public override string ToString()
        {
            var str = string.Empty;

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    str += (char)Tiles[x + y * Width];
                }

                str += '\n';
            }

            return str;
        }


        public enum Tile
        {
            Wall = '#',
            Empty = '.',
        }
    }
}
