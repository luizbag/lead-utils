namespace Utils.Providers
{
    public class PathProvider : IPathProvider
    {
        public string Combine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        public bool Exists(string filePath)
        {
            return Path.Exists(filePath);
        }
    }
}