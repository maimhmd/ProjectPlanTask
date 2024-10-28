using OneTrack.PM.Core.Specifications;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security;

namespace OneTrack.PM.Repositories.Specifications.Security
{
    public class UsersSpecifications : BaseSpecification<SecUser>
    {
        public UsersSpecifications(int contactId, string password)
            : base(C => C.ContactId == contactId && C.Password == password)
        {
            Includes.Add(C => C.Group);
            IncludeStrings.Add($"{nameof(SecUser.Contact)}.{nameof(SecContact.Title)}");
        }

        public UsersSpecifications(int id)
            : base(C => C.Id == id)
        {
            Includes.Add(C => C.Group);
            IncludeStrings.Add($"{nameof(SecUser.Contact)}.{nameof(SecContact.Title)}");
        }

        public UsersSpecifications(UsersParameters specParams, bool isPaginationEnabled)
            : base(C => (!specParams.ContactId.HasValue || C.ContactId == specParams.ContactId)
            && (!specParams.MainModuleId.HasValue || C.Group.MainModuleId == specParams.MainModuleId))
        {
            Includes.Add(C => C.Group);
            Includes.Add(C => C.Status);
            IncludeStrings.Add($"{nameof(SecUser.Contact)}.{nameof(SecContact.Title)}");
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
