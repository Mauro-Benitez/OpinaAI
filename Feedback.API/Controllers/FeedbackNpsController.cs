using Feedback.Application;
using Feedback.Application.Interfaces;
using Feedback.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Feedback.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeedbackNpsController : Controller
    {
        private readonly IFeedbackNpsService _feedbackNpsService;
        private readonly IFeedbackNpsRepository _feedbackNpsRepository;

        public FeedbackNpsController(IFeedbackNpsService feedbackNpsService, IFeedbackNpsRepository feedbackNpsRepository)
        {
            _feedbackNpsService = feedbackNpsService;
            _feedbackNpsRepository = feedbackNpsRepository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] FeedbackNpsDTO feedback)
        {
            if (feedback == null) return BadRequest();

            try
            {
                var result = await _feedbackNpsService.CreateFeedbackAsync(feedback);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }


       

    }
}
