﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OneTrack.PM.Entities.Models.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace OneTrack.PM.Entities.Models.DB
{
    public partial interface IOneTrackPMContextProcedures
    {
        Task<List<SP_SEC_SelectUserPermissionResult>> SP_SEC_SelectUserPermissionAsync(int? UserId, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
    }
}
