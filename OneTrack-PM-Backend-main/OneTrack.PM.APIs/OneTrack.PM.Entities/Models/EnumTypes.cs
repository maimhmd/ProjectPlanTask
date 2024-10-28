namespace OneTrack.PM.Entities.Models
{
    public enum CodesLengthEnum
    {
        JobTitles = 3,
        EntityObjectiveClassifications=3,
        EntitySpecialistClassifications=3,
        ContactCode = 5,
        Country=3
    }
    public enum StatusEnum
    {
        UnderApprove = 1,
        Approved = 2,
        Freezed = 3
    }
    public enum SettingsEnum
    {
        AddedValue = 1,
        DuePeriod = 2
    }
    public enum ContactTypesEnum
    {
        Person = 1,
        Entity = 2
    }
    public enum GendersEnum
    {
        Male = 1,
        Female = 2
    }
}
