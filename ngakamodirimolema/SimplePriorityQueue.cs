using System;
using System.Collections.Generic;

namespace ngakamodirimolema
{
    // Simple priority queue compatible with .NET versions without PriorityQueue<T,P>
    // Lower numeric priority value indicates higher priority (1 = High)
    public class SimplePriorityQueue<T>
    {
        private readonly SortedDictionary<int, Queue<T>> _dict = new();
        private readonly object _lock = new object();

        public void Enqueue(T item, int priority)
        {
            lock (_lock)
            {
                if (!_dict.TryGetValue(priority, out var q))
                {
                    q = new Queue<T>();
                    _dict.Add(priority, q);
                }
                q.Enqueue(item);
            }
        }

        public bool TryDequeue(out T? item)
        {
            lock (_lock)
            {
                foreach (var kvp in _dict)
                {
                    var q = kvp.Value;
                    if (q.Count > 0)
                    {
                        item = q.Dequeue();
                        if (q.Count == 0)
                            _dict.Remove(kvp.Key);
                        return true;
                    }
                }
            }
            item = default;
            return false;
        }

        public int Count
        {
            get
            {
                int c = 0;
                lock (_lock)
                {
                    foreach (var q in _dict.Values) c += q.Count;
                }
                return c;
            }
        }
    }
}
