using System.Text;
using utils.Daily;

namespace Utils.Daily
{
    public class RandomDailyConfiguration
    {
        public IList<RandomDailyTeam> Teams { get; set; } = new List<RandomDailyTeam>();

        public RandomDailyNotes Notes { get; set; } = new RandomDailyNotes();

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Teams");
            foreach(var team in Teams)
            {
                sb.AppendLine(string.Format("{0}: {1}", team.Name, string.Join(',', team.Members)));
            }
            return sb.ToString();
        }
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

        public FeedbackArchive Archive { get; set; } = FeedbackArchive.Daily;
    }
}