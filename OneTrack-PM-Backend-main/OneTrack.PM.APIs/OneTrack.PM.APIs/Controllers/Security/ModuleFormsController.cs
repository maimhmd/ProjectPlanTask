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
    public class ModuleFormsController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ModuleFormsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ModuleFormsDTO>>> GetModuleFormsLookupAsync([FromQuery]byte mainModuleId)
        {
            var moduleForms = await _unitOfWork.Repository<SecModuleForm>().GetAllWithSpecAsync(new ModuleFormsSpecifications(mainModuleId), trackChanges: false);
            var moduleFormsDto = _mapper.Map<IReadOnlyList<ModuleFormsDTO>>(moduleForms);
            return Ok(moduleFormsDto);
        }
    }
}
