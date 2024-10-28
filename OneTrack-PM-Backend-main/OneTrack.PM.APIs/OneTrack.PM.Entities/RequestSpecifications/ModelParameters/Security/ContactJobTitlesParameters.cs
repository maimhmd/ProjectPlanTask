using OneTrack.PM.Entities.RequestFeatures;

namespace OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security
{
    public class ContactJobTitlesParameters : RequestParameters
    {
        public int? Id { get; set; }
        public int? ContactId { get; set; }
        public int? JobTitleId { get; set; }
        public int? EntityId { get; set; }
        public bool? IsActive { get; set; }
    }
}
