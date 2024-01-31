namespace Utils.Providers
{
    public class FileProvider : IFileProvider
    {
        public StreamWriter AppendText(string path)
        {
            return File.AppendText(path);
        }
    }
}