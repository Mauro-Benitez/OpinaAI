using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.Application
{
    public class FeedbackNpsDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        [Range(0, 10, ErrorMessage = "O Score deve ser um número inteiro entre 0 e 10.")]
        public int Score { get; set; }

        [RegularExpression(@"^[A-Za-zÀ-ÿ]{2,50}$", ErrorMessage = "Invalid Comment.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 2 and 50 characters.")]
        public string? Comment { get; set; }
        public DateTime SubmittedDate { get; set; }

        
    }
}
