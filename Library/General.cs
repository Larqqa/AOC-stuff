using Newtonsoft.Json;

namespace Library
{
    public class General
    {
        public static string GetInput(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch
            {
                throw new Exception("No file was found!");
            }
        }

        public static string ConvertToJson<T>(T num)
        {
            return JsonConvert.SerializeObject(num);
        }
        public static T ConvertJsonToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json) ?? throw new Exception("Could not convert to JSON!");
        }
        public static T Clone<T>(T obj)
        {
            return ConvertJsonToObject<T>(ConvertToJson(obj));
        }
    }
}