namespace ngakamodirimolema
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenReportIssuesChild();
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
        }

        private void logoPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
