using System.Text;

namespace Utils.Daily
{
    public class RandomDailyConfiguration
    {
        public IList<Team> Teams { get; set; }

        public Notes Notes { get; set; }
    }

    public class Team
    {
        public string Name { get; set; }

        public IList<string> Members { get; set; }

        public string MembersToString() {
            var sb = new StringBuilder();
            foreach(var member in Members) {
                sb.Append(member + ", ");
            }
            sb.Remove(sb.Length-2, 2);
            return sb.ToString();
        }
    }

    public class Notes
    {
        public string FilePath { get; set; }

        public string WriteMode { get; set; }
    }
}