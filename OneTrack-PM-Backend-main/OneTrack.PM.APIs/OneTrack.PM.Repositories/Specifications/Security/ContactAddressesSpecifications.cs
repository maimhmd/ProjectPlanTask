using OneTrack.PM.Core.Specifications;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security;

namespace OneTrack.PM.Repositories.Specifications.Security
{
    public class ContactAddressesSpecifications : BaseSpecification<SecContactAddress>
    {
        public ContactAddressesSpecifications(int id)
            : base(C => C.Id == id) { }

        public ContactAddressesSpecifications(ContactAddressesParameters specParams, bool isPaginationEnabled)
            : base(C => (!specParams.ContactId.HasValue || C.ContactId == specParams.ContactId)
            && (!specParams.Id.HasValue || C.Id == specParams.Id)
            && (string.IsNullOrEmpty(specParams.Street) || C.Street.Contains(specParams.Street))
            && (string.IsNullOrEmpty(specParams.BuildingNo) || C.BuildingNo.Contains(specParams.BuildingNo))
            && (string.IsNullOrEmpty(specParams.Address) || C.Address.Contains(specParams.Address))
            && (string.IsNullOrEmpty(specParams.NearestHallmark) || C.NearestHallmark.Contains(specParams.NearestHallmark))
            && (string.IsNullOrEmpty(specParams.Phone) || C.Phone.Equals(specParams.Phone))
            && (!specParams.GovernorateId.HasValue || C.GovernorateId.Equals(specParams.GovernorateId))
            && (!specParams.CityId.HasValue || C.CityId.Equals(specParams.CityId))
            && (!specParams.IsHomeAddress.HasValue || C.IsHomeAddress.Equals(specParams.IsHomeAddress)))
        {
            Includes.Add(C => C.Country);
            Includes.Add(C => C.Governorate);
            Includes.Add(C => C.City);

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
