using OneTrack.PM.Entities.RequestFeatures;

namespace OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security
{
    public class ContactPhonesParameters : RequestParameters
    {
        public int? ContactId { get; set; }
        public string Number { get; set; }
        public bool? Whatsapp { get; set; }
    }
}
