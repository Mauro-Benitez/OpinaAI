using Feedback.Application.DTOs;
using Feedback.Application.Interfaces.Services;
using Feedback.Application.Services;
using Feedback.Domain.Enums;
using Feedback.Domain.Repositories;


namespace Feedback.Application.Services_Implementation
{
    public class DashboardService : IDashboardService
    {
        //private readonly NpsCalculatorService _npsCalculatorService;
        private readonly IFeedbackNpsRepository _feedbackRepository;
        private readonly IReportRepository _reportRepository;

        public DashboardService(IReportRepository reportRepository, IFeedbackNpsRepository feedbackRepository)
        {
            _reportRepository = reportRepository;
            _feedbackRepository = feedbackRepository;
        }


        public async Task<DashboardSummaryDTO> GetLastMonthSummary()
        {
            var latestReport = await _reportRepository.GetLatestMonthlyReportAsync();

            DateTime startDate, endDate;
            if(latestReport!=null)
            {
                startDate = latestReport.ReportMonth;
                endDate = latestReport.ReportMonth.AddMonths(1).AddTicks(-1);
            }
            else
            {
                var today = DateTime.UtcNow;
                startDate = DateTime.SpecifyKind(new DateTime(today.Year, today.Month, 1).AddMonths(-1), DateTimeKind.Utc);
                endDate = DateTime.SpecifyKind(startDate.AddMonths(1).AddTicks(-1), DateTimeKind.Utc);

            }

            var feedbacks = await _feedbackRepository.GetFeedbacksByPeriodAsync(startDate, endDate);
            var feedbackList = feedbacks.ToList();

            if(!feedbackList.Any() && latestReport == null)
            {
                return new DashboardSummaryDTO();// Retorna um resumo vazio
            }


            //Usa o Score do relatorio, se não houver o padrão e 0
            var npsScore = latestReport?.FinalNpsScore ?? 0;


            //Contar Sentimentos
            var positiveCount = feedbackList.Count(f => f.Sentiment == Sentiment.Positive);
            var negativeCount = feedbackList.Count(f => f.Sentiment == Sentiment.Negative);
            var neutralCount = feedbackList.Count(f => f.Sentiment == Sentiment.Neutral);
            var feedbackWithoutCommentsCount = feedbackList.Count(f => f.Comment == null || f.Comment == "");

            //Contar e agrupar os Top Tópicos
            var topicCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            var feedbacksWithTopics = feedbackList.Where(f => !string.IsNullOrWhiteSpace(f.Topics));

            foreach(var feedback in feedbacksWithTopics)
            {
                var topics = feedback.Topics.Split(";", StringSplitOptions.RemoveEmptyEntries);
                foreach(var topic in topics)
                {
                    var cleanTopic = topic.Trim();
                    if (topicCounts.ContainsKey(cleanTopic))
                    {
                        topicCounts[cleanTopic]++;
                    }
                    else
                    {
                        topicCounts[cleanTopic] = 1;
                    }
                }
            }

            //Ordena pelos topicos mais frequentes, pega os 5 primeiros converte em um objeto TopicCountDTOe retorna como uma lista.
            var topTopics = topicCounts.OrderByDescending(kvp => kvp.Value)
                                       .Take(5)
                                       .Select(kvp => new TopicCountDTO {Topic = kvp.Key, Count = kvp.Value })
                                       .ToList();


            //Monta o DTO e rotorna
            return new DashboardSummaryDTO
            {
                NpsScore = npsScore,
                TotalFeedbacks = feedbackList.Count,
                PositiveCount = positiveCount,
                NeutralCount = neutralCount,
                NegativeCount = negativeCount,
                FeedbackWithoutCommentsCount = feedbackWithoutCommentsCount,
                ReportFileKey = latestReport?.FileUrl,
                TopTopics = topTopics
                
            };

        }
    }
}
