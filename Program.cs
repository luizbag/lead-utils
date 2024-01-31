using CommandLine;
using FluentValidation;
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
            try
            {
                var configurationRoot = new ConfigurationBuilder().AddJsonFile(opts.ConfigFile).Build();
                var configuration = configurationRoot.GetSection(nameof(RandomDailyConfiguration)).Get<RandomDailyConfiguration>();
                return new RandomDaily(opts, configuration).Run();
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
                var configuration = GetConfiguration<WeeklyReportsConfiguration>(opts.ConfigFile);
                return new WeeklyReports(opts, configuration).Run();
            }
            catch (ValidationException e)
            {
                Console.WriteLine("Config file not valid!");
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        static T GetConfiguration<T>(string configFile)
        {
            var configurationRoot = new ConfigurationBuilder().AddJsonFile(configFile).Build();
            var configuration = configurationRoot.GetSection(nameof(T)).Get<T>();
            return configuration;
        }
    }
}