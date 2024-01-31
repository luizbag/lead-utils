using CommandLine;
using Microsoft.Extensions.Configuration;
using Utils.Daily;
using Utils.Weekly;

namespace Utils
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<DailyCliOptions, WeeklyReportsOptions>(args)
                .MapResult(
                    (DailyCliOptions opts) => RunDaily(opts),
                    (WeeklyReportsOptions opts) => RunWeeklyChecks(opts),
                    errs => 1
                );
        }

        static int RunDaily(DailyCliOptions opts)
        {
            var configurationRoot = new ConfigurationBuilder().AddJsonFile(opts.ConfigFile).Build();
            var dailyConfiguration = configurationRoot.GetSection(nameof(RandomDailyOptions)).Get<RandomDailyOptions>();
            return new RandomDaily(opts, dailyConfiguration).Run();
        }

        static int RunWeeklyChecks(WeeklyReportsOptions opts)
        {
            return new WeeklyReports(opts).Run();
        }
    }
}