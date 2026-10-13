using System;
using System.Drawing;
using System.Windows.Forms;

namespace ngakamodirimolema
{
    partial class LocalEventsForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvEvents;
        private TextBox txtSearch;
        private ComboBox cmbCategoryFilter;
        private Button btnSearch;
        private Button btnClear;
        private DateTimePicker dtFrom;
        private DateTimePicker dtTo;
        private Button btnPrevSearch;
        private Button btnEnqueue;
        private Button btnDequeue;
        private Button btnProcessPriority;
        private ListBox lstRecommendations;
        private Label lblEmptyState;
        private TableLayoutPanel contentTable;
        private Panel pnlFilterBar;
        private Panel pnlActionBar;
        private Panel pnlResults;
        private Panel pnlRecommendations;
        private Label lblRecommendationReason;
        private Label lblHashtableCount;
        private Label lblDictionaryCount;
        private Label lblSortedDictionaryCount;
        private Label lblQueueCount;
        private Label lblPriorityQueueCount;
        private Label lblRecommendedHeader;
        private ToolTip toolTip1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            dgvEvents = new DataGridView();
            txtSearch = new TextBox();
            cmbCategoryFilter = new ComboBox();
            btnSearch = new Button();
            btnClear = new Button();
            dtFrom = new DateTimePicker();
            dtTo = new DateTimePicker();
            btnPrevSearch = new Button();
            btnEnqueue = new Button();
            btnDequeue = new Button();
            btnProcessPriority = new Button();
            lstRecommendations = new ListBox();
            lblEmptyState = new Label();
            contentTable = new TableLayoutPanel();
            pnlFilterBar = new Panel();
            pnlActionBar = new Panel();
            pnlResults = new Panel();
            pnlRecommendations = new Panel();
            lblRecommendationReason = new Label();
            lblHashtableCount = new Label();
            lblDictionaryCount = new Label();
            lblSortedDictionaryCount = new Label();
            lblQueueCount = new Label();
            lblPriorityQueueCount = new Label();
            lblRecommendedHeader = new Label();
            toolTip1 = new ToolTip(components);

            // helper layout containers (children of the panels above)
            var tlpFilter = new TableLayoutPanel();
            var tlpActions = new TableLayoutPanel();
            var flowInfo = new FlowLayoutPanel();
            var lblSearchCaption = new Label();
            var lblCategoryCaption = new Label();
            var lblFromCaption = new Label();
            var lblToCaption = new Label();
            var lblQueueCaption = new Label();

