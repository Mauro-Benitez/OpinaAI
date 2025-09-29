using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.Domain.Entities
{
    public class Report
    {      

        public Guid Id { get; private set; }


        /// <summary>
        /// O primeiro dia do mês ao qual este relatório se refere.
        /// Ex: 2025-09-01 00:00:00
        /// </summary>
        public DateTime ReportMonth { get; private set; }

        /// <summary>
        /// O score final do NPS calculado para o mês, variando de -100 a 100.
        /// </summary>
        public double FinalNpsScore { get; private set; }

        /// <summary>
        /// A data e hora em que este relatório foi processado pelo worker.
        /// </summary>
        public DateTime ProcessingDate { get; private set; }


        /// <summary>
        /// Link para o arquivo de relatório detalhado (PDF, CSV) no S3 ou outro storage.
        /// Será opcional para o MVP. 
                    /// </summary>
        public string? FileUrl { get; private set; }

        //EF
        public Report()
        {
        }


        public Report(DateTime reportMonth, double finalNpsScore)
        {
            var starMonth = new DateTime(reportMonth.Year, reportMonth.Month, 1);
            ReportMonth = DateTime.SpecifyKind(starMonth, DateTimeKind.Utc);
            FinalNpsScore = finalNpsScore;
            ProcessingDate = DateTime.UtcNow;
            FileUrl = null;
            Id = Guid.NewGuid();
        }

        public void SetFileUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                FileUrl = url;
            }
        }
    }
}
