using System.ComponentModel.DataAnnotations;

namespace OneTrack.PM.Entities.DTO.Security
{
    public class ContactJobTitlesDTO
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string Entity { get; set; }
        public string JobTitle { get; set; }
        public bool IsActive { get; set; }
    }
    public class ContactJobTitlesLookupDTO : ContactJobTitlesDTO
    { }
    public class ContactJobTitlesCreateResponseDTO
    {
        public int Id { get; set; }
    }
    public class ContactJobTitlesFormCreateDTO
    {
        [Required]
        public int ContactId { get; set; }
        [Required]
        public int EntityId { get; set; }
        [Required]
        public int JobTitleId { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
    public class ContactJobTitlesUpdateDTO : ContactJobTitlesCreateDTO
    {
        [Required]
        public int Id { get; set; }
    }
    public class ContactJobTitlesUpdateFormDTO : ContactJobTitlesFormCreateDTO
    {
        [Required]
        public int Id { get; set; }
    }
    public class ContactJobTitlesCreateDTO : ContactJobTitlesFormCreateDTO
    { }
    public class ContactJobTitlesByIdDTO : ContactJobTitlesCreateDTO
    { }
    public class ContactbJobTitlesUpdateActivationDTO
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
    }
}
