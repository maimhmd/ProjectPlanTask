using System.ComponentModel.DataAnnotations;

namespace OneTrack.PM.Entities.DTOs.Security
{
    public struct LoginDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
