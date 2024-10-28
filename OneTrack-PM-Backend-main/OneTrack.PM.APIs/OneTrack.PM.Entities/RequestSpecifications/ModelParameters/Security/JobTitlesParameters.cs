using OneTrack.PM.Entities.RequestFeatures;

namespace OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security
{
    public class JobTitlesParameters : RequestParameters
    {
        public int? Id { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte? StatusId { get; set; }
    }
}
