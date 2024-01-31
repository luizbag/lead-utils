using CommandLine;
using utils;
using utils.Weekly;

namespace Utils.Weekly
{
    [Verb("weekly", HelpText = "Run the weekly reports checks")]
    public class WeeklyReportsCliOptions : BaseOptions
    {

    }

    public class WeeklyReports : BaseRunner
    {
        public WeeklyReports(WeeklyReportsCliOptions options, WeeklyReportsConfiguration configuration) { }
        public override int Run()
        {
            return 0;
        }

    }
}