
namespace OneTrack.PM.Entities.DTOs.Security
{
    public class TitlesDTO
    {
        public byte Id { get; set; }
        public string Name { get; set; }
    }
    public class TitlesLookupDTO : TitlesDTO
    { }
    public class TitlesCreateDTO
    {
        public string Name { get; set; }
    }
    public class TitleUpdateDTO : TitlesCreateDTO
    {
        public byte Id { get; set; }
    }
    public class TitlesByIdDTO : TitlesDTO
    { }
}
