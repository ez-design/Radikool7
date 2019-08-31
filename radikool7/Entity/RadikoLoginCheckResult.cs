using Newtonsoft.Json;

namespace Radikool7.Entity
{
    public class RadikoLoginCheckResult
    {
        [JsonProperty("user_key")]
        public string UserKey { get; set; }
        
        [JsonProperty("areafree")]
        public string AreaFree { get; set; }
        
        [JsonProperty("paid_member")]
        public string PaidMember { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}