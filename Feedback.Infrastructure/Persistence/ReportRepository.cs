using Feedback.Domain.Entities;
using Feedback.Domain.Repositories;
using Feedback.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;


namespace Feedback.Infrastructure.Persistence
{
    public class ReportRepository : IReportRepository
    {

        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
        }

        public async Task<Report?> GetLatestMonthlyReportAsync()
        {
            return await _context.Reports
                .AsNoTracking()
                .OrderByDescending(r => r.ReportMonth)
                .FirstOrDefaultAsync();
        }
    }
}
