using Feedback.Domain.Entities;
using Feedback.Domain.Repositories;
using Feedback.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.Infrastructure.Persistence
{
    public class FeedbackNpsRepository : IFeedbackNpsRepository
    {
        private readonly AppDbContext _context;

        public FeedbackNpsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<FeedbackNps> AddAsync(FeedbackNps feedback)
        {
            await _context.Feedbacks.AddAsync(feedback);                 
            return feedback;
        }

        public async Task DeleteAsync(Guid id)
        {
            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.Id == id);

            if (feedback is null)
                throw new ArgumentException("Feedback not found");

            _context.Feedbacks.Remove(feedback);
           
        }  

        public Task<List<FeedbackNps?>> GetFeedbacksByCustomerIdAsync(string customerId)
        {
            var feedbacks = _context.Feedbacks
                .Where(f => f.UserId == customerId)
                .AsNoTracking()
                .ToListAsync();
         
            return feedbacks;
        }

        public Task<FeedbackNps?> GetFeedbackByIdAsync(Guid Id)
        {
            var feedback = _context.Feedbacks
                .FirstOrDefaultAsync(f => f.Id == Id);
                      
            return feedback;

        }

        public Task<List<FeedbackNps?>> GetFeedbacksByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var result = _context.Feedbacks
                .Where(f => f.SubmittedDate >= startDate && f.SubmittedDate <= endDate)
                .AsNoTracking()
                .ToListAsync();

            return result;
        }
    }
}
