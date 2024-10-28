using OneTrack.PM.Core.Specifications;
using OneTrack.PM.Entities.Models.DB;

namespace OneTrack.PM.Repositories.Specifications.Security
{
    public class ModuleFormsSpecifications : BaseSpecification<SecModuleForm>
    {
        public ModuleFormsSpecifications(byte mainModuleId)
            : base(X => X.Module.MainModuleId == mainModuleId)
        {
            Includes.Add(C => C.Module);
            Includes.Add(C => C.SecFormActionTypes);

            AddOrderBy(P => P.Module.OrderValue);
        }
        public ModuleFormsSpecifications(int formId)
            : base(X => X.Id == formId)
        { }
    }
}
