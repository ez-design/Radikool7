using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Radikool7.Entities
{
    public class Reserve
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; }
        public string StationId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string FormatId { get; set; }
        public List<int> Repeat { get; set; } = new List<int>();
        public bool Enabled { get; set; } = true;
        public bool IsTimeFree { get; set; } = true;
        
        [JsonIgnore]
        public string NextStart { get; set; }
        [JsonIgnore]
        public string NextEnd { get; set; }
        [JsonIgnore]
        public string RepeatText { get; set; }
        [JsonIgnore]
        public string StationName { get; set; }

        
        
    }
}