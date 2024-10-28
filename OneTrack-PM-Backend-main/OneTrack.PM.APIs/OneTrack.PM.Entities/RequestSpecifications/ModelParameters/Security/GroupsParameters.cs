using OneTrack.PM.Entities.RequestFeatures;

namespace OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security
{
    public class GroupsParameters : RequestParameters
    { 
        public int? Id { get; set; }
        public string Name { get; set; }
        public byte? MainModuleId { get; set; }
    }
}
