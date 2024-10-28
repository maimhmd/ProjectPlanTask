using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneTrack.PM.APIs.Errors;
using OneTrack.PM.Core;
using OneTrack.PM.Core.Services;
using OneTrack.PM.Entities.DTOs.Security;
using OneTrack.PM.Entities.Helpers;
using OneTrack.PM.Entities.Models;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Repositories.Specifications.Security;

namespace OneTrack.PM.APIs.Controllers.Security
{
    [Route("api/security/[controller]")]
    public class AccountController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authManager;
        public AccountController(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticationService authManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authManager = authManager;
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(Tokens), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("Login")]
        public async Task<ActionResult<Tokens>> Login([FromBody] LoginDTO loginDTO)
        {
            var contact = await _unitOfWork.Repository<SecContact>().GetWithSpecAsync(new ContactsSpecifications(loginDTO.Email), trackChanges: false);
            if (contact == null)
                return BadRequest(new ApiResponse(400, "اسم المستخدم او كلمة السر غير صحيح"));
            var user = await _unitOfWork.Repository<SecUser>().GetWithSpecAsync(new UsersSpecifications(contact.Id, EncryptString.Encrypt(loginDTO.Password)), trackChanges: false);
            if (user == null)
                return BadRequest(new ApiResponse(400, "اسم المستخدم او كلمة السر غير صحيح"));
            if (contact.StatusId != (byte)StatusEnum.Approved || user.StatusId != (byte)StatusEnum.Approved)
                return Unauthorized(new ApiResponse(401, "المستخدم غير مفعل"));
            if (user.IsLocked ?? false)
                return Unauthorized(new ApiResponse(401, "عفواً، لقد تجاوزت عدد محاولات الدخول"));
            var token = _authManager.CreateToken(user, "").Result;
            if (token == null)
                return BadRequest(new ApiResponse(400, "حاول مره اخرى"));
            var refreshTokenEntity = _mapper.Map<SecUsersRefreshToken>(new UsersRefreshTokensCreateDTO
            {
                UserId = user.Id,
                RefreshToken = token.RefreshToken,
                IsActive = true,
            });
            await _unitOfWork.Repository<SecUsersRefreshToken>().Add(refreshTokenEntity);

            var userLoginEntity = _mapper.Map<SecUsersLogin>(new UsersLoginCreateDTO
            {
                UserId = user.Id,
                Date = DateTime.Now,
            });
            await _unitOfWork.Repository<SecUsersLogin>().Add(userLoginEntity);
            await _unitOfWork.Complete();
            return Ok(token);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(Tokens), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(Tokens token)
        {
            var principal = _authManager.GetPrincipalFromExpiredToken(token.AccessToken);
            if (principal == null)
                return BadRequest(new ApiResponse(400, "حاول مرة أخرى"));
            var userId = principal.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            var savedRefreshToken = await _unitOfWork.Repository<SecUsersRefreshToken>().GetWithSpecAsync(new UsersRefreshTokensSpecifications(int.Parse(userId), token.RefreshToken), trackChanges: false);
            if (savedRefreshToken == null || savedRefreshToken.RefreshToken != token.RefreshToken)
                return BadRequest(new ApiResponse(400, "حاول مرة أخرى"));
            var newJwtToken = _authManager.CreateRefreshToken(savedRefreshToken.User, savedRefreshToken.FireBaseToken).Result;
            if (newJwtToken == null)
                return BadRequest(new ApiResponse(400, "حاول مرة أخرى"));

            var refreshTokenEntity = _mapper.Map<SecUsersRefreshToken>(new UsersRefreshTokensCreateDTO
            {
                UserId = int.Parse(userId),
                RefreshToken = newJwtToken.RefreshToken,
                FireBaseToken = newJwtToken.FireBaseToken,
                IsActive = true,
            });

            _unitOfWork.Repository<SecUsersRefreshToken>().Delete(savedRefreshToken);
            await _unitOfWork.Repository<SecUsersRefreshToken>().Add(refreshTokenEntity);
            await _unitOfWork.Complete();
            return Ok(newJwtToken);
        }

        [AllowAnonymous]
        [HttpPost("ValidateToken")]
        public IActionResult TokenIsValid(TokenValidation token)
        {
            return Ok(new { isValid = _authManager.IsTokenValid(token.AccessToken) });
        }

        [HttpPost("Decrypt/{code}")]
        public IActionResult Decrypt(string code)
        {
            string decrypted = EncryptString.Decrypt(code);
            return Ok(new { decrypted });
        }
    }
}