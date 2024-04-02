using System.Net.NetworkInformation;
using Utils.Daily;

namespace Utils.Configuration
{
    public static class ConfigurationDefaults
    {
        public static readonly string ConfigFilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        public const string ConfigFileName = "utils.config.json";

        public static string ConfigFile
        {
            get { return Path.Join(ConfigFilePath, ConfigFileName); }
        }

        public static string FeedbackFilePath
        {
            get { return ConfigFilePath; }
        }

        public static FeedbackArchive FeedbackArchive
        {
            get { return FeedbackArchive.Daily; }
        }
    }
}