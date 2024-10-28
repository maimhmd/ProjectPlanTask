using System;
using System.ComponentModel.DataAnnotations;

namespace OneTrack.PM.Entities.DTO.Security
{
    public class ContactImagesDTO
    {
        public int Id { get; set; }
        public string Filename { get; set; }
        public string Caption { get; set; }
        public int UploadedBy { get; set; }
        public DateTime UploadDate { get; set; }
    }
    public class ContactImagesCreateDTO
    {
        public int ContactId { get; set; }
        public string Filename { get; set; }
        public string Caption { get; set; }
        public int UploadedBy { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.Now;
    }
    public class ContactImagesFormCreateDTO
    {
        public int? Id { get; set; }
        [Required]
        public string Filename { get; set; }
        public string Base64 { get; set; }
        public string Caption { get; set; }
        [Required]
        public int UploadedBy { get; set; }
        [Required]
        public DateTime UploadDate { get; set; } = DateTime.Now;
        public byte Deleted { get; set; } = 0;
    }
    public class ContactImagesFormUpdateDTO : ContactImagesFormCreateDTO { }
    public class ContactImagesUpdateDTO : ContactImagesCreateDTO
    {
        [Required]
        public int Id { get; set; }
    }
    public class ContactImagesEditDTO : ContactImagesFormUpdateDTO { }
}
