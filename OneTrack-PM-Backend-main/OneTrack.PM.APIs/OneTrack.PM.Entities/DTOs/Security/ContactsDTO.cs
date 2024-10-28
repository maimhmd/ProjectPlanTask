
using OneTrack.PM.Entities.DTOs.Shared;
using OneTrack.PM.Entities.Models;
using System;
using System.Collections.Generic;

namespace OneTrack.PM.Entities.DTOs.Security
{
    public class ContactsDTO
    {
        public int Id { get; set; }
        public string Barcode { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public string NationalId { get; set; }
        public string Status { get; set; }
        public string Roles { get; set; }
        public string Committees { get; set; }
        public ICollection<ContactRolesLinkDTO> RolesIds { get; set; }
    }
    public class ContactsCreateResponseDTO
    {
        public int id { get; set; }
    }
    public class ContactsLookupDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phones { get; set; }
        public string Barcode { get; set; }
    }
    public class ContactsFormCreateDTO
    {
        public string FullName { get; set; }
        public byte? TitleId { get; set; }
        public byte? GenderId { get; set; }
        public byte ContactTypeId { get; set; }
        public string Email { get; set; }
        public string NationalId { get; set; }
        public string Biography { get; set; }
        public int? EntityId { get; set; }
        public int? JobTitleId { get; set; }
        public FileCreateDTO Avatar { get; set; }
        public ICollection<ContactRolesLinkFormCreateDTO> Roles { get; set; }
       public byte StatusId { get; set; } = (byte)StatusEnum.UnderApprove;
    }
    public class ContactsUpdateDTO : ContactsCreateDTO
    {
        public int Id { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifyDate { get; set; } = DateTime.Now;
    }
    public class ContactsUpdateFormDTO : ContactsFormCreateDTO
    {
        public int Id { get; set; }
   }
    public class ContactsCreateDTO
    {
        public int Code { get; set; }
        public string Barcode { get; set; }
        public string FullName { get; set; }
        public string NormalizedFullName { get; set; }
        public byte ContactTypeId { get; set; }
        public byte? TitleId { get; set; }
        public string Email { get; set; }
        public string NationalId { get; set; }
        public string Biography { get; set; }
        public string Avatar { get; set; }
        public byte? GenderId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public byte StatusId { get; set; } = (int)StatusEnum.UnderApprove;
    }
    public class ContactsByIdDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public byte ContactTypeId { get; set; }
        public byte? TitleId { get; set; }
        public byte GenderId { get; set; }
        public string Email { get; set; }
        public string NationalId { get; set; }
        public string Avatar { get; set; }
        public string Biography { get; set; }
        public ICollection<ContactRolesLinkDTO> Roles { get; set; }
        public byte StatusId { get; set; }
    }
    public class ContactsUpdateStatusDTO
    {
        public int Id { get; set; }
        public byte StatusId { get; set; }
    }
    public class ContactsUpdateEmailDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
    }
}
