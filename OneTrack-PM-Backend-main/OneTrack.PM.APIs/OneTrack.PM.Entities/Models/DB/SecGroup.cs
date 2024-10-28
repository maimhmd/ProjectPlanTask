﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace OneTrack.PM.Entities.Models.DB
{
    public partial class SecGroup
    {
        public SecGroup()
        {
            SecGroupActions = new HashSet<SecGroupAction>();
            SecUsers = new HashSet<SecUser>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }
        public int? MainModuleId { get; set; }
        public byte? ParentId { get; set; }
        public byte StatusId { get; set; }

        public virtual SecMainModule MainModule { get; set; }
        public virtual LtStatus Status { get; set; }
        public virtual ICollection<SecGroupAction> SecGroupActions { get; set; }
        public virtual ICollection<SecUser> SecUsers { get; set; }
    }
}