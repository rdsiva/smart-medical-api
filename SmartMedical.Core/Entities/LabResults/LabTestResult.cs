using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartMedical.Core.Entities.LabResults
{
    public class LabTestResult
    {
        [Key]
        public Guid Id { get; set; }
        public Guid LabTestId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentCode { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public string ReferenceRange { get; set; }
        public string AbnormalFlag { get; set; } // normal, low, high, critical_low, critical_high
        public string Interpretation { get; set; }
        public string Status { get; set; } // preliminary, final, corrected, canceled
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public LabTest LabTest { get; set; }
        public ICollection<LabTestResultHistory> History { get; set; }
    }
}
