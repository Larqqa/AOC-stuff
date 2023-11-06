using Library;

namespace _2022.Days.Day17
{
    public class Chamber
    {
        public char[] Map;
        public int Width;
        public int Height;

        public Chamber(char[] map, int height, int width = 7)
        {
            Map = map;
            Width = width;
            Height = height;
        }

        public void Draw()
        {
            Point p = new ();
            for (int y = 0; y < Height; y--)
            {
                p.Y = y;
                for (int x = 0; x < Width; x++)
                {
                    p.X = x;
                    Console.Write(Map[p.ToIndex(Width)]);
                }
                Console.WriteLine();
            }
        }

        public void AddLayer()
        {
            char[] row = new char[Width];
            Map = row.Concat(Map).ToArray();
        }
    }
}
