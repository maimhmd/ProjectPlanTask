using OneTrack.PM.Entities.RequestFeatures;

namespace OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security
{
    public class ContactAddressesParameters : RequestParameters
    {
        public int? Id { get; set; }
        public int? ContactId { get; set; }
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public string Street { get; set; }
        public string BuildingNo { get; set; }
        public string Address { get; set; }
        public string NearestHallmark { get; set; }
        public string Phone { get; set; }
        public bool? IsHomeAddress { get; set; }
    }
}

