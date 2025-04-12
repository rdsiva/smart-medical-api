using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartMedical.Core.Entities.LabResults
{
    public class LabTest
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string TestName { get; set; }
        public string TestCode { get; set; }
        public string Category { get; set; }
        public DateTime CollectionDate { get; set; }
        public DateTime ResultDate { get; set; }
        public string OrderingProvider { get; set; }
        public string PerformingLab { get; set; }
        public string SpecimenType { get; set; }
        public string Status { get; set; } // ordered, collected, in_progress, completed, canceled
        public string Notes { get; set; }
        public string ReportUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Auth.User User { get; set; }
        public ICollection<LabTestResult> Results { get; set; }
    }
}
