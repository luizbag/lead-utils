using CommandLine;

namespace Utils
{
    public abstract class BaseOptions
    {
        [Option('c', "config file", Required = false, HelpText = "Config file path")]
        public required string ConfigFile { get; set; }
    }
}