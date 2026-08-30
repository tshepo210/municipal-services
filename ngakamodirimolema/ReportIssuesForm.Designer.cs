using System;
using System.Windows.Forms;

namespace ngakamodirimolema
{
    partial class ReportIssuesForm
    {
        private System.ComponentModel.IContainer components = null;
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
            txtLocation = new TextBox();
            cmbCategory = new ComboBox();
            rtbDescription = new RichTextBox();
            btnAttach = new Button();
            lblAttachment = new Label();
            btnSubmit = new Button();
            progressBarEngagement = new ProgressBar();
            btnBack = new Button();
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Allowed files|*.jpg;*.jpeg;*.png;*.pdf;*.docx";
            openFileDialog1.Multiselect = false;
            timerSubmission = new System.Windows.Forms.Timer(components);
            SuspendLayout();

            // txtLocation
            txtLocation.Location = new System.Drawing.Point(20, 40);
            txtLocation.Name = "txtLocation";
            txtLocation.PlaceholderText = "Enter location";
            txtLocation.Size = new System.Drawing.Size(360, 23);
            txtLocation.TabIndex = 0;
            txtLocation.TextChanged += txtLocation_TextChanged;

            // cmbCategory
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.Items.AddRange(new object[] { "Select category...", "Road", "Water", "Electricity", "Sanitation", "Other" });
            cmbCategory.SelectedIndex = 0;
            cmbCategory.Location = new System.Drawing.Point(20, 95);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new System.Drawing.Size(200, 23);
            cmbCategory.TabIndex = 1;
            cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;

            // rtbDescription
            rtbDescription.Location = new System.Drawing.Point(20, 150);
            rtbDescription.Name = "rtbDescription";
            rtbDescription.Size = new System.Drawing.Size(360, 120);
            rtbDescription.TabIndex = 2;
            rtbDescription.Text = "Description";
            rtbDescription.ForeColor = System.Drawing.Color.Gray;
            rtbDescription.Enter += rtbDescription_Enter;
            rtbDescription.Leave += rtbDescription_Leave;
            rtbDescription.TextChanged += rtbDescription_TextChanged;

            // btnAttach
            btnAttach.Location = new System.Drawing.Point(20, 290);
            btnAttach.Name = "btnAttach";
            btnAttach.Size = new System.Drawing.Size(120, 30);
            btnAttach.TabIndex = 3;
            btnAttach.Text = "Attach image/document";
            btnAttach.UseVisualStyleBackColor = true;
            btnAttach.Click += btnAttach_Click;

            // lblAttachment
            lblAttachment.Location = new System.Drawing.Point(150, 295);
            lblAttachment.Name = "lblAttachment";
            lblAttachment.Size = new System.Drawing.Size(230, 23);
            lblAttachment.Text = "No file attached";

            // btnSubmit
            btnSubmit.Location = new System.Drawing.Point(20, 340);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new System.Drawing.Size(120, 30);
            btnSubmit.TabIndex = 4;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            btnSubmit.Enabled = false;

            // progressBarEngagement
            progressBarEngagement.Location = new System.Drawing.Point(20, 390);
            progressBarEngagement.Name = "progressBarEngagement";
            progressBarEngagement.Size = new System.Drawing.Size(360, 20);
            progressBarEngagement.TabIndex = 5;

            // btnBack
            btnBack.Location = new System.Drawing.Point(260, 340);
            btnBack.Name = "btnBack";
            btnBack.Size = new System.Drawing.Size(120, 30);
            btnBack.TabIndex = 6;
            btnBack.Text = "Back to Main Menu";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;

            // timerSubmission
            timerSubmission.Interval = 100;
            timerSubmission.Tick += timerSubmission_Tick;

            // ReportIssuesForm
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(400, 430);
            Controls.Add(btnBack);
            Controls.Add(progressBarEngagement);
            Controls.Add(btnSubmit);
            Controls.Add(lblAttachment);
            Controls.Add(btnAttach);
            Controls.Add(rtbDescription);
            Controls.Add(cmbCategory);
            Controls.Add(txtLocation);
            Name = "ReportIssuesForm";
            Text = "Report Issues";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
