using Feedback.Application.DTO;
using Feedback.Application.Interfaces;
using Feedback.Domain.Entities;
using Feedback.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Feedback.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Retorna o último relatório mensal de NPS processado.
        /// </summary>
        [HttpGet("monthly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
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


        
    }
}
