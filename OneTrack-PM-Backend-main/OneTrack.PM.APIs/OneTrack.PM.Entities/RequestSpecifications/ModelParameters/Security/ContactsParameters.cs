using OneTrack.PM.Entities.RequestFeatures;

namespace OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security
{
    public class ContactsParameters:RequestParameters
    {
        public int? Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Barcode { get; set; }
        public byte? RoleId { get; set; }
        public byte? ContactTypeId { get; set; }
        public int? CreatedBy { get; set; }
        public byte? StatusId { get; set; }
        public string SearchTerm { get; set; }
    }
}
