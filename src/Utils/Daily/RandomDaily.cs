using System.Text;
using CommandLine;
using FluentValidation;
using Sharprompt;
using utils.Daily;
using Utils.Providers;

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

        private const string DateFormat = "yyyy_MM_dd";

        private readonly IDateTimeProvider _dateTimeProvider;

        private readonly IPathProvider _pathProvider;

        private readonly IFileProvider _fileProvider;

        private readonly IDirectoryProvider _directoryProvider;

        public RandomDaily(DailyCliOptions options, RandomDailyConfiguration configuration,
            IDateTimeProvider dateTimeProvider,
            IPathProvider pathProvider,
            IFileProvider fileProvider,
            IDirectoryProvider directoryProvider)
        {
            TeamName = options.TeamName;
            Configuration = configuration;
            validator = new RandomDailyConfigurationValidator();
            _dateTimeProvider = dateTimeProvider;
            _pathProvider = pathProvider;
            _fileProvider = fileProvider;
            _directoryProvider = directoryProvider;
        }

        public override int Run()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Prompt.ThrowExceptionOnCancel = true;
            validator.ValidateAndThrow(Configuration);
            TeamName = TeamName ?? GetTeam();
            var random = new Random();
            var team = Configuration.Teams.First(t => t.Name.Equals(TeamName));
            var members = team.Members.ToArray();
            IList<string> missing = new List<string>();
            IDictionary<string, string> feedbacks = new Dictionary<string, string>();
            random.Shuffle<string>(members);
            foreach (var it in members.Select((m, i) => new { Member = m, Index = i + 1 }))
            {
                var member = it.Member;
                var index = it.Index;
                Console.WriteLine("{0}/{1} - {2}", index, members.Count(), member);
                var present = Prompt.Confirm("Present:", true);
                if (present)
                {
                    var feedback = Prompt.Input<string>("Feedback:");
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
                teamName = Prompt.Select<string>("Choose a team", Configuration.Teams.Select(t => t.Name));
            }
            return teamName;
        }

        public void WriteNotes(RandomDailyNotes notes, RandomDailyTeam team, IList<string> missing, IDictionary<string, string> feedbacks)
        {
            if (!_pathProvider.Exists(notes.FilePath))
            {
                _directoryProvider.CreateDirectory(notes.FilePath);
            }
            string fileName = GetFileName(notes.Archive);
            var fullPath = _pathProvider.Combine(notes.FilePath, fileName);
            using (var writer = _fileProvider.AppendText(fullPath))
            {
                writer.WriteLine();
                writer.WriteLine("Daily for team {0} on {1}", team.Name, _dateTimeProvider.Today.ToShortDateString());
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

        public string GetFileName(FeedbackArchive feedbackArchive)
        {
            var now = _dateTimeProvider.Now;
            var dateString = now.ToString(DateFormat);
            if (feedbackArchive == FeedbackArchive.Weekly)
            {
                dateString = now.AddDays(DayOfWeek.Monday - now.DayOfWeek).ToString(DateFormat);
            }
            return string.Format("{0}_notes_{1}.txt", feedbackArchive, dateString);
        }

        public bool ValidateTeam(string teamName)
        {
            return Configuration.Teams.Any(t => t.Name.Equals(teamName));
        }
    }
}