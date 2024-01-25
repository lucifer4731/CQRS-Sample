using CQRS.Application.CQRS.UserCQRS.Query;
using CQRS.Application.Dto;
using CQRS.Application.Utilities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly EncryptionUtility encryptionUtility;
        private readonly IMediator mediator;
        private readonly ILogger logger;

        public AuthenticationController(EncryptionUtility encryptionUtility,IMediator mediator,ILogger logger)
        {
            this.encryptionUtility = encryptionUtility;
            this.mediator = mediator;
            this.logger = logger;
        }

        public async Task<IActionResult> SigneIn([FromQuery] GetUserByUserNameQuery getUserByUserNameQuery)
        {
            var user = await mediator.Send(getUserByUserNameQuery);
            try
            {
                if (user.User == null)
                    return BadRequest("Invalid Username or Password");

                if (user.User.Deleted)
                    return BadRequest("User is not active");

                var hashPassword = encryptionUtility.GetSHA256(getUserByUserNameQuery.Password);

                if (user.User.Password != hashPassword) return BadRequest("Invalid username or password");

                var token = encryptionUtility.GenerateToken(user.User.Id);

               
                var result = new AuthenticateDto
                {
                    UserName = user.User.UserName,
                    Token = token
                };
                logger.LogInformation($"User login : {user.User.FullName} - {user.User.Id} Date:{DateTime.Now} - status : successfull ");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"User login :  {user.User.FullName} - {user.User.Id} Date:{DateTime.Now} - status : failed - Exception : {ex.Message}");
                return BadRequest("Request failed, please try again");
            }
        }
    }
}
