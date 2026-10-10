using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ngakamodirimolema
{
    public static class EventRepository
    {
        private static readonly List<MunicipalEvent> _events = new();
        private static readonly object _lock = new();

        // Additional data structures
        private static readonly Hashtable _hashtable = new(); // eventId -> event
        private static readonly Dictionary<string, MunicipalEvent> _dictionary = new(); // eventId -> event
        private static readonly SortedDictionary<DateTime, List<MunicipalEvent>> _sortedByDate = new();
        private static readonly Queue<MunicipalEvent> _upcomingQueue = new();
        private static readonly SimplePriorityQueue<MunicipalEvent> _priorityQueue = new();
        private static readonly HashSet<string> _uniqueCategories = new();

        static EventRepository()
        {
            LoadSampleData();
        }

        public static IReadOnlyList<MunicipalEvent> GetAll()
        {
            lock (_lock)
            {
                return _events.OrderBy(e => e.EventDate).ThenBy(e => e.Title).ToList().AsReadOnly();
            }
        }

        private static void IndexEvent(MunicipalEvent ev)
        {
            _hashtable[ev.Id] = ev;
            _dictionary[ev.Id] = ev;

            var key = ev.EventDate.Date;
            if (!_sortedByDate.TryGetValue(key, out var list))
            {
                list = new List<MunicipalEvent>();
                _sortedByDate[key] = list;
            }
            list.Add(ev);

            if (ev.EventDate.Date >= DateTime.Now.Date)
                _upcomingQueue.Enqueue(ev);

            // lower numeric value = higher priority in our queue
            _priorityQueue.Enqueue(ev, (int)ev.Priority);

            _uniqueCategories.Add(ev.Category);
        }

        private static void LoadSampleData()
        {
            lock (_lock)
            {
                if (_events.Count > 0) return;

                // Add realistic South African sample events and announcements
                var samples = new List<MunicipalEvent>
                {
                    new MunicipalEvent
                    {
                        Title = "Community Clean-up: Soweto North",
                        Description = "Join neighbours for a litter pick and recycling awareness.",
                        Category = "Community",
                        EventDate = DateTime.Now.Date.AddDays(5),
                        StartTime = new TimeSpan(8,0,0),
                        Location = "Soweto North Community Hall",
                        Type = EventType.Event,
                        Priority = EventPriority.Medium
                    },
                    new MunicipalEvent
                    {
                        Title = "Water Interruption: Protea Glen",
                        Description = "Planned maintenance. Expected interruption between 09:00 and 16:00.",
                        Category = "Water",
                        EventDate = DateTime.Now.Date.AddDays(2),
                        StartTime = new TimeSpan(9,0,0),
                        EndDate = DateTime.Now.Date.AddDays(2),
                        Location = "Protea Glen and surrounding suburbs",
                        Type = EventType.Announcement,
                        Priority = EventPriority.High
                    },
                    new MunicipalEvent
                    {
                        Title = "Electricity Maintenance: Midrand Substation",
                        Description = "Temporary outage while crews replace transformers.",
                        Category = "Electricity",
                        EventDate = DateTime.Now.Date.AddDays(3),
                        StartTime = new TimeSpan(7,30,0),
                        EndDate = DateTime.Now.Date.AddDays(3),
                        Location = "Midrand - Area 5",
                        Type = EventType.Announcement,
                        Priority = EventPriority.High
                    },
                    new MunicipalEvent
                    {
                        Title = "Youth Development Workshop",
                        Description = "Skills and CV writing workshop for local youth.",
                        Category = "Youth",
                        EventDate = DateTime.Now.Date.AddDays(10),
                        StartTime = new TimeSpan(9,0,0),
                        Location = "Kimberley Youth Centre",
                        Type = EventType.Event,
                        Priority = EventPriority.Low
                    },
                    new MunicipalEvent
                    {
                        Title = "Recycling Initiative: Cape Town Central",
                        Description = "Drop-off points for paper and plastics this weekend.",
                        Category = "Recycling",
                        EventDate = DateTime.Now.Date.AddDays(1),
                        StartTime = new TimeSpan(10,0,0),
                        Location = "Cape Town Civic Centre",
                        Type = EventType.Event,
                        Priority = EventPriority.Medium
                    },
                    new MunicipalEvent
                    {
                        Title = "Public Health Awareness: TB and HIV Testing",
                        Description = "Free testing and counselling.",
                        Category = "Health",
                        EventDate = DateTime.Now.Date.AddDays(7),
                        StartTime = new TimeSpan(8,0,0),
                        Location = "Durban Community Clinic",
                        Type = EventType.Event,
                        Priority = EventPriority.Medium
                    },
                    new MunicipalEvent
                    {
                        Title = "Municipal Budget Meeting - Public Participation",
                        Description = "Share your views on the 2026 municipal budget.",
                        Category = "Government",
                        EventDate = DateTime.Now.Date.AddDays(15),
                        StartTime = new TimeSpan(18,0,0),
                        Location = "Polokwane Town Hall",
                        Type = EventType.Event,
                        Priority = EventPriority.High
                    },
                    new MunicipalEvent
                    {
                        Title = "Job Fair: Informal Traders Support",
                        Description = "Local employers and training providers will be present.",
                        Category = "Employment",
                        EventDate = DateTime.Now.Date.AddDays(12),
                        StartTime = new TimeSpan(9,0,0),
                        Location = "Bloemfontein Sports Centre",
                        Type = EventType.Event,
                        Priority = EventPriority.Low
                    }
                };

                foreach (var ev in samples)
                {
                    _events.Add(ev);
                    IndexEvent(ev);
                }
            }
        }

        public static List<MunicipalEvent> Search(string? keyword, string? category, DateTime? fromDate, DateTime? toDate)
        {
            lock (_lock)
            {
                IEnumerable<MunicipalEvent> query = _events;

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    var k = keyword.Trim().ToLowerInvariant();
                    query = query.Where(e => e.Title.ToLowerInvariant().Contains(k) || e.Description.ToLowerInvariant().Contains(k));
                }

                if (!string.IsNullOrWhiteSpace(category) && category != "All")
                {
                    query = query.Where(e => string.Equals(e.Category, category, StringComparison.OrdinalIgnoreCase));
                }

                if (fromDate.HasValue)
                {
                    query = query.Where(e => e.EventDate.Date >= fromDate.Value.Date);
                }

                if (toDate.HasValue)
                {
                    query = query.Where(e => e.EventDate.Date <= toDate.Value.Date);
                }

                var results = query.OrderBy(e => e.EventDate).ThenBy(e => e.Title).ToList();
                return results;
            }
        }

        public static bool TryGetById(string id, out MunicipalEvent? ev)
        {
            if (string.IsNullOrWhiteSpace(id)) { ev = null; return false; }
            lock (_lock)
            {
                if (_dictionary.TryGetValue(id, out var item))
                {
                    ev = item;
                    return true;
                }
                ev = null;
                return false;
            }
        }

        public static Hashtable GetHashtableSnapshot()
        {
            lock (_lock)
            {
                return (Hashtable)_hashtable.Clone();
            }
        }

        public static Dictionary<string, MunicipalEvent> GetDictionarySnapshot()
        {
            lock (_lock)
            {
                return new Dictionary<string, MunicipalEvent>(_dictionary);
            }
        }

        public static SortedDictionary<DateTime, List<MunicipalEvent>> GetSortedByDateSnapshot()
        {
            lock (_lock)
            {
                return new SortedDictionary<DateTime, List<MunicipalEvent>>(_sortedByDate);
            }
        }

        public static Queue<MunicipalEvent> GetUpcomingQueueSnapshot()
        {
            lock (_lock)
            {
                return new Queue<MunicipalEvent>(_upcomingQueue);
            }
        }

        public static SimplePriorityQueue<MunicipalEvent> GetPriorityQueue()
        {
            return _priorityQueue;
        }

        public static HashSet<string> GetUniqueCategories()
        {
            lock (_lock)
            {
                return new HashSet<string>(_uniqueCategories);
            }
        }
    }
}
