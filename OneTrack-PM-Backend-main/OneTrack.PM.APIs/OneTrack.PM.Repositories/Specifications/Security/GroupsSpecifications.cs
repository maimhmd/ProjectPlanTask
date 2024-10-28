using OneTrack.PM.Core.Specifications;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security;

namespace OneTrack.PM.Repositories.Specifications.Security
{
    public class GroupsSpecifications : BaseSpecification<SecGroup>
    {
        public GroupsSpecifications(int id)
            : base(C => C.Id == id)
        {
            //IncludeStrings.Add($"{nameof(SecGroupAction.FormActionType)}.{nameof(SecFormActionType.ActionType)}");
            IncludeStrings.Add($"{nameof(SecGroup.SecGroupActions)}.{nameof(SecGroupAction.FormActionType)}");
        }

        public GroupsSpecifications(GroupsParameters specParams, bool isPaginationEnabled)
            : base(C => (!specParams.Id.HasValue || C.Id == specParams.Id)
            && (!specParams.MainModuleId.HasValue || C.MainModuleId == specParams.MainModuleId)
            && (string.IsNullOrEmpty(specParams.Name) || C.Name.Contains(specParams.Name)))
        {
            Includes.Add(C => C.SecGroupActions);
            if (!string.IsNullOrEmpty(specParams.OrderBy))
            {
                switch (specParams.OrderBy)
                {
                    case "idAsc":
                        AddOrderBy(P => P.Id);
                        break;
                    case "idDesc":
                        AddOrderByDesc(P => P.Id);
                        break;
                }
            }
            if (isPaginationEnabled)
                ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
        }
    }
}
