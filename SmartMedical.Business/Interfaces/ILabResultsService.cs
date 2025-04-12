using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartMedical.Core.Entities.LabResults;

namespace SmartMedical.Business.Interfaces
{
    public interface ILabResultsService
    {
        // Lab Tests
        Task<IEnumerable<LabTest>> GetLabTestsAsync(Guid userId, string category = null, string status = null, DateTime? startDate = null, DateTime? endDate = null, int limit = 50, int offset = 0);
        Task<LabTest> GetLabTestByIdAsync(Guid id, Guid userId);
        Task<LabTest> CreateLabTestAsync(LabTest labTest);
        Task<LabTest> UpdateLabTestAsync(LabTest labTest);
        Task<bool> DeleteLabTestAsync(Guid id, Guid userId);
        
        // Lab Test Results
        Task<IEnumerable<LabTestResult>> GetLabTestResultsAsync(Guid labTestId);
        Task<LabTestResult> GetLabTestResultByIdAsync(Guid id);
        Task<LabTestResult> CreateLabTestResultAsync(LabTestResult labTestResult);
        Task<LabTestResult> UpdateLabTestResultAsync(LabTestResult labTestResult);
        Task<bool> DeleteLabTestResultAsync(Guid id);
        
        // Lab Test Result History
        Task<IEnumerable<LabTestResultHistory>> GetLabTestResultHistoryAsync(Guid labTestResultId);
        Task<LabTestResultHistory> CreateLabTestResultHistoryAsync(LabTestResultHistory history);
        
        // Specialized Queries
        Task<IEnumerable<LabTestResult>> GetAbnormalLabResultsAsync(Guid userId, DateTime? startDate = null, DateTime? endDate = null, int limit = 50, int offset = 0);
        Task<IEnumerable<LabTestResult>> GetLabResultsByComponentAsync(Guid userId, string componentName, DateTime? startDate = null, DateTime? endDate = null, int limit = 50, int offset = 0);
        
        // Additional Features
        Task<Dictionary<string, object>> GetLabResultExplanationAsync(Guid labTestResultId);
        Task<Dictionary<string, object>> GetLabResultComparisonAsync(Guid labTestResultId, int historyCount = 5);
    }
}
