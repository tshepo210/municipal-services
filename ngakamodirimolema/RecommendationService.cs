using System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ngakamodirimolema
{
    public class Recommendation
    {
        public MunicipalEvent Event { get; set; } = null!;
        public double Score { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public class RecommendationService
    {
        private readonly SearchHistoryService _history;

        public RecommendationService(SearchHistoryService history)
        {
            _history = history ?? throw new ArgumentNullException(nameof(history));
        }

        // Weights for scoring (sum to 1.0)
        private const double CategoryWeight = 0.4;
        private const double KeywordWeight = 0.3;
        private const double SearchInterestWeight = 0.2;
        private const double DateWeight = 0.1;

        // Produce top N recommendations based on current history and repository
        public List<Recommendation> Recommend(int topN = 3)
        {
            var all = EventRepository.GetAll().Where(e => !e.IsExpired()).ToList();
            if (all.Count == 0) return new List<Recommendation>();

            // compute category preference scores
            var totalCategorySearches = _history.CategoryCounts.Values.Sum();

            var recs = new List<Recommendation>();
            foreach (var ev in all)
            {
                double catScore = 0.0;
                if (!string.IsNullOrWhiteSpace(ev.Category) && _history.CategoryCounts.TryGetValue(ev.Category, out var ccount) && totalCategorySearches > 0)
                {
                    catScore = (double)ccount / totalCategorySearches; // 0..1
                }

                double keywordScore = 0.0;
                if (_history.KeywordCounts.Count > 0)
                {
                    // compute overlap between keywords and event title/description
                    foreach (var kw in _history.KeywordCounts.Keys)
                    {
                        if (ev.Title.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0 || ev.Description.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            keywordScore += 1.0;
                        }
                    }
                    // normalize
                    keywordScore = Math.Min(1.0, keywordScore / Math.Max(1, _history.KeywordCounts.Count));
                }

                double searchInterestScore = 0.0;
                if (!string.IsNullOrWhiteSpace(ev.Category) && _history.CategoryCounts.TryGetValue(ev.Category, out var icount))
                {
                    // modest boost for categories the user searched often
                    searchInterestScore = Math.Min(1.0, icount / 5.0);
                }

                double dateScore = 0.0;
                var daysUntil = (ev.EventDate.Date - DateTime.Now.Date).TotalDays;
                if (daysUntil >= 0 && daysUntil <= 30) dateScore = 1.0; // upcoming within 30 days
                else if (daysUntil > 30 && daysUntil <= 90) dateScore = 0.5;

                double final = (CategoryWeight * catScore) + (KeywordWeight * keywordScore) + (SearchInterestWeight * searchInterestScore) + (DateWeight * dateScore);

                if (final > 0)
                {
                    var reasonParts = new List<string>();
                    if (catScore > 0) reasonParts.Add($"Matches category you search frequently ({ev.Category})");
                    if (keywordScore > 0) reasonParts.Add($"Title/description matches your recent keywords");
                    if (dateScore > 0) reasonParts.Add($"Upcoming in {Math.Max(0,(ev.EventDate.Date - DateTime.Now.Date).Days)} days");

                    recs.Add(new Recommendation { Event = ev, Score = final, Reason = string.Join("; ", reasonParts) });
                }
            }

            // If we have insufficient personalized recommendations, fall back to top priority upcoming events
            if (recs.Count == 0)
            {
                var fallback = EventRepository.GetAll().Where(e => !e.IsExpired()).OrderBy(e => e.Priority).ThenBy(e => e.EventDate).Take(topN);
                foreach (var ev in fallback)
                {
                    recs.Add(new Recommendation { Event = ev, Score = 0.0, Reason = "Fallback: upcoming or high-priority event" });
                }
            }

            return recs.OrderByDescending(r => r.Score).ThenBy(r => r.Event.EventDate).Take(topN).ToList();
        }
    }
}
