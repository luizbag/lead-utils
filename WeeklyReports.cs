using CommandLine;

namespace Utils.Weekly
{
    [Verb("weekly", HelpText = "Run the weekly reports checks")]
    public class WeeklyReportsOptions : BaseOptions
    {

    }
    public class WeeklyReports : IRunnable
    {
        public WeeklyReports(WeeklyReportsOptions opts) { }
        public int Run()
        {
            return 0;
        }

    }
}