using System.Collections.Generic;

namespace OneTrack.PM.Entities.DTOs.Security
{
    public class ContactAddressesDTO
    {
        public int Id { get; set; }
        public int? CountryId { get; set; }
        public string Country { get; set; }
        public int? GovernorateId { get; set; }
        public string Governorate { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNo { get; set; }
        public string Address { get; set; }
        public string NearestHallmark { get; set; }
        public string Phone { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool? IsHomeAddress { get; set; }
    }

    public class ContactAddressesCreateDTO : ContactAddressesFormCreateDTO
    {
        public int ContactId { get; set; }
    }

    public class ContactAddressesUpdateDTO : ContactAddressesCreateDTO
    {
        public int Id { get; set; }
    }


    public class ContactAddressesFormCreateDTO
    {
        public int? CountryId { get; set; }
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public string Street { get; set; }
        public string BuildingNo { get; set; }
        public string Address { get; set; }
        public string NearestHallmark { get; set; }
        public string Phone { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool? IsHomeAddress { get; set; }
    }

    public class ContactAddressesFormUpdateDTO
    {
        public int? Id { get; set; }
        public int CountryId { get; set; }
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public string Address { get; set; }
        public byte Deleted { get; set; } = 0;
    }

    public class ContactAddressesFormArrayUpdateDTO
    {
        public int Id { get; set; }
        public ICollection<ContactAddressesFormUpdateDTO> addresses { get; set; }
    }

    public class ContactAddressesEditDTO : ContactAddressesFormUpdateDTO { }

}
