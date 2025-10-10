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

        public ReportController(IReportService reportService, IDashboardService dashboardService)
        {
            _reportService = reportService;
            _dashboardService = dashboardService;
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



    }
}
