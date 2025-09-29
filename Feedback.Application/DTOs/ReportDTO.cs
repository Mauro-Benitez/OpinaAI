using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.Application.DTO
{
    public class ReportDTO
    {
        public Guid Id { get; set; }


        /// <summary>
        /// O primeiro dia do mês ao qual este relatório se refere.
        /// Ex: 2025-09-01 00:00:00
        /// </summary>
        public DateTime ReportMonth { get; set; }

        /// <summary>
        /// O score final do NPS calculado para o mês, variando de -100 a 100.
        /// </summary>
        public double FinalNpsScore { get; set; }

        /// <summary>
        /// A data e hora em que este relatório foi processado pelo worker.
        /// </summary>
        public DateTime ProcessingDate { get; set; }


        /// <summary>
        /// Link para o arquivo de relatório detalhado (PDF, CSV) no S3 ou outro storage.
        /// Será opcional para o MVP. 
        /// </summary>
        public string? FileUrl { get; set; }
    }
}
