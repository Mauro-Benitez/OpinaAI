using Amazon.S3;
using Feedback.Application.DTO;
using Feedback.Application.Interfaces.Services;
using Feedback.Domain.Entities;
using Feedback.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Feedback.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IDashboardService _dashboardService;
        private readonly IStorageService _storageService;

        public ReportController(IReportService reportService, IDashboardService dashboardService, IStorageService storageService)
        {
            _reportService = reportService;
            _dashboardService = dashboardService;
            _storageService = storageService;
        }

        /// <summary>
        /// Retorna o último relatório mensal de NPS processado.
        /// </summary>
        [HttpGet("monthly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]    
        public async Task<IActionResult> GetMonthlyReport()
        {
            var report = await _reportService.GetLatestMonthlyReportAsync();
            if (report == null)
            {
                // Se o serviço não encontrou nenhum relatório, retornamos 404 Not Found.
                return NotFound("Nenhum relatório mensal encontrado.");
            }

            // Se encontrou, retornamos 200 OK com os dados do relatório.
            return Ok(report);
        }


        /// <summary>
        /// Retorna o NPS, Contagem dos comentarios, Lista de tópicos mais mencionados
        /// </summary>
        [HttpGet("Summary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]     
        public async Task<IActionResult> GetDashboardSummary()
        {
            var report = await _dashboardService.GetLastMonthSummary();
            if (report == null)
            {
                // Se o serviço não encontrou nenhum relatório, retornamos 404 Not Found.
                return NotFound("Nenhuma informação para exibir.");
            }

            // Se encontrou, retornamos 200 OK com os dados do relatório.
            return Ok(report);
        }



        [HttpGet("Download")]
        [ProducesResponseType(StatusCodes.Status302Found)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadReport([FromQuery] string fileKey)
        {
            if (string.IsNullOrEmpty(fileKey))
            {
                return BadRequest("O nome do arquivo (filekey) é obrigatório.");
            }

            try
            {
                var presignedUrl = await _storageService.GetPresignedUrlAsync(fileKey);
                return Redirect(presignedUrl);
            }
            catch(AmazonS3Exception ex)
            {
                if(ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound("O arquivo solicitado não foi encontrado");
                }
                throw;
            }

            catch (Exception ex)            {
               
                return StatusCode(500, $"Erro interno ao processar a solicitação.{ex.Message}");
            }
        }



    }
}
