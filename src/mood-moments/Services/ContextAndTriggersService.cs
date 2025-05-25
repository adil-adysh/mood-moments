using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using mood_moments.Models;
using Microsoft.Maui.Storage;

namespace mood_moments.Services
{
    public class ContextAndTriggersService
    {
        private const string JsonFileName = "ContextAndTriggers.json";
        public ContextAndTriggersData Data { get; private set; } = new();
        private readonly IAppFileProvider _fileProvider;

        public ContextAndTriggersService(IAppFileProvider? fileProvider = null)
        {
            _fileProvider = fileProvider ?? new MauiAppFileProvider();
            LoadData();
        }

        private void LoadData()
        {
            // Use injected file provider for testability
            using var stream = _fileProvider.OpenAppPackageFileAsync(JsonFileName).GetAwaiter().GetResult();
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<ContextAndTriggersData>(json, options);
                if (data == null)
                    throw new InvalidDataException("Deserialized ContextAndTriggersData is null.");
                Data = data;
            }
            catch (JsonException ex)
            {
                throw new InvalidDataException($"Failed to deserialize ContextAndTriggers.json: {ex.Message}", ex);
            }
        }
    }
}
