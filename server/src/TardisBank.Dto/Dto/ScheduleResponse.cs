using System;

namespace TardisBank.Dto
{
    public class ScheduleResponse : ResponseModelBase
    {
        public ScheduleTimePeriod TimePeriod { get; set; }
        public DateTimeOffset NextRun { get; set; }
        public decimal Amount { get; set;}
    }
}