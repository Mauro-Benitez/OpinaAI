using Feedback.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.Application.Models
{
    public class FeedbackAnalysisResult
    {
        public Sentiment Sentiment { get; set; }
        public List<string> Topics { get; set; } = new List<string>();

    }
}
