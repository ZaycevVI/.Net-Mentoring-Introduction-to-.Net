using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileLibrary.Configuration
{
    public class Configuration : IConfiguration
    {
        private const string SettingSectionKey = "settingSection";

        public Configuration()
        {
            SettingSection = ConfigurationManager.GetSection(SettingSectionKey)
                as SettingSection;
        }

        public SettingSection SettingSection { get; }
        public List<(Regex template, string destination)> GetRules()
        {
            return (from object rule in SettingSection.Rules
                    select rule as File into fileRule
                    select (new Regex(fileRule.Template), fileRule.Destination))
                    .ToList();
        }
    }
}