using System;

using System;

namespace ngakamodirimolema
{
    public enum EventType
    {
        Event,
        Announcement
    }

    public enum EventPriority
    {
        High = 1,
        Medium = 2,
        Low = 3
    }

    public class MunicipalEvent
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public string Location { get; set; } = string.Empty;
        public EventType Type { get; set; } = EventType.Event;
        public EventPriority Priority { get; set; } = EventPriority.Medium;

        public bool IsExpired()
        {
            var now = DateTime.Now;
            if (Type == EventType.Announcement && EndDate.HasValue)
                return EndDate.Value.Date < now.Date;

            return EventDate.Date < now.Date;
        }
    }
}
