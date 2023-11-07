
using Library;

namespace _2022.Days.Day17.Rocks
{
    public class Dash : Rock
    {
        public List<Point> vertices = new()
        {
            new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0),
        };

        public Dash(Point position) : base(position)
        {
            Position = position;
            Shape = Shapes.Dash;
            Vertices = vertices;
            Width = 4;
            Height = 1;
        }
    }
}
