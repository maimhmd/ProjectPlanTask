using OneTrack.PM.Core.Specifications;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security;
using System.Linq;

namespace OneTrack.PM.Repositories.Specifications.Security
{
    public class ContactsSpecifications : BaseSpecification<SecContact>
    {
        public ContactsSpecifications(ContactsParameters specParams, bool isPaginationEnabled)
            : base(C => (!specParams.Id.HasValue || C.Id == specParams.Id)
            && (string.IsNullOrEmpty(specParams.FullName) || C.FullName.Contains(specParams.FullName))
            && (string.IsNullOrEmpty(specParams.Email) || C.Email.Equals(specParams.Email))
            && (string.IsNullOrEmpty(specParams.Barcode) || C.Barcode.Equals(specParams.Barcode))
            && (!specParams.RoleId.HasValue || C.SecContactRolesLinks.Any(x => x.RoleId.Equals(specParams.RoleId)))
            && (!specParams.ContactTypeId.HasValue || C.ContactTypeId.Equals(specParams.ContactTypeId))
            && (!specParams.CreatedBy.HasValue || C.CreatedBy.Equals(specParams.CreatedBy))
            && (!specParams.StatusId.HasValue || C.StatusId.Equals(specParams.StatusId))
            && (string.IsNullOrEmpty(specParams.SearchTerm) || C.Barcode.Equals(specParams.SearchTerm)
                                                            || C.NormalizedFullName.Contains(specParams.SearchTerm)
                                                            || C.SecContactPhones.Any(x => x.Number.Equals(specParams.SearchTerm))))
        {
            Includes.Add(C => C.Title);
            Includes.Add(C => C.Status);
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
                    case "dateAsc":
                        AddOrderBy(P => P.CreationDate);
                        break;
                    case "dateDesc":
                        AddOrderByDesc(P => P.CreationDate);
                        break;
                }
            }
            if (isPaginationEnabled)
                ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
        }

        public ContactsSpecifications(string email)
            : base(C => C.Email.Trim() == email.Trim())
        {
        }

        public ContactsSpecifications(int id)
            : base(C => C.Id == id)
        {
            IncludeStrings.Add($"{nameof(SecContact.SecContactRolesLinks)}.{nameof(SecContactRolesLink.Role)}");
        }

        public ContactsSpecifications()
            : base()
        {
            AddOrderByDesc(P => P.Id);
        }
    }
}
