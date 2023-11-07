using Library;

namespace _2022.Days.Day17.Rocks
{
    public class Line : Rock
    {
        public List<Point> vertices = new()
        {
            new Point(0, -3),
            new Point(0, -2),
            new Point(0, -1),
            new Point(0, 0),
        };

        public Line(Point position) : base(position)
        {
            Position = position;
            Shape = Shapes.Line;
            Vertices = vertices;
            Width = 1;
            Height = 4;
        }
    }
}
