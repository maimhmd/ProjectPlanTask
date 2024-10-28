using System.Collections.Generic;

namespace OneTrack.PM.Entities.DTOs.Security
{
    public class ModuleFormsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ModuleName { get; set; }
        public int ModuleId { get; set; }
        public ICollection<FormActionTypesDTO> FormActionTypes { get; set; }
    }
}
