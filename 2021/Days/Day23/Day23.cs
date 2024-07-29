namespace _2021.Days.Day23
{
    public class Day23
    {
        public static void Run()
        {

            Console.WriteLine("---Day 23---");
            var d = new Day23();
            d.Operation();
            Console.WriteLine("------------");
        }
        private void Operation()
        {
            var res = Burrow.Solve($"#############\r\n#...........#\r\n###D#D#C#B###\r\n  #B#A#A#C#  \r\n  #########  ");
            Console.WriteLine($"Least amount of moves is: {res}");
            var res2 = Burrow.Solve($"#############\r\n#...........#\r\n###D#D#C#B###\r\n  #D#C#B#A#  \r\n  #D#B#A#C#  \r\n  #B#A#A#C#  \r\n  #########  ", 13, 7);
            Console.WriteLine($"Least amount of moves is: {res2}");
        }
    }
}
