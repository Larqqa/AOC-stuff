using Newtonsoft.Json;

namespace _2021.Days.Day18
{
    public class SnailfishNumber
    {
        public int? LeftNumber { get; set; }
        public int? RightNumber { get; set; }
        public SnailfishNumber? LeftSnailfishNumber { get; set; }
        public SnailfishNumber? RightSnailfishNumber { get; set; }

        public SnailfishNumber Clone()
        {
            var temp = JsonConvert.DeserializeObject<SnailfishNumber>(JsonConvert.SerializeObject(this));
            if (temp != null) return temp;
            throw new Exception("Failed to clone number!");
        }
    }
}
