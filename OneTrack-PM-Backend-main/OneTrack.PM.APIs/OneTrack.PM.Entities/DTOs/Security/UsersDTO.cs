using OneTrack.PM.Entities.Models;
namespace OneTrack.PM.Entities.DTOs.Security
{
    public class UsersDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int ContactId { get; set; }
        public string Name { get; set; }
        public byte GroupId { get; set; }
        public byte StatusId { get; set; }
        public string Group { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
    }
    public class UsersCreateDTO
    {
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public byte GroupId { get; set; }
        public byte statusId { get; set; } = (byte)StatusEnum.UnderApprove;
    }
    public class UsersByIdDTO:UsersDTO
    {
    }
    public class UsersUpdateDTO : UsersCreateDTO
    {
        public int Id
        {
            get; set;
        }
    }
    public class UsersChangePasswordFormDTO
    {
        public int ContactId { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
    }
    public class UsersChangePasswordDTO
    {
        public int Id { get; set; }
        public string Password { get; set; }
    }
    public class UsersResetPasswordDTO
    {
        public int Id { get; set; }
        public string Password { get; set; }
    }
    public class UsersUpdateStatusDTO
    {
        public int Id { get; set; }
        public byte StatusId { get; set; }
    }
}
