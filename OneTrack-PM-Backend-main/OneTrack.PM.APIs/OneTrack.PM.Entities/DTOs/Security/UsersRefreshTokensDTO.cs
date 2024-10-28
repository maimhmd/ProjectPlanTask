using System.ComponentModel.DataAnnotations;

namespace OneTrack.PM.Entities.DTOs.Security
{
    public struct UsersRefreshTokensCreateDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string RefreshToken { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public string FireBaseToken { get; set; }
    }
}
