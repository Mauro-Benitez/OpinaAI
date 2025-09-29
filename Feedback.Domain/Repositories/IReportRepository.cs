using Feedback.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.Domain.Repositories
{
    public interface IReportRepository
    {
        /// <summary>
        /// Adiciona um novo relatório ao banco de dados.
        /// </summary>
        Task AddAsync(Report report);


        /// <summary>
        /// Adiciona um novo relatório ao banco de dados.
        /// </summary>
       Task<Report?> GetLatestMonthlyReportAsync();
    }
}
