using CommandLine;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Sharprompt;
using utils.Weekly;
using Utils.Configuration;
using Utils.Daily;
using Utils.Providers;
using Utils.Weekly;

namespace Utils
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var dateTimeProvider = new DateTimeProvider();
            var fileProvider = new FileProvider();
            var pathProvider = new PathProvider();
            var directoryProvider = new DirectoryProvider();
            Parser.Default.ParseArguments<DailyCliOptions, WeeklyReportsCliOptions, ConfigOptions>(args)
                .MapResult(
                    (DailyCliOptions opts) => RunRandomDaily(opts,
                        dateTimeProvider,
                        pathProvider,
                        fileProvider,
                        directoryProvider),
                    (WeeklyReportsCliOptions opts) => RunWeeklyChecks(opts),
                    (ConfigOptions configOptions) => new Config().Run(configOptions, args),
                    errs => 1
                );
        }

        static int RunRandomDaily(DailyCliOptions opts,
            IDateTimeProvider dateTimeProvider,
            IPathProvider pathProvider,
            IFileProvider fileProvider,
            IDirectoryProvider directoryProvider)
        {
            try
            {
                if (string.IsNullOrEmpty(opts.ConfigFile))
                    opts.ConfigFile = ConfigurationDefaults.ConfigFile;
                var configurationRoot = new ConfigurationBuilder().AddJsonFile(opts.ConfigFile).Build();
                var configuration = configurationRoot.GetSection(nameof(RandomDailyConfiguration)).Get<RandomDailyConfiguration>() ?? new RandomDailyConfiguration();
                return new RandomDaily(opts,
                    configuration,
                    dateTimeProvider,
                    pathProvider,
                    fileProvider,
                    directoryProvider)
                    .Run();
            }
            catch (ValidationException e)
            {
                Console.WriteLine("Config file not valid!");
                Console.WriteLine(e.Message);
                return -1;
            }
            catch (PromptCanceledException)
            {
                Console.WriteLine("Canceled");
                return 0;
            }
        }

        static int RunWeeklyChecks(WeeklyReportsCliOptions opts)
        {
            try
            {
                if (string.IsNullOrEmpty(opts.ConfigFile))
                    opts.ConfigFile = ConfigurationDefaults.ConfigFile;
                var configurationRoot = new ConfigurationBuilder().AddJsonFile(opts.ConfigFile).Build();
                var configuration = configurationRoot.GetSection(nameof(WeeklyReportsConfiguration)).Get<WeeklyReportsConfiguration>() ?? new WeeklyReportsConfiguration();
                return new WeeklyReports(opts, configuration).Run();
            }
            catch (ValidationException e)
            {
                Console.WriteLine("Config file not valid!");
                Console.WriteLine(e.Message);
                return -1;
            }
        }
    }
}