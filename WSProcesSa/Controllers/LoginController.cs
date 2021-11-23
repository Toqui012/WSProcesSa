using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSProcesSa.Classes;
using WSProcesSa.DTO;
using WSProcesSa.Request;
using WSProcesSa.Services;

namespace WSProcesSa.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        // Metodo de Autentificacion
        [HttpPost]
        [Route("addlogin")]

        public async Task<IActionResult> Autentification([FromBody] AuthRequest authRequest)
        {
            try
            {
                List<Error> errors = new List<Error>();
                if (!string.IsNullOrWhiteSpace(authRequest.Email) || !string.IsNullOrWhiteSpace(authRequest.Password))
                {
                  //  authRequest.Password = Encrypt.GetSHA256(authRequest.Password);
                    var userResponse = _userService.Auth(authRequest);
                    if (userResponse != null)
                    {
                        return Ok(new Response()
                        {
                            Data = new UserResponse(userResponse),
                            Errors = errors
                        });
                    }
                    else
                    {
                        return BadRequest(userResponse);
                    }
                }
                else
                {
                    return BadRequest(authRequest);
                }
                
            }
            catch (Exception err)
            {
                Response response = new Response();
                response.Errors.Add(new Error()
                {
                    Id = 1,
                    Status = "Internal Server Error",
                    Code = 500,
                    Title = err.Message,
                    Detail = err.InnerException != null ? err.InnerException.ToString() : err.Message
                });
                return StatusCode(500, response);
            }
        }
    }
}
