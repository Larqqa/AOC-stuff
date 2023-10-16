namespace _2021
{
    internal class Library
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
    }
}
