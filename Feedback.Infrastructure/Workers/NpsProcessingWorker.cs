using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feedback.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using Feedback.Application.Services;


namespace Feedback.Infrastructure.Workers
{
    public class NpsProcessingWorker : BackgroundService
    {
        private readonly ILogger<NpsProcessingWorker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public NpsProcessingWorker(ILogger<NpsProcessingWorker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("NPS Processing Worker esta iniciando em: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {

                // ... Lógica do trabalho ...

                _logger.LogInformation("Worker executando a tarefa periódica em: {time}", DateTimeOffset.Now);
                try
                {
                    using(var scope = _serviceScopeFactory.CreateScope())
                    {
                        var feedbackRepository = scope.ServiceProvider.GetRequiredService<IFeedbackNpsRepository>();
                        var calculator = new NpsCalculatorService();


                        //Define o periodo, por exemplo o mes anterior
                        var today = DateTime.UtcNow;
                        var startDate = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
                        var endDate = new DateTime(today.Year, today.Month, 1).AddTicks(-1);

                        // Busca os feedbacks do período
                        var feedbacks = await feedbackRepository.GetFeedbacksByPeriodAsync(startDate, endDate);

                        if (feedbacks.Any())
                        {
                            var scores = feedbacks.Select(f => f.Score).ToList();
                            var npsScore = calculator.CalculateNps(scores);

                            _logger.LogInformation("NPS Calculado para o período de {StartDate} a {EndDate}: {Score}", startDate.ToShortDateString(), endDate.ToShortDateString(), npsScore.ToString("F2"));

                            //Salvar o resultado no banco de dados na tabela de relatórios...
                        }
                        else
                        {
                            _logger.LogInformation("Nenhum feedback encontrado para o período de {StartDate} a {EndDate}.", startDate.ToShortDateString(), endDate.ToShortDateString());
                        }

                    }
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, "Ocorreu um erro ao processar o NPS do período.");
                }

                // Espera 24 horas para a próxima execução
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }

            _logger.LogInformation("NPS Processing Worker está parando.");
        }
    }
}
