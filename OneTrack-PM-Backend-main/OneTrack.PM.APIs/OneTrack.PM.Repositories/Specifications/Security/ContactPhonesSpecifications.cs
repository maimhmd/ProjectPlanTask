using OneTrack.PM.Core.Specifications;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security;

namespace OneTrack.PM.Repositories.Specifications.Security
{
    public class ContactPhonesSpecifications : BaseSpecification<SecContactPhone>
    {
        public ContactPhonesSpecifications(int id)
            : base(C => C.Id == id) { }

        public ContactPhonesSpecifications(ContactPhonesParameters specParams, bool isPaginationEnabled)
            : base(C => (!specParams.ContactId.HasValue || C.ContactId == specParams.ContactId)
            && (string.IsNullOrEmpty(specParams.Number) || C.Number.Equals(specParams.Number))
            && (!specParams.Whatsapp.HasValue || C.Whatsapp.Equals(specParams.Whatsapp)))
        {
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
