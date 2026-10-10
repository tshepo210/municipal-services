using System;
using System.Windows.Forms;

namespace ngakamodirimolema
{
    partial class ReportIssuesForm
    {
        private System.ComponentModel.IContainer components = null;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel contentPanel;
        private TextBox txtLocation;
        private ComboBox cmbCategory;
        private RichTextBox rtbDescription;
        private Button btnAttach;
        private Label lblAttachment;
        private Button btnSubmit;
        private ProgressBar progressBarEngagement;
        private Button btnBack;
        private OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Timer timerSubmission;

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
            tableLayoutPanel1 = new TableLayoutPanel();
            contentPanel = new Panel();
            btnBack = new Button();
            progressBarEngagement = new ProgressBar();
            btnSubmit = new Button();
            lblAttachment = new Label();
            btnAttach = new Button();
            rtbDescription = new RichTextBox();
            cmbCategory = new ComboBox();
            txtLocation = new TextBox();
            openFileDialog1 = new OpenFileDialog();
            timerSubmission = new System.Windows.Forms.Timer(components);
            tableLayoutPanel1.SuspendLayout();
            contentPanel.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(contentPanel, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(400, 430);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // contentPanel
            // 
            contentPanel.AutoScroll = true;
            contentPanel.Controls.Add(btnBack);
            contentPanel.Controls.Add(progressBarEngagement);
            contentPanel.Controls.Add(btnSubmit);
            contentPanel.Controls.Add(lblAttachment);
            contentPanel.Controls.Add(btnAttach);
            contentPanel.Controls.Add(rtbDescription);
            contentPanel.Controls.Add(cmbCategory);
            contentPanel.Controls.Add(txtLocation);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Location = new Point(3, 3);
            contentPanel.Name = "contentPanel";
            contentPanel.Size = new Size(394, 424);
            contentPanel.TabIndex = 0;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(243, 300);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(120, 30);
            btnBack.TabIndex = 6;
            btnBack.Text = "Back to Main Menu";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // progressBarEngagement
            // 
            progressBarEngagement.Dock = DockStyle.Top;
            progressBarEngagement.Location = new Point(0, 0);
            progressBarEngagement.Name = "progressBarEngagement";
            progressBarEngagement.Size = new Size(394, 10);
            progressBarEngagement.TabIndex = 5;
            // 
            // btnSubmit
            // 
            btnSubmit.Enabled = false;
            btnSubmit.Location = new Point(32, 300);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(120, 30);
            btnSubmit.TabIndex = 4;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // lblAttachment
            // 
            lblAttachment.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblAttachment.Location = new Point(150, 255);
            lblAttachment.Name = "lblAttachment";
            lblAttachment.Size = new Size(244, 23);
            lblAttachment.TabIndex = 7;
            lblAttachment.Text = "No file attached";
            lblAttachment.Click += lblAttachment_Click;
            // 
            // btnAttach
            // 
            btnAttach.Location = new Point(32, 248);
            btnAttach.Name = "btnAttach";
            btnAttach.Size = new Size(120, 30);
            btnAttach.TabIndex = 3;
            btnAttach.Text = "Attach image/document";
            btnAttach.UseVisualStyleBackColor = true;
            btnAttach.Click += btnAttach_Click;
            // 
            // rtbDescription
            // 
            rtbDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            rtbDescription.ForeColor = Color.Gray;
            rtbDescription.Location = new Point(32, 151);
            rtbDescription.Name = "rtbDescription";
            rtbDescription.Size = new Size(331, 80);
            rtbDescription.TabIndex = 2;
            rtbDescription.Text = "Description";
            rtbDescription.TextChanged += rtbDescription_TextChanged;
            rtbDescription.Enter += rtbDescription_Enter;
            rtbDescription.Leave += rtbDescription_Leave;
            // 
            // cmbCategory
            // 
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.Items.AddRange(new object[] { "Select category...", "Road", "Water", "Electricity", "Sanitation", "Other" });
            cmbCategory.Location = new Point(32, 96);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(331, 23);
            cmbCategory.TabIndex = 1;
            cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            // 
            // txtLocation
            // 
            txtLocation.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtLocation.Location = new Point(32, 42);
            txtLocation.Name = "txtLocation";
            txtLocation.PlaceholderText = "Enter location";
            txtLocation.Size = new Size(331, 23);
            txtLocation.TabIndex = 0;
            txtLocation.TextChanged += txtLocation_TextChanged;
            // 
            // openFileDialog1
            // 
            // include Excel in the dialog so users can see/select it, but Excel will be rejected on submit
            openFileDialog1.Filter = "Images and Documents|*.jpg;*.jpeg;*.png;*.pdf;*.docx;*.xls;*.xlsx|All files|*.*";
            // 
            // timerSubmission
            // 
            timerSubmission.Tick += timerSubmission_Tick;
            // 
            // ReportIssuesForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 430);
            Controls.Add(tableLayoutPanel1);
            Name = "ReportIssuesForm";
            Text = "Report Issues";
            tableLayoutPanel1.ResumeLayout(false);
            contentPanel.ResumeLayout(false);
            contentPanel.PerformLayout();
            ResumeLayout(false);
        }
    }
}
