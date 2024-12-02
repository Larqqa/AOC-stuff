using _2023.Days.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _2023.Days.Day07;

[TestClass]
public class DayTests
{
    [TestMethod]
    public void TestP1()
    {
        var d = new Day07()
        {
            Input = "32T3K 765\r\nT55J5 684\r\nKK677 28\r\nKTJJT 220\r\nQQQJA 483"
        };

        d.ParseInput();
        
        foreach (var hand in d.HandList)
        {
            var t = d.GetHandType(hand.hand);

            if (!d.Hands.ContainsKey(t)) d.Hands.Add(t, []);

            d.Hands[t].Add(hand);
        }

        foreach (var handType in d.Hands)
        {
            d.SortHands(handType.Value);
        }

        var res = d.GetResult(d.Hands).ToString();

        Assert.AreEqual("6440", res);
    }

    [TestMethod]
    public void TestHandTypes()
    {
        var d = new Day07();

        Assert.AreEqual(Day07.Hand.HighCard, d.GetHandType([2, 3, 4, 5, 6]));
        Assert.AreEqual(Day07.Hand.OnePair, d.GetHandType([2, 2, 4, 5, 6]));
        Assert.AreEqual(Day07.Hand.TwoPair, d.GetHandType([2, 2, 4, 4, 6]));
        Assert.AreEqual(Day07.Hand.ThreeOfAKind, d.GetHandType([2, 2, 2, 4, 6]));
        Assert.AreEqual(Day07.Hand.FullHouse, d.GetHandType([4, 2, 2, 2, 4]));
        Assert.AreEqual(Day07.Hand.FourOfAKind, d.GetHandType([2, 2, 2, 2, 6]));
        Assert.AreEqual(Day07.Hand.FiveOfAKind, d.GetHandType([2, 2, 2, 2, 2]));

        Assert.AreEqual(Day07.Hand.FiveOfAKind, d.GetHandType([10,10,10,10,10]));
        Assert.AreEqual(Day07.Hand.HighCard, d.GetHandType([10, 11, 12, 13, 14]));
    }

    [TestMethod]
    public void TestSorting()
    {
        var d = new Day07();

        var hands = new List<(List<int> hand, int bid)>
        {
            ([2,3,4,5,6], 123),
            ([2,3,4,6,5], 123),
            ([2,3,5,4,6], 123),
        };
        d.SortHands(hands);
    }
}
public class Day07() : Day("07")
{
    public List<(List<int> hand, int bid)> HandList = [];
    public Dictionary<Hand, List<(List<int> hand, int bid)>> Hands = [];

    public override void ParseInput()
    {
        var input = Input.Split("\r\n");
        HandList = input.Select(x =>
        {
            var vals = x.Split(' ');
            return (vals[0].ToCharArray().Select(GetCardType).ToList(), int.Parse(vals[1]));
        }).ToList();
    }

    public override string PartOne()
    {
        ParseInput();

        foreach (var hand in HandList)
        {
            var t = GetHandType(hand.hand);
         
            if (!Hands.ContainsKey(t)) Hands.Add(t, []);

            Hands[t].Add(hand);
        }

        foreach (var handType in Hands)
        {
            SortHands(handType.Value);
        }

        return GetResult(Hands).ToString();

        // 252879387 too low
    }

    public override string PartTwo()
    {
        throw new NotImplementedException();
    }

    public void SortHands(List<(List<int> hand, int bid)> hands)
    {
        hands.Sort((a, b) =>
        {
            for (var i = 0; i <= 5; i++)
            {
                if (a.hand[i] == b.hand[i]) continue;
                return a.hand[i] > b.hand[i] ? 1 : 0;
            }

            return 0;
        });
    }

    public long GetResult(Dictionary<Hand, List<(List<int> hand, int bid)>> hands)
    {
        var bids = new List<long>();

        foreach (Hand hand in Enum.GetValues(typeof(Hand)))
        {
            if (hands.TryGetValue(hand, out var h))
                bids = bids.Concat(h.Select(x =>(long)x.bid)).ToList();
        }

        long i = 1;
        return bids.Sum(x =>
        {
            x *= i;
            i++;
            return x;
        });
    }

    public Hand GetHandType(List<int> Cards)
    {
        var occurrences = Cards.GroupBy(i => i).ToList();

        if (occurrences.Exists(x => x.Count() == 5)) return Hand.FiveOfAKind;
        if (occurrences.Exists(x => x.Count() == 4)) return Hand.FourOfAKind;

        if (occurrences.Exists(x => x.Count() == 3))
        {
            if (occurrences.Exists(x => x.Count() == 2)) return Hand.FullHouse;

            return Hand.ThreeOfAKind;
        }

        var twos = occurrences.Where(x => x.Count() == 2).ToList();

        if (twos.Count == 2) return Hand.TwoPair;
        if (twos.Count == 1) return Hand.OnePair;

        // High card
        return Hand.HighCard;
    }

    public enum Hand
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }

    public int GetCardType(char c)
    {
        return c switch
        {
            '2' => 2,
            '3' => 3,
            '4' => 4,
            '5' => 5,
            '6' => 6,
            '7' => 7,
            '8' => 8,
            '9' => 9,
            'T' => 10,
            'J' => 11,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };
    }
}
