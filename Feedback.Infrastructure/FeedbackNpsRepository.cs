using Feedback.Domain.Entities;
using Feedback.Domain.Repositories;
using Feedback.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.Infrastructure
{
    public class FeedbackNpsRepository : IFeedbackNpsRepository
    {
        private readonly AppDbContext _context;

        public async Task CreateAsync(FeedbackNps feedback)
        {
            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(Guid id)
        {
            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.Id == id);

            if (feedback is null)
                throw new ArgumentException("Feedback not found");

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
        }

        public Task<List<FeedbackNps>> GetAllAsync()
        {
            return _context.Feedbacks.ToListAsync();
        }

        public Task<List<FeedbackNps>> GetFeedbacksByCustomerIdAsync(string customerId)
        {
            var feedbacks = _context.Feedbacks
                .Where(f => f.UserId == customerId)
                .ToListAsync();

            if (feedbacks is null)
                throw new ArgumentException("Feedback not found");

            return feedbacks;
        }
    }
}
