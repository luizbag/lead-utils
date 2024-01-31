using CommandLine;
using FluentValidation;
using utils.Daily;

namespace Utils.Daily
{
    [Verb("daily", isDefault: true, HelpText = "Run the daily meeting")]
    public class DailyCliOptions : BaseOptions
    {
        [Option('t', "team", Required = false, HelpText = "Team choice")]
        public string? TeamName { get; set; }
    }

    public class RandomDaily : BaseRunner
    {
        private string? TeamName { get; set; } = null;

        private RandomDailyConfiguration Configuration { get; set; }

        private readonly RandomDailyConfigurationValidator validator;

        private const string DateFormat = "yyyy_mm_dd";

        public RandomDaily(DailyCliOptions options, RandomDailyConfiguration configuration)
        {
            TeamName = options.TeamName;
            Configuration = configuration;
            validator = new RandomDailyConfigurationValidator();
        }

        public override int Run()
        {
            validator.ValidateAndThrow(Configuration);
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
                if (present != null && !present.ToLowerInvariant().Equals("n"))
                {
                    Console.Write("Feedback: ");
                    var feedback = Console.ReadLine();
                    feedbacks[member] = feedback ?? "";
                    continue;
                }
                missing.Add(member);
            }
            if (missing.Any())
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

        public string GetTeam()
        {
            string? teamName = "";
            while (string.IsNullOrEmpty(teamName) || !ValidateTeam(teamName))
            {
                Console.WriteLine("Choose a team ({0}):", string.Join(", ", Configuration.Teams.Select(t => t.Name)));
                teamName = Console.ReadLine();
            }
            return teamName;
        }

        public void WriteNotes(RandomDailyNotes notes, RandomDailyTeam team, IList<string> missing, IDictionary<string, string> feedbacks)
        {
            if(!Path.Exists(notes.FilePath))
            {
                Directory.CreateDirectory(notes.FilePath);
            }
            string fileName = GetFileName(notes.Archive);
            var fullPath = Path.Combine(notes.FilePath, fileName);
            using (var writer = File.AppendText(fullPath))
            {
                writer.WriteLine();
                writer.WriteLine("Daily for team {0} on {1}", team.Name, DateTime.Today.ToShortTimeString());
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

        public string GetFileName(FeedbackArchive feedbackArchive) {
            var now = DateTime.Now;
            var dateString = now.ToString(DateFormat);
            if(feedbackArchive == FeedbackArchive.Weekly) {
                dateString = now.AddDays(now.DayOfWeek - DayOfWeek.Monday).ToString(DateFormat);
            }
            return string.Format("{0}_notes_{1}.txt", feedbackArchive, dateString);
        }

        public bool ValidateTeam(string teamName)
        {
            return Configuration.Teams.Any(t => t.Name.Equals(teamName));
        }
    }
}