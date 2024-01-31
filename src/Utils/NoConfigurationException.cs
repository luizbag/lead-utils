namespace utils
{
    public class NoConfigurationException : Exception
    {
        public NoConfigurationException() { }
        public NoConfigurationException(string message) : base(message) { }

    }
}