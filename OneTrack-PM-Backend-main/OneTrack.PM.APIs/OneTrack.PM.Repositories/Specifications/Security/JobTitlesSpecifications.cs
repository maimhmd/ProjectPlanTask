using OneTrack.PM.Core.Specifications;
using OneTrack.PM.Entities.Models;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security;

namespace OneTrack.PM.Repositories.Specifications.Security
{
    public class JobTitlesSpecifications : BaseSpecification<SecJobTitle>
    {
        public JobTitlesSpecifications(int id)
            : base(C => C.Id == id) { }
        public JobTitlesSpecifications(JobTitlesParameters specParams, bool isPaginationEnabled)
        : base(C => (!specParams.Id.HasValue || C.Id == specParams.Id)
        && (string.IsNullOrEmpty(specParams.Barcode) || (Constants.JobTitlesBarcodePrefix + C.Code.ToString().PadLeft((int)CodesLengthEnum.JobTitles, '0') == specParams.Barcode))
        && (!specParams.StatusId.HasValue || C.StatusId == specParams.StatusId)
        && (string.IsNullOrEmpty(specParams.Name) || C.Name.Contains(specParams.Name))
        && (string.IsNullOrEmpty(specParams.Description) || C.Description.Contains(specParams.Description)))
        {
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
                }
            }
            if (isPaginationEnabled)
                ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
        }

        public JobTitlesSpecifications()
            : base()
        {
            AddOrderByDesc(P => P.Id);
        }
    }
}


