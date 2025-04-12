using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartMedical.Core.Entities.LabResults;
using SmartMedical.Core.Interfaces;
using SmartMedical.Infrastructure.Data;

namespace SmartMedical.Infrastructure.Repositories
{
    public class LabResultsRepository : ILabResultsRepository
    {
        private readonly ApplicationDbContext _context;

        public LabResultsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Lab Tests
        public async Task<IEnumerable<LabTest>> GetLabTestsAsync(Guid userId, string category = null, string status = null, DateTime? startDate = null, DateTime? endDate = null, int limit = 50, int offset = 0)
        {
            var query = _context.LabTests.Where(t => t.UserId == userId);

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(t => t.Category == category);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(t => t.Status == status);
            }

            if (startDate.HasValue)
            {
                query = query.Where(t => t.CollectionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.CollectionDate <= endDate.Value);
            }

            return await query
                .OrderByDescending(t => t.CollectionDate)
                .Skip(offset)
                .Take(limit)
                .Include(t => t.Results)
                .ToListAsync();
        }

        public async Task<LabTest> GetLabTestByIdAsync(Guid id, Guid userId)
        {
            return await _context.LabTests
                .Include(t => t.Results)
                .ThenInclude(r => r.History)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<LabTest> CreateLabTestAsync(LabTest labTest)
        {
            labTest.Id = Guid.NewGuid();
            labTest.CreatedAt = DateTime.UtcNow;
            labTest.UpdatedAt = DateTime.UtcNow;

            _context.LabTests.Add(labTest);
            await _context.SaveChangesAsync();

            return labTest;
        }

        public async Task<LabTest> UpdateLabTestAsync(LabTest labTest)
        {
            labTest.UpdatedAt = DateTime.UtcNow;

            _context.LabTests.Update(labTest);
            await _context.SaveChangesAsync();

            return labTest;
        }

        public async Task<bool> DeleteLabTestAsync(Guid id, Guid userId)
        {
            var labTest = await _context.LabTests
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (labTest == null)
            {
                return false;
            }

            _context.LabTests.Remove(labTest);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion

        #region Lab Test Results
        public async Task<IEnumerable<LabTestResult>> GetLabTestResultsAsync(Guid labTestId)
        {
            return await _context.LabTestResults
                .Where(r => r.LabTestId == labTestId)
                .OrderBy(r => r.ComponentName)
                .ToListAsync();
        }

        public async Task<LabTestResult> GetLabTestResultByIdAsync(Guid id)
        {
            return await _context.LabTestResults
                .Include(r => r.History)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<LabTestResult> CreateLabTestResultAsync(LabTestResult labTestResult)
        {
            labTestResult.Id = Guid.NewGuid();
            labTestResult.CreatedAt = DateTime.UtcNow;
            labTestResult.UpdatedAt = DateTime.UtcNow;

            _context.LabTestResults.Add(labTestResult);
            await _context.SaveChangesAsync();

            return labTestResult;
        }

        public async Task<LabTestResult> UpdateLabTestResultAsync(LabTestResult labTestResult)
        {
            // Before updating, create a history record
            var history = new LabTestResultHistory
            {
                Id = Guid.NewGuid(),
                LabTestResultId = labTestResult.Id,
                TestDate = labTestResult.UpdatedAt,
                Value = labTestResult.Value,
                Unit = labTestResult.Unit,
                ReferenceRange = labTestResult.ReferenceRange,
                AbnormalFlag = labTestResult.AbnormalFlag,
                CreatedAt = DateTime.UtcNow
            };

            _context.LabTestResultHistory.Add(history);

            // Now update the result
            labTestResult.UpdatedAt = DateTime.UtcNow;
            _context.LabTestResults.Update(labTestResult);
            
            await _context.SaveChangesAsync();

            return labTestResult;
        }

        public async Task<bool> DeleteLabTestResultAsync(Guid id)
        {
            var labTestResult = await _context.LabTestResults
                .FirstOrDefaultAsync(r => r.Id == id);

            if (labTestResult == null)
            {
                return false;
            }

            _context.LabTestResults.Remove(labTestResult);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion

        #region Lab Test Result History
        public async Task<IEnumerable<LabTestResultHistory>> GetLabTestResultHistoryAsync(Guid labTestResultId)
        {
            return await _context.LabTestResultHistory
                .Where(h => h.LabTestResultId == labTestResultId)
                .OrderByDescending(h => h.TestDate)
                .ToListAsync();
        }

        public async Task<LabTestResultHistory> CreateLabTestResultHistoryAsync(LabTestResultHistory history)
        {
            history.Id = Guid.NewGuid();
            history.CreatedAt = DateTime.UtcNow;

            _context.LabTestResultHistory.Add(history);
            await _context.SaveChangesAsync();

            return history;
        }
        #endregion

        #region Specialized Queries
        public async Task<IEnumerable<LabTestResult>> GetAbnormalLabResultsAsync(Guid userId, DateTime? startDate = null, DateTime? endDate = null, int limit = 50, int offset = 0)
        {
            var query = _context.LabTestResults
                .Join(_context.LabTests,
                    result => result.LabTestId,
                    test => test.Id,
                    (result, test) => new { Result = result, Test = test })
                .Where(joined => joined.Test.UserId == userId && joined.Result.AbnormalFlag != "normal");

            if (startDate.HasValue)
            {
                query = query.Where(joined => joined.Test.CollectionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(joined => joined.Test.CollectionDate <= endDate.Value);
            }

            return await query
                .OrderByDescending(joined => joined.Test.CollectionDate)
                .Skip(offset)
                .Take(limit)
                .Select(joined => joined.Result)
                .ToListAsync();
        }

        public async Task<IEnumerable<LabTestResult>> GetLabResultsByComponentAsync(Guid userId, string componentName, DateTime? startDate = null, DateTime? endDate = null, int limit = 50, int offset = 0)
        {
            var query = _context.LabTestResults
                .Join(_context.LabTests,
                    result => result.LabTestId,
                    test => test.Id,
                    (result, test) => new { Result = result, Test = test })
                .Where(joined => joined.Test.UserId == userId && joined.Result.ComponentName == componentName);

            if (startDate.HasValue)
            {
                query = query.Where(joined => joined.Test.CollectionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(joined => joined.Test.CollectionDate <= endDate.Value);
            }

            return await query
                .OrderByDescending(joined => joined.Test.CollectionDate)
                .Skip(offset)
                .Take(limit)
                .Select(joined => joined.Result)
                .ToListAsync();
        }
        #endregion
    }
}
