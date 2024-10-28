using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneTrack.PM.ActionFilters.FilterAttributeCreate;
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
    public class GroupActionsController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GroupActionsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupActionsAsync([FromQuery] byte groupId)
        {
            var actions = await _unitOfWork.Repository<SecGroupAction>().GetAllWithSpecAsync(new GroupActionsSpecifications(groupId), trackChanges: false);
            var actionsDto = _mapper.Map<IEnumerable<GroupsLookupDTO>>(actions);
            return Ok(actionsDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateGroupActions([FromBody] GroupActionsCreateDTO form)
        {
            var activityEntity = _mapper.Map<SecGroupAction>(form);
            await _unitOfWork.Repository<SecGroupAction>().Add(activityEntity);
            await _unitOfWork.Complete();
            return Ok(new { id = activityEntity.Id });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteGroupAction(SecGroupAction action)
        {
            _unitOfWork.Repository<SecGroupAction>().Delete(action);
            await _unitOfWork.Complete();
            return Ok();
        }

    }
}
