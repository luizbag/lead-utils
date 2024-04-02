using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommandLine;
using Sharprompt;
using Utils.Daily;

namespace Utils.Configuration
{
    [Verb("init", HelpText = "Initialize Configuration File")]
    public class InitConfigOptions
    { }

    public class UtilsConfiguration
    {
        public RandomDailyConfiguration? RandomDailyConfiguration { get; set; }
    }

    public class InitConfig : BaseRunner
    {
        private readonly ConfigOptions _configOptions;
        private readonly InitConfigOptions _initConfigOPtions;
        private readonly JsonSerializerOptions _options;

        public InitConfig(ConfigOptions configOptions, InitConfigOptions initConfigOptions)
        {
            _configOptions = configOptions;
            _initConfigOPtions = initConfigOptions;
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter<FeedbackArchive>()
                }
            };
        }

        public override int Run()
        {
            string configFile = _configOptions.ConfigFile ?? ConfigurationDefaults.ConfigFile;
            if (File.Exists(configFile) && !Prompt.Confirm("Config file already exists. Overwrite?", false))
            {
                return 0;
            }

            var utilsConfiguration = new UtilsConfiguration();
            utilsConfiguration.RandomDailyConfiguration = CreateRandomDailyConfiguration();

            var json = JsonSerializer.Serialize(utilsConfiguration, _options);
            File.WriteAllText(configFile, json);
            return 0;
        }

        private RandomDailyConfiguration CreateRandomDailyConfiguration()
        {
            RandomDailyConfiguration configuration = new RandomDailyConfiguration();
            if(configuration.Teams.Count == 0)
            {
                configuration.Teams.Add(CreateTeam());
            }
            var addAnother = Prompt.Confirm("Add another team?");
            while(addAnother)
            {
                configuration.Teams.Add(CreateTeam());
                addAnother = Prompt.Confirm("Add another team?");
            }

            configuration.Notes = CreateNotes();

            return configuration;
        }

        private RandomDailyTeam CreateTeam()
        {
            var team = new RandomDailyTeam();
            team.Name = Prompt.Input<string>("Team Name:");
            var nMembers = Prompt.Input<int>("How many members:");
            for(var i=0;i<nMembers;i++)
            {
                var memberName = Prompt.Input<string>("Member Name:");
                team.Members.Add(memberName);
            }
            return team;
        }

        private RandomDailyNotes CreateNotes()
        {
            var notes = new RandomDailyNotes();
            notes.FilePath = Prompt.Input<string>("Where do you want to save your notes?", ConfigurationDefaults.FeedbackFilePath);
            notes.Archive = Prompt.Select<FeedbackArchive>("How do you want to save your notes?", FeedbackArchiveExtensions.GetFeedbackArchiveOptions(), defaultValue: FeedbackArchive.Daily);
            return notes;
        }
    }
}