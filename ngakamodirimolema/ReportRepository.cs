using System.Collections.Generic;

namespace ngakamodirimolema
{
    public static class ReportRepository
    {
        private static readonly List<ReportData> _reports = new List<ReportData>();
        private static readonly object _lock = new object();

        public static void Add(ReportData report)
        {
            if (report == null) return;
            lock (_lock)
            {
                _reports.Add(report);
            }
        }

        public static IReadOnlyList<ReportData> GetAll()
        {
            lock (_lock)
            {
                return _reports.AsReadOnly();
            }
        }

        public static void Clear()
        {
            lock (_lock)
            {
                _reports.Clear();
            }
        }
    }
}
