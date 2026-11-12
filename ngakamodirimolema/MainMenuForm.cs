namespace ngakamodirimolema
{
    using System;
    using System.Windows.Forms;

    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
            // enable Local Events button at runtime to avoid designer changes
            try { btnLocalEvents.Enabled = true; btnLocalEvents.Click += btnLocalEvents_Click; btnServiceRequestStatus.Enabled = true; btnServiceRequestStatus.Click += btnServiceRequestStatus_Click; } catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenReportIssuesChild();
        }

        private void btnLocalEvents_Click(object sender, EventArgs e)
        {
            OpenLocalEventsChild();
        }

        private void OpenReportIssuesChild()
        {
            if (mainPanel == null)
                return;

            // dispose existing child controls
            foreach (Control c in mainPanel.Controls)
            {
                try { c.Dispose(); } catch { }
            }
            mainPanel.Controls.Clear();

            var child = new ReportIssuesForm();
            child.TopLevel = false;
            child.FormBorderStyle = FormBorderStyle.None;
            child.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(child);
            child.Show();
            Home.Text = "REPORT ISSUES";
        }

        private void OpenLocalEventsChild()
        {
            if (mainPanel == null)
                return;

            // dispose existing child controls
            foreach (Control c in mainPanel.Controls)
            {
                try { c.Dispose(); } catch { }
            }
            mainPanel.Controls.Clear();

            var child = new LocalEventsForm();
            child.TopLevel = false;
            child.FormBorderStyle = FormBorderStyle.None;
            child.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(child);
            child.Show();
            Home.Text = "LOCAL EVENTS";
        }

        private void btnServiceRequestStatus_Click(object? sender, EventArgs e)
        {
            if (mainPanel == null) return;
            foreach (Control c in mainPanel.Controls) { try { c.Dispose(); } catch { } }
            mainPanel.Controls.Clear();
            var child = new ServiceRequestStatusForm { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            mainPanel.Controls.Add(child); child.Show(); Home.Text = "SERVICE REQUEST STATUS";
        }

        private void logoPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
