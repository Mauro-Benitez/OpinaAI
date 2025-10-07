using Feedback.Application.DTO;
using Feedback.Application.InputModels;
using Feedback.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.Application.Interfaces.Services
{
    public interface IFeedbackNpsService
    {

        Task<List<FeedbackNpsDTO>> GetFeedbacksByPeriodAsync(DateTime startDate, DateTime endDate);
        Task<List<FeedbackNpsDTO>> GetFeedbacksByCustomerIdAsync(string customerId);
        Task<FeedbackNpsDTO> GetFeedbackByIdAsync(Guid Id);
        Task<FeedbackNpsDTO> CreateFeedbackAsync(FeedbackInput feedbackInput);
        Task<bool> DeleteFeedbackAsync(Guid id);
    }
}
