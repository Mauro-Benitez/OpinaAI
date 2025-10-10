using Feedback.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.Application.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDTO> GetLastMonthSummary();

    }
}
