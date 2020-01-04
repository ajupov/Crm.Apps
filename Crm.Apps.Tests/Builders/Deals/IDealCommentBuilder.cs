using System;
using System.Threading.Tasks;

namespace Crm.Apps.Tests.Builders.Deals
{
    public interface IDealCommentBuilder
    {
        DealCommentBuilder WithDealId(Guid dealId);
        
        Task BuildAsync();
    }
}