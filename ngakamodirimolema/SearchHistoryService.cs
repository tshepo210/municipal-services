using System;
using System.Collections.Generic;

namespace ngakamodirimolema
{
    public class SearchFilter
    {
        public string? Keyword { get; set; }
        public string? Category { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class SearchHistoryService
    {
        private readonly Stack<SearchFilter> _history = new();
        private readonly object _lock = new();

        // tracking for recommendations
        public readonly Dictionary<string, int> CategoryCounts = new(StringComparer.OrdinalIgnoreCase);
        public readonly Dictionary<string, int> KeywordCounts = new(StringComparer.OrdinalIgnoreCase);

        public void Push(SearchFilter f)
        {
            if (f == null) return;
            lock (_lock)
            {
                _history.Push(f);

                if (!string.IsNullOrWhiteSpace(f.Category))
                {
                    if (!CategoryCounts.ContainsKey(f.Category!)) CategoryCounts[f.Category!] = 0;
                    CategoryCounts[f.Category!]++;
                }

                if (!string.IsNullOrWhiteSpace(f.Keyword))
                {
                    var words = f.Keyword!.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var w in words)
                    {
                        var k = w.Trim();
                        if (k.Length == 0) continue;
                        if (!KeywordCounts.ContainsKey(k)) KeywordCounts[k] = 0;
                        KeywordCounts[k]++;
                    }
                }
            }
        }

        public bool TryPop(out SearchFilter? f)
        {
            lock (_lock)
            {
                if (_history.Count > 0)
                {
                    f = _history.Pop();
                    return true;
                }
                f = null;
                return false;
            }
        }

        public IReadOnlyCollection<SearchFilter> GetHistorySnapshot()
        {
            lock (_lock)
            {
                return _history.ToArray();
            }
        }
    }
}
