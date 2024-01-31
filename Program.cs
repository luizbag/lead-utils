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
                    (DailyCliOptions opts) => RunRandomDaily(opts),
                    (WeeklyReportsCliOptions opts) => RunWeeklyChecks(opts),
                    errs => 1
                );
        }

        static int RunRandomDaily(DailyCliOptions opts)
        {
            try
            {
                var configuration = GetConfiguration<RandomDailyConfiguration>(opts.ConfigFile);
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

        static T GetConfiguration<T>(string configFile) where T : new()
        {
            var configurationRoot = new ConfigurationBuilder().AddJsonFile(configFile).Build();
            var configuration = configurationRoot.GetSection(nameof(T)).Get<T>();
            return configuration ?? new T();
        }
    }
}