using Xunit;
using mood_moments.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace mood_moments.Tests
{
    public class ContextAndTriggersServiceTests
    {
        [Fact]
        public void Constructor_LoadsDataWithoutException()
        {
            // Arrange
            var fakeProvider = new FakeAppFileProvider();
            // Act
            var exception = Record.Exception(() => new ContextAndTriggersService(fakeProvider));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void Constructor_ParsesRealContextAndTriggersJson()
        {
            // Arrange: use a real sample from the JSON file
            var json = @"{
    ""domains"": [
        {
            ""name"": ""Body & Health"",
            ""description"": ""How your body feels and functions, including sleep, food, and physical well-being."",
            ""contexts"": [
                {
                    ""name"": ""Sleep"",
                    ""description"": ""Your current state of rest and quality of sleep."",
                    ""triggers"": {
                        ""positive"": [""Woke up refreshed"",""Slept deeply"",""Had a refreshing nap"",""Peaceful dreams""],
                        ""neutral"": [""Rested but groggy"",""Light sleep"",""Woke up to an alarm"",""Short nap""],
                        ""negative"": [""Woke up feeling exhausted"",""Restless night"",""Insomnia"",""Couldn't fall asleep"",""Nightmares""]
                    }
                }
            ]
        }
    ]
}";
            var fakeProvider = new FakeAppFileProvider(json);

            // Act
            var service = new ContextAndTriggersService(fakeProvider);

            // Assert
            Assert.NotNull(service.Data);
            Assert.NotNull(service.Data.Domains);
            Assert.Single(service.Data.Domains);
            Assert.Equal("Body & Health", service.Data.Domains[0].Name);
            Assert.Single(service.Data.Domains[0].Contexts);
            Assert.Equal("Sleep", service.Data.Domains[0].Contexts[0].Name);
            Assert.Equal("Woke up refreshed", service.Data.Domains[0].Contexts[0].Triggers.Positive[0]);
        }
    }

    // Fake provider for testing with custom JSON
    public class FakeAppFileProvider : IAppFileProvider
    {
        private readonly string _json;
        public FakeAppFileProvider(string json = "{\"Contexts\":[],\"Triggers\":[]}")
        {
            _json = json;
        }
        public Task<Stream> OpenAppPackageFileAsync(string filename)
        {
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(_json));
            return Task.FromResult<Stream>(stream);
        }
    }
}
