using Feedback.Domain.Entities;

namespace Feedback.Domain.Repositories
{
    public interface IFeedbackNpsRepository
    {
        Task<List<FeedbackNps>> GetAllAsync();
        Task<List<FeedbackNps>> GetFeedbacksByCustomerIdAsync(string customerId);
        Task CreateAsync(FeedbackNps feedback);
        Task DeleteAsync(Guid id);
    }
}
