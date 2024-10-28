using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneTrack.PM.ActionFilters.FilterAttributeCreate;
using OneTrack.PM.APIs.ActionFilters.FilterAttributePutDelete.Security;
using OneTrack.PM.APIs.Errors;
using OneTrack.PM.APIs.Helpers;
using OneTrack.PM.Core;
using OneTrack.PM.Entities.DTOs.Security;
using OneTrack.PM.Entities.Helpers;
using OneTrack.PM.Entities.Models;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security;
using OneTrack.PM.Repositories.Specifications.Security;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneTrack.PM.APIs.Controllers.Security
{
    [Route("api/security/[controller]")]
    [Authorize]
    public class UsersController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UsersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateUser([FromBody] UsersCreateDTO form)
        {
            form.Password = EncryptString.Encrypt("123");
            var userEntity = _mapper.Map<SecUser>(form);
            await _unitOfWork.Repository<SecUser>().Add(userEntity);
            var contactEntity = await _unitOfWork.Repository<SecContact>().GetWithSpecAsync(new ContactsSpecifications(form.ContactId), trackChanges: true);
            _mapper.Map(new ContactsUpdateEmailDTO
            {
                Id = form.ContactId,
                Email = form.Email
            }, contactEntity);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, Constants.ErrorSaveMessage));
            return Ok(new ApiResponse(200, Constants.SuccessSaveMessage, new { id = userEntity.Id }));
        }

        [HttpGet("Paged")]
        public async Task<IActionResult> GetUsersAsync([FromQuery] UsersParameters p)
        {
            var query = await _unitOfWork.Repository<SecUser>().GetAllWithSpecAsync(new UsersSpecifications(p, true), trackChanges: false);
            var data = _mapper.Map<IReadOnlyList<SecUser>, IReadOnlyList<UsersDTO>>(query);
            var count = await _unitOfWork.Repository<SecUser>().GetCountWithSpecAsync(new UsersSpecifications(p, false));
            return Ok(new Pagination<UsersDTO>(p.PageIndex, p.PageSize, count, data));
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateUserExistsAttribute))]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UsersUpdateDTO form)
        {
            var userEntity = HttpContext.Items["User"] as SecUser;
            _mapper.Map(new UsersUpdateDTO
            {
                Id = userEntity.Id,
                ContactId = form.ContactId,
                Name = form.Name,
                Password = userEntity.Password,
                GroupId = form.GroupId,
                statusId = form.statusId
            }, userEntity);
            var contactEntity = await _unitOfWork.Repository<SecContact>().GetWithSpecAsync(new ContactsSpecifications(form.ContactId), trackChanges: true);
            _mapper.Map(new ContactsUpdateEmailDTO
            {
                Id = form.ContactId,
                Email = form.Email
            }, contactEntity);
            await _unitOfWork.Complete();
            return Ok(new ApiResponse(200, Constants.SuccessSaveMessage));
        }

        [HttpPut("UpdateStatus/{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateUserExistsAttribute))]
        public async Task<IActionResult> UpdateUserStatus(int id, [FromBody] UsersUpdateStatusDTO user)
        {
            var userEntity = HttpContext.Items["User"] as SecUser;
            _mapper.Map(user, userEntity);
            await _unitOfWork.Complete();
            return Ok();
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(ValidateUserExistsAttribute))]
        public IActionResult GetUserById(int id)
        {
            var user = HttpContext.Items["User"] as SecUser;
            var userDto = _mapper.Map<UsersByIdDTO>(user);
            return Ok(userDto);
        }

        [HttpPut("ChangePassword/{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] UsersChangePasswordFormDTO form)
        {
            SecUser userEntity = await _unitOfWork.Repository<SecUser>().GetWithSpecAsync(new UsersSpecifications(id), trackChanges: true);
            if (userEntity != null)
            {
                if (form.OldPassword != null && form.OldPassword != EncryptString.Decrypt(userEntity.Password))
                    return Ok(new { statusId = "0", message = "كلمة السر القديمة غير صحيحة" });
                _mapper.Map(new UsersChangePasswordDTO
                {
                    Id= id,
                    Password = EncryptString.Encrypt(form.Password)
                }, userEntity);
                await _unitOfWork.Complete();
                return Ok(new { statusId = "1", message = "تم تغيير كلمة المرور بنجاح" });
            }
            else
                return Ok(new { statusId = "0", message = "لم تتم عملية تغيير كلمة المرور بنجاح" });
        }

        [HttpPut("ResetPassword/{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateUserExistsAttribute))]
        public async Task<IActionResult> ResetPassword(int id, [FromBody] UsersResetPasswordDTO form)
        {
            form.Password = EncryptString.Encrypt("123");
            var entity = HttpContext.Items["User"] as SecUser;
            _mapper.Map(form, entity);
            await _unitOfWork.Complete();
            return Ok(new ApiResponse(200, Constants.SuccessSaveMessage));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ServiceFilter(typeof(ValidateUserExistsAttribute))]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var entity = HttpContext.Items["User"] as SecUser;
            _unitOfWork.Repository<SecUser>().Delete(entity);
            await _unitOfWork.Complete();
            return Ok(new ApiResponse(200, Constants.SuccessDeleteMessage)); ;
        }
    }
}