using System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
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
            // apply visual styling before control population
            ApplyModernStyling();
            _recommendationService = new RecommendationService(SharedHistory);
            _localQueue = EventRepository.GetUpcomingQueueSnapshot();
            InitializeControls();
            LoadAllEvents();
            UpdateRecommendations();
            UpdateDataStructureInfo();
        }

        // ApplyModernStyling: centralises UI visual tweaks to keep Designer changes minimal
        private void ApplyModernStyling()
        {
            // reuse app colours from MainMenuForm
            var sidebarColor = Color.FromArgb(51, 51, 76);
            var teal = Color.FromArgb(0, 150, 136);

            // DataGridView base styling
            dgvEvents.BackgroundColor = Color.White;
            dgvEvents.BorderStyle = BorderStyle.None;
            dgvEvents.EnableHeadersVisualStyles = false;
            dgvEvents.RowHeadersVisible = false;
            dgvEvents.AllowUserToResizeRows = false;
            dgvEvents.ReadOnly = true;
            dgvEvents.MultiSelect = false;
            dgvEvents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEvents.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvEvents.GridColor = Color.FromArgb(230, 230, 230);
            dgvEvents.RowTemplate.Height = 36;
            dgvEvents.RowTemplate.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            dgvEvents.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvEvents.ColumnHeadersDefaultCellStyle.BackColor = teal;
            dgvEvents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEvents.ColumnHeadersDefaultCellStyle.Font = new Font(dgvEvents.Font, FontStyle.Bold);
            dgvEvents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // selection color - soft teal
            var selStyle = new DataGridViewCellStyle();
            selStyle.BackColor = Color.FromArgb(200, 240, 235);
            selStyle.ForeColor = Color.Black;
            dgvEvents.DefaultCellStyle.SelectionBackColor = selStyle.BackColor;
            dgvEvents.DefaultCellStyle.SelectionForeColor = selStyle.ForeColor;

            // owner-draw recommendations list (keeps the control name)
            lstRecommendations.DrawMode = DrawMode.OwnerDrawVariable;
            lstRecommendations.MeasureItem += LstRecommendations_MeasureItem;
            lstRecommendations.DrawItem += LstRecommendations_DrawItem;

            // double-buffer the grid to reduce flicker (uses protected property)
            try
            {
                typeof(DataGridView).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(dgvEvents, true, null);
            }
            catch { /* ignore if not allowed */ }

            // databinding complete toggles empty state overlay and adjusts columns
            dgvEvents.DataBindingComplete += DgvEvents_DataBindingComplete;
            dgvEvents.CellFormatting += DgvEvents_CellFormatting;

            // style buttons consistently
            StyleButtonPrimary(btnSearch, teal, Color.White);
            StyleButtonSecondary(btnClear, teal);
            StyleButtonNeutral(btnPrevSearch);
            StyleButtonNeutral(btnEnqueue);
            StyleButtonNeutral(btnDequeue);
            StyleButtonPrimary(btnProcessPriority, Color.FromArgb(32, 120, 180), Color.White);

            // search AcceptButton
            AcceptButton = btnSearch;
        }

        // Button styling helpers
        private void StyleButtonPrimary(Button btn, Color back, Color fore)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = back;
            btn.ForeColor = fore;
            btn.Height = 36;
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(8, 0, 8, 0);
            // simple hover effect
            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(back);
            btn.MouseLeave += (s, e) => btn.BackColor = back;
        }

        private void StyleButtonSecondary(Button btn, Color accent)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = accent;
            btn.BackColor = Color.White;
            btn.ForeColor = accent;
            btn.Height = 36;
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(8, 0, 8, 0);
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(245, 245, 245);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.White;
        }

        private void StyleButtonNeutral(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.FromArgb(240, 240, 240);
            btn.ForeColor = Color.Black;
            btn.Height = 34;
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(8, 0, 8, 0);
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(230, 230, 230);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(240, 240, 240);
        }

        // Toggle empty state and adjust column fill weights
        private void DgvEvents_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                var count = dgvEvents.Rows.Cast<DataGridViewRow>().Count(r => !r.IsNewRow);
                // show overlay label when empty
                if (count == 0)
                {
                    lblEmptyState.Text = "No events match your search. Try changing or clearing your filters.";
                    lblEmptyState.Visible = true;
                }
                else
                {
                    lblEmptyState.Visible = false;
                }

                // ensure sensible column sizing
                if (dgvEvents.Columns.Count > 0)
                {
                    foreach (DataGridViewColumn col in dgvEvents.Columns) col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    // assign fill weights
                    if (dgvEvents.Columns.Contains("Title")) dgvEvents.Columns["Title"].FillWeight = 45;
                    if (dgvEvents.Columns.Contains("Date")) dgvEvents.Columns["Date"].FillWeight = 12;
                    if (dgvEvents.Columns.Contains("Category")) dgvEvents.Columns["Category"].FillWeight = 12;
                    if (dgvEvents.Columns.Contains("Location")) dgvEvents.Columns["Location"].FillWeight = 18;
                    if (dgvEvents.Columns.Contains("Type")) dgvEvents.Columns["Type"].FillWeight = 8;
                    if (dgvEvents.Columns.Contains("Priority")) dgvEvents.Columns["Priority"].FillWeight = 8;
                }
            }
            catch { }
        }

        // CellFormatting for priority and type colouring
        private void DgvEvents_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                var colName = dgvEvents.Columns[e.ColumnIndex].Name;
                if (colName == "Priority" && e.Value != null)
                {
                    var s = e.Value.ToString();
                    if (s == "High") e.CellStyle.ForeColor = Color.FromArgb(200, 50, 50);
                    else if (s == "Medium") e.CellStyle.ForeColor = Color.FromArgb(200, 130, 30);
                    else if (s == "Low") e.CellStyle.ForeColor = Color.FromArgb(30, 130, 60);
                }
                if (colName == "Type" && e.Value != null)
                {
                    var s = e.Value.ToString();
                    if (s == "Announcement") e.CellStyle.ForeColor = Color.FromArgb(90, 90, 90);
                }
            }
            catch { }
        }

        // Owner-draw recommendations list items as cards
        private void LstRecommendations_MeasureItem(object? sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 56;
        }

        private void LstRecommendations_DrawItem(object? sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index < 0) return;

            var g = e.Graphics;
            var recs = lstRecommendations.Tag as List<Recommendation>;

            string title = lstRecommendations.Items[e.Index]?.ToString() ?? string.Empty;
            string reason = string.Empty;
            string meta = string.Empty;

            if (recs != null && e.Index < recs.Count)
            {
                var r = recs[e.Index];
                title = r.Event.Title;
                meta = $"{r.Event.Category} • {r.Event.EventDate:dd/MM/yyyy} • {r.Event.Location}";
                reason = r.Reason;
            }
            else
            {
                // fallback to existing item text
                meta = string.Empty;
            }

            var rect = e.Bounds;
            var titleFont = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
            var metaFont = new Font(Font.FontFamily, Font.Size - 1, FontStyle.Regular);
            var reasonFont = new Font(Font.FontFamily, Font.Size - 1, FontStyle.Italic);

            using (titleFont)
            using (metaFont)
            using (reasonFont)
            using (var brush = new SolidBrush(e.ForeColor))
            {
                g.DrawString(title, titleFont, brush, rect.Left + 6, rect.Top + 4);
                g.DrawString(meta, metaFont, new SolidBrush(Color.Gray), rect.Left + 6, rect.Top + 22);
                if (!string.IsNullOrWhiteSpace(reason)) g.DrawString(reason, reasonFont, new SolidBrush(Color.DarkGray), rect.Left + 6, rect.Top + 38);
            }
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
                EventDate = e.EventDate.ToString("dd/MM/yyyy"),
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
                EventDate = e.EventDate.ToString("dd/MM/yyyy"),
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
                var s = $"{r.Event.Title} - {r.Event.Category} - {r.Event.EventDate:dd/MM/yyyy} - {r.Event.Location}";
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
