using Microsoft.Playwright;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KinescopeTesting.Drivers.DriverConfigs
{
    public class BrowserConfig
    {
        [JsonPropertyName("Headless")]
        public bool Headless { get; set; }

        [JsonPropertyName("Args")]
        public List<string> Args { get; set; }
    }
    public static class DriverConfigurator
    {
        private static BrowserConfig _config;

        static DriverConfigurator()
        {
            LoadConfiguration("Drivers/DriverConfigs/driver_config.json");
        }

        private static void LoadConfiguration(string filePath)
        {
            var json = File.ReadAllText(filePath);

            _config = JsonSerializer.Deserialize<BrowserConfig>(json);
        }

        public static BrowserTypeLaunchOptions GetBrowserLaunchOptions()
        {
            return new BrowserTypeLaunchOptions
            {
                Headless = _config?.Headless ?? false,
                Args = _config?.Args?.ToArray() ?? []
            };
        }
    }
}