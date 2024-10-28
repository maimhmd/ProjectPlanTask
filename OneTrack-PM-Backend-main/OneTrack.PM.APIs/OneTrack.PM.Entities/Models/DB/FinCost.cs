﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace OneTrack.PM.Entities.Models.DB
{
    public partial class FinCost
    {
        public FinCost()
        {
            FinCostAttachments = new HashSet<FinCostAttachment>();
            FinCostDetails = new HashSet<FinCostDetail>();
        }

        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public int Code { get; set; }
        public DateTime Date { get; set; }
        public int SupplierId { get; set; }
        public int CurrencyId { get; set; }
        public DateTime DueDate { get; set; }
        public byte StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifyDate { get; set; }

        public virtual SecUser CreatedByNavigation { get; set; }
        public virtual LtCurrency Currency { get; set; }
        public virtual SecUser ModifiedByNavigation { get; set; }
        public virtual PmProject Project { get; set; }
        public virtual LtStatus Status { get; set; }
        public virtual SecContact Supplier { get; set; }
        public virtual ICollection<FinCostAttachment> FinCostAttachments { get; set; }
        public virtual ICollection<FinCostDetail> FinCostDetails { get; set; }
    }
}