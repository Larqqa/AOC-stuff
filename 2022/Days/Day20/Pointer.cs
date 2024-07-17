using _2022.Days.Day17;
using static System.Net.Mime.MediaTypeNames;

namespace _2022.Days.Day20
{
    public class Pointer
    {
        public Pointer? Previous { get; set; }
        public Pointer? Next { get; set; }
        public long Value { get; set; }

        public void SetNeighbor(Direction d, Pointer? p)
        {
            switch (d)
            {
                case Direction.Next:
                    if (p is null) break;
                    Previous = p;
                    p.Next = this;
                    break;
                case Direction.Previous:
                    if (p is null) break;
                    Next = p;
                    p.Next = this;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(d), d, null);
            }
        }

        public enum Direction
        {
            Previous, Next
        }

        public void Step(Direction d)
        {
            switch (d)
            {
                case Direction.Next:
                    if (Next?.Next is null || Previous is null) break;

                    var tempNext = Next.Next;

                    Next.Next.Previous = this;

                    Next.Next = this;
                    Next.Previous = Previous;

                    Previous.Next = Next;

                    Previous = Next;
                    Next = tempNext;

                    break;
                case Direction.Previous:
                    if (Next is null || Previous?.Previous is null) break;

                    var tempPrevious = Previous.Previous;
                    Previous.Previous.Next = this;

                    Previous.Previous = this;
                    Previous.Next = Next;

                    Next.Previous = Previous;

                    Next = Previous;
                    Previous = tempPrevious;

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(d), d, null);
            }
        }

        public override string ToString()
        {
            return $"{Previous?.Value ?? 0} -> {Value} -> {Next?.Value ?? 0}";
        }
    }
}
