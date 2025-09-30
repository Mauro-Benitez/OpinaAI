using Feedback.Application.InputModels;
using Feedback.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Feedback.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeedbackNpsController : Controller
    {
        private readonly IFeedbackNpsService _feedbackNpsService;      

        public FeedbackNpsController(IFeedbackNpsService feedbackNpsService)
        {
            _feedbackNpsService = feedbackNpsService;
           
        }

        [HttpPost("NPS")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] FeedbackInput feedback)
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
