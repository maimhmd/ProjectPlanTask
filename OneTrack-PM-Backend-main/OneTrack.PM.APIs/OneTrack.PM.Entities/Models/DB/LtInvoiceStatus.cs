﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace OneTrack.PM.Entities.Models.DB
{
    public partial class LtInvoiceStatus
    {
        public LtInvoiceStatus()
        {
            FinSupplyOrderInvoices = new HashSet<FinSupplyOrderInvoice>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<FinSupplyOrderInvoice> FinSupplyOrderInvoices { get; set; }
    }
}