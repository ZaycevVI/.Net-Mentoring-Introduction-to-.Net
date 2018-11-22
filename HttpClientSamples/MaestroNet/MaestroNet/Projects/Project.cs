using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaestroNet.Projects
{
    public class Project
    {
        [JsonProperty("pmcCode")]
        public string PmcCode { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("zone")]
        public string Zone { get; set; }
        
        [JsonProperty("shapes")]
        public List<string> Shapes { get; set; }
        
        [JsonProperty("primaryContacts")]
        public List<string> PrimaryContacts { get; set; }
        
        [JsonProperty("secondaryContacts")]
        public List<string> SecondaryContacts { get; set; }
        
        [JsonProperty("instanceCreationIntervalCount")]
        public int InstanceCreationIntervalCount { get; set; }
        
        [JsonProperty("instanceCreationIntervalHours")]
        public int InstanceCreationIntervalHours { get; set; }
        
        [JsonProperty("volumeCreationIntervalCount")]
        public int VolumeCreationIntervalCount { get; set; }
        
        [JsonProperty("volumeCreationIntervalHours")]
        public int VolumeCreationIntervalHours { get; set; }
        
        [JsonProperty("maxVolumeSizeGb")]
        public int MaxVolumeSizeGb { get; set; }
        
        [JsonProperty("maxCheckpointsPerInstance")]
        public int MaxCheckpointsPerInstance { get; set; }
        
        [JsonProperty("active")]
        public bool Active { get; set; }
        
        //[JsonProperty("activationDate")]
        //public DateTime ActivationDate { get; set; }
        
        //[JsonProperty("deactivationDate")]
        //public DateTime DeactivationDate { get; set; }
        
        [JsonProperty("autoConfigurationDisabled")]
        public bool AutoConfigurationDisabled { get; set; }
        
        [JsonProperty("defaultVlanId")]
        public string DefaultVlanId { get; set; }
        
        [JsonProperty("defaultForCommonCosts")]
        public bool DefaultForCommonCosts { get; set; }
    }
}