            SuspendLayout();

            
            // contentTable: 5 rows -> filters / actions / grid / info / recommendations            
            contentTable.Dock = DockStyle.Fill;
            contentTable.Padding = new Padding(12);
            contentTable.ColumnCount = 1;
            contentTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            contentTable.RowCount = 5;
            contentTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));   // filter bar
            contentTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));   // action bar
            contentTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));   // results grid
            contentTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));   // data structure info
            contentTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 190F));  // recommendations

            
            // Filter bar: captions on row 0, inputs on row 1
            pnlFilterBar.Dock = DockStyle.Fill;
            pnlFilterBar.Margin = new Padding(0);

            tlpFilter.Dock = DockStyle.Fill;
            tlpFilter.ColumnCount = 6;
            tlpFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F)); // search grows
            tlpFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F)); // category
            tlpFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F)); // from
            tlpFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F)); // to
            tlpFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F)); // search btn
            tlpFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F)); // clear btn
            tlpFilter.RowCount = 2;
            tlpFilter.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tlpFilter.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tlpFilter.Margin = new Padding(0);

            ConfigureCaption(lblSearchCaption, "Search");
            ConfigureCaption(lblCategoryCaption, "Category");
            ConfigureCaption(lblFromCaption, "From date");
            ConfigureCaption(lblToCaption, "To date");

            txtSearch.PlaceholderText = "Search title or keywords";
            txtSearch.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.Margin = new Padding(0, 7, 8, 0);

            cmbCategoryFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoryFilter.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbCategoryFilter.Margin = new Padding(0, 7, 8, 0);

            dtFrom.Format = DateTimePickerFormat.Short;
            dtFrom.ShowCheckBox = true;
            dtFrom.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            dtFrom.Margin = new Padding(0, 7, 8, 0);

            dtTo.Format = DateTimePickerFormat.Short;
            dtTo.ShowCheckBox = true;
            dtTo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            dtTo.Margin = new Padding(0, 7, 8, 0);

            btnSearch.Text = "Search";
            btnSearch.Dock = DockStyle.Fill;
            btnSearch.Margin = new Padding(0, 0, 8, 2);
            btnSearch.Click += btnSearch_Click;
            toolTip1.SetToolTip(btnSearch, "Filter events by keyword, category and date range");

            btnClear.Text = "Clear Filters";
            btnClear.Dock = DockStyle.Fill;
            btnClear.Margin = new Padding(0, 0, 0, 2);
            btnClear.Click += btnClear_Click;
            toolTip1.SetToolTip(btnClear, "Reset all search filters and show all events");

            tlpFilter.Controls.Add(lblSearchCaption, 0, 0);
            tlpFilter.Controls.Add(lblCategoryCaption, 1, 0);
            tlpFilter.Controls.Add(lblFromCaption, 2, 0);
            tlpFilter.Controls.Add(lblToCaption, 3, 0);
            tlpFilter.Controls.Add(txtSearch, 0, 1);
            tlpFilter.Controls.Add(cmbCategoryFilter, 1, 1);
            tlpFilter.Controls.Add(dtFrom, 2, 1);
            tlpFilter.Controls.Add(dtTo, 3, 1);
            tlpFilter.Controls.Add(btnSearch, 4, 1);
            tlpFilter.Controls.Add(btnClear, 5, 1);
            pnlFilterBar.Controls.Add(tlpFilter);

            
            // Action bar: Previous (stack) on the left, queue buttons on the right  
            pnlActionBar.Dock = DockStyle.Fill;
            pnlActionBar.Margin = new Padding(0);

            tlpActions.Dock = DockStyle.Fill;
            tlpActions.ColumnCount = 6;
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F)); // previous
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));  // spacer
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F)); // caption
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F)); // enqueue
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F)); // dequeue
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F)); // process
            tlpActions.RowCount = 1;
            tlpActions.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpActions.Margin = new Padding(0);

            btnPrevSearch.Text = "Previous";
            btnPrevSearch.Dock = DockStyle.Fill;
            btnPrevSearch.Margin = new Padding(0, 6, 0, 6);
            btnPrevSearch.Click += btnPrevSearch_Click;
            toolTip1.SetToolTip(btnPrevSearch, "Reapply the most recent search (LIFO)");

            lblQueueCaption.Text = "Event queue:";
            lblQueueCaption.Dock = DockStyle.Fill;
            lblQueueCaption.TextAlign = ContentAlignment.MiddleRight;
            lblQueueCaption.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblQueueCaption.Margin = new Padding(0, 0, 8, 0);

            btnEnqueue.Text = "Enqueue";
            btnEnqueue.Dock = DockStyle.Fill;
            btnEnqueue.Margin = new Padding(0, 6, 8, 6);
            btnEnqueue.Click += btnEnqueue_Click;
            toolTip1.SetToolTip(btnEnqueue, "Add selected event to the local FIFO queue");

            btnDequeue.Text = "Dequeue";
            btnDequeue.Dock = DockStyle.Fill;
            btnDequeue.Margin = new Padding(0, 6, 8, 6);
            btnDequeue.Click += btnDequeue_Click;
            toolTip1.SetToolTip(btnDequeue, "Remove the next event from the queue (FIFO)");

            btnProcessPriority.Text = "Process Priority";
            btnProcessPriority.Dock = DockStyle.Fill;
            btnProcessPriority.Margin = new Padding(0, 6, 0, 6);
            btnProcessPriority.Click += btnProcessPriority_Click;
            toolTip1.SetToolTip(btnProcessPriority, "Process the highest-priority announcement first");

            tlpActions.Controls.Add(btnPrevSearch, 0, 0);
            tlpActions.Controls.Add(lblQueueCaption, 2, 0);
            tlpActions.Controls.Add(btnEnqueue, 3, 0);
            tlpActions.Controls.Add(btnDequeue, 4, 0);
            tlpActions.Controls.Add(btnProcessPriority, 5, 0);
            pnlActionBar.Controls.Add(tlpActions);

            
            // Results: grid fills the panel; empty-state label overlays it
            // (first control added = front of z-order)
            pnlResults.Dock = DockStyle.Fill;
            pnlResults.Margin = new Padding(0);

            dgvEvents.Dock = DockStyle.Fill;
            dgvEvents.ReadOnly = true;
            dgvEvents.AllowUserToAddRows = false;
            dgvEvents.AllowUserToDeleteRows = false;
            dgvEvents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEvents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEvents.Margin = new Padding(0);

            lblEmptyState.Text = string.Empty;
            lblEmptyState.Dock = DockStyle.Fill;
            lblEmptyState.TextAlign = ContentAlignment.MiddleCenter;
            lblEmptyState.BackColor = Color.White;
            lblEmptyState.ForeColor = Color.FromArgb(110, 110, 110);
            lblEmptyState.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
            lblEmptyState.Visible = false;

            pnlResults.Controls.Add(lblEmptyState);
            pnlResults.Controls.Add(dgvEvents);

            
            // Data structure info strip (single row under the grid)   
            flowInfo.Dock = DockStyle.Fill;
            flowInfo.FlowDirection = FlowDirection.LeftToRight;
            flowInfo.WrapContents = false;
            flowInfo.AutoScroll = false;
            flowInfo.Margin = new Padding(0);

            foreach (var lbl in new[] { lblHashtableCount, lblDictionaryCount, lblSortedDictionaryCount, lblQueueCount, lblPriorityQueueCount })
            {
                lbl.AutoSize = true;
                lbl.ForeColor = Color.FromArgb(90, 90, 90);
                lbl.Font = new Font("Segoe UI", 8.5F);
                lbl.Margin = new Padding(0, 8, 24, 0);
                flowInfo.Controls.Add(lbl);
            }

            
            pnlRecommendations.Dock = DockStyle.Fill;
            pnlRecommendations.Margin = new Padding(0);
            pnlRecommendations.Padding = new Padding(0, 4, 0, 0);

            lstRecommendations.Dock = DockStyle.Fill;
            lstRecommendations.BorderStyle = BorderStyle.None;
            lstRecommendations.ScrollAlwaysVisible = false;

            lblRecommendationReason.Dock = DockStyle.Bottom;
            lblRecommendationReason.Height = 26;
            lblRecommendationReason.TextAlign = ContentAlignment.MiddleLeft;
            lblRecommendationReason.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblRecommendationReason.ForeColor = Color.FromArgb(0, 120, 110);
            lblRecommendationReason.Text = string.Empty;

            lblRecommendedHeader.Text = "Recommended for you";
            lblRecommendedHeader.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblRecommendedHeader.Dock = DockStyle.Top;
            lblRecommendedHeader.Height = 28;
            lblRecommendedHeader.TextAlign = ContentAlignment.MiddleLeft;

            pnlRecommendations.Controls.Add(lstRecommendations);
            pnlRecommendations.Controls.Add(lblRecommendationReason);
            pnlRecommendations.Controls.Add(lblRecommendedHeader);

            
            // Assemble
            contentTable.Controls.Add(pnlFilterBar, 0, 0);
            contentTable.Controls.Add(pnlActionBar, 0, 1);
            contentTable.Controls.Add(pnlResults, 0, 2);
            contentTable.Controls.Add(flowInfo, 0, 3);
            contentTable.Controls.Add(pnlRecommendations, 0, 4);

            AutoScaleMode = AutoScaleMode.Font;
            MinimumSize = new Size(800, 560);
            StartPosition = FormStartPosition.CenterParent;
            Controls.Add(contentTable);
            Name = "LocalEventsForm";
            Text = "Local Events and Announcements";

            ResumeLayout(false);
            PerformLayout();
        }

        // small helper for the grey captions above the filter inputs
        private static void ConfigureCaption(Label lbl, string text)
        {
            lbl.Text = text;
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.BottomLeft;
            lbl.ForeColor = Color.FromArgb(90, 90, 90);
            lbl.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lbl.Margin = new Padding(0);
        }
    }
}