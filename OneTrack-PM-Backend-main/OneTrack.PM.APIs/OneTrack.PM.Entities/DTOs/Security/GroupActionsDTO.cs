namespace OneTrack.PM.Entities.DTOs.Security
{
    public class GroupActionsDTO
    {
        public int FormActionTypeId{get;set;}
        public int FormId { get; set; }
        public byte ActionTypeId { get; set; }
    }
    public class GroupActionsCreateDTO
    {
        public byte GroupId { get; set; }
        public int FormActionTypeId { get; set; }
    }
}
