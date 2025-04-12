using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartMedical.Core.Entities.LabResults
{
    public class LabTestResultHistory
    {
        [Key]
        public Guid Id { get; set; }
        public Guid LabTestResultId { get; set; }
        public DateTime TestDate { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public string ReferenceRange { get; set; }
        public string AbnormalFlag { get; set; } // normal, low, high, critical_low, critical_high
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public LabTestResult LabTestResult { get; set; }
    }
}
