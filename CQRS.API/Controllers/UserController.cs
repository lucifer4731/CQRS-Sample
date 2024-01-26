using CQRS.Application.CQRS.UserCQRS.Command;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CQRS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;
        //private readonly ILogger logger;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
            //this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(CreateUserCommand createUserCommand)
        {
            try
            {
                var result = await mediator.Send(createUserCommand);
                //logger.LogInformation($"crete new user : - current user id : {HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId").Value} - created user : {createUserCommand.UserName} - Date:{DateTime.Now} - status : successfull ");
                return Ok(result.UserId);
            }
            catch (Exception ex)
            {
                //logger.LogInformation($"crete new user : - current user id : {HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId").Value} - Date:{DateTime.Now} - status : failed - Erroe Message : {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
