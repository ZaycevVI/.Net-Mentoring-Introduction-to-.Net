using System.Configuration;

namespace FileLibrary.Configuration
{
    public class SettingSection : ConfigurationSection
    {
        [ConfigurationProperty("culture", IsKey = true, IsRequired = true)]
        public string Culture
        {
            get => (string)this["culture"];
            set => this["culture"] = value;
        }

        [ConfigurationProperty("directories")]
        [ConfigurationCollection(typeof(Directories), AddItemName = "directory")]
        public Directories Directories
        {
            get
            {
                var o = this["directories"];
                return o as Directories;
            }
        }

        [ConfigurationProperty("rules")]
        [ConfigurationCollection(typeof(Directories), AddItemName = "file")]
        public Rules Rules
        {
            get
            {
                var o = this["rules"];
                return o as Rules;
            }
        }

        [ConfigurationProperty("directoryDestination")]
        public DirectoryDestination DirectoryDestinationies
        {
            get
            {
                var o = this["directoryDestination"];
                return o as DirectoryDestination;
            }
        }

        [ConfigurationProperty("orderNumber")]
        public OrderNumber OrderNumber
        {
            get
            {
                var o = this["orderNumber"];
                return (OrderNumber)o;
            }
        }

        [ConfigurationProperty("date")]
        public Date Date
        {
            get
            {
                var o = this["date"];
                return (Date)o;
            }
        }

        [ConfigurationProperty("defaultDir")]
        public DefaultDirectory DefaultDirectory
        {
            get
            {
                var o = this["defaultDir"];
                return (DefaultDirectory)o;
            }
        }
    }

    public class Directory : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get => (string)this["name"];
            set => this["name"] = value;
        }
    }

    public class File : ConfigurationElement
    {
        [ConfigurationProperty("template", IsKey = true, IsRequired = true)]
        public string Template
        {
            get => (string)this["template"];
            set => this["template"] = value;
        }

        [ConfigurationProperty("destination", IsKey = true, IsRequired = true)]
        public string Destination
        {
            get => (string)this["destination"];
            set => this["destination"] = value;
        }
    }

    public class DirectoryDestination : ConfigurationElement
    {
        [ConfigurationProperty("value", IsKey = true, IsRequired = true)]
        public string Value
        {
            get => (string)this["value"];
            set => this["value"] = value;
        }
    }

    public class OrderNumber : ConfigurationElement
    {
        [ConfigurationProperty("isEnabled", IsKey = true, IsRequired = true)]
        public bool IsEnabled
        {
            get => (bool)this["isEnabled"];
            set => this["isEnabled"] = value;
        }
    }

    public class Date : ConfigurationElement
    {
        [ConfigurationProperty("isEnabled", IsKey = true, IsRequired = true)]
        public bool IsEnabled
        {
            get => (bool)this["isEnabled"];
            set => this["isEnabled"] = value;
        }
    }

    public class DefaultDirectory : ConfigurationElement
    {
        [ConfigurationProperty("path", IsKey = true, IsRequired = true)]
        public string Path
        {
            get => (string)this["path"];
            set => this["path"] = value;
        }
    }

    public class Directories
        : ConfigurationElementCollection
    {
        public Directory this[int index]
        {
            get => BaseGet(index) as Directory;
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new Directory this[string responseString]
        {
            get => (Directory)BaseGet(responseString);
            set
            {
                if (BaseGet(responseString) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
                }
                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Directory();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Directory)element).Name;
        }
    }

    public class Rules
        : ConfigurationElementCollection
    {
        public File this[int index]
        {
            get => BaseGet(index) as File;
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new File this[string responseString]
        {
            get => (File)BaseGet(responseString);
            set
            {
                if (BaseGet(responseString) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
                }
                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new File();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((File)element).Template;
        }
    }
}