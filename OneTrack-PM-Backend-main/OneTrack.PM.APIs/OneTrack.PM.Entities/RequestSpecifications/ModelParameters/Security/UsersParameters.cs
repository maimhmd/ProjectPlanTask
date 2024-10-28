using OneTrack.PM.Entities.RequestFeatures;

namespace OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security
{
    public class UsersParameters:RequestParameters
    {
        public string Password { get; set; }
        public int? ContactId { get; set; }
        public byte? MainModuleId { get; set; }
    }
}
