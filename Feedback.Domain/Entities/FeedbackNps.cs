using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.Domain.Entities
{
    public class FeedbackNps
    {

        public Guid Id { get; private set; }
        public string UserId { get; private set; }
        public int Score { get; private set; }
        public string? Comment { get; private set; }
        public DateTime SubmittedDate { get; private set; }

        public FeedbackNps()
        {
        }

        public FeedbackNps(string userId, int score, string? comment)
        {

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));
            }

            if (score < 0 || score > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 0 and 10.");
            }
            
            Id = Guid.NewGuid();
            UserId = userId;
            Score = score;
            Comment = comment;
            SubmittedDate = DateTime.UtcNow;
        }
    }
}
