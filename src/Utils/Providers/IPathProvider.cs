namespace Utils.Providers
{
    public interface IPathProvider
    {
        string Combine(string path1, string path2);

        bool Exists(string filePath);
    }
}