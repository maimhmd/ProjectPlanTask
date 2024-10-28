using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OneTrack.PM.Entities.DTO.Security
{
    public class ContactPhonesAppDTO
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public bool Whatsapp { get; set; }
    }
    public class ContactPhonesCreateDTO
    {
        [Required]
        public int ContactId { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public bool Whatsapp { get; set; }
    }
    public class ContactPhonesFormCreateDTO
    {
        [Required]
        public string Number { get; set; }
        [Required]
        public bool Whatsapp { get; set; }
    }
    public class ContactPhonesUpdateDTO : ContactPhonesCreateDTO
    {
        [Required]
        public int Id { get; set; }
    }
    public class ContactPhonesFormUpdateDTO
    {
        public int? Id { get; set; }
        [Required]
        public string Number { get; set; }
        public bool? Whatsapp { get; set; }
        public byte Deleted { get; set; } = 0;
    }
    public class ContactPhonesFormArrayUpdateDTO
    {
        public int Id { get; set; } 
        public ICollection<ContactPhonesFormUpdateDTO> Phones { get; set; }
    }
    public class ContactPhonesEditDTO : ContactPhonesFormUpdateDTO { }
}
