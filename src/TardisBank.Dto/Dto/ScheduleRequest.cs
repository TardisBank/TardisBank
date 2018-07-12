using System;

namespace TardisBank.Dto
{
    public class ScheduleRequest : IRequestModel
    {
        public ScheduleTimePeriod TimePeriod { get; set; }
        public DateTimeOffset NextRun { get; set; }
        public decimal Amount { get; set;}
    }

    public enum ScheduleTimePeriod
    {
        day,
        week,
        month
    }
}