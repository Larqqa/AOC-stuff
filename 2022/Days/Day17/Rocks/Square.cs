using Library;

namespace _2022.Days.Day17.Rocks
{
    public class Square : Rock
    {
        public List<Point> vertices = new()
        {
            new Point(0, -1), new Point(1, -1),
            new Point(0, 0), new Point(1, 0),
        };

        public Square(Point position) : base(position)
        {
            Position = position;
            Shape = Shapes.Square;
            Vertices = vertices;
            Width = 2;
            Height = 2;
        }
    }
}
