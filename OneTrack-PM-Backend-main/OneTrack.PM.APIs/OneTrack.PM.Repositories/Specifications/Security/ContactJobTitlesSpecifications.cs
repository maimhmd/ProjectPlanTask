using OneTrack.PM.Core.Specifications;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security;

namespace OneTrack.PM.Repositories.Specifications.Security
{
    public class ContactJobTitlesSpecifications : BaseSpecification<SecContactJobTitle>
    {
        public ContactJobTitlesSpecifications(int id)
            : base(C => C.Id == id)
        { }

        public ContactJobTitlesSpecifications(ContactJobTitlesParameters specParams, bool isPaginationEnabled)
        : base(C => (!specParams.Id.HasValue || C.Id == specParams.Id)
        && (!specParams.ContactId.HasValue || C.ContactId == specParams.ContactId)
        && (!specParams.EntityId.HasValue || C.EntityId == specParams.EntityId)
        && (!specParams.JobTitleId.HasValue || C.JobTitleId == specParams.JobTitleId)
        && (!specParams.IsActive.HasValue || C.IsActive == specParams.IsActive))
        {
            Includes.Add(C => C.JobTitle);
            Includes.Add(C => C.Entity);
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

        public ContactJobTitlesSpecifications()
            : base()
        {
            AddOrderByDesc(P => P.Id);
        }
    }
}
