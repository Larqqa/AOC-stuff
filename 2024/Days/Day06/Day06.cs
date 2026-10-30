using _2024.Days.Template;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2024.Days.Day06;

[TestClass]
public class Day06Tests
{
    [TestMethod]
    public void Test()
    {
        var d = new Day06()
        {
            Input = "....#.....\r\n.........#\r\n..........\r\n..#.......\r\n.......#..\r\n..........\r\n.#..^.....\r\n........#.\r\n#.........\r\n......#..."
        };

        d.ParseInput();

        Assert.AreEqual("41", d.PartOne());
    }

        [TestMethod]
    public void Test2()
    {
        var d = new Day06()
        {
            Input = "....#.....\r\n.........#\r\n..........\r\n..#.......\r\n.......#..\r\n..........\r\n.#..^.....\r\n........#.\r\n#.........\r\n......#..."
        };

        d.ParseInput();

        Assert.AreEqual("6", d.PartTwo());
    }
}

public enum Tile
{
    Guard = '^',
    Wall = '#',
    Empty = '.',
    Visited = 'X'
}

public class HallMap(int width, int height, Tile[] t) : Map<Tile>(width, height)
{
    public override Tile[] Tiles { get; set; } = t;

    public static Tile GetTileValue(char tile)
    {
        return tile switch
        {
            (char)Tile.Guard => Tile.Guard,
            (char)Tile.Wall => Tile.Wall,
            (char)Tile.Visited => Tile.Visited,
            _ => Tile.Empty
        };
    }

    public bool IsInside(Point p)
    {
        return p.X >= 0 && p.X < Width && p.Y >= 0 && p.Y < Height;
    }
}


public class Guard
{
    public enum Direction
    {
        North, East, South, West
    };

    public Direction Heading = Direction.North;
    public Point Location;

    public Guard(int x, int y, Direction heading = Direction.North)
    {
        Location = new(x,y);
        Heading = heading;
    }

    public Guard(Point p, Direction heading = Direction.North)
    {
        Location = p;
        Heading = heading;
    }

    private readonly Dictionary<Direction, Point> DirectionModifier = new()
    {
        {Direction.North, new Point(0, -1)},
        {Direction.East, new Point(1, 0)},
        {Direction.South, new Point(0, 1)},
        {Direction.West, new Point(-1, 0)},
    };

    public Point GetHeading()
    {
        return DirectionModifier[Heading];
    }

    public void Turn()
    {
        Heading = Heading switch
        {
          Direction.North => Direction.East,
          Direction.East => Direction.South,
          Direction.South => Direction.West,
          Direction.West => Direction.North,
          _ => Direction.North,
        };
    }
}

public class Day06() : Day("06")
{

    private HallMap map;
    private  Guard guard;

    public override void ParseInput()
    {
        var rows = Input.Split("\r\n");
        var tiles = rows.Aggregate(Array.Empty<Tile>(), (map, row) =>
        {
            var mappedRow = row
                .ToCharArray()
                .Select(HallMap.GetTileValue);

            return [.. map, .. mappedRow];
        });

        map = new HallMap(rows[0].Length, rows.Length, tiles);

        var guardLocation = tiles.ToList().IndexOf(Tile.Guard);
        map.Tiles[guardLocation] = Tile.Visited;
        guard = new Guard(map.GetCoords(guardLocation));
    }

    private static (HallMap, bool) Simulate(HallMap map, Guard guard)
    {
        Dictionary<Point, List<Guard.Direction>> Visited = [];

        while(map.IsInside(guard.Location))
        {
            if (Visited.TryGetValue(guard.Location, out var dirs))
            {
                if (dirs.Contains(guard.Heading))
                {
                    return (map, true); // found a loop, visited same location at same heading
                }
                dirs.Add(guard.Heading);
            } else
            {
                Visited.Add(guard.Location, [guard.Heading]);
            }

            // move
            var nextLoc = guard.Location.Add(guard.GetHeading());
            if (!map.IsInside(nextLoc)) break; // outside of map

            if (map.Tiles[map.GetIndex(nextLoc)] == Tile.Wall)
            {
                guard.Turn();
                continue;
            }

            guard.Location = nextLoc;

            var i = map.GetIndex(guard.Location);
            map.Tiles[i] = Tile.Visited;
        }

        return (map, false);
    }

    public override string PartOne()
    {
        var (m, _) = Simulate(new HallMap(map.Width, map.Height, (Tile[]) map.Tiles.Clone()), new Guard(guard.Location, guard.Heading));
        return m.Tiles.Count(x => x == Tile.Visited).ToString();
    }

    public override string PartTwo()
    {
        var rocks = new HashSet<Point>();

        while(map.IsInside(guard.Location))
        {
            // move
            var nextLoc = guard.Location.Add(guard.GetHeading());
            if (!map.IsInside(nextLoc)) break;

            if (map.Tiles[map.GetIndex(nextLoc)] == Tile.Wall)
            {
                guard.Turn();
                continue;
            }

            // Simulate a rock if empty space
            if (map.Tiles[map.GetIndex(nextLoc)] != Tile.Visited)
            {
                var addRock = (Tile[]) map.Tiles.Clone();
                addRock[nextLoc.ToIndex(map.Width)] = Tile.Wall;
                var (_, loops) = Simulate(new HallMap(map.Width, map.Height, addRock), new Guard(guard.Location, guard.Heading));
                if (loops) rocks.Add(nextLoc);
            }

            guard.Location = nextLoc;

            var i = map.GetIndex(guard.Location);
            map.Tiles[i] = Tile.Visited;
        }
        Console.WriteLine(map.Tiles.Count(x => x == Tile.Visited));

        return rocks.Count().ToString();
        // 1914 too high
        // 1781 too high
    }
}
