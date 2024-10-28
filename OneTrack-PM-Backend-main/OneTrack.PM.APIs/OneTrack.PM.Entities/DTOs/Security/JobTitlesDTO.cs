namespace OneTrack.PM.Entities.DTOs.Security
{
    public class JobTitlesDTO
    {
        public int Id { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
    public class JobTitlesLookupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class JobTitlesFormCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public byte StatusId { get; set; }
    }
    public class JobTitlesCreateDTO : JobTitlesFormCreateDTO
    {
        public int Code { get; set; }
    }
    public class JobTitleFormUpdateDTO : JobTitlesFormCreateDTO
    {
        public int Id { get; set; }
    }
    public class JobTitleUpdateDTO: JobTitlesCreateDTO
    {
        public int Id { get; set; }
    }
    public class JobTitlesByIdDTO : JobTitleUpdateDTO
    {
        public string Barcode { get; set; }
    }
    public class JobTitleUpdateStatusDTO
    {
        public int Id { get; set; }
        public byte StatusId { get; set; }
    }
}
