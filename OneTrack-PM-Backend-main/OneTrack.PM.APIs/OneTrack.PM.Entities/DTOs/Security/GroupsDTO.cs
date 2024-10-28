using OneTrack.PM.Entities.Models;

namespace OneTrack.PM.Entities.DTOs.Security
{
    public class GroupsDTO
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public byte? MainModuleId { get; set; }
        public string MainModule { get; set; }
        public string Status { get; set; }
    }
    public class GroupsLookupDTO
    {
        public byte Id { get; set; }
        public string Name { get; set; }
    }

    public class GroupsUpdateFormDTO : GroupsFormCreateDTO
    {
    }
    public class GroupsUpdateDTO : GroupsCreateDTO
    {
        public byte Id { get; set; }
    }
    public class GroupsFormCreateDTO
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public byte? MainModuleId { get; set; }
        public byte StatusId { get; set; } = (byte)StatusEnum.UnderApprove;
        public ICollection<GroupActionsDTO> Actions { get; set; }
    }
    public class GroupsCreateDTO
    {
        public string Name { get; set; }
        public byte? MainModuleId { get; set; }
        public byte StatusId { get; set; } = (byte)StatusEnum.UnderApprove;
    }
    public class GroupsUpdateStatusDTO
    {
        public byte Id { get; set; }
        public byte StatusId { get; set; }
    }
    public class GroupsByIdDTO : GroupsCreateDTO
    {
        public ICollection<GroupActionsDTO> Actions { get; set; } 
    }
}
