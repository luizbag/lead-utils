using CommandLine;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using utils.Weekly;
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
            Parser.Default.ParseArguments<DailyCliOptions, WeeklyReportsCliOptions>(args)
                .MapResult(
                    (DailyCliOptions opts) => RunRandomDaily(opts,
                        dateTimeProvider,
                        pathProvider,
                        fileProvider,
                        directoryProvider),
                    (WeeklyReportsCliOptions opts) => RunWeeklyChecks(opts),
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
        }

        static int RunWeeklyChecks(WeeklyReportsCliOptions opts)
        {
            try
            {
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