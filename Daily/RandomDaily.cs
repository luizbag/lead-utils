using CommandLine;

namespace Utils.Daily
{
    [Verb("daily", isDefault: true, HelpText = "Run the daily meeting")]
    public class DailyCliOptions : BaseOptions
    {
        [Option('t', "team", Required = false, HelpText = "Team choice")]
        public string? TeamName { get; set; }
    }

    public class RandomDaily : IRunnable
    {
        private string? TeamName { get; set; } = null;

        private RandomDailyOptions Configuration { get; set; }

        public RandomDaily(DailyCliOptions options, RandomDailyOptions configuration)
        {
            TeamName = options.TeamName;
            Configuration = configuration;
        }

        public int Run()
        {
            TeamName = TeamName ?? GetTeam();
            var random = new Random();
            var team = Configuration.Teams.First(t => t.Name.Equals(TeamName));
            var members = team.Members.ToArray();
            IList<string> missing = new List<string>();
            IDictionary<string, string> feedbacks = new Dictionary<string, string>();
            random.Shuffle<string>(members);
            foreach (var member in members)
            {
                Console.WriteLine(member);
                Console.Write("Present (Y/n): ");
                var present = Console.ReadLine();
                if (!present.ToLowerInvariant().Equals("n"))
                {
                    Console.Write("Feedback: ");
                    var feedback = Console.ReadLine();
                    feedbacks[member] = feedback;
                    continue;
                }
                missing.Add(member);
            }
            if (missing.Count > 0)
            {
                Console.WriteLine("Missing people: {0}", string.Join(", ", missing));
            }
            else
            {
                Console.WriteLine("Everybody showed up!");
            }
            WriteNotes(Configuration.Notes, team, missing, feedbacks);
            return 0;
        }

        private string GetTeam()
        {
            string teamName = "";
            while (string.IsNullOrEmpty(teamName) || !ValidateTeam(teamName))
            {
                Console.WriteLine("Choose a team ({0}):", string.Join(", ", Configuration.Teams.Select(t => t.Name)));
                teamName = Console.ReadLine();
            }
            return teamName;
        }

        private void WriteNotes(Notes notes, Team team, IList<string> missing, IDictionary<string, string> feedbacks)
        {
            using (var writer = File.AppendText(notes.FilePath))
            {
                writer.WriteLine();
                writer.WriteLine("Daily for team {0} on {1}", team.Name, DateTime.Today.ToShortDateString());
                foreach ((var member, var feedback) in feedbacks)
                {
                    writer.WriteLine("\t- {0}: {1}", member, feedback);
                }
                if (missing.Count > 0)
                {
                    writer.WriteLine("Missing: {0}", string.Join(", ", missing));
                }
            }
        }

        public bool ValidateTeam(string teamName)
        {
            return Configuration.Teams.Any(t => t.Name.Equals(teamName));
        }
    }
}