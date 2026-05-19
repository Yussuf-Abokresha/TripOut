using Microsoft.AspNetCore.Mvc;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Interfaces;
using Modules.Users.Application.Services;
using Microsoft.AspNetCore.Http;

namespace Modules.Users.Presentation.Controllers.v1
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController(UserService userService) : ControllerBase
    {
        /// <summary>
        /// Refresh token endpoint
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>

        [HttpPost("refresh-token")]
        public async Task<ActionResult<LoginUserResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto requestDto)
        {
            var loginUserResponse = await userService.RefreshUserAsnc(requestDto);
            return Ok(loginUserResponse);
        }


        /// <summary>
        /// Login endpoint
        /// </summary>
        /// <param name="request">Login credentials containing identifier and password</param>
        /// <returns>Authentication token</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginUserResponseDto>> Login([FromBody] LoginUserRequestDto request)
        {
            var loginResponse = await userService.LoginUserAsync(request);
            return Ok(loginResponse);
        }

        /// <summary>
        /// Register endpoint
        /// </summary>
        /// <param name="request">Registration details</param>
        /// <returns>Newly created user information</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OtpResponseDto>> Register([FromBody] CreateUserRequestDto request)
        {
            var otpResponse = await userService.CreateUserAsync(request);
            return Ok(otpResponse);
        }

        /// <summary>
        /// Verify OTP endpoint
        /// </summary>
        /// <param name="request">OTP verification details</param>
        /// <returns>Verification result</returns>
        [HttpPost("otp/verify")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginUserResponseDto>> VerifyOtp([FromBody] VerifyOtpRequestDto request)
        {
            var loginResponse = await userService.VerifyOtpAsync(request);
            return Ok(loginResponse);
        }

        /// <summary>
        /// Forget password endpoint
        /// </summary>
        /// <param name="request">Password reset details</param>
        /// <returns>Password reset confirmation</returns>
        [HttpPost("forget-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OtpResponseDto>> ForgetPassword([FromBody] ForgetPasswordRequestIdentifierDto request)
        {
            var otpResponse = await userService.ForgetPasswordAsync(request);
            return Ok(otpResponse);
        }
    }
}