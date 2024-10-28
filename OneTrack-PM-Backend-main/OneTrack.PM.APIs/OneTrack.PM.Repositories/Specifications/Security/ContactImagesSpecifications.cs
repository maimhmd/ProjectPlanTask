using OneTrack.PM.Core.Specifications;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security;

namespace OneTrack.PM.Repositories.Specifications.Security
{
    public class ContactImagesSpecifications : BaseSpecification<SecContactImage>
    {
        public ContactImagesSpecifications(int id)
            : base(C => C.Id == id) { }

        public ContactImagesSpecifications(ContactImagesParameters specParams, bool isPaginationEnabled)
            : base(C => (!specParams.ContactId.HasValue || C.ContactId == specParams.ContactId)
            && (string.IsNullOrEmpty(specParams.Caption) || C.Caption.Contains(specParams.Caption)))
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
