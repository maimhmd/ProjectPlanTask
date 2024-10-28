using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneTrack.PM.ActionFilters.FilterAttributeCreate;
using OneTrack.PM.APIs.ActionFilters.FilterAttributePutDelete.Security;
using OneTrack.PM.APIs.Errors;
using OneTrack.PM.APIs.Helpers;
using OneTrack.PM.Core;
using OneTrack.PM.Entities.DTO.Security;
using OneTrack.PM.Entities.Models;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security;
using OneTrack.PM.Repositories.Specifications.Security;

namespace OneTrack.PM.APIs.Controllers.Security
{
    [Route("api/security/[controller]")]
    [Authorize]
    public class ContactJobTitlesController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ContactJobTitlesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateContactJobTitle([FromBody] ContactJobTitlesFormCreateDTO form)
        {
            var contactJobTitleEntity = _mapper.Map<SecContactJobTitle>(new ContactJobTitlesCreateDTO
            {
                ContactId = form.ContactId,
                EntityId = form.EntityId,
                JobTitleId = form.JobTitleId,
                IsActive = form.IsActive
            });
            await _unitOfWork.Repository<SecContactJobTitle>().Add(contactJobTitleEntity);
            await _unitOfWork.Complete();
            return Ok(new ApiResponse(200, Constants.SuccessSaveMessage, new ContactJobTitlesCreateResponseDTO { Id = contactJobTitleEntity.Id }));
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateContactJobTitleExistsAttribute))]
        public async Task<IActionResult> UpdateContactJobTitle(int id, [FromBody] ContactJobTitlesUpdateFormDTO form)
        {
            var contactJobTitleEntity = HttpContext.Items["ContactJobTitle"] as SecContactJobTitle;
            ContactJobTitlesUpdateDTO contactJobTitle = new ContactJobTitlesUpdateDTO
            {
                Id = form.Id,
                ContactId = form.ContactId,
                EntityId = form.EntityId,
                JobTitleId = form.JobTitleId,
                IsActive = form.IsActive
            };
            _mapper.Map(contactJobTitle, contactJobTitleEntity);
            await _unitOfWork.Complete();
            return Ok();
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(ValidateContactJobTitleExistsAttribute))]
        public IActionResult GetContactJobTitleById(int id)
        {
            var contactJobTitle = HttpContext.Items["ContactJobTitle"] as SecContactJobTitle;
            var contactJobTitleDto = _mapper.Map<ContactJobTitlesByIdDTO>(contactJobTitle);
            return Ok(contactJobTitleDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetcontactJobTitlesLookupAsync([FromQuery] ContactJobTitlesParameters p)
        {
            var contactJobTitles = await _unitOfWork.Repository<SecContactJobTitle>().GetAllWithSpecAsync(new ContactJobTitlesSpecifications(p, false), trackChanges: false);
            var contactJobTitlesDto = _mapper.Map<IEnumerable<ContactJobTitlesLookupDTO>>(contactJobTitles);
            return Ok(contactJobTitlesDto);
        }

        [HttpGet("Paged")]
        public async Task<ActionResult<IReadOnlyList<ContactJobTitlesDTO>>> GetcontactJobTitlesAsync([FromQuery] ContactJobTitlesParameters p)
        {
            var query = await _unitOfWork.Repository<SecContactJobTitle>().GetAllWithSpecAsync(new ContactJobTitlesSpecifications(p, true), trackChanges: false);
            var data = _mapper.Map<IReadOnlyList<SecContactJobTitle>, IReadOnlyList<ContactJobTitlesDTO>>(query);
            var count = await _unitOfWork.Repository<SecContactJobTitle>().GetCountWithSpecAsync(new ContactJobTitlesSpecifications(p, false));
            return Ok(new Pagination<ContactJobTitlesDTO>(p.PageIndex, p.PageSize, count, data));
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateContactJobTitleExistsAttribute))]
        public async Task<IActionResult> DeleteContactJobTitle(int id)
        {
            var contactJobTitle = HttpContext.Items["ContactJobTitle"] as SecContactJobTitle;
            _unitOfWork.Repository<SecContactJobTitle>().Delete(contactJobTitle);
            await _unitOfWork.Complete();
            return Ok();
        }

        [HttpPut("ChangeActivation/{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateContactJobTitleExistsAttribute))]
        public async Task<IActionResult> ChangeJobTitleActivation(int id, [FromBody] ContactbJobTitlesUpdateActivationDTO form)
        {
            var contactJobTitleEntity = HttpContext.Items["ContactJobTitle"] as SecContactJobTitle;
            _mapper.Map(form, contactJobTitleEntity);
            await _unitOfWork.Complete();
            return Ok(new ApiResponse(200, Constants.SuccessSaveMessage));
        }

    }
}