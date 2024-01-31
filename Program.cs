using CommandLine;
using Microsoft.Extensions.Configuration;
using utils.Weekly;
using Utils.Daily;
using Utils.Weekly;

namespace Utils
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<DailyCliOptions, WeeklyReportsCliOptions>(args)
                .MapResult(
                    (DailyCliOptions opts) => RunDaily(opts),
                    (WeeklyReportsCliOptions opts) => RunWeeklyChecks(opts),
                    errs => 1
                );
        }

        static int RunDaily(DailyCliOptions opts)
        {
            var configurationRoot = new ConfigurationBuilder().AddJsonFile(opts.ConfigFile).Build();
            var configuration = configurationRoot.GetSection(nameof(RandomDailyConfiguration)).Get<RandomDailyConfiguration>();
            return new RandomDaily(opts, configuration).Run();
        }

        static int RunWeeklyChecks(WeeklyReportsCliOptions opts)
        {
            var configuration = GetConfiguration<WeeklyReportsConfiguration>(opts.ConfigFile);
            return new WeeklyReports(opts, configuration).Run();
        }

        static T GetConfiguration<T>(string configFile)
        {
            var configurationRoot = new ConfigurationBuilder().AddJsonFile(configFile).Build();
            var configuration = configurationRoot.GetSection(nameof(T)).Get<T>();
            return configuration;
        }
    }
}