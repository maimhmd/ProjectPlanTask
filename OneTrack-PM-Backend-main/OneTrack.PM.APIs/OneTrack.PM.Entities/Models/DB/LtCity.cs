﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace OneTrack.PM.Entities.Models.DB
{
    public partial class LtCity
    {
        public LtCity()
        {
            SecContactAddresses = new HashSet<SecContactAddress>();
        }

        public int Id { get; set; }
        public int GovernorateId { get; set; }
        public string Name { get; set; }

        public virtual LtGovernorate Governorate { get; set; }
        public virtual ICollection<SecContactAddress> SecContactAddresses { get; set; }
    }
}