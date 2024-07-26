using Library;

namespace _2022.Days.Day24
{
    public class Blizzard
    {
        public Point Location;
        public Direction Direction;

        public Blizzard(Point p, Direction dir)
        {
            Location = p;
            Direction = dir;
        }

        public Blizzard NextLocation(int width, int height)
        {
            var newLocation = Direction switch
            {
                Direction.Up => new Point(Location.X, Location.Y - 1),
                Direction.Right => new Point(Location.X + 1, Location.Y),
                Direction.Down => new Point(Location.X, Location.Y + 1),
                Direction.Left => new Point(Location.X - 1, Location.Y),
                _ => throw new ArgumentOutOfRangeException()
            };

            // Keep blizzard inside the walls
            // conservation of energy loops around to other side
            if (newLocation.X < 1) newLocation.X = width - 2;
            if (newLocation.X > width - 2) newLocation.X = 1;
            if (newLocation.Y < 1) newLocation.Y = height - 2;
            if (newLocation.Y > height - 2) newLocation.Y = 1;

            return new Blizzard(newLocation, Direction);
        }

        public static Direction? GetDirection(char c)
        {
            return c switch
            {
                '^' => Direction.Up,
                '>' => Direction.Right,
                'v' => Direction.Down,
                '<' => Direction.Left,
                _ => null
            };
        }
    }

    public enum Direction
    {
        Up = '^', Right = '>', Down = 'v', Left = '<'
    }
}
