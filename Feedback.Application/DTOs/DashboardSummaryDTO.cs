using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.Application.DTOs
{
    public class DashboardSummaryDTO
    {
        public double NpsScore { get; set; }
        public int TotalFeedbacks { get; set; }
        public int PositiveCount { get; set; }
        public int NegativeCount { get; set; }
        public int NeutralCount { get; set; }
        public int FeedbackWithoutCommentsCount { get; set; }
        public List<TopicCountDTO> TopTopics { get; set; } = new List<TopicCountDTO>();            


    }

    public class TopicCountDTO
    {
        public string Topic { get; set; }
        public int Count { get; set; }
    }
}
