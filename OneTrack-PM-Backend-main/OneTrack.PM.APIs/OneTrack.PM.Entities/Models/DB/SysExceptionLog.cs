﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace OneTrack.PM.Entities.Models.DB
{
    public partial class SysExceptionLog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string PageName { get; set; }
        public int? UserId { get; set; }

        public virtual SecUser User { get; set; }
    }
}