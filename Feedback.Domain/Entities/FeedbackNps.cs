using Feedback.Domain.Enums;
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
        public string Name { get; private set; }
        public int Score { get; private set; }
        public string? Comment { get; private set; }
        public DateTime SubmittedDate { get; private set; }
        public Sentiment Sentiment { get; private set; } 
        public string? Topics { get; private set; }

        public FeedbackNps()
        {
        }

        public FeedbackNps(string userId, int score, string? comment, string name)
        {

            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));


            if (score < 0 || score > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 0 and 10.");
            }
            
            Id = Guid.NewGuid();
            UserId = userId;
            Score = score;
            Comment = comment;
            SubmittedDate = DateTime.UtcNow;
            Sentiment = Sentiment.NotAnalyzed;
            Topics = null;
            Name = name;
        }

        public void SetSentiment(Sentiment sentiment)
        {
            Sentiment = sentiment;
        }

        public void SetTopics(IEnumerable<string> topics)
        {
            if(topics != null && topics.Any())
            {
                Topics = string.Join(";", topics);
            }
        }
    }
}
