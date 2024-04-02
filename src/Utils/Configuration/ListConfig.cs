using CommandLine;

namespace Utils.Configuration
{
    [Verb("list", HelpText = "List Configurations")]
    public class ListConfigOptions
    { }

    public class ListConfig : BaseRunner
    {
        private readonly ConfigOptions _configOptions;

        public ListConfig(ConfigOptions configOptions)
        {
            _configOptions = configOptions;
        }

        public override int Run()
        {
            try
            {
                var lines = File.ReadAllLines(_configOptions.ConfigFile ?? ConfigurationDefaults.ConfigFile);
                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }
                return 0;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found. Use utils config init to create one.");
                return 1;
            }
        }
    }
}