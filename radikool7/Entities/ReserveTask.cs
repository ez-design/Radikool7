using System;
using System.Collections.Generic;

namespace Radikool7.Entities
{
    public class ReserveTask
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string ReserveId { get; set; }
        public string Status { get; set; }
        public bool IsTimeFree { get; set; }
        public DateTime ProgramStart { get; set; }
        public DateTime ProgramEnd { get; set; }
    }
}