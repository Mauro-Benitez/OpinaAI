
﻿using Feedback.Application.Interfaces;
using Feedback.Application.Services;
using Feedback.Domain.Entities;
using Feedback.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;



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

                        var reportRepository = scope.ServiceProvider.GetRequiredService<IReportRepository>(); 
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                        var calculator = new NpsCalculatorService();


                        //Define o periodo, por exemplo o mes anterior
                        var today = DateTime.UtcNow;


                        //Pegamos o primeiro dia do mês atual (ex: 01/09/2025)
                        var firstDayOfCurrentMonth = new DateTime(today.Year, today.Month, 1);

                        //Subtraimos um mês para obter o primeiro dia do mês anterior (ex: 01/08/2025)
                        var firstDayOfPreviusMonth = firstDayOfCurrentMonth.AddMonths(-1);

                        //Especificamos que as datas são UTC
                        var startDate = DateTime.SpecifyKind(firstDayOfPreviusMonth, DateTimeKind.Utc);
                        var endDate = DateTime.SpecifyKind(firstDayOfCurrentMonth, DateTimeKind.Utc);                

                        // Busca os feedbacks do período
                        var feedbacks = await feedbackRepository.GetFeedbacksByPeriodAsync(startDate, endDate);

                        if (feedbacks.Any())
                        {
                            var scores = feedbacks.Select(f => f.Score).ToList();
                            var npsScore = calculator.CalculateNps(scores);

                            _logger.LogInformation("NPS Calculado para o período de {StartDate} a {EndDate}: {Score}", startDate.ToShortDateString(), endDate.ToShortDateString(), npsScore.ToString("F2"));


                            // ----- Logica para salvar o relatório

                            // 1. Cria a nova entidade de relatório
                            var report = new Report(startDate, npsScore);

                            // 2. Adiciona o relatório ao repositório
                            await reportRepository.AddAsync(report);

                            // 3. Salva as mudanças no banco de dados
                            await unitOfWork.SaveChangesAsync(stoppingToken);

                            _logger.LogInformation("Relatório do mês {Month} salvo com sucesso.", startDate.ToString("yyyy-MM"));

                            // ------------------------------------                           

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
