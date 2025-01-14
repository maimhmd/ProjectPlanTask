﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace OneTrack.PM.Entities.Models.DB
{
    public partial class LtDocType
    {
        public LtDocType()
        {
            FinCostAttachments = new HashSet<FinCostAttachment>();
            FinInvoiceAttachments = new HashSet<FinInvoiceAttachment>();
            FinSupplyOrderAttachments = new HashSet<FinSupplyOrderAttachment>();
            PmProjectAttachments = new HashSet<PmProjectAttachment>();
            PmProjectPlanAttachments = new HashSet<PmProjectPlanAttachment>();
            SecFormActionTypeAttachments = new HashSet<SecFormActionTypeAttachment>();
            SecMainModuleAttachments = new HashSet<SecMainModuleAttachment>();
            SecModuleAttachments = new HashSet<SecModuleAttachment>();
            SecModuleFormAttachments = new HashSet<SecModuleFormAttachment>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<FinCostAttachment> FinCostAttachments { get; set; }
        public virtual ICollection<FinInvoiceAttachment> FinInvoiceAttachments { get; set; }
        public virtual ICollection<FinSupplyOrderAttachment> FinSupplyOrderAttachments { get; set; }
        public virtual ICollection<PmProjectAttachment> PmProjectAttachments { get; set; }
        public virtual ICollection<PmProjectPlanAttachment> PmProjectPlanAttachments { get; set; }
        public virtual ICollection<SecFormActionTypeAttachment> SecFormActionTypeAttachments { get; set; }
        public virtual ICollection<SecMainModuleAttachment> SecMainModuleAttachments { get; set; }
        public virtual ICollection<SecModuleAttachment> SecModuleAttachments { get; set; }
        public virtual ICollection<SecModuleFormAttachment> SecModuleFormAttachments { get; set; }
    }
}