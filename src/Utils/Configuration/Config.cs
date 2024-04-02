using CommandLine;

namespace Utils.Configuration
{
    [Verb("config", HelpText = "Run the daily meeting")]
    public class ConfigOptions : BaseOptions
    {
        
    }

    public class Config
    {
        public int Run(ConfigOptions configOptions, string[] args)
        {
            return Parser.Default.ParseArguments<ListConfigOptions, InitConfigOptions>(args.Skip(1).ToArray())
                .MapResult(
                    (ListConfigOptions opts) => new ListConfig(configOptions).Run(),
                    (InitConfigOptions opts) => new InitConfig(configOptions).Run(),
                    errs => 1
                );
        }
    }
}