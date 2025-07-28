
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using mood_moments.Models;
using Microsoft.Maui.Storage;

namespace mood_moments.Services
{
    public class ContextAndTriggersService
    {
        private const string JsonFolder = "context-triggers";
        private readonly IAppFileProvider _fileProvider;

        public ContextAndTriggersService(IAppFileProvider? fileProvider = null)
        {
            _fileProvider = fileProvider ?? new MauiAppFileProvider();
        }

        // Returns a list of available domain files and their metadata (name, description, filename)
        public async Task<List<DomainInfo>> ListAvailableDomainsAsync()
        {
            // Hardcoded list of domain files (could be made dynamic if needed)
            var domainFiles = new[]
            {
                "BodyAndHealth.json",
                "DailyLifeAndEnvironment.json",
                "FinancialWellBeing.json",
                "LeisureAndSelfExpression.json",
                "MindAndEmotions.json",
                "RelationshipsAndSocialLife.json",
                "Spirituality.json",
                "WorkStudyAndPurpose.json"
            };
            var result = new List<DomainInfo>();
            foreach (var file in domainFiles)
            {
                try
                {
                    var domain = await LoadDomainAsync(file);
                    if (domain != null)
                    {
                        result.Add(new DomainInfo
                        {
                            Name = domain.Name,
                            Description = domain.Description,
                            FileName = file
                        });
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to load domain '{file}': {ex.Message}");
                }
            }
            if (result.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("No domains loaded. Check if JSON files are present and accessible.");
            }
            return result;
        }


        public async Task<Domain?> LoadDomainAsync(string domainFileName)
        {
            var folderPath = $"Resources/Raw/{JsonFolder}";
            var path = $"{folderPath}/{domainFileName}";
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            try
            {
                using var stream = await _fileProvider.OpenAppPackageFileAsync(path);
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();
                var domain = JsonSerializer.Deserialize<Domain>(json, options);
                return domain;
            }
            catch (JsonException ex)
            {
                throw new InvalidDataException($"Failed to deserialize {domainFileName}: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to load {domainFileName}: {ex.Message}", ex);
            }
        }
    }
}
