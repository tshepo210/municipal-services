using System;
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
        private Label lblRecommendationReason;
        private Label lblHashtableCount;
        private Label lblDictionaryCount;
        private Label lblSortedDictionaryCount;
        private Label lblQueueCount;
        private Label lblPriorityQueueCount;

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
            lblRecommendationReason = new Label();
            lblHashtableCount = new Label();
            lblDictionaryCount = new Label();
            lblSortedDictionaryCount = new Label();
            lblQueueCount = new Label();
            lblPriorityQueueCount = new Label();

            SuspendLayout();

            // txtSearch
            txtSearch.PlaceholderText = "Search title or keywords";
            txtSearch.Location = new System.Drawing.Point(16, 16);
            txtSearch.Width = 220;

            // cmbCategoryFilter
            cmbCategoryFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoryFilter.Location = new System.Drawing.Point(248, 16);
            cmbCategoryFilter.Width = 180;

            // dtFrom
            dtFrom.Format = DateTimePickerFormat.Short;
            dtFrom.Location = new System.Drawing.Point(448, 16);
            dtFrom.Width = 110;

            // dtTo
            dtTo.Format = DateTimePickerFormat.Short;
            dtTo.Location = new System.Drawing.Point(568, 16);
            dtTo.Width = 110;

            // btnSearch
            btnSearch.Text = "Search";
            btnSearch.Location = new System.Drawing.Point(692, 12);
            btnSearch.Click += btnSearch_Click;

            // btnClear
            btnClear.Text = "Clear Filters";
            btnClear.Location = new System.Drawing.Point(772, 12);
            btnClear.Click += btnClear_Click;

            // btnPrevSearch
            btnPrevSearch.Text = "Previous Search";
            btnPrevSearch.Location = new System.Drawing.Point(16, 48);
            btnPrevSearch.Click += btnPrevSearch_Click;

            // dgvEvents
            dgvEvents.Location = new System.Drawing.Point(16, 84);
            dgvEvents.Size = new System.Drawing.Size(720, 300);

            // enqueue/dequeue
            btnEnqueue.Text = "Enqueue";
            btnEnqueue.Location = new System.Drawing.Point(752, 84);
            btnEnqueue.Click += btnEnqueue_Click;

            btnDequeue.Text = "Dequeue";
            btnDequeue.Location = new System.Drawing.Point(752, 120);
            btnDequeue.Click += btnDequeue_Click;

            btnProcessPriority.Text = "Process Priority";
            btnProcessPriority.Location = new System.Drawing.Point(752, 156);
            btnProcessPriority.Click += btnProcessPriority_Click;

            // recommendations
            lstRecommendations.Location = new System.Drawing.Point(16, 400);
            lstRecommendations.Size = new System.Drawing.Size(480, 100);

            lblRecommendationReason.Location = new System.Drawing.Point(512, 400);
            lblRecommendationReason.Size = new System.Drawing.Size(320, 100);
            lblRecommendationReason.Text = "";

            // data structure labels
            lblHashtableCount.Location = new System.Drawing.Point(512, 84);
            lblHashtableCount.Size = new System.Drawing.Size(320, 20);

            lblDictionaryCount.Location = new System.Drawing.Point(512, 108);
            lblDictionaryCount.Size = new System.Drawing.Size(320, 20);

            lblSortedDictionaryCount.Location = new System.Drawing.Point(512, 132);
            lblSortedDictionaryCount.Size = new System.Drawing.Size(320, 20);

            lblQueueCount.Location = new System.Drawing.Point(512, 156);
            lblQueueCount.Size = new System.Drawing.Size(320, 20);

            lblPriorityQueueCount.Location = new System.Drawing.Point(512, 180);
            lblPriorityQueueCount.Size = new System.Drawing.Size(320, 20);

            // LocalEventsForm
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(txtSearch);
            Controls.Add(cmbCategoryFilter);
            Controls.Add(dtFrom);
            Controls.Add(dtTo);
            Controls.Add(btnSearch);
            Controls.Add(btnClear);
            Controls.Add(btnPrevSearch);
            Controls.Add(dgvEvents);
            Controls.Add(btnEnqueue);
            Controls.Add(btnDequeue);
            Controls.Add(btnProcessPriority);
            Controls.Add(lstRecommendations);
            Controls.Add(lblRecommendationReason);
            Controls.Add(lblHashtableCount);
            Controls.Add(lblDictionaryCount);
            Controls.Add(lblSortedDictionaryCount);
            Controls.Add(lblQueueCount);
            Controls.Add(lblPriorityQueueCount);

            Name = "LocalEventsForm";
            Text = "Local Events and Announcements";
            Size = new System.Drawing.Size(860, 520);

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
