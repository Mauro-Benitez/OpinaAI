using Feedback.Domain.Entities;

namespace Feedback.Domain.Repositories
{
    public interface IFeedbackNpsRepository
    {
        Task<List<FeedbackNps>> GetFeedbacksByCustomerIdAsync(string customerId);
        Task<FeedbackNps> GetFeedbackByIdAsync(Guid Id);
        Task<FeedbackNps> AddAsync(FeedbackNps feedback);
        Task DeleteAsync(Guid id);
        Task<List<FeedbackNps>> GetFeedbacksByPeriodAsync(DateTime startDate, DateTime endDate);

    }
}
