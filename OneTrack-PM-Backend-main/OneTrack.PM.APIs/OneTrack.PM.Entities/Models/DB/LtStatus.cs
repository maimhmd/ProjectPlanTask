﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace OneTrack.PM.Entities.Models.DB
{
    public partial class LtStatus
    {
        public LtStatus()
        {
            FinCostCenters = new HashSet<FinCostCenter>();
            FinCosts = new HashSet<FinCost>();
            FinProjectSupplyOrders = new HashSet<FinProjectSupplyOrder>();
            PmProjects = new HashSet<PmProject>();
            PmStandardTasks = new HashSet<PmStandardTask>();
            SecContacts = new HashSet<SecContact>();
            SecGroups = new HashSet<SecGroup>();
            SecJobTitles = new HashSet<SecJobTitle>();
            SecUsers = new HashSet<SecUser>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<FinCostCenter> FinCostCenters { get; set; }
        public virtual ICollection<FinCost> FinCosts { get; set; }
        public virtual ICollection<FinProjectSupplyOrder> FinProjectSupplyOrders { get; set; }
        public virtual ICollection<PmProject> PmProjects { get; set; }
        public virtual ICollection<PmStandardTask> PmStandardTasks { get; set; }
        public virtual ICollection<SecContact> SecContacts { get; set; }
        public virtual ICollection<SecGroup> SecGroups { get; set; }
        public virtual ICollection<SecJobTitle> SecJobTitles { get; set; }
        public virtual ICollection<SecUser> SecUsers { get; set; }
    }
}