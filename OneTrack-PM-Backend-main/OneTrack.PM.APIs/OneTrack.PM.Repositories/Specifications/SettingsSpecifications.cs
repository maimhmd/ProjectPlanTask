using OneTrack.PM.Core.Specifications;
using OneTrack.PM.Entities.Models.DB;

namespace OneTrack.PM.Repositories.Specifications
{
    public class SettingsSpecifications : BaseSpecification<SysSetting>
    {
        public SettingsSpecifications(byte id)
            : base(G => G.Id == id)
        { }
    }
}
