using System.ComponentModel.DataAnnotations;

namespace OneTrack.PM.Entities.DTOs.Security
{
    public class ContactRolesLinkDTO
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
    }

    public class ContactRolesLinkCreateDTO
    {
        [Required]
        public int RoleId { get; set; }
    }
    public class ContactRolesLinkFormCreateDTO
    {
        public byte RoleId { get; set; }
    }
}
