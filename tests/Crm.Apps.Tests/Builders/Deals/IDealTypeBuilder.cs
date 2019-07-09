using System;
using System.Threading.Tasks;
using Crm.Clients.Deals.Models;

namespace Crm.Apps.Tests.Builders.Deals
{
    public interface IDealTypeBuilder
    {
        DealTypeBuilder WithAccountId(Guid accountId);

        DealTypeBuilder WithName(string name);

        Task<DealType> BuildAsync();
    }
}