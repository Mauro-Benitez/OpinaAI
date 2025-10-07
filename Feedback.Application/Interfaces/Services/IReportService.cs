using Feedback.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.Application.Interfaces.Services
{
    public interface IReportService
    {
        Task<ReportDTO> GetLatestMonthlyReportAsync();
    }
}
