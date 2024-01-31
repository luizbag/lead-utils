namespace utils.Weekly
{
    public class WeeklyReportsConfiguration
    {
        public IList<WeeklyReportsTeam>? Teams { get; set; }
    }

    public class WeeklyReportsTeam
    {
        public string? Name { get; set; }

        public IList<string>? Members { get; set; }
    }
}