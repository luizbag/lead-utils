using System.Text;

namespace Utils.Daily
{
    public class RandomDailyConfiguration
    {
        public IList<RandomDailyTeam> Teams { get; set; } = new List<RandomDailyTeam>();

        public RandomDailyNotes Notes { get; set; } = new RandomDailyNotes();
    }

    public class RandomDailyTeam
    {
        public string Name { get; set; } = string.Empty;

        public IList<string> Members { get; set; } = new List<string>();

        public string MembersToString()
        {
            var sb = new StringBuilder();
            foreach (var member in Members)
            {
                sb.Append(member + ", ");
            }
            sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }
    }

    public class RandomDailyNotes
    {
        public string FilePath { get; set; } = string.Empty;

        public string WriteMode { get; set; } = string.Empty;
    }
}