using Feedback.Application.DTO;
using Feedback.Application.Interfaces;
using Feedback.Domain.Entities;
using Feedback.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.Application.Services
{
    public class ReportService :  IReportService
    {
        private readonly IReportRepository _reportRepository;       

        public ReportService(IReportRepository reportRepository, IUnitOfWork unitOfWork)
        {
            _reportRepository = reportRepository;
            
        }

        public async Task<ReportDTO> GetLatestMonthlyReportAsync()
        {
           var report = await _reportRepository.GetLatestMonthlyReportAsync();
           
            if (report == null) return null;

            return MapToDTO(report);
        }

        private ReportDTO MapToDTO(Report report)
        {
            return new ReportDTO
            {
                Id = report.Id,
                ReportMonth = report.ReportMonth,
                FinalNpsScore = report.FinalNpsScore,
                ProcessingDate = report.ProcessingDate,
                FileUrl = report.FileUrl

            };
        }
    }
}
