using API.Service.AI.Openai.ChatGPT.Domain.Implementations.Interfaces;
using API.Service.AI.Openai.ChatGPT.Models.Input;
using API.Service.AI.Openai.ChatGPT.Models.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Service.AI.Openai.ChatGPT.Service.Controllers
{
    [Route("v1/chatgpt")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "chatgpt")]
    public class ChatGPTController : ControllerBase
    {
        private readonly IChatGPTService _chatGPTService;

        public ChatGPTController(
                IChatGPTService chatGPTService
            )
        {
            _chatGPTService = chatGPTService;
        }

        [HttpPost("question")]
        [SwaggerOperation(Summary = "")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status417ExpectationFailed)]
        public IActionResult Question(ChatGPTQuestionInput Obj)
        {
            var Data = _chatGPTService.Question<QuestionOutput>(Obj).Result;
            var Message = ((int)Data.StatusCode).Equals(200) ? Data.Items![0].Answer : Data.Items![0].Message;

            return StatusCode((int)Data.StatusCode, Message);
        }
    }
}
