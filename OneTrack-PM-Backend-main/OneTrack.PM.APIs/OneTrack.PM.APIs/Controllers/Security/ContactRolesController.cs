using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneTrack.PM.ActionFilters.FilterAttributeCreate;
using OneTrack.PM.APIs.Errors;
using OneTrack.PM.Core;
using OneTrack.PM.Entities.DTOs.Security;
using OneTrack.PM.Entities.Models;
using OneTrack.PM.Entities.Models.DB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneTrack.PM.APIs.Controllers.Security
{
    [Route("api/security/[controller]")]
    [Authorize]
    public class ContactRolesController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ContactRolesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ContactRolesLookupDTO>>> GetContactRolesAsync()
        {
            var query = await _unitOfWork.Repository<SecContactRole>().GetAllAsync();
            var data = _mapper.Map<IReadOnlyList<ContactRolesLookupDTO>>(query);
            return Ok(data);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateContactRolesLink([FromBody] ContactRolesLinkCreateDTO form)
        {
            var roleEntity = _mapper.Map<SecContactRolesLink>(form);
            await _unitOfWork.Repository<SecContactRolesLink>().Add(roleEntity);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, Constants.ErrorSaveMessage));
            return Ok(new ApiResponse(200, Constants.SuccessSaveMessage));
        }
    }
}