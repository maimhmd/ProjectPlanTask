using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneTrack.PM.Core;
using OneTrack.PM.Entities.DTOs.Security;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Repositories.Specifications.Security;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneTrack.PM.APIs.Controllers.Security
{
    [Route("api/security/[controller]")]
    [Authorize]
    public class ContactGenderController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ContactGenderController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ContactGenderDTO>>> GetContactGenderAsync()
        {
            var query = await _unitOfWork.Repository<SecContactGender>().GetAllWithSpecAsync(new ContactGenderSpecifications(), false);
            var data = _mapper.Map<IReadOnlyList<ContactGenderDTO>>(query);
            return Ok(data);
        }
    }
}