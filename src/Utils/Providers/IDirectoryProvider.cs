namespace Utils.Providers
{
    public interface IDirectoryProvider
    {
        DirectoryInfo CreateDirectory(string path);
    }
}