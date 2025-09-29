<<<<<<< HEAD
﻿using Feedback.Application.Interfaces;
using Feedback.Application.Services;
using Feedback.Domain.Entities;
=======
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
>>>>>>> a9fb3b7aab2552b65667cbd05f36e0cca3067de0
using Feedback.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
=======
using System.Runtime.InteropServices;
using Feedback.Application.Services;
>>>>>>> a9fb3b7aab2552b65667cbd05f36e0cca3067de0


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
<<<<<<< HEAD
                        var reportRepository = scope.ServiceProvider.GetRequiredService<IReportRepository>(); 
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
=======
>>>>>>> a9fb3b7aab2552b65667cbd05f36e0cca3067de0
                        var calculator = new NpsCalculatorService();


                        //Define o periodo, por exemplo o mes anterior
                        var today = DateTime.UtcNow;
<<<<<<< HEAD

                        //Pegamos o primeiro dia do mês atual (ex: 01/09/2025)
                        var firstDayOfCurrentMonth = new DateTime(today.Year, today.Month, 1);

                        //Subtraimos um mês para obter o primeiro dia do mês anterior (ex: 01/08/2025)
                        var firstDayOfPreviusMonth = firstDayOfCurrentMonth.AddMonths(-1);

                        //Especificamos que as datas são UTC
                        var startDate = DateTime.SpecifyKind(firstDayOfPreviusMonth, DateTimeKind.Utc);
                        var endDate = DateTime.SpecifyKind(firstDayOfCurrentMonth, DateTimeKind.Utc);
=======
                        var startDate = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
                        var endDate = new DateTime(today.Year, today.Month, 1).AddTicks(-1);
>>>>>>> a9fb3b7aab2552b65667cbd05f36e0cca3067de0

                        // Busca os feedbacks do período
                        var feedbacks = await feedbackRepository.GetFeedbacksByPeriodAsync(startDate, endDate);

                        if (feedbacks.Any())
                        {
                            var scores = feedbacks.Select(f => f.Score).ToList();
                            var npsScore = calculator.CalculateNps(scores);

                            _logger.LogInformation("NPS Calculado para o período de {StartDate} a {EndDate}: {Score}", startDate.ToShortDateString(), endDate.ToShortDateString(), npsScore.ToString("F2"));

<<<<<<< HEAD
                            // ----- Logica para salvar o relatório

                            // 1. Cria a nova entidade de relatório
                            var report = new Report(startDate, npsScore);

                            // 2. Adiciona o relatório ao repositório
                            await reportRepository.AddAsync(report);

                            // 3. Salva as mudanças no banco de dados
                            await unitOfWork.SaveChangesAsync(stoppingToken);

                            _logger.LogInformation("Relatório do mês {Month} salvo com sucesso.", startDate.ToString("yyyy-MM"));

                            // ------------------------------------
=======
                            //Salvar o resultado no banco de dados na tabela de relatórios...
>>>>>>> a9fb3b7aab2552b65667cbd05f36e0cca3067de0
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
<<<<<<< HEAD
                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
=======
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
>>>>>>> a9fb3b7aab2552b65667cbd05f36e0cca3067de0
            }

            _logger.LogInformation("NPS Processing Worker está parando.");
        }
    }
}
