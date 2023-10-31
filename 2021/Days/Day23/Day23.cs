using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace _2021.Days.Day23
{
    public class Day23
    {
        public static void Run()
        {

            Console.WriteLine("---Day 23---");
            var d = new Day23();
            var input = $"#############\r\n#...........#\r\n###D#D#C#B###\r\n  #B#A#A#C#  \r\n  #########  ";
            d.Operation(input);

            //var input2 = $"#############\r\n#...........#\r\n###D#D#C#B###\r\n  #D#C#B#A#  \r\n  #D#B#A#C#  \r\n  #B#A#A#C#  \r\n  #########  ";
            //d.Operation(input2);

            Console.WriteLine("------------");
        }
        private void Operation(string input)
        {
            var b = new Burrow(input);
            var res = Burrow.SolveBurrow(b);
            Console.WriteLine($"Least amount of moves is: {res}");
        }
    }
}
