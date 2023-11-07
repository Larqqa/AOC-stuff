using Library;

namespace _2022.Days.Day17.Rocks
{
    public class Cross : Rock
    {
        public List<Point> vertices = new()
        {
                              new Point(1, -2),
            new Point(0, -1), new Point(1, -1), new Point(2, -1),
                              new Point(1, 0),
        };

        public Cross(Point position) : base(position)
        {
            Position = position;
            Shape = Shapes.Cross;
            Vertices = vertices;
            Width = 3;
            Height = 3;
        }
    }
}
