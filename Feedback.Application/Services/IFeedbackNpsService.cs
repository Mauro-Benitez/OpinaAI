using Feedback.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.Application.Services
{
    public interface IFeedbackNpsService
    {

        Task<List<FeedbackNpsDTO>> GetFeedbacksByPeriodAsync(DateTime startDate, DateTime endDate);
        Task<List<FeedbackNpsDTO>> GetFeedbacksByCustomerIdAsync(string customerId);
        Task<FeedbackNpsDTO> GetFeedbackByIdAsync(Guid Id);
        Task<FeedbackNpsDTO> CreateFeedbackAsync(FeedbackNpsDTO feedbackDto);
        Task<bool> DeleteFeedbackAsync(Guid id);
    }
}
