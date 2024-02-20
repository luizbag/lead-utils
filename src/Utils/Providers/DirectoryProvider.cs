namespace Utils.Providers
{
    public class DirectoryProvider : IDirectoryProvider
    {
        public DirectoryInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }
    }
}