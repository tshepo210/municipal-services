using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ngakamodirimolema
{
    public partial class LocalEventsForm : Form
    {
        // Shared across the application session
        private static readonly SearchHistoryService SharedHistory = new SearchHistoryService();

        private readonly RecommendationService _recommendationService;
        private Queue<MunicipalEvent> _localQueue;

        public LocalEventsForm()
        {
            InitializeComponent();
            _recommendationService = new RecommendationService(SharedHistory);
            _localQueue = EventRepository.GetUpcomingQueueSnapshot();
            InitializeControls();
            LoadAllEvents();
            UpdateRecommendations();
            UpdateDataStructureInfo();
        }

        private void InitializeControls()
        {
            // categories
            cmbCategoryFilter.Items.Clear();
            cmbCategoryFilter.Items.Add("All");
            var cats = EventRepository.GetUniqueCategories();
            foreach (var c in cats.OrderBy(x => x)) cmbCategoryFilter.Items.Add(c);
            cmbCategoryFilter.SelectedIndex = 0;

            dtFrom.Checked = false;
            dtTo.Checked = false;

            dgvEvents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEvents.MultiSelect = false;
            dgvEvents.AutoGenerateColumns = false;

            dgvEvents.Columns.Clear();
            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", DataPropertyName = "Id", Visible = false });
            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn { Name = "Title", HeaderText = "Title", DataPropertyName = "Title", Width = 200 });
            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn { Name = "Category", HeaderText = "Category", DataPropertyName = "Category" });
            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn { Name = "Date", HeaderText = "Date", DataPropertyName = "EventDate" });
            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn { Name = "Location", HeaderText = "Location", DataPropertyName = "Location" });
            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn { Name = "Type", HeaderText = "Type", DataPropertyName = "Type" });
            dgvEvents.Columns.Add(new DataGridViewTextBoxColumn { Name = "Priority", HeaderText = "Priority", DataPropertyName = "Priority" });

            dgvEvents.CellDoubleClick += DgvEvents_CellDoubleClick;
            lstRecommendations.SelectedIndexChanged += LstRecommendations_SelectedIndexChanged;
        }

        private void LoadAllEvents()
        {
            var all = EventRepository.GetAll();
            dgvEvents.DataSource = all.Select(e => new
            {
                e.Id,
                e.Title,
                e.Category,
                EventDate = e.EventDate.ToString("yyyy-MM-dd"),
                e.Location,
                Type = e.Type.ToString(),
                Priority = e.Priority.ToString()
            }).ToList();

            if (all.Count == 0) MessageBox.Show("No events available.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string? keyword = string.IsNullOrWhiteSpace(txtSearch.Text) ? null : txtSearch.Text.Trim();
            string? category = cmbCategoryFilter.SelectedItem?.ToString();
            DateTime? from = dtFrom.Checked ? dtFrom.Value.Date : (DateTime?)null;
            DateTime? to = dtTo.Checked ? dtTo.Value.Date : (DateTime?)null;

            if (from.HasValue && to.HasValue && from.Value.Date > to.Value.Date)
            {
                MessageBox.Show("From date must be before or equal to To date.", "Invalid dates", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var results = EventRepository.Search(keyword, category, from, to);

            dgvEvents.DataSource = results.Select(e => new
            {
                e.Id,
                e.Title,
                e.Category,
                EventDate = e.EventDate.ToString("yyyy-MM-dd"),
                e.Location,
                Type = e.Type.ToString(),
                Priority = e.Priority.ToString()
            }).ToList();

            // track search
            SharedHistory.Push(new SearchFilter { Keyword = keyword, Category = category, FromDate = from, ToDate = to });

            if (results.Count == 0)
            {
                MessageBox.Show("No events match the specified filters.", "No results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            UpdateRecommendations();
            UpdateDataStructureInfo();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbCategoryFilter.SelectedIndex = 0;
            dtFrom.Checked = false;
            dtTo.Checked = false;
            LoadAllEvents();
        }

        private void btnPrevSearch_Click(object sender, EventArgs e)
        {
            if (SharedHistory.TryPop(out var filter))
            {
                // reapply
                txtSearch.Text = filter.Keyword ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(filter.Category) && cmbCategoryFilter.Items.Contains(filter.Category))
                    cmbCategoryFilter.SelectedItem = filter.Category;
                else
                    cmbCategoryFilter.SelectedIndex = 0;

                if (filter.FromDate.HasValue) { dtFrom.Value = filter.FromDate.Value; dtFrom.Checked = true; } else dtFrom.Checked = false;
                if (filter.ToDate.HasValue) { dtTo.Value = filter.ToDate.Value; dtTo.Checked = true; } else dtTo.Checked = false;

                btnSearch_Click(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("No previous search in history.", "History", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DgvEvents_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var id = dgvEvents.Rows[e.RowIndex].Cells["Id"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(id)) return;
            if (EventRepository.TryGetById(id, out var ev))
            {
                // show details
                MessageBox.Show($"{ev.Title}\n\n{ev.Description}\n\nDate: {ev.EventDate:d}\nLocation: {ev.Location}", "Event details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // track as implicit interest
                SharedHistory.Push(new SearchFilter { Keyword = ev.Title, Category = ev.Category });
                UpdateRecommendations();
            }
        }

        private void btnEnqueue_Click(object sender, EventArgs e)
        {
            if (dgvEvents.SelectedRows.Count == 0) { MessageBox.Show("Select an event to enqueue.", "Queue", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            var id = dgvEvents.SelectedRows[0].Cells["Id"].Value?.ToString();
            if (id == null) return;
            if (EventRepository.TryGetById(id, out var ev))
            {
                _localQueue.Enqueue(ev);
                MessageBox.Show($"Enqueued: {ev.Title}", "Queue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateDataStructureInfo();
            }
        }

        private void btnDequeue_Click(object sender, EventArgs e)
        {
            if (_localQueue.Count == 0) { MessageBox.Show("Queue is empty.", "Queue", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            var ev = _localQueue.Dequeue();
            MessageBox.Show($"Dequeued: {ev.Title}", "Queue", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateDataStructureInfo();
        }

        private void btnProcessPriority_Click(object sender, EventArgs e)
        {
            var pq = EventRepository.GetPriorityQueue();
            if (pq.TryDequeue(out var ev))
            {
                MessageBox.Show($"Processed priority item: {ev.Title} (Priority: {ev.Priority})", "Priority Queue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateDataStructureInfo();
            }
            else
            {
                MessageBox.Show("No items in priority queue.", "Priority Queue", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateRecommendations()
        {
            lstRecommendations.Items.Clear();
            var recs = _recommendationService.Recommend(5);
            if (recs.Count == 0)
            {
                lstRecommendations.Items.Add("No personalised recommendations yet. Try searching for categories or keywords.");
                return;
            }

            foreach (var r in recs)
            {
                var s = $"{r.Event.Title} - {r.Event.Category} - {r.Event.EventDate:d}";
                lstRecommendations.Items.Add(s);
            }

            // store reasons in Tag for convenience
            lstRecommendations.Tag = recs;
        }

        private void LstRecommendations_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (lstRecommendations.SelectedIndex < 0) { lblRecommendationReason.Text = string.Empty; return; }
            var recs = lstRecommendations.Tag as List<Recommendation>;
            if (recs == null || lstRecommendations.SelectedIndex >= recs.Count) return;
            lblRecommendationReason.Text = recs[lstRecommendations.SelectedIndex].Reason;
        }

        private void UpdateDataStructureInfo()
        {
            var ht = EventRepository.GetHashtableSnapshot();
            var dict = EventRepository.GetDictionarySnapshot();
            var sd = EventRepository.GetSortedByDateSnapshot();

            lblHashtableCount.Text = $"Hashtable entries: {ht.Count}";
            lblDictionaryCount.Text = $"Dictionary entries: {dict.Count}";
            lblSortedDictionaryCount.Text = $"SortedDictionary dates: {sd.Count}";
            lblQueueCount.Text = $"Local queue: {_localQueue.Count}";
            lblPriorityQueueCount.Text = $"Priority queue approx: {EventRepository.GetPriorityQueue().Count}";
        }
    }
}
