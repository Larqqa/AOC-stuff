using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Library;

[TestClass]
public class NodeTests
{
    [TestMethod]
    public void TestRelations()
    {
        var a = new Node<char>('a');
        var b = new Node<char>('b');
        var c = new Node<char>('c');
        var d = new Node<char>('d');

        a.SetLink(Direction.Next, b);
        b.SetLink(Direction.Next, c);
        d.SetLink(Direction.Previous, c);

        Assert.AreEqual(null, a.Previous);
        Assert.AreSame(b, a.Next);

        Assert.AreSame(a, b.Previous);
        Assert.AreSame(c, b.Next);

        Assert.AreSame(b, c.Previous);
        Assert.AreSame(d, c.Next);

        Assert.AreSame(c, d.Previous);
        Assert.AreEqual(null, d.Next);

        d.SetLink(Direction.Next, a);

        Assert.AreSame(a, d.Next);
        Assert.AreSame(d, a.Previous);

        Console.WriteLine(a);
        Console.WriteLine(b);
        Console.WriteLine(c);
        Console.WriteLine(d);
    }

    [TestMethod]
    public void TestGetting()
    {
        var a = new Node<char>('a');
        var b = new Node<char>('b');
        var c = new Node<char>('c');

        a.SetLink(Direction.Next, b);
        b.SetLink(Direction.Next, c);

        Assert.AreSame(c, a.GetNthNeighbor(Direction.Next, 2));
        Assert.AreEqual(null, a.GetNthNeighbor(Direction.Previous));

        c.SetLink(Direction.Next, a);

        Assert.AreSame(c, a.GetNthNeighbor(Direction.Next, 5));
        Assert.AreSame(c, a.GetNthNeighbor(Direction.Previous));
    }

    [TestMethod]
    public void TestMoving()
    {
        var a = new Node<char>('a');
        var b = new Node<char>('b');
        var c = new Node<char>('c');

        a.SetLink(Direction.Next, b);
        b.SetLink(Direction.Next, c);

        a.MoveSelfTo(Direction.Previous);

        Assert.AreEqual(null, a.Previous);
        Assert.AreSame(b, a.Next);

        a.MoveSelfTo(Direction.Next);

        Assert.AreEqual(null, b.Previous);
        Assert.AreSame(a, b.Next);

        Assert.AreSame(b, a.Previous);
        Assert.AreSame(c, a.Next);

        Assert.AreSame(a, c.Previous);
        Assert.AreEqual(null, c.Next);

        c.MoveSelfTo(Direction.Next);

        Assert.AreSame(a, c.Previous);
        Assert.AreEqual(null, c.Next);

        c.SetLink(Direction.Next, b);
        c.MoveSelfTo(Direction.Next);

        Assert.AreSame(b, c.Previous);
        Assert.AreSame(a, c.Next);

        Assert.AreSame(c, a.Previous);
        Assert.AreSame(b, a.Next);

        Assert.AreSame(a, b.Previous);
        Assert.AreSame(c, b.Next);
    }
}

public class Node<T>(T value)
{
    public Node<T>? Previous { get; set; }
    public Node<T>? Next { get; set; }
    public T Value { get; set; } = value;

    public void SetLink(Direction d, Node<T>? p)
    {
        if (p is null) return;

        switch (d)
        {
            case Direction.Previous:
                Previous = p;
                p.Next = this;
                break;
            case Direction.Next:
                Next = p;
                p.Previous = this;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(d), d, null);
        }
    }

    public Node<T>? GetNthNeighbor(Direction d, int n = 1)
    {
        var temp = this;
        for (var i = 0; i < n; i++)
        {
            temp = d switch
            {
                Direction.Previous => temp.Previous,
                Direction.Next => temp.Next,
                _ => throw new ArgumentOutOfRangeException(nameof(d), d, null)
            };

            if (temp is null) return null;
        }

        return temp;
    }

    public void MoveSelfTo(Direction d)
    {
        if (Previous is not null)
            Previous.Next = Next;

        if (Next is not null)
            Next.Previous = Previous;

        switch (d)
        {
            case Direction.Next:
                if (Next is null) break;

                var tempNext = Next.Next;

                if (Next.Next is not null)
                    Next.Next.Previous = this;

                Next.Next = this;

                Previous = Next;
                Next = tempNext;

                break;
            case Direction.Previous:
                if (Previous is null) break;

                var tempPrevious = Previous.Previous;

                if (Previous.Previous is not null)
                    Previous.Previous.Next = this;

                Previous.Previous = this;

                Next = Previous;
                Previous = tempPrevious;

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(d), d, null);
        }
    }

    public override string ToString()
    {
        var str = new StringBuilder();

        if (Previous != null)
        {
            str.Append(Previous.Value);
            str.Append(" <- ");
        }

        str.Append(Value);

        if (Next != null)
        {
            str.Append(" -> ");
            str.Append(Next.Value);
        }

        return str.ToString();
    }
}

public enum Direction
{
    Previous,
    Next
}