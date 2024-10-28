﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace OneTrack.PM.Entities.Models.DB
{
    public partial class PmStandardTask
    {
        public PmStandardTask()
        {
            PmProjectPlans = new HashSet<PmProjectPlan>();
        }

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int Code { get; set; }
        public string Barcode { get; set; }
        public string Task { get; set; }
        public int ParentId { get; set; }
        public decimal RelativeWeight { get; set; }
        public bool IsAutomaticCalc { get; set; }
        public byte Lvl { get; set; }
        public bool? IsCycled { get; set; }
        public byte StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifyDate { get; set; }

        public virtual SecUser CreatedByNavigation { get; set; }
        public virtual SecUser ModifiedByNavigation { get; set; }
        public virtual PmProject Project { get; set; }
        public virtual LtStatus Status { get; set; }
        public virtual ICollection<PmProjectPlan> PmProjectPlans { get; set; }
    }
}