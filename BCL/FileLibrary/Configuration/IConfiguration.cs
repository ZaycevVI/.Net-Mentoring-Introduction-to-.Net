using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FileLibrary.Configuration
{
    public interface IConfiguration
    {
        SettingSection SettingSection { get; }
        List<(Regex template, string destination)> GetRules();
    }
}