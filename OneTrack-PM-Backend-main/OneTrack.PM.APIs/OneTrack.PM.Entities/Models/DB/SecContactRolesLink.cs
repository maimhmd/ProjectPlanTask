﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace OneTrack.PM.Entities.Models.DB
{
    public partial class SecContactRolesLink
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public byte RoleId { get; set; }

        public virtual SecContact Contact { get; set; }
        public virtual SecContactRole Role { get; set; }
    }
}