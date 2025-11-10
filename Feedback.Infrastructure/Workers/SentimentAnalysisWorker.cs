using Feedback.Application.Interfaces;
using Feedback.Application.Interfaces.Services;
using Feedback.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace Feedback.Infrastructure.Workers
{

    //Working responsavel por analisar os sentimentos dos feedbacks com comentarios salvos no banco
    public class SentimentAnalysisWorker : BackgroundService
    {
        private readonly ILogger<SentimentAnalysisWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public SentimentAnalysisWorker(ILogger<SentimentAnalysisWorker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Sentiment Analysis está iniciando");


            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Sentiment Analysis Worker iniciando a tarefa em: {Time}",DateTimeOffset.Now);

                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var feedbackRepository = scope.ServiceProvider.GetRequiredService<IFeedbackNpsRepository>();
                        var sentimentService = scope.ServiceProvider.GetRequiredService<ISentimentAnalysisService>();
                        var unitOfWorker = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                        //Busca feedbacks com comentários que ainda não forma analisados
                        var feedbackToAnalyze = await feedbackRepository.GetUnanalyzedFeedbacksAsync();

                        if (feedbackToAnalyze.Any())
                        {
                            _logger.LogInformation("{Count} feedbacks encontrados para análise de sentimento", feedbackToAnalyze.Count());

                            foreach(var feedback in feedbackToAnalyze)
                            {
                                if (!string.IsNullOrEmpty(feedback.Comment))
                                {
                                    var analysisResult = await sentimentService.AnalizeTextAsync(feedback.Comment);
                                    feedback.SetSentiment(analysisResult.Sentiment);
                                    feedback.SetTopics(analysisResult.Topics); 

                                }
                            }

                            //Salva todas as alterações de uma vez
                            await unitOfWorker.SaveChangesAsync(stoppingToken);
                            _logger.LogInformation("Análise de sentimento concluida e salva para {Count} feedbacks",feedbackToAnalyze.Count());
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, "Ocorreu um erro no Sentiment Analysis Worker");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            

            
        }
    }
}
