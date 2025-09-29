using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.Application.InputModels
{
    public class FeedbackInput
    {
        public string UserId { get; set; }

        [Range(0, 10, ErrorMessage = "O Score deve ser um número inteiro entre 0 e 10.")]
        public int Score { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 2 and 50 characters.")]
        public string? Comment { get; set; }
        public DateTime SubmittedDate { get; set; }
    }
}
