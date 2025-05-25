using System.Collections.Generic;

namespace mood_moments.Models
{
    public class TriggerSet
    {
        public List<string> Positive { get; set; }
        public List<string> Neutral { get; set; }
        public List<string> Negative { get; set; }
    }

    public class Context
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TriggerSet Triggers { get; set; }
    }

    public class Domain
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Context> Contexts { get; set; }
    }

    public class ContextAndTriggersData
    {
        public List<Domain> Domains { get; set; }
    }
}
