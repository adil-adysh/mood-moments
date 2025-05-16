// This file is obsolete. All logic is now in Services/TimelineService.cs.
// You may safely delete this file.

using System.Collections.ObjectModel;
using mood_moments.Models;
using System.Linq;
using System;

namespace mood_moments.Logic
{
    public static class MainPageLogic
    {
        public static IEnumerable<string> GetTimeValues(IEnumerable<MoodJournalEntry> entries, string unit)
        {
            var allDates = entries
                .Select(e => e.Date)
                .Where(date => !string.IsNullOrWhiteSpace(date) && DateTime.TryParse(date, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _))
                .Select(date => DateTime.Parse(date!, System.Globalization.CultureInfo.InvariantCulture))
                .ToList();
            if (!allDates.Any())
                return new List<string>();
            try
            {
                var years = allDates.Select(d => d.Year.ToString()).Distinct().OrderByDescending(y => y);
                var months = allDates.Select(d => d.ToString("yyyy-MM")).Distinct().OrderByDescending(m => m);
                var weeks = allDates.Select(d => $"{d.Year}-W{System.Globalization.ISOWeek.GetWeekOfYear(d):D2}").Distinct().OrderByDescending(w => w);
                var days = allDates.Select(d => d.ToString("yyyy-MM-dd")).Distinct().OrderByDescending(d => d);
                if (unit == "Year") return years;
                if (unit == "Month") return months;
                if (unit == "Week") return weeks;
                if (unit == "Day") return days;
            }
            catch
            {
                // Swallow and return empty if something goes wrong
            }
            return new List<string>();
        }

        public static IEnumerable<EntryGroup> GroupEntries(IEnumerable<MoodJournalEntry> entries, string unit, string? value)
        {
            var all = entries?.ToList() ?? new List<MoodJournalEntry>();
            var result = new List<EntryGroup>();
            if (!all.Any())
            {
                result.Add(new EntryGroup { GroupTitle = "No Entries" });
                return result;
            }
            try
            {
                switch (unit)
                {
                    case "All":
                        result.Add(CreateGroup("All Entries", all.OrderByDescending(e => e.Date ?? "")));
                        break;
                    case "Year" when !string.IsNullOrEmpty(value):
                        result.Add(CreateGroup(value, all.Where(e => !string.IsNullOrEmpty(e.Date) && e.Date.StartsWith(value)).OrderByDescending(e => e.Date)));
                        break;
                    case "Month" when !string.IsNullOrEmpty(value):
                        result.Add(CreateGroup(value, all.Where(e => !string.IsNullOrEmpty(e.Date) && e.Date.StartsWith(value)).OrderByDescending(e => e.Date)));
                        break;
                    case "Week" when !string.IsNullOrEmpty(value):
                        result.Add(CreateGroup(value, all.Where(e => IsWeekMatch(e.Date, value)).OrderByDescending(e => e.Date)));
                        break;
                    case "Day" when !string.IsNullOrEmpty(value):
                        result.Add(CreateGroup(value, all.Where(e => e.Date == value).OrderByDescending(e => e.Date)));
                        break;
                    default:
                        result.Add(new EntryGroup { GroupTitle = "No Entries" });
                        break;
                }
            }
            catch
            {
                result.Clear();
                result.Add(new EntryGroup { GroupTitle = "Error loading entries" });
            }
            return result;
        }

        private static EntryGroup CreateGroup(string title, IEnumerable<MoodJournalEntry> entries)
        {
            var group = new EntryGroup { GroupTitle = title };
            foreach (var e in entries)
                group.Add(e);
            return group.Count > 0 ? group : new EntryGroup { GroupTitle = "No Entries" };
        }

        private static bool IsWeekMatch(string? date, string weekValue)
        {
            if (string.IsNullOrWhiteSpace(date)) return false;
            if (DateTime.TryParse(date, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var dt))
            {
                var week = $"{dt.Year}-W{System.Globalization.ISOWeek.GetWeekOfYear(dt):D2}";
                return week == weekValue;
            }
            return false;
        }
    }
}
