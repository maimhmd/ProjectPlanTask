using OneTrack.PM.Core.Specifications;
using OneTrack.PM.Entities.DTOs.Security;
using OneTrack.PM.Entities.Models.DB;

namespace OneTrack.PM.Repositories.Specifications.Security
{
    public class TitlesSpecifications : BaseSpecification<SecTitle>
    {
        public TitlesSpecifications(byte id)
            : base(X => X.Id == id) { }
        public TitlesSpecifications(TitlesParameters specParams, bool isPaginationEnabled)
            : base()
        {
            if (isPaginationEnabled)
                ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
        }
    }
}

