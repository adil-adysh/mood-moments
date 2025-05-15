namespace mood_moments.Models
{
    public class MoodJournalEntry
    {
        public string? Date { get; set; }
        public string? Mood { get; set; }
        public string? Context { get; set; }
        public string? Trigger { get; set; }
        public string? Intensity { get; set; }
        public string? Notes { get; set; }
    }
}
