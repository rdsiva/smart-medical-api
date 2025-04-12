using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartMedical.Business.Interfaces;
using SmartMedical.Core.Entities.LabResults;
using SmartMedical.Core.Interfaces;

namespace SmartMedical.Business.Services
{
    public class LabResultsService : ILabResultsService
    {
        private readonly ILabResultsRepository _labResultsRepository;

        public LabResultsService(ILabResultsRepository labResultsRepository)
        {
            _labResultsRepository = labResultsRepository;
        }

        #region Lab Tests
        public async Task<IEnumerable<LabTest>> GetLabTestsAsync(Guid userId, string category = null, string status = null, DateTime? startDate = null, DateTime? endDate = null, int limit = 50, int offset = 0)
        {
            return await _labResultsRepository.GetLabTestsAsync(userId, category, status, startDate, endDate, limit, offset);
        }

        public async Task<LabTest> GetLabTestByIdAsync(Guid id, Guid userId)
        {
            return await _labResultsRepository.GetLabTestByIdAsync(id, userId);
        }

        public async Task<LabTest> CreateLabTestAsync(LabTest labTest)
        {
            return await _labResultsRepository.CreateLabTestAsync(labTest);
        }

        public async Task<LabTest> UpdateLabTestAsync(LabTest labTest)
        {
            var existingLabTest = await _labResultsRepository.GetLabTestByIdAsync(labTest.Id, labTest.UserId);
            if (existingLabTest == null)
            {
                throw new KeyNotFoundException($"Lab test with ID {labTest.Id} not found");
            }

            return await _labResultsRepository.UpdateLabTestAsync(labTest);
        }

        public async Task<bool> DeleteLabTestAsync(Guid id, Guid userId)
        {
            return await _labResultsRepository.DeleteLabTestAsync(id, userId);
        }
        #endregion

        #region Lab Test Results
        public async Task<IEnumerable<LabTestResult>> GetLabTestResultsAsync(Guid labTestId)
        {
            return await _labResultsRepository.GetLabTestResultsAsync(labTestId);
        }

        public async Task<LabTestResult> GetLabTestResultByIdAsync(Guid id)
        {
            return await _labResultsRepository.GetLabTestResultByIdAsync(id);
        }

        public async Task<LabTestResult> CreateLabTestResultAsync(LabTestResult labTestResult)
        {
            return await _labResultsRepository.CreateLabTestResultAsync(labTestResult);
        }

        public async Task<LabTestResult> UpdateLabTestResultAsync(LabTestResult labTestResult)
        {
            var existingLabTestResult = await _labResultsRepository.GetLabTestResultByIdAsync(labTestResult.Id);
            if (existingLabTestResult == null)
            {
                throw new KeyNotFoundException($"Lab test result with ID {labTestResult.Id} not found");
            }

            return await _labResultsRepository.UpdateLabTestResultAsync(labTestResult);
        }

        public async Task<bool> DeleteLabTestResultAsync(Guid id)
        {
            return await _labResultsRepository.DeleteLabTestResultAsync(id);
        }
        #endregion

        #region Lab Test Result History
        public async Task<IEnumerable<LabTestResultHistory>> GetLabTestResultHistoryAsync(Guid labTestResultId)
        {
            return await _labResultsRepository.GetLabTestResultHistoryAsync(labTestResultId);
        }

        public async Task<LabTestResultHistory> CreateLabTestResultHistoryAsync(LabTestResultHistory history)
        {
            return await _labResultsRepository.CreateLabTestResultHistoryAsync(history);
        }
        #endregion

        #region Specialized Queries
        public async Task<IEnumerable<LabTestResult>> GetAbnormalLabResultsAsync(Guid userId, DateTime? startDate = null, DateTime? endDate = null, int limit = 50, int offset = 0)
        {
            return await _labResultsRepository.GetAbnormalLabResultsAsync(userId, startDate, endDate, limit, offset);
        }

        public async Task<IEnumerable<LabTestResult>> GetLabResultsByComponentAsync(Guid userId, string componentName, DateTime? startDate = null, DateTime? endDate = null, int limit = 50, int offset = 0)
        {
            return await _labResultsRepository.GetLabResultsByComponentAsync(userId, componentName, startDate, endDate, limit, offset);
        }
        #endregion

        #region Additional Features
        public async Task<Dictionary<string, object>> GetLabResultExplanationAsync(Guid labTestResultId)
        {
            // In a real implementation, this would use a medical knowledge base or AI service
            // to provide explanations of lab test results
            // For now, we'll implement a simple mock explanation

            var result = await _labResultsRepository.GetLabTestResultByIdAsync(labTestResultId);
            if (result == null)
            {
                throw new KeyNotFoundException($"Lab test result with ID {labTestResultId} not found");
            }

            // Get the parent lab test
            var labTest = await _labResultsRepository.GetLabTestByIdAsync(result.LabTestId, Guid.Empty);

            var explanation = new Dictionary<string, object>
            {
                ["resultId"] = result.Id,
                ["componentName"] = result.ComponentName,
                ["value"] = result.Value,
                ["unit"] = result.Unit,
                ["referenceRange"] = result.ReferenceRange,
                ["abnormalFlag"] = result.AbnormalFlag,
                ["testName"] = labTest?.TestName,
                ["explanation"] = GetMockExplanation(result.ComponentName, result.AbnormalFlag),
                ["whatToDoNext"] = GetMockRecommendation(result.ComponentName, result.AbnormalFlag),
                ["possibleCauses"] = GetMockCauses(result.ComponentName, result.AbnormalFlag),
                ["relatedTests"] = GetMockRelatedTests(result.ComponentName)
            };

            return explanation;
        }

        public async Task<Dictionary<string, object>> GetLabResultComparisonAsync(Guid labTestResultId, int historyCount = 5)
        {
            var result = await _labResultsRepository.GetLabTestResultByIdAsync(labTestResultId);
            if (result == null)
            {
                throw new KeyNotFoundException($"Lab test result with ID {labTestResultId} not found");
            }

            // Get the parent lab test
            var labTest = await _labResultsRepository.GetLabTestByIdAsync(result.LabTestId, Guid.Empty);

            // Get history for this component
            var history = await _labResultsRepository.GetLabTestResultHistoryAsync(labTestResultId);
            
            // Get previous results for the same component
            var previousResults = await _labResultsRepository.GetLabResultsByComponentAsync(
                labTest.UserId, 
                result.ComponentName, 
                null, 
                labTest.CollectionDate.AddDays(-1), // Get results before this test
                historyCount, 
                0);

            var comparisonData = new Dictionary<string, object>
            {
                ["resultId"] = result.Id,
                ["componentName"] = result.ComponentName,
                ["currentValue"] = result.Value,
                ["currentUnit"] = result.Unit,
                ["referenceRange"] = result.ReferenceRange,
                ["testDate"] = labTest.CollectionDate,
                ["trend"] = CalculateTrend(result, previousResults),
                ["previousResults"] = previousResults.Select(r => new
                {
                    r.Id,
                    TestDate = _labResultsRepository.GetLabTestByIdAsync(r.LabTestId, Guid.Empty).Result.CollectionDate,
                    r.Value,
                    r.Unit,
                    r.AbnormalFlag
                }).OrderByDescending(r => r.TestDate).ToList(),
                ["historicalValues"] = history.Select(h => new
                {
                    h.TestDate,
                    h.Value,
                    h.Unit,
                    h.AbnormalFlag
                }).OrderByDescending(h => h.TestDate).ToList()
            };

            return comparisonData;
        }
        #endregion

        #region Helper Methods
        private string GetMockExplanation(string componentName, string abnormalFlag)
        {
            // In a real implementation, this would be much more sophisticated
            // and would use a medical knowledge base
            
            if (string.IsNullOrEmpty(abnormalFlag) || abnormalFlag == "normal")
            {
                return $"{componentName} is within the normal range, which indicates normal function.";
            }
            
            if (abnormalFlag.Contains("low"))
            {
                return $"{componentName} is lower than the reference range. This may indicate various conditions depending on your overall health.";
            }
            
            if (abnormalFlag.Contains("high"))
            {
                return $"{componentName} is higher than the reference range. This may indicate various conditions depending on your overall health.";
            }
            
            return $"The result for {componentName} requires interpretation by a healthcare professional.";
        }

        private string GetMockRecommendation(string componentName, string abnormalFlag)
        {
            if (string.IsNullOrEmpty(abnormalFlag) || abnormalFlag == "normal")
            {
                return "Continue with regular health check-ups as recommended by your healthcare provider.";
            }
            
            if (abnormalFlag.Contains("critical"))
            {
                return "Contact your healthcare provider immediately to discuss these results.";
            }
            
            return "Follow up with your healthcare provider to discuss these results during your next appointment.";
        }

        private List<string> GetMockCauses(string componentName, string abnormalFlag)
        {
            var causes = new List<string>();
            
            // This is a simplified mock implementation
            // In a real application, this would be based on medical knowledge
            
            if (componentName.Contains("glucose"))
            {
                if (abnormalFlag.Contains("high"))
                {
                    causes.Add("Diabetes or prediabetes");
                    causes.Add("Recent meal (especially high in carbohydrates)");
                    causes.Add("Stress or illness");
                    causes.Add("Certain medications");
                }
                else if (abnormalFlag.Contains("low"))
                {
                    causes.Add("Skipping meals");
                    causes.Add("Excessive exercise");
                    causes.Add("Certain medications, especially insulin");
                    causes.Add("Liver or kidney disorders");
                }
            }
            else if (componentName.Contains("cholesterol"))
            {
                if (abnormalFlag.Contains("high"))
                {
                    causes.Add("Diet high in saturated fats");
                    causes.Add("Sedentary lifestyle");
                    causes.Add("Genetic factors");
                    causes.Add("Obesity");
                }
            }
            
            // Default causes if none of the specific conditions match
            if (causes.Count == 0)
            {
                if (abnormalFlag.Contains("high"))
                {
                    causes.Add("Diet and lifestyle factors");
                    causes.Add("Certain medications");
                    causes.Add("Underlying medical conditions");
                }
                else if (abnormalFlag.Contains("low"))
                {
                    causes.Add("Nutritional deficiencies");
                    causes.Add("Certain medications");
                    causes.Add("Underlying medical conditions");
                }
            }
            
            return causes;
        }

        private List<string> GetMockRelatedTests(string componentName)
        {
            // This is a simplified mock implementation
            // In a real application, this would be based on medical knowledge
            
            if (componentName.Contains("glucose"))
            {
                return new List<string> { "HbA1c", "Insulin", "C-peptide", "Oral Glucose Tolerance Test" };
            }
            else if (componentName.Contains("cholesterol"))
            {
                return new List<string> { "LDL", "HDL", "Triglycerides", "Apolipoprotein B" };
            }
            else if (componentName.Contains("hemoglobin") || componentName.Contains("hematocrit"))
            {
                return new List<string> { "Complete Blood Count", "Iron", "Ferritin", "Vitamin B12", "Folate" };
            }
            
            // Default related tests
            return new List<string> { "Complete Blood Count", "Comprehensive Metabolic Panel" };
        }

        private string CalculateTrend(LabTestResult current, IEnumerable<LabTestResult> previous)
        {
            if (!previous.Any())
            {
                return "insufficient_data";
            }
            
            // Try to parse the values as decimal for comparison
            // This is a simplified approach and would need to be more sophisticated in a real application
            if (!decimal.TryParse(current.Value, out decimal currentValue))
            {
                return "non_numeric";
            }
            
            var previousValues = new List<decimal>();
            foreach (var result in previous)
            {
                if (decimal.TryParse(result.Value, out decimal value))
                {
                    previousValues.Add(value);
                }
            }
            
            if (previousValues.Count == 0)
            {
                return "insufficient_data";
            }
            
            decimal average = previousValues.Average();
            
            // Calculate percent change
            decimal percentChange = (currentValue - average) / average * 100;
            
            if (Math.Abs(percentChange) < 5)
            {
                return "stable";
            }
            else if (percentChange > 0)
            {
                return percentChange > 20 ? "significant_increase" : "increasing";
            }
            else
            {
                return percentChange < -20 ? "significant_decrease" : "decreasing";
            }
        }
        #endregion
    }
}
