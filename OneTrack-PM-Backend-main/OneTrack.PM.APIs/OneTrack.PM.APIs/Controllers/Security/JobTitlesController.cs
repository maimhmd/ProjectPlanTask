using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneTrack.PM.ActionFilters.FilterAttributeCreate;
using OneTrack.PM.APIs.ActionFilters.FilterAttributePutDelete.Security;
using OneTrack.PM.APIs.Errors;
using OneTrack.PM.APIs.Helpers;
using OneTrack.PM.Core;
using OneTrack.PM.Entities.DTOs.Security;
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
    public class JobTitlesController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public JobTitlesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateJobTitle([FromBody] JobTitlesFormCreateDTO form)
        {
            int code = 0;
            var lastJobTitle = await _unitOfWork.Repository<SecJobTitle>().GetWithSpecAsync(new JobTitlesSpecifications(), trackChanges: false);
            if (lastJobTitle != null)
                code = lastJobTitle.Code;
            code += 1;
            var jobTitleEntity = _mapper.Map<SecJobTitle>(new JobTitlesCreateDTO
            {
                Code = code,
                Name = form.Name,
                Description = form.Description,
                StatusId = form.StatusId
            });
            await _unitOfWork.Repository<SecJobTitle>().Add(jobTitleEntity);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, Constants.ErrorSaveMessage));
            return Ok(new ApiResponse(200, Constants.SuccessSaveMessage, new { id = jobTitleEntity.Id }));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateJobTitleExistsAttribute))]
        public async Task<IActionResult> UpdateJobTitle(int id, [FromBody] JobTitleFormUpdateDTO form)
        {
            var jobTitleEntity = HttpContext.Items["JobTitle"] as SecJobTitle;
            JobTitleUpdateDTO jobTitle = new JobTitleUpdateDTO
            {
                Id = form.Id,
                Code = jobTitleEntity.Code,
                Name = form.Name,
                Description = form.Description,
                StatusId = form.StatusId
            };
            _mapper.Map(jobTitle, jobTitleEntity);
            await _unitOfWork.Complete();
            return Ok(new ApiResponse(200, Constants.SuccessSaveMessage));
        }

        [HttpPut("UpdateStatus/{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateJobTitleExistsAttribute))]
        public async Task<IActionResult> UpdateJobTitleStatus(int id, [FromBody] JobTitleUpdateStatusDTO jobTitle)
        {
            var jobTitleEntity = HttpContext.Items["JobTitle"] as SecJobTitle;
            _mapper.Map(jobTitle, jobTitleEntity);
            await _unitOfWork.Complete();
            return Ok(new ApiResponse(200, Constants.SuccessSaveMessage));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ServiceFilter(typeof(ValidateJobTitleExistsAttribute))]
        public IActionResult GetJobTitleById(int id)
        {
            var jobTitle = HttpContext.Items["JobTitle"] as SecJobTitle;
            var jobTitleDto = _mapper.Map<JobTitlesByIdDTO>(jobTitle);
            return Ok(jobTitleDto);
        }

        [HttpGet("Paged")]
        public async Task<ActionResult<IReadOnlyList<JobTitlesDTO>>> GetJobTitlesPaged([FromQuery] JobTitlesParameters p)
        {
            var query = await _unitOfWork.Repository<SecJobTitle>().GetAllWithSpecAsync(new JobTitlesSpecifications(p, true), false);
            var data = _mapper.Map<IReadOnlyList<JobTitlesDTO>>(query);
            var count = await _unitOfWork.Repository<SecJobTitle>().GetCountWithSpecAsync(new JobTitlesSpecifications(p, false));
            return Ok(new Pagination<JobTitlesDTO>(p.PageIndex, p.PageSize, count, data));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<JobTitlesLookupDTO>>> GetJobTitles()
        {
            var query = await _unitOfWork.Repository<SecJobTitle>().GetAllAsync();
            var data = _mapper.Map<IReadOnlyList<JobTitlesLookupDTO>>(query);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ServiceFilter(typeof(ValidateJobTitleExistsAttribute))]
        public async Task<ActionResult> DeleteJobTitle(int id)
        {
            var entity = HttpContext.Items["JobTitle"] as SecJobTitle;
            _unitOfWork.Repository<SecJobTitle>().Delete(entity);
            await _unitOfWork.Complete();
            return Ok(new ApiResponse(200, Constants.SuccessDeleteMessage)); ;
        }
    }
}