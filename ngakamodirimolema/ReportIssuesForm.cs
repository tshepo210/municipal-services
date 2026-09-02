using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ngakamodirimolema
{
    public partial class ReportIssuesForm : Form
    {
        private readonly string _descriptionPlaceholder = "Description";
        private string? _attachmentPath;
        // allowed when attaching (so user can see/select Excel), but submission rules differ
        private readonly string[] _allowedAttachmentExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".docx", ".xls", ".xlsx" };
        // allowed when submitting
        private readonly string[] _submitAllowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".docx" };

        public ReportIssuesForm()
        {
            InitializeComponent();
            UpdateProgressAndSubmitState();
        }

        private void btnAttach_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var file = openFileDialog1.FileName;
                if (!File.Exists(file))
                {
                    MessageBox.Show("Selected file does not exist.", "Attachment Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var ext = Path.GetExtension(file)?.ToLowerInvariant();
                if (ext != null && Array.IndexOf(_allowedAttachmentExtensions, ext) >= 0)
                {
                    _attachmentPath = file;
                    lblAttachment.Text = Path.GetFileName(_attachmentPath);
                }
                else
                {
                    MessageBox.Show($"Invalid file type '{ext ?? ""}'. Allowed: *.jpg; *.jpeg; *.png; *.pdf; *.docx", "Invalid Attachment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _attachmentPath = null;
                    lblAttachment.Text = "No file attached";
                }
            }
        }

        private void rtbDescription_Enter(object sender, EventArgs e)
        {
            if (rtbDescription.Text == _descriptionPlaceholder && rtbDescription.ForeColor == System.Drawing.Color.Gray)
            {
                rtbDescription.Text = string.Empty;
                rtbDescription.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void rtbDescription_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rtbDescription.Text))
            {
                rtbDescription.Text = _descriptionPlaceholder;
                rtbDescription.ForeColor = System.Drawing.Color.Gray;
            }
            UpdateProgressAndSubmitState();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // validate attachment type before submission

            if (!string.IsNullOrWhiteSpace(_attachmentPath))
            {
                if (!File.Exists(_attachmentPath))
                {
                    MessageBox.Show("Attached file no longer exists.", "Attachment Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var ext = Path.GetExtension(_attachmentPath)?.ToLowerInvariant();
                // If the attachment type is not allowed for submission, show validation
                if (ext == null || Array.IndexOf(_submitAllowedExtensions, ext) < 0)
                {
                    MessageBox.Show($"Attachment type '{ext ?? ""}' is not allowed for submission. Allowed types: *.jpg, *.jpeg, *.png, *.pdf, *.docx.", "Invalid Attachment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            btnSubmit.Enabled = false;
            btnAttach.Enabled = false;
            txtLocation.Enabled = false;
            cmbCategory.Enabled = false;
            rtbDescription.Enabled = false;
            // reset progress bar and start submission animation
            progressBarEngagement.Value = 0;
            timerSubmission.Start();
        }

        private void txtLocation_TextChanged(object sender, EventArgs e)
        {
            UpdateProgressAndSubmitState();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProgressAndSubmitState();
        }

        private void rtbDescription_TextChanged(object sender, EventArgs e)
        {
            // ignore placeholder text
            UpdateProgressAndSubmitState();
        }

        private void UpdateProgressAndSubmitState()
        {
            int required = 3; // location, category, description
            int completed = 0;

            if (!string.IsNullOrWhiteSpace(txtLocation.Text))
                completed++;

            if (cmbCategory.SelectedIndex > 0)
                completed++;

            if (!string.IsNullOrWhiteSpace(rtbDescription.Text) && rtbDescription.Text != _descriptionPlaceholder)
                completed++;

            int progress = (int)Math.Round((completed / (double)required) * 100.0);
            progressBarEngagement.Value = Math.Min(progressBarEngagement.Maximum, Math.Max(progressBarEngagement.Minimum, progress));
            btnSubmit.Enabled = (completed == required);
        }

        private void timerSubmission_Tick(object sender, EventArgs e)
        {
            if (progressBarEngagement.Value < progressBarEngagement.Maximum)
            {
                progressBarEngagement.Value = Math.Min(progressBarEngagement.Maximum, progressBarEngagement.Value + 10);
            }
            else
            {
                timerSubmission.Stop();
                // save the report in-memory
                SaveReport();
                MessageBox.Show("Report submitted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void SaveReport()
        {
            try
            {
                var report = new ReportData
                {
                    Location = txtLocation.Text?.Trim(),
                    Category = cmbCategory.SelectedItem?.ToString() ?? string.Empty,
                    Description = (rtbDescription.Text == _descriptionPlaceholder ? string.Empty : rtbDescription.Text)?.Trim(),
                    AttachmentPath = _attachmentPath,
                    SubmittedAt = DateTime.UtcNow
                };

                ReportRepository.Add(report);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save report in memory: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblAttachment_Click(object sender, EventArgs e)
        {

        }
    }
}
