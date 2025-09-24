using Feedback.Domain.Entities;
using Feedback.Domain.Repositories;
using Feedback.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Feedback.Application.Services
{
    public class FeedbackNpsService : IFeedbackNpsService
    {
        private readonly IFeedbackNpsRepository _feedbackNpsRepository;
        private readonly AppDbContext _appDbContext;

        public FeedbackNpsService(IFeedbackNpsRepository feedbackNpsRepository, AppDbContext appDbContext)
        {
            _feedbackNpsRepository = feedbackNpsRepository;
            _appDbContext = appDbContext;
        }

        public async Task<FeedbackNpsDTO> CreateFeedbackAsync(FeedbackNpsDTO feedbackDto)
        {
           var feedback = new FeedbackNps(
                feedbackDto.UserId,
                feedbackDto.Score,
                feedbackDto.Comment
            );

            var result =  await _feedbackNpsRepository.AddAsync(feedback);
            await _appDbContext.SaveChangesAsync();

            return MapToDTO(result);

        }

        public async Task<bool> DeleteFeedbackAsync(Guid id)
        {
            var feedbackToDelete = await _feedbackNpsRepository.GetFeedbackByIdAsync(id);

            if (feedbackToDelete == null)
            {
                return false; // Feedback not found
            }

            await _feedbackNpsRepository.DeleteAsync(id);
            await _appDbContext.SaveChangesAsync();
            return true;

        }
        public async Task<FeedbackNpsDTO> GetFeedbackByIdAsync(Guid Id)
        {
           var result = await _feedbackNpsRepository.GetFeedbackByIdAsync(Id);
           
          if (result == null)
            {
                return null;
            }

           return MapToDTO(result);
        }

        public async Task<List<FeedbackNpsDTO>> GetFeedbacksByCustomerIdAsync(string customerId)
        {
            var result =  await _feedbackNpsRepository.GetFeedbacksByCustomerIdAsync(customerId);

            if (result == null)
            {
                return null;
            }

            return result.Select(f => MapToDTO(f)).ToList();
        }

        public async Task<List<FeedbackNpsDTO>> GetFeedbacksByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var result = await _feedbackNpsRepository.GetFeedbacksByPeriodAsync(startDate, endDate);

            return result.Select(f => MapToDTO(f)).ToList();
        }

        private FeedbackNpsDTO MapToDTO(FeedbackNps feedback)
        {
            return new FeedbackNpsDTO
            {
                Id = feedback.Id,
                UserId = feedback.UserId,
                Score = feedback.Score,
                Comment = feedback.Comment,
                SubmittedDate = feedback.SubmittedDate
            };
        }
    }
}
