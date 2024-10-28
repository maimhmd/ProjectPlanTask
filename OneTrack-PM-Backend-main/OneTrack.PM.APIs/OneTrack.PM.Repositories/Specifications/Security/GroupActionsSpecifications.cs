using OneTrack.PM.Core.Specifications;
using OneTrack.PM.Entities.Models.DB;

namespace OneTrack.PM.Repositories.Specifications.Security
{
    public class GroupActionsSpecifications : BaseSpecification<SecGroupAction>
    {
        public GroupActionsSpecifications(byte groupId)
            : base(C => C.GroupId == groupId) 
        {
            Includes.Add(C => C.FormActionType);
        }
    }
}
