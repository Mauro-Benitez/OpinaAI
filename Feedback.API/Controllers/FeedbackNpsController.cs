using Feedback.Application.InputModels;
using Feedback.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Feedback.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackNpsController : Controller
    {
        private readonly IFeedbackNpsService _feedbackNpsService;      

        public FeedbackNpsController(IFeedbackNpsService feedbackNpsService)
        {
            _feedbackNpsService = feedbackNpsService;
           
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] FeedbackInput feedback)
        {
            if (feedback == null) return BadRequest();

            try
            {
                var result = await _feedbackNpsService.CreateFeedbackAsync(feedback);
                return StatusCode(201, result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }


       

    }
}
