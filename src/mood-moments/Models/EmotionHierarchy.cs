using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace mood_moments.Models
{
    public class EmotionHierarchy
    {
        public List<CoreEmotion> Emotions { get; set; } = new();

        public static EmotionHierarchy LoadFromJson(string jsonPath)
        {
            var json = File.ReadAllText(jsonPath);
            var list = JsonSerializer.Deserialize<List<CoreEmotion>>(json)!;
            return new EmotionHierarchy { Emotions = list };
        }

        public static async Task<EmotionHierarchy> LoadFromJsonAsync(Stream jsonStream)
        {
            using var reader = new StreamReader(jsonStream);
            var json = await reader.ReadToEndAsync();
            var list = JsonSerializer.Deserialize<List<CoreEmotion>>(json)!;
            return new EmotionHierarchy { Emotions = list };
        }

        public IEnumerable<string> GetCoreEmotions() => Emotions.Select(e => e.Core);
        public IEnumerable<string> GetMidLevelEmotions(string core) => Emotions.FirstOrDefault(e => e.Core == core)?.MidLevel.Select(m => m.Name) ?? new List<string>();
        public IEnumerable<string> GetNuancedEmotions(string core, string mid) =>
            Emotions.FirstOrDefault(e => e.Core == core)?.MidLevel.FirstOrDefault(m => m.Name == mid)?.Nuanced ?? new List<string>();
    }

    public class CoreEmotion
    {
        [JsonPropertyName("core")]
        public string Core { get; set; } = string.Empty;
        [JsonPropertyName("mid_level")]
        public List<MidLevelEmotion> MidLevel { get; set; } = new();
    }

    public class MidLevelEmotion
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("nuanced")]
        public List<string> Nuanced { get; set; } = new();
    }
}
