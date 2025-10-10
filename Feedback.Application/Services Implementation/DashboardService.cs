using Feedback.Application.DTOs;
using Feedback.Application.Interfaces.Services;
using Feedback.Application.Services;
using Feedback.Domain.Enums;
using Feedback.Domain.Repositories;


namespace Feedback.Application.Services_Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly NpsCalculatorService _npsCalculatorService;
        private readonly IFeedbackNpsRepository _feedbackRepository;

        public DashboardService(NpsCalculatorService npsCalculatorService, IFeedbackNpsRepository feedbackRepository)
        {
            _npsCalculatorService = npsCalculatorService;
            _feedbackRepository = feedbackRepository;
        }


        public async Task<DashboardSummaryDTO> GetLastMonthSummary()
        {
            var today = DateTime.UtcNow;
            var startDate = DateTime.SpecifyKind(new DateTime(today.Year, today.Month, 1).AddMonths(-1), DateTimeKind.Utc);
            var endDate = DateTime.SpecifyKind(startDate.AddMonths(1).AddTicks(-1), DateTimeKind.Utc);

            var feedbacks = await _feedbackRepository.GetFeedbacksByPeriodAsync(startDate, endDate);
            var feedbackList = feedbacks.ToList();

            if (!feedbackList.Any())
            {
                return new DashboardSummaryDTO(); // Retorna um resumo vazio
            }

            //Calcular o Score NPS
            var npsScore = _npsCalculatorService.CalculateNps(feedbackList.Select(f => f.Score).ToList());

            //Contar Sentimentos
            var positiveCount = feedbackList.Count(f => f.Sentiment == Sentiment.Positive);
            var negativeCount = feedbackList.Count(f => f.Sentiment == Sentiment.Negative);
            var neutralCount = feedbackList.Count(f => f.Sentiment == Sentiment.Neutral);

            //Contar e agrupar os Top Tópicos
            var topicCounts = new Dictionary<string, int>();
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
                TopTopics = topTopics
            };

        }
    }
}
