using System;
using System.Collections.Generic;

namespace Radikool7.Entities
{
    public class SearchCondition
    {
        public List<string> StationIds { get; set; } = new List<string>();
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string Keyword { get; set; }
    }
}