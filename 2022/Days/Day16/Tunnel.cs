namespace _2022.Days.Day16
{
    public class Tunnel
    {
        public string Key { get; set; }
        public int FlowRate { get; set; }
        public string[] Connections { get; set; }

        public Tunnel(string key, int flowRate, string[] connections)
        { 
            Key = key;
            FlowRate = flowRate;
            Connections = connections;
        }
    }
}
