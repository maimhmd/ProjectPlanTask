﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace OneTrack.PM.Entities.Models.DB
{
    public partial class SecContactRole
    {
        public SecContactRole()
        {
            SecContactRoleGroups = new HashSet<SecContactRoleGroup>();
            SecContactRolesLinks = new HashSet<SecContactRolesLink>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SecContactRoleGroup> SecContactRoleGroups { get; set; }
        public virtual ICollection<SecContactRolesLink> SecContactRolesLinks { get; set; }
    }
}