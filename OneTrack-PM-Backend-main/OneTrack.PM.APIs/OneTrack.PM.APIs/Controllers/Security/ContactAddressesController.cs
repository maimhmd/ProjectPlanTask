using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneTrack.PM.ActionFilters.FilterAttributeCreate;
using OneTrack.PM.Core;
using OneTrack.PM.Entities.DTOs.Security;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security;
using OneTrack.PM.Repositories.Specifications.Security;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneTrack.PM.APIs.Controllers.Security
{
    [Route("api/security/[controller]")]
    [Authorize]
    public class ContactAddressesController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ContactAddressesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetByContact")]
        public async Task<IActionResult> GetContactAddressesByContactId(int contactId)
        {
            var p = new ContactAddressesParameters { ContactId = contactId };
            var query = await _unitOfWork.Repository<SecContactAddress>().GetAllWithSpecAsync(new ContactAddressesSpecifications(p, false), false);
            var data = _mapper.Map<IEnumerable<ContactAddressesEditDTO>>(query);
            return Ok(data);
        }

        [HttpPut]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateContactAddresses(int id, [FromBody] ContactAddressesFormArrayUpdateDTO form)
        {
            var addressesCollection = form.addresses;
            if (addressesCollection != null)
            {
                foreach (var g in addressesCollection)
                {
                    if (g.Id != null && g.Deleted == 1)
                    {
                        var address = await _unitOfWork.Repository<SecContactAddress>().GetWithSpecAsync(new ContactAddressesSpecifications(id), trackChanges: true);
                        if (address != null)
                        {
                            _unitOfWork.Repository<SecContactAddress>().Delete(address);
                        }
                    }
                    else if (g.Id == null)
                    {
                        var addressEntity = _mapper.Map<SecContactAddress>(new ContactAddressesCreateDTO
                        {
                            ContactId = form.Id,
                            CountryId = g.CountryId,
                            GovernorateId = g.GovernorateId,
                            CityId = g.CityId,
                            Address = g.Address
                        });
                        await _unitOfWork.Repository<SecContactAddress>().Add(addressEntity);
                    }
                    else
                    {
                        ContactAddressesUpdateDTO address = new ContactAddressesUpdateDTO
                        {
                            Id = g.Id ?? 0,
                            ContactId = form.Id,
                            CountryId = g.CountryId,
                            GovernorateId = g.GovernorateId,
                            CityId = g.CityId,
                            Address = g.Address
                        };
                        var addressEntity = _mapper.Map<SecContactAddress>(address);
                        _unitOfWork.Repository<SecContactAddress>().Update(addressEntity);
                    }
                }
            }

            await _unitOfWork.Complete();
            return Ok();
        }
    }
}