using OneTrack.PM.Entities.RequestFeatures;

namespace OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security
{
    public class ContactImagesParameters : RequestParameters
    {
        public int? ContactId { get; set; }
        public string Caption { get; set; }
    }
}
