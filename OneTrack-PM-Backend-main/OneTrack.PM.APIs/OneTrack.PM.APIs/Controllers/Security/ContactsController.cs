using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneTrack.PM.ActionFilters.FilterAttributeCreate;
using OneTrack.PM.APIs.ActionFilters.FilterAttributePutDelete.Security;
using OneTrack.PM.APIs.Errors;
using OneTrack.PM.APIs.Helpers;
using OneTrack.PM.Core;
using OneTrack.PM.Core.Services;
using OneTrack.PM.Entities.DTO.Security;
using OneTrack.PM.Entities.DTOs.Security;
using OneTrack.PM.Entities.Models;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security;
using OneTrack.PM.Repositories.Specifications.Security;
using FileInfo = System.IO.FileInfo;

namespace OneTrack.PM.APIs.Controllers.Security
{
    [Route("api/security/[controller]")]
    [Authorize]
    public class ContactsController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authService;
        public ContactsController(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticationService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateContact([FromBody] ContactsFormCreateDTO form)
        {
            Request.Headers.TryGetValue("Authorization", out var authorizationValue);
            var claims = _authService.DecodeToken(authorizationValue);

            var contacts = await _unitOfWork.Repository<SecContact>().GetAllWithSpecAsync(new ContactsSpecifications(new ContactsParameters
            {
                FullName = Normalize.RemoveDiacritics(form.FullName),
                Email = form.Email ?? string.Empty
            }, false), false);
            if (contacts.Count() > 0)
            {
                var contactsDto = _mapper.Map<IEnumerable<ContactsLookupDTO>>(contacts);
                return Ok(new ContactsCreateResponseDTO { id = contactsDto.FirstOrDefault().Id });
            }
            string avatar = null;
            if (form.Avatar != null && form.Avatar.Base64 != null && form.Avatar.Base64 != string.Empty)
            {
                FileInfo fi = new FileInfo(form.Avatar.Filename);
                avatar = Upload.UploadFiles(fi.Extension, form.Avatar.Base64, "ContactsUploads");
            }
            int code = 0;
            var lastContact = await _unitOfWork.Repository<SecContact>().GetWithSpecAsync(new ContactsSpecifications(), trackChanges: false);
            if (lastContact != null)
                code = lastContact.Code;
            code += 1;
            string barcode =(form.ContactTypeId==(byte)ContactTypesEnum.Entity? Constants.EntityContactsBarcodePrefix:Constants.PersonContactsBarcodePrefix) + code.ToString().PadLeft((int)CodesLengthEnum.ContactCode, '0');
            var contactEntity = _mapper.Map<SecContact>(new ContactsCreateDTO
            {
                Code = code,
                Avatar = avatar,
                Barcode = barcode,
                FullName = form.FullName,
                NormalizedFullName = Normalize.RemoveDiacritics(form.FullName),
                ContactTypeId = form.ContactTypeId,
                TitleId = form.TitleId,
                Email = form.Email,
                NationalId = form.NationalId,
                Biography = form.Biography,
                GenderId = form.GenderId,
                CreatedBy = claims.Id,
                CreationDate = DateTime.Now,
                StatusId = form.StatusId
            });
            if (form.Roles != null)
            {
                var rolesLink = new List<SecContactRolesLink>();
                foreach (var r in form.Roles)
                {
                    var roleEntity = _mapper.Map<SecContactRolesLink>(new ContactRolesLinkCreateDTO
                    {
                        RoleId = r.RoleId
                    });
                    rolesLink.Add(roleEntity);
                }
                contactEntity.SecContactRolesLinks = rolesLink;
            }
            await _unitOfWork.Repository<SecContact>().Add(contactEntity);
            var result = await _unitOfWork.Complete();
            if (form.JobTitleId != null)
            {
                var jobTitleEntity = _mapper.Map<SecContactJobTitle>(new ContactJobTitlesCreateDTO
                {
                    ContactId = contactEntity.Id,
                    JobTitleId = form.JobTitleId ?? 0,
                    EntityId = form.EntityId ?? 0,
                    IsActive = true
                });
                await _unitOfWork.Repository<SecContactJobTitle>().Add(jobTitleEntity);
                await _unitOfWork.Complete();
            }
            if (result <= 0) return BadRequest(new ApiResponse(400, Constants.ErrorSaveMessage));
            return Ok(new ApiResponse(200, Constants.SuccessSaveMessage, new { id = contactEntity.Id }));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateContactExistsAttribute))]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] ContactsUpdateFormDTO form)
        {
            Request.Headers.TryGetValue("Authorization", out var authorizationValue);
            var claims = _authService.DecodeToken(authorizationValue);

            string avatar = null;
            if (form.Avatar != null && form.Avatar.Base64 != null && form.Avatar.Base64 != string.Empty)
            {
                FileInfo fi = new FileInfo(form.Avatar.Filename);
                avatar = Upload.UploadFiles(fi.Extension, form.Avatar.Base64, "ContactsUploads");
            }
            var contactEntity = HttpContext.Items["Contact"] as SecContact;
            ContactsUpdateDTO contact = new ContactsUpdateDTO
            {
                Id = form.Id,
                Code = contactEntity.Code,
                Barcode = contactEntity.Barcode,
                FullName = form.FullName,
                NormalizedFullName = Normalize.RemoveDiacritics(form.FullName),
                Avatar = avatar != null ? avatar : contactEntity.Avatar,
                ContactTypeId = form.ContactTypeId,
                TitleId = form.TitleId,
                Biography = form.Biography,
                GenderId = form.GenderId,
                Email = form.Email,
                NationalId = form.NationalId,
                ModifiedBy = claims.Id,
                ModifyDate = DateTime.Now,
                CreatedBy = contactEntity.CreatedBy ?? 0,
                CreationDate = contactEntity.CreationDate,
                StatusId = form.StatusId
            };
            _mapper.Map(contact, contactEntity);
            await _unitOfWork.Complete();
            return Ok(new ApiResponse(200, Constants.SuccessSaveMessage));
        }

        [HttpPut("UpdateStatus/{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateContactExistsAttribute))]
        public async Task<IActionResult> UpdateContactStatus(int id, [FromBody] ContactsUpdateStatusDTO contact)
        {
            var contactEntity = HttpContext.Items["Contact"] as SecContact;
            _mapper.Map(contact, contactEntity);
            await _unitOfWork.Complete();
            return Ok(new ApiResponse(200, Constants.SuccessSaveMessage));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ServiceFilter(typeof(ValidateContactExistsAttribute))]
        public IActionResult GetContactById(int id)
        {
            var contact = HttpContext.Items["Contact"] as SecContact;
            var contactDto = _mapper.Map<ContactsByIdDTO>(contact);
            return Ok(contactDto);
        }

        [HttpGet("Paged")]
        public async Task<ActionResult<IReadOnlyList<ContactsDTO>>> GetContactsPaged([FromQuery] ContactsParameters p)
        {
            p.FullName = Normalize.RemoveDiacritics(p.FullName);
            var query = await _unitOfWork.Repository<SecContact>().GetAllWithSpecAsync(new ContactsSpecifications(p, true), false);
            var data = _mapper.Map<IReadOnlyList<ContactsDTO>>(query);
            var count = await _unitOfWork.Repository<SecContact>().GetCountWithSpecAsync(new ContactsSpecifications(p, false));
            return Ok(new Pagination<ContactsDTO>(p.PageIndex, p.PageSize, count, data));
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts([FromQuery] ContactsParameters p)
        {
            var contacts = await _unitOfWork.Repository<SecContact>().GetAllWithSpecAsync(new ContactsSpecifications(p, false), false);
            var contactsDto = _mapper.Map<IEnumerable<ContactsLookupDTO>>(contacts);
            return Ok(contactsDto);
        }

        [HttpGet("Autocomplete/{searchtext}")]
        public async Task<IActionResult> GetContactsAutocomplete(string searchtext, [FromQuery] byte? roleId, [FromQuery] byte? contactTypeId)
        {
            var contacts = await _unitOfWork.Repository<SecContact>().GetAllWithSpecAsync(new ContactsSpecifications(new ContactsParameters
            {
                RoleId = roleId,
                ContactTypeId=contactTypeId,
                SearchTerm = Normalize.RemoveDiacritics(searchtext)
            }, false), false);
            var contactsDto = _mapper.Map<IEnumerable<ContactsLookupDTO>>(contacts);
            return Ok(contactsDto);
        }

        [HttpGet("IsUnique")]
        public async Task<IActionResult> IsUnique([FromQuery] ContactsParameters p)
        {
            p.FullName = Normalize.RemoveDiacritics(p.FullName);
            var query = await _unitOfWork.Repository<SecContact>().GetWithSpecAsync(new ContactsSpecifications(p, false), trackChanges: false);
            return Ok(query != null ? true : false);
        }
    }
}