using System;
using System.Threading.Tasks;
using Crm.Apps.Clients.Deals.Models;

namespace Crm.Apps.Tests.Builders.Deals
{
    public interface IDealTypeBuilder
    {
        DealTypeBuilder WithAccountId(Guid accountId);

        DealTypeBuilder WithName(string name);

        DealTypeBuilder AsDeleted();

        Task<DealType> BuildAsync();
    }
}