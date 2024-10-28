using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneTrack.PM.ActionFilters.FilterAttributeCreate;
using OneTrack.PM.Core;
using OneTrack.PM.Entities.DTO.Security;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security;
using OneTrack.PM.Repositories.Specifications.Security;

namespace OneTrack.PM.APIs.Controllers.Security
{
    [Route("api/security/[controller]")]
    [Authorize]
    public class ContactPhonesController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ContactPhonesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateContactPhone(int id, [FromBody] ContactPhonesFormArrayUpdateDTO form)
        {
            var phonesCollection = form.Phones;
            if (phonesCollection != null)
            {
                foreach (var g in phonesCollection)
                {
                    if (g.Id != null && g.Deleted == 1)
                    {
                        var phone = await _unitOfWork.Repository<SecContactPhone>().GetWithSpecAsync(new ContactPhonesSpecifications(g.Id ?? 0), trackChanges: true);
                        if (phone != null)
                        {
                            _unitOfWork.Repository<SecContactPhone>().Delete(phone);
                        }
                    }
                    else if (g.Id == null)
                    {
                        var phoneEntity = _mapper.Map<SecContactPhone>(new ContactPhonesCreateDTO
                        {
                            ContactId = id,
                            Number = g.Number,
                            Whatsapp = g.Whatsapp ?? false
                        });
                        await _unitOfWork.Repository<SecContactPhone>().Add(phoneEntity);
                    }
                    else
                    {
                        ContactPhonesUpdateDTO phone = new ContactPhonesUpdateDTO
                        {
                            Id = g.Id ?? 0,
                            ContactId = id,
                            Number = g.Number,
                            Whatsapp = g.Whatsapp ?? false
                        };
                        var phoneEntity = _mapper.Map<SecContactPhone>(phone);
                        _unitOfWork.Repository<SecContactPhone>().Update(phoneEntity);
                    }
                }
            }
            await _unitOfWork.Complete();
            return Ok();
        }

        [HttpGet("GetByContact")]
        public async Task<ActionResult<IReadOnlyList<ContactPhonesEditDTO>>> GetContactPhonesByContactId(int contactId)
        {
            var query = await _unitOfWork.Repository<SecContactPhone>().GetAllWithSpecAsync(new ContactPhonesSpecifications(new ContactPhonesParameters
            {
                ContactId = contactId
            }, false), false);
            var data = _mapper.Map<IReadOnlyList<ContactPhonesEditDTO>>(query);
            return Ok(data);
        }
    }
}