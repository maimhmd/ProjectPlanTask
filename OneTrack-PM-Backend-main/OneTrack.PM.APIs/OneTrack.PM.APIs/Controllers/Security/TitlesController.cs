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
using OneTrack.PM.Repositories.Specifications.Security;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneTrack.PM.APIs.Controllers.Security
{
    [Route("api/security/[controller]")]
    [Authorize]
    public class TitlesController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TitlesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTitle([FromBody] TitlesCreateDTO form)
        {
            var titleEntity = _mapper.Map<SecTitle>(form);
            await _unitOfWork.Repository<SecTitle>().Add(titleEntity);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, Constants.ErrorSaveMessage));
            return Ok(new ApiResponse(200, Constants.SuccessSaveMessage, new { id = titleEntity.Id }));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateTitleActionExistsAttribute))]
        public async Task<IActionResult> UpdateTitle(byte id, [FromBody] TitleUpdateDTO form)
        {
            var titleEntity = HttpContext.Items["Title"] as SecTitle;
            TitleUpdateDTO title = new TitleUpdateDTO
            {
                Id = form.Id,
                Name = form.Name
            };
            _mapper.Map(title, titleEntity);
            await _unitOfWork.Complete();
            return Ok(new ApiResponse(200, Constants.SuccessSaveMessage));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ServiceFilter(typeof(ValidateTitleActionExistsAttribute))]
        public IActionResult GetTitleById(byte id)
        {
            var title = HttpContext.Items["Title"] as SecTitle;
            var titleDto = _mapper.Map<TitlesByIdDTO>(title);
            return Ok(titleDto);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TitlesDTO>>> GetTitles()
        {
            var query = await _unitOfWork.Repository<SecTitle>().GetAllAsync();
            var data = _mapper.Map<IReadOnlyList<TitlesDTO>>(query);
            return Ok(data);
        }

        [HttpGet("Paged")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public async Task<ActionResult<IReadOnlyList<TitlesDTO>>> GetTitles([FromQuery] TitlesParameters p)
        {
            var query = await _unitOfWork.Repository<SecTitle>().GetAllWithSpecAsync(new TitlesSpecifications(p, true), false);
            var data = _mapper.Map<IReadOnlyList<SecTitle>, IReadOnlyList<TitlesDTO>>(query);
            var count = await _unitOfWork.Repository<SecTitle>().GetCountWithSpecAsync(new TitlesSpecifications(p, false));
            return Ok(new Pagination<TitlesDTO>(p.PageIndex, p.PageSize, count, data));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ServiceFilter(typeof(ValidateTitleActionExistsAttribute))]
        public async Task<ActionResult> DeleteTitle(byte id)
        {
            var entity = HttpContext.Items["Title"] as SecTitle;
            _unitOfWork.Repository<SecTitle>().Delete(entity);
            await _unitOfWork.Complete();
            return Ok(new ApiResponse(200, Constants.SuccessDeleteMessage)); ;
        }
    }
}