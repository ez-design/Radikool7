using System;

namespace Radikool7.Entity
{
    public class RadioProgram
    {
        public string Id { get; set; }
        public string StationId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Title { get; set; }
        public string Cast { get; set; }
        public string Description { get; set; }
        public string TsNg { get; set; }

        //  [JsonIgnore]
        public RadioStation RadioStation { get; set; }
    }
}