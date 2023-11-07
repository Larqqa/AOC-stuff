using Library;

namespace _2022.Days.Day17.Rocks
{
    public class Corner : Rock
    {
        public List<Point> vertices = new()
        {
                                              new Point(2, -2),
                                              new Point(2, -1),
            new Point(0, 0), new Point(1, 0), new Point(2, 0),
        };

        public Corner(Point position) : base(position)
        {
            Position = position;
            Shape = Shapes.Corner;
            Vertices = vertices;
            Width = 3;
            Height = 3;
        }
    }
}
