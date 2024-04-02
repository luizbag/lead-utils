namespace Utils.Daily
{
    public enum FeedbackArchive
    {
        Daily,
        Weekly
    }

    public static class FeedbackArchiveExtensions
    {
        public static IEnumerable<FeedbackArchive> GetFeedbackArchiveOptions()
        {
            var options = new List<FeedbackArchive>();
            options.Add(FeedbackArchive.Daily);
            options.Add(FeedbackArchive.Weekly);
            return options;
        } 
    }
}