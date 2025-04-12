using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartMedical.Business.Interfaces;
using SmartMedical.Core.Entities.LabResults;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartMedical.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LabResultsController : ControllerBase
    {
        private readonly ILogger<LabResultsController> _logger;
        private readonly ILabResultsService _labResultsService;

        public LabResultsController(
            ILogger<LabResultsController> logger,
            ILabResultsService labResultsService)
        {
            _logger = logger;
            _labResultsService = labResultsService;
        }

        #region Lab Tests
        [HttpGet("tests")]
        public async Task<IActionResult> GetLabTests(
            [FromQuery] string category = null,
            [FromQuery] string status = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int limit = 50,
            [FromQuery] int offset = 0)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                // In a real application, this would come from the authenticated user
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var tests = await _labResultsService.GetLabTestsAsync(userId, category, status, startDate, endDate, limit, offset);
                
                return Ok(new
                {
                    data = tests,
                    pagination = new
                    {
                        limit = limit,
                        offset = offset,
                        total = tests.Count
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving lab tests");
                return BadRequest(new { message = "Failed to retrieve lab tests" });
            }
        }

        [HttpGet("tests/{id}")]
        public async Task<IActionResult> GetLabTest(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var test = await _labResultsService.GetLabTestByIdAsync(id, userId);
                
                if (test == null)
                {
                    return NotFound(new { message = "Lab test not found" });
                }
                
                return Ok(test);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving lab test");
                return BadRequest(new { message = "Failed to retrieve lab test" });
            }
        }

        [HttpPost("tests")]
        public async Task<IActionResult> CreateLabTest([FromBody] LabTestRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var test = new LabTest
                {
                    UserId = userId,
                    TestName = request.TestName,
                    TestCode = request.TestCode,
                    Category = request.Category,
                    CollectionDate = request.CollectionDate,
                    ResultDate = request.ResultDate,
                    OrderingProvider = request.OrderingProvider,
                    PerformingLab = request.PerformingLab,
                    SpecimenType = request.SpecimenType,
                    Status = request.Status,
                    Notes = request.Notes,
                    ReportUrl = request.ReportUrl
                };

                var createdTest = await _labResultsService.CreateLabTestAsync(test);
                
                return CreatedAtAction(nameof(GetLabTest), new { id = createdTest.Id }, createdTest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lab test");
                return BadRequest(new { message = "Failed to create lab test" });
            }
        }

        [HttpPut("tests/{id}")]
        public async Task<IActionResult> UpdateLabTest(Guid id, [FromBody] LabTestRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var existingTest = await _labResultsService.GetLabTestByIdAsync(id, userId);
                
                if (existingTest == null)
                {
                    return NotFound(new { message = "Lab test not found" });
                }

                existingTest.TestName = request.TestName ?? existingTest.TestName;
                existingTest.TestCode = request.TestCode ?? existingTest.TestCode;
                existingTest.Category = request.Category ?? existingTest.Category;
                existingTest.CollectionDate = request.CollectionDate;
                existingTest.ResultDate = request.ResultDate;
                existingTest.OrderingProvider = request.OrderingProvider ?? existingTest.OrderingProvider;
                existingTest.PerformingLab = request.PerformingLab ?? existingTest.PerformingLab;
                existingTest.SpecimenType = request.SpecimenType ?? existingTest.SpecimenType;
                existingTest.Status = request.Status ?? existingTest.Status;
                existingTest.Notes = request.Notes ?? existingTest.Notes;
                existingTest.ReportUrl = request.ReportUrl ?? existingTest.ReportUrl;

                var updatedTest = await _labResultsService.UpdateLabTestAsync(existingTest);
                
                return Ok(updatedTest);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Lab test not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating lab test");
                return BadRequest(new { message = "Failed to update lab test" });
            }
        }

        [HttpDelete("tests/{id}")]
        public async Task<IActionResult> DeleteLabTest(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var result = await _labResultsService.DeleteLabTestAsync(id, userId);
                
                if (!result)
                {
                    return NotFound(new { message = "Lab test not found" });
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting lab test");
                return BadRequest(new { message = "Failed to delete lab test" });
            }
        }
        #endregion

        #region Lab Test Results
        [HttpGet("tests/{testId}/results")]
        public async Task<IActionResult> GetLabTestResults(Guid testId)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                // Verify the test belongs to the user
                var test = await _labResultsService.GetLabTestByIdAsync(testId, userId);
                if (test == null)
                {
                    return NotFound(new { message = "Lab test not found" });
                }

                var results = await _labResultsService.GetLabTestResultsAsync(testId);
                
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving lab test results");
                return BadRequest(new { message = "Failed to retrieve lab test results" });
            }
        }

        [HttpGet("results/{id}")]
        public async Task<IActionResult> GetLabTestResult(Guid id)
        {
            try
            {
                var result = await _labResultsService.GetLabTestResultByIdAsync(id);
                
                if (result == null)
                {
                    return NotFound(new { message = "Lab test result not found" });
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving lab test result");
                return BadRequest(new { message = "Failed to retrieve lab test result" });
            }
        }

        [HttpPost("tests/{testId}/results")]
        public async Task<IActionResult> CreateLabTestResult(Guid testId, [FromBody] LabTestResultRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                // Verify the test belongs to the user
                var test = await _labResultsService.GetLabTestByIdAsync(testId, userId);
                if (test == null)
                {
                    return NotFound(new { message = "Lab test not found" });
                }

                var result = new LabTestResult
                {
                    LabTestId = testId,
                    ComponentName = request.ComponentName,
                    ComponentCode = request.ComponentCode,
                    Value = request.Value,
                    Unit = request.Unit,
                    ReferenceRange = request.ReferenceRange,
                    AbnormalFlag = request.AbnormalFlag,
                    Interpretation = request.Interpretation,
                    Status = request.Status
                };

                var createdResult = await _labResultsService.CreateLabTestResultAsync(result);
                
                return CreatedAtAction(nameof(GetLabTestResult), new { id = createdResult.Id }, createdResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lab test result");
                return BadRequest(new { message = "Failed to create lab test result" });
            }
        }

        [HttpPut("results/{id}")]
        public async Task<IActionResult> UpdateLabTestResult(Guid id, [FromBody] LabTestResultRequest request)
        {
            try
            {
                var existingResult = await _labResultsService.GetLabTestResultByIdAsync(id);
                
                if (existingResult == null)
                {
                    return NotFound(new { message = "Lab test result not found" });
                }

                existingResult.ComponentName = request.ComponentName ?? existingResult.ComponentName;
                existingResult.ComponentCode = request.ComponentCode ?? existingResult.ComponentCode;
                existingResult.Value = request.Value ?? existingResult.Value;
                existingResult.Unit = request.Unit ?? existingResult.Unit;
                existingResult.ReferenceRange = request.ReferenceRange ?? existingResult.ReferenceRange;
                existingResult.AbnormalFlag = request.AbnormalFlag ?? existingResult.AbnormalFlag;
                existingResult.Interpretation = request.Interpretation ?? existingResult.Interpretation;
                existingResult.Status = request.Status ?? existingResult.Status;

                var updatedResult = await _labResultsService.UpdateLabTestResultAsync(existingResult);
                
                return Ok(updatedResult);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Lab test result not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating lab test result");
                return BadRequest(new { message = "Failed to update lab test result" });
            }
        }

        [HttpDelete("results/{id}")]
        public async Task<IActionResult> DeleteLabTestResult(Guid id)
        {
            try
            {
                var result = await _labResultsService.DeleteLabTestResultAsync(id);
                
                if (!result)
                {
                    return NotFound(new { message = "Lab test result not found" });
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting lab test result");
                return BadRequest(new { message = "Failed to delete lab test result" });
            }
        }
        #endregion

        #region Lab Test Result History
        [HttpGet("results/{resultId}/history")]
        public async Task<IActionResult> GetLabTestResultHistory(Guid resultId)
        {
            try
            {
                var history = await _labResultsService.GetLabTestResultHistoryAsync(resultId);
                
                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving lab test result history");
                return BadRequest(new { message = "Failed to retrieve lab test result history" });
            }
        }
        #endregion

        #region Specialized Queries
        [HttpGet("abnormal")]
        public async Task<IActionResult> GetAbnormalLabResults(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int limit = 50,
            [FromQuery] int offset = 0)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var results = await _labResultsService.GetAbnormalLabResultsAsync(userId, startDate, endDate, limit, offset);
                
                return Ok(new
                {
                    data = results,
                    pagination = new
                    {
                        limit = limit,
                        offset = offset,
                        total = results.Count
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving abnormal lab results");
                return BadRequest(new { message = "Failed to retrieve abnormal lab results" });
            }
        }

        [HttpGet("component/{componentName}")]
        public async Task<IActionResult> GetLabResultsByComponent(
            string componentName,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int limit = 50,
            [FromQuery] int offset = 0)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var results = await _labResultsService.GetLabResultsByComponentAsync(userId, componentName, startDate, endDate, limit, offset);
                
                return Ok(new
                {
                    data = results,
                    pagination = new
                    {
                        limit = limit,
                        offset = offset,
                        total = results.Count
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving lab results by component");
                return BadRequest(new { message = "Failed to retrieve lab results by component" });
            }
        }
        #endregion

        #region Additional Features
        [HttpGet("results/{resultId}/explanation")]
        public async Task<IActionResult> GetLabResultExplanation(Guid resultId)
        {
            try
            {
                var explanation = await _labResultsService.GetLabResultExplanationAsync(resultId);
                
                return Ok(explanation);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Lab test result not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving lab result explanation");
                return BadRequest(new { message = "Failed to retrieve lab result explanation" });
            }
        }

        [HttpGet("results/{resultId}/comparison")]
        public async Task<IActionResult> GetLabResultComparison(Guid resultId, [FromQuery] int historyCount = 5)
        {
            try
            {
                var comparison = await _labResultsService.GetLabResultComparisonAsync(resultId, historyCount);
                
                return Ok(comparison);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Lab test result not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving lab result comparison");
                return BadRequest(new { message = "Failed to retrieve lab result comparison" });
            }
        }
        #endregion
    }

    #region Request Models
    public class LabTestRequest
    {
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
    }

    public class LabTestResultRequest
    {
        public string ComponentName { get; set; }
        public string ComponentCode { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public string ReferenceRange { get; set; }
        public string AbnormalFlag { get; set; } // normal, low, high, critical_low, critical_high
        public string Interpretation { get; set; }
        public string Status { get; set; } // preliminary, final, corrected, canceled
    }
    #endregion
}
