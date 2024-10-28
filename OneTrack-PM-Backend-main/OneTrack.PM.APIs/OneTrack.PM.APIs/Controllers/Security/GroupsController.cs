using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneTrack.PM.ActionFilters.FilterAttributeCreate;
using OneTrack.PM.APIs.ActionFilters.FilterAttributePutDelete.Security;
using OneTrack.PM.APIs.Helpers;
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
    public class GroupsController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public readonly GroupActionsController _groupActionsController;
        public GroupsController(IUnitOfWork unitOfWork, IMapper mapper, GroupActionsController groupActionsController)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _groupActionsController = groupActionsController;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupsLookupAsync([FromQuery] GroupsParameters p)
        {
            var groups = await _unitOfWork.Repository<SecGroup>().GetAllWithSpecAsync(new GroupsSpecifications(p, false), trackChanges: false);
            var groupsDto = _mapper.Map<IEnumerable<GroupsLookupDTO>>(groups);
            return Ok(groupsDto);
        }

        [HttpGet("Paged")]
        public async Task<ActionResult<IReadOnlyList<GroupsDTO>>> GetGroupsAsync([FromQuery] GroupsParameters p)
        {
            var query = await _unitOfWork.Repository<SecGroup>().GetAllWithSpecAsync(new GroupsSpecifications(p, true), trackChanges: false);
            var data = _mapper.Map<IReadOnlyList<SecGroup>, IReadOnlyList<GroupsDTO>>(query);
            var count = await _unitOfWork.Repository<SecGroup>().GetCountWithSpecAsync(new GroupsSpecifications(p, false));
            return Ok(new Pagination<GroupsDTO>(p.PageIndex, p.PageSize, count, data));
        }


        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateGroups([FromBody] GroupsFormCreateDTO form)
        {
            GroupsCreateDTO group = new GroupsCreateDTO
            {
                Name = form.Name,
                MainModuleId = form.MainModuleId,
                StatusId = form.StatusId
            };
            var groupEntity = _mapper.Map<SecGroup>(group);
            await _unitOfWork.Repository<SecGroup>().Add(groupEntity);
            await _unitOfWork.Complete();
            foreach (var g in form.Actions)
            {
                await _groupActionsController.CreateGroupActions(new GroupActionsCreateDTO
                {
                    FormActionTypeId = g.FormActionTypeId,
                    GroupId = groupEntity.Id
                });
            }
            return Ok(new { id = groupEntity.Id });
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateGroupExistsAttribute))]
        public async Task<IActionResult> UpdateGroups(byte id, [FromBody] GroupsUpdateFormDTO form)
        {
            var groupEntity = HttpContext.Items["Group"] as SecGroup;
            GroupsUpdateDTO group = new GroupsUpdateDTO
            {
                Id = id,
                Name = form.Name,
                MainModuleId = form.MainModuleId,
                StatusId = form.StatusId
            };
            _mapper.Map(group, groupEntity);
            await _unitOfWork.Complete();
            var groupActions = groupEntity.SecGroupActions;
            foreach (var action in groupActions)
            {
                await _groupActionsController.DeleteGroupAction(action);
            }
            foreach (var g in form.Actions)
            {
                await _groupActionsController.CreateGroupActions(new GroupActionsCreateDTO
                {
                    FormActionTypeId = g.FormActionTypeId,
                    GroupId = groupEntity.Id
                });
            }
            return Ok();
        }

        [HttpPut("UpdateStatus/{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateGroupExistsAttribute))]
        public async Task<IActionResult> UpdateGroupsStatus(byte id, [FromBody] GroupsUpdateStatusDTO group)
        {
            var groupEntity = HttpContext.Items["Group"] as SecGroup;
            _mapper.Map(group, groupEntity);
            await _unitOfWork.Complete();
            return Ok();
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateGroupExistsAttribute))]
        public async Task<IActionResult> DeleteGroups(byte id)
        {
            var group = HttpContext.Items["Group"] as SecGroup;
            _unitOfWork.Repository<SecGroup>().Delete(group);
            await _unitOfWork.Complete();
            return Ok();
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(ValidateGroupExistsAttribute))]
        public IActionResult GetGroupsById(byte id)
        {
            var group = HttpContext.Items["Group"] as SecGroup;
            var groupDto = _mapper.Map<GroupsByIdDTO>(group);
            return Ok(groupDto);
        }
    }
}
