namespace ngakamodirimolema
{
    partial class MainMenuForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnReportIssues = new Button();
            sideBarPanel = new Panel();
            btnLocalEvents = new Button();
            btnServiceRequestStatus = new Button();
            logoPanel = new Panel();
            panelTitleBar = new Panel();
            Home = new Label();
            mainPanel = new Panel();
            sideBarPanel.SuspendLayout();
            panelTitleBar.SuspendLayout();
            SuspendLayout();

            // btnReportIssues
            btnReportIssues.FlatAppearance.BorderSize = 0;
            btnReportIssues.FlatStyle = FlatStyle.Flat;
            btnReportIssues.ForeColor = Color.Gainsboro;
            btnReportIssues.Location = new Point(0, 77);
            btnReportIssues.Name = "btnReportIssues";
            btnReportIssues.Padding = new Padding(12, 0, 0, 0);
            btnReportIssues.Size = new Size(220, 60);
            btnReportIssues.TabIndex = 0;
            btnReportIssues.Text = "Report Issues";
            btnReportIssues.TextAlign = ContentAlignment.MiddleLeft;
            btnReportIssues.UseVisualStyleBackColor = true;
            btnReportIssues.Click += button1_Click;

            // sideBarPanel
            sideBarPanel.BackColor = Color.FromArgb(51, 51, 76);
            sideBarPanel.Controls.Add(btnLocalEvents);
            sideBarPanel.Controls.Add(btnServiceRequestStatus);
            sideBarPanel.Controls.Add(logoPanel);
            sideBarPanel.Controls.Add(btnReportIssues);
            sideBarPanel.Dock = DockStyle.Left;
            sideBarPanel.Location = new Point(0, 0);
            sideBarPanel.Name = "sideBarPanel";
            sideBarPanel.Size = new Size(220, 450);
            sideBarPanel.TabIndex = 0;

            // btnLocalEvents
            btnLocalEvents.Enabled = false;
            btnLocalEvents.FlatAppearance.BorderSize = 0;
            btnLocalEvents.FlatStyle = FlatStyle.Flat;
            btnLocalEvents.ForeColor = Color.Gainsboro;
            btnLocalEvents.Location = new Point(0, 143);
            btnLocalEvents.Name = "btnLocalEvents";
            btnLocalEvents.Padding = new Padding(12, 0, 0, 0);
            btnLocalEvents.Size = new Size(220, 60);
            btnLocalEvents.TabIndex = 4;
            btnLocalEvents.Text = "Local Events and Announcements";
            btnLocalEvents.TextAlign = ContentAlignment.MiddleLeft;
            btnLocalEvents.UseVisualStyleBackColor = true;

            // btnServiceRequestStatus
            btnServiceRequestStatus.Enabled = false;
            btnServiceRequestStatus.FlatAppearance.BorderSize = 0;
            btnServiceRequestStatus.FlatStyle = FlatStyle.Flat;
            btnServiceRequestStatus.ForeColor = Color.Gainsboro;
            btnServiceRequestStatus.Location = new Point(0, 195);
            btnServiceRequestStatus.Name = "btnServiceRequestStatus";
            btnServiceRequestStatus.Padding = new Padding(12, 0, 0, 0);
            btnServiceRequestStatus.Size = new Size(220, 60);
            btnServiceRequestStatus.TabIndex = 3;
            btnServiceRequestStatus.Text = "Service Request Status";
            btnServiceRequestStatus.TextAlign = ContentAlignment.MiddleLeft;
            btnServiceRequestStatus.UseVisualStyleBackColor = true;

            // logoPanel
            logoPanel.BackColor = Color.FromArgb(39, 39, 58);
            logoPanel.Dock = DockStyle.Top;
            logoPanel.Location = new Point(0, 0);
            logoPanel.Name = "logoPanel";
            logoPanel.Size = new Size(220, 80);
            logoPanel.TabIndex = 0;
            logoPanel.Paint += logoPanel_Paint;

            // panelTitleBar
            panelTitleBar.BackColor = Color.FromArgb(0, 150, 136);
            panelTitleBar.Controls.Add(Home);
            panelTitleBar.Dock = DockStyle.Top;
            panelTitleBar.Location = new Point(220, 0);
            panelTitleBar.Name = "panelTitleBar";
            panelTitleBar.Size = new Size(580, 80);
            panelTitleBar.TabIndex = 1;

            // Home
            Home.Anchor = AnchorStyles.None;
            Home.AutoSize = true;
            Home.Font = new Font("Calibri", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Home.ForeColor = Color.White;
            Home.Location = new Point(264, 26);
            Home.Name = "Home";
            Home.Size = new Size(67, 26);
            Home.TabIndex = 0;
            Home.Text = "HOME";

            // mainPanel
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(220, 80);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(580, 370);
            mainPanel.TabIndex = 2;

            // MainMenuForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(mainPanel);
            Controls.Add(panelTitleBar);
            Controls.Add(sideBarPanel);
            Name = "MainMenuForm";
            Text = "Main Menu";
            sideBarPanel.ResumeLayout(false);
            panelTitleBar.ResumeLayout(false);
            panelTitleBar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button btnReportIssues;
        private Button btnLocalEvents;
        private Button btnServiceRequestStatus;
        private Panel sideBarPanel;
        private Panel logoPanel;
        private Panel mainPanel;
        private Panel panelTitleBar;
        private Label Home;
    }
}
