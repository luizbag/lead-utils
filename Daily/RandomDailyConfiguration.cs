using System.Text;

namespace Utils.Daily
{
    public class RandomDailyConfiguration
    {
        public IList<RandomDailyTeam> Teams { get; set; }

        public RandomDailyNotes Notes { get; set; }
    }

    public class RandomDailyTeam
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

    public class RandomDailyNotes
    {
        public string FilePath { get; set; }

        public string WriteMode { get; set; }
    }
}