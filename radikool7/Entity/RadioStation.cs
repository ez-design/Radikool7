namespace Radikool7.Entity
{
    public class RadioStation
    {
        public string Id { get; set; }
        
        public string Code { get; set; }
        public string Name { get; set; }
        public string RegionId { get; set; }
        public string RegionName { get; set; }
        public string AreaId { get; set; }
        public string AreaName { get; set; }
        public string Type { get; set; }
        public string StationUrl { get; set; }
        
        public string MediaUrl { get; set; }
        
        public string TimetableUrl { get; set; }
        public int Sequence { get; set; }
    }
}