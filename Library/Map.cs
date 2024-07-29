using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Library;

[TestClass]
public class MapTests
{
    private readonly Tile[] _t =
    [
        Tile.Wall,Tile.Empty,Tile.Player,
        Tile.Empty,Tile.Wall,Tile.Empty,
        Tile.Empty,Tile.Wall,Tile.Empty,
        Tile.Empty,Tile.Empty,Tile.Wall,
    ];

    [TestMethod]
    public void IndexAndCoordinateFetchWork()
    {
        var map = new BaseMap(3, 4, _t);

        Assert.AreEqual(7, map.GetIndex(new Point(1, 2)));

        Assert.AreEqual(new Point(1, 2), map.GetCoords(7));
    }

    [TestMethod]
    public void ToStringWorks()
    {
        var map = new BaseMap(3, 4, _t);
        Console.WriteLine(map);
        Assert.AreEqual("\u2588\u2591\u25cf\n\u2591\u2588\u2591\n\u2591\u2588\u2591\n\u2591\u2591\u2588", map.ToString());
    }

    [TestMethod]
    public void RotationsWork()
    {
        var map = new BaseMap(3, 4, _t);

        CollectionAssert.AreEqual(new [] {
            Tile.Wall,Tile.Empty,Tile.Player,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Empty,Tile.Empty,Tile.Wall,
        }, map.Tiles);

        map.RotateMap(90);

        CollectionAssert.AreEqual(new [] {
            Tile.Empty,Tile.Empty,Tile.Empty,Tile.Wall,
            Tile.Empty,Tile.Wall,Tile.Wall,Tile.Empty,
            Tile.Wall,Tile.Empty,Tile.Empty,Tile.Player,
        }, map.Tiles);

        map.RotateMap(90);

        CollectionAssert.AreEqual(new [] {
            Tile.Wall,Tile.Empty,Tile.Empty,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Player,Tile.Empty,Tile.Wall,
        }, map.Tiles);


        map.RotateMap(90);

        CollectionAssert.AreEqual(new [] {
            Tile.Player,Tile.Empty,Tile.Empty,Tile.Wall,
            Tile.Empty,Tile.Wall,Tile.Wall,Tile.Empty,
            Tile.Wall,Tile.Empty,Tile.Empty,Tile.Empty,
        }, map.Tiles);

        map.RotateMap(90);

        CollectionAssert.AreEqual(new [] {
            Tile.Wall,Tile.Empty,Tile.Player,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Empty,Tile.Empty,Tile.Wall,
        }, map.Tiles);

        map.RotateMap(-90);

        CollectionAssert.AreEqual(new [] {
            Tile.Player,Tile.Empty,Tile.Empty,Tile.Wall,
            Tile.Empty,Tile.Wall,Tile.Wall,Tile.Empty,
            Tile.Wall,Tile.Empty,Tile.Empty,Tile.Empty,
        }, map.Tiles);

        map.RotateMap(-90);

        CollectionAssert.AreEqual(new [] {
            Tile.Wall,Tile.Empty,Tile.Empty,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Player,Tile.Empty,Tile.Wall,
        }, map.Tiles);


        map.RotateMap(-90);

        CollectionAssert.AreEqual(new [] {
            Tile.Empty,Tile.Empty,Tile.Empty,Tile.Wall,
            Tile.Empty,Tile.Wall,Tile.Wall,Tile.Empty,
            Tile.Wall,Tile.Empty,Tile.Empty,Tile.Player,
        }, map.Tiles);

        map.RotateMap(-90);

        CollectionAssert.AreEqual(new [] {
            Tile.Wall,Tile.Empty,Tile.Player,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Empty,Tile.Empty,Tile.Wall,
        }, map.Tiles);

        map.RotateMap(180);

        CollectionAssert.AreEqual(new[] {
            Tile.Wall,Tile.Empty,Tile.Empty,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Player,Tile.Empty,Tile.Wall,
        }, map.Tiles);

        map.RotateMap(-180);

        CollectionAssert.AreEqual(new[] {
            Tile.Wall,Tile.Empty,Tile.Player,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Empty,Tile.Empty,Tile.Wall,
        }, map.Tiles);

        map.RotateMap(810);

        CollectionAssert.AreEqual(new [] {
            Tile.Empty,Tile.Empty,Tile.Empty,Tile.Wall,
            Tile.Empty,Tile.Wall,Tile.Wall,Tile.Empty,
            Tile.Wall,Tile.Empty,Tile.Empty,Tile.Player,
        }, map.Tiles);

        map.RotateMap(-90);

        CollectionAssert.AreEqual(new[] {
            Tile.Wall,Tile.Empty,Tile.Player,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Empty,Tile.Empty,Tile.Wall,
        }, map.Tiles);

        map.RotateMap(-810);

        CollectionAssert.AreEqual(new [] {
            Tile.Player,Tile.Empty,Tile.Empty,Tile.Wall,
            Tile.Empty,Tile.Wall,Tile.Wall,Tile.Empty,
            Tile.Wall,Tile.Empty,Tile.Empty,Tile.Empty,
        }, map.Tiles);
    }

    [TestMethod]
    public void FlipsWork()
    {
        var map = new BaseMap(3, 4, _t);

        map.FlipMap();

        CollectionAssert.AreEqual(new[] {
            Tile.Wall,Tile.Empty,Tile.Empty,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Player,Tile.Empty,Tile.Wall,
        }, map.Tiles);

        map.FlipMap();

        CollectionAssert.AreEqual(new[] {
            Tile.Wall,Tile.Empty,Tile.Player,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Empty,Tile.Empty,Tile.Wall,
        }, map.Tiles);
    }

    [TestMethod]
    public void MirrorWorks()
    {
        var map = new BaseMap(3, 4, _t);

        map.MirrorMap();

        CollectionAssert.AreEqual(new[] {
            Tile.Player,Tile.Empty,Tile.Wall,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Wall,Tile.Empty,Tile.Empty,
        }, map.Tiles);

        map.MirrorMap();

        CollectionAssert.AreEqual(new[] {
            Tile.Wall,Tile.Empty,Tile.Player,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Empty,Tile.Wall,Tile.Empty,
            Tile.Empty,Tile.Empty,Tile.Wall,
        }, map.Tiles);
    }
}

public abstract class Map<T> where T : struct, Enum
{
    public int Width { get; set; }
    public int Height { get; set; }
    public abstract T[] Tiles { get; set; }

    protected Map(int width, int height)
    {
        Width = width;
        Height = height;
    }

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
        var str = new StringBuilder();

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var tile = Tiles[x + y * Width];
                var value = Convert.ToChar(tile);
                str.Append(value == char.MinValue ? (char)Tile.Empty : value);
            }

            if (y < Height - 1) str.Append('\n');
        }

        return str.ToString();
    }

    /// <summary>
    /// Rotate a 2D map by increments of 90 degrees.
    /// </summary>
    /// <param name="degrees"></param>
    public void RotateMap(int degrees)
    {
        if (degrees % 90 != 0) return;

        var times = (degrees / 90) % 4;

        if (times == 0) return;
        
        if (Math.Abs(times) == 2)
        {
            FlipMap();
            return;
        }

        var clockwise = times switch
        {
            > 0 => times == 1,
            < 0 => times == -3,
            _ => throw new ArgumentOutOfRangeException(nameof(degrees))
        };

        var newTiles = new T[Width * Height];

        for (var y = 0; y < Width; ++y)
        {
            for (var x = 0; x < Height; ++x)
            {
                var next = clockwise ? new Point(y, Height - x - 1) : new Point(Width - y - 1, x);
                newTiles[new Point(x, y).ToIndex(Height)] = Tiles[GetIndex(next)];
            }
        }

        (Width, Height) = (Height, Width);
        Tiles = newTiles;
    }

    public void FlipMap()
    {
        Tiles = Tiles.Reverse().ToArray();
    }


    public void MirrorMap()
    {
        var newTiles = new T[Width * Height];

        for (var y = 0; y < Height; ++y)
        {
            for (var x = 0; x < Width; ++x)
            {
                newTiles[GetIndex(new Point(x, y))] = Tiles[GetIndex(new Point((Width - 1) - x, y))];
            }
        }

        Tiles = newTiles;
    }
}

public enum Tile
{
    Player = '\u25cf',
    Wall = '\u2588',
    Empty = '\u2591'
}

public class BaseMap(int width, int height, Tile[] t) : Map<Tile>(width, height)
{
    public override Tile[] Tiles { get; set; } = t;
}