﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Settings.V1.Requests;
using Crm.Apps.Settings.V1.Responses;

namespace Crm.Apps.Settings.Services
{
    public interface IAccountSettingChangesService
    {
        Task<AccountSettingChangeGetPagedListResponse> GetPagedListAsync(Guid accountId,
            AccountSettingChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}