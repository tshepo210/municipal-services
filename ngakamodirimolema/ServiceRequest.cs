using System;
using System.Collections.Generic;

namespace ngakamodirimolema
{
    public enum ServiceRequestPriority { Low = 1, Medium = 2, High = 3, Critical = 4 }
    public enum ServiceRequestStatus { Submitted, Acknowledged, InProgress, Resolved }

    public sealed class StatusHistoryEntry
    {
        public DateTime Timestamp { get; init; }
        public ServiceRequestStatus Status { get; init; }
        public string Note { get; init; } = string.Empty;
    }

    public sealed class ServiceRequest
    {
        public int SequenceNumber { get; init; }
        public string RequestId => $"SR-{SequenceNumber:0000}";
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = "Other";
        public string Location { get; set; } = string.Empty;
        public ServiceRequestPriority Priority { get; set; }
        public ServiceRequestStatus Status { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string AssignedDepartment { get; set; } = string.Empty;
        public string? AttachmentPath { get; set; }
        public List<StatusHistoryEntry> StatusHistory { get; } = new();
    }
}
