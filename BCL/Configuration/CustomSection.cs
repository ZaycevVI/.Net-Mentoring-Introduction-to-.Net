using System.Configuration;

namespace Configuration
{
    public class CustomSection : ConfigurationSection
    {
        [ConfigurationProperty("countrycode", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string CountryCode
        {
            get => (string) this["countrycode"];
            set => this["countrycode"] = value;
        }

        [ConfigurationProperty("isenabled", DefaultValue = true, IsRequired = true)]
        public bool IsEnabled
        {
            get => (bool) this["isenabled"];
            set => this["isenabled"] = value;
        }
    }
}