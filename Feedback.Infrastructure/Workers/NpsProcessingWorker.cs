
using Amazon.S3;
using Amazon.S3.Model;
using CsvHelper;
using Feedback.Application.Interfaces;
using Feedback.Application.Services;
using Feedback.Domain.Entities;
using Feedback.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Formats.Asn1;
using System.Globalization;



namespace Feedback.Infrastructure.Workers
{

    //Worker responsavel por calcular o NPS periodicamente e salvar na tabela Reports
    public class NpsProcessingWorker : BackgroundService
    {
        private readonly ILogger<NpsProcessingWorker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;

        public NpsProcessingWorker(ILogger<NpsProcessingWorker> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("NpsProcessingWorker esta iniciando em: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {

                //logica do Worker

                _logger.LogInformation("Worker executando a tarefa periódica em: {time}", DateTimeOffset.Now);
                try
                {
                    using(var scope = _serviceScopeFactory.CreateScope())
                    {
                        var feedbackRepository = scope.ServiceProvider.GetRequiredService<IFeedbackNpsRepository>();

                        var reportRepository = scope.ServiceProvider.GetRequiredService<IReportRepository>(); 
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                        var calculator = new NpsCalculatorService();


                        //Data atual
                        var today = DateTime.UtcNow;
                        
                        //Especifica o mes anterior
                        var firstDayOfCurrentMonth = new DateTime(today.Year, today.Month, 1);                        
                        var firstDayOfPreviusMonth = firstDayOfCurrentMonth.AddMonths(-1);

                        
                        var startDate = DateTime.SpecifyKind(firstDayOfPreviusMonth, DateTimeKind.Utc);
                        var endDate = DateTime.SpecifyKind(firstDayOfCurrentMonth, DateTimeKind.Utc);             
                                                
                        var feedbacks = await feedbackRepository.GetFeedbacksByPeriodAsync(startDate, endDate);

                        if (feedbacks.Any())
                        {
                            var scores = feedbacks.Select(f => f.Score).ToList();
                            var npsScore = calculator.CalculateNps(scores);

                            _logger.LogInformation("NPS Calculado para o período de {StartDate} a {EndDate}: {Score}", startDate.ToShortDateString(), endDate.ToShortDateString(), npsScore.ToString("F2"));
                                                        
                            
                            var report = new Report(startDate, npsScore);

                            try
                            {
                                var csvStream = GenerateCsvReport(feedbacks);
                                var reportFileName = $"NPS_Report_{startDate:yyyy-MM}.csv";
                                var fileUrl = await UploadToS3Async(csvStream, reportFileName);                               
                                report.SetFileUrl(fileUrl);
                                _logger.LogInformation("Relatório salvo no S3: {Path}", fileUrl);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Falha ao gerar e salvar o relatório CSV no S3.");
                            }

                            await reportRepository.AddAsync(report);                           
                            await unitOfWork.SaveChangesAsync(stoppingToken);

                            _logger.LogInformation("Relatório do mês {Month} salvo com sucesso.", startDate.ToString("yyyy-MM"));
                         

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

                //Time para próxima execução           

                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);

            }

            _logger.LogInformation("NpsProcessingWorker está parando.");
        }

        private Stream GenerateCsvReport(IEnumerable<FeedbackNps> feedbacks)
        {
            var memoryStream = new MemoryStream();

            using (var writer = new StreamWriter(memoryStream,leaveOpen:true))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                //cabeçalho
                csv.WriteField("Data");
                csv.WriteField("Score");
                csv.WriteField("Sentimento");
                csv.WriteField("Topicos");
                csv.NextRecord();

                //Dados
                foreach (var feedback in feedbacks)
                {
                    csv.WriteField(feedback.SubmittedDate.ToString("yyyy-MM-dd HH:mm"));
                    csv.WriteField(feedback.Score);
                    csv.WriteField(feedback.Sentiment.ToString());
                    csv.WriteField(feedback.Topics);
                    csv.NextRecord();
                }


            }

            memoryStream.Position = 0;
            return memoryStream;
        }


        private async Task<string> UploadToS3Async(Stream fileStream, string fileName)
        {
            var awsConfig = _configuration.GetSection("AWS:S3");
            var bucketName = awsConfig["BucketName"];
            var accessKey = awsConfig["AccessKey"];
            var secretKey = awsConfig["SecretKey"];
            var region = Amazon.RegionEndpoint.GetBySystemName(awsConfig["Region"]);

            using (var client = new AmazonS3Client(accessKey, secretKey, region))
            {
                var request = new PutObjectRequest
                {
                    InputStream = fileStream,
                    BucketName = bucketName,
                    Key = fileName,
                    
                };
                await client.PutObjectAsync(request);

                return $"https://{bucketName}.s3.{region.SystemName}.amazonaws.com/{fileName}";
            }


        }

    }


       

}
