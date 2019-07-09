using System;
using System.Threading.Tasks;
using Crm.Clients.Deals.Models;

namespace Crm.Apps.Tests.Builders.Deals
{
    public interface IDealBuilder
    {
        DealBuilder WithAccountId(Guid accountId);

        DealBuilder WithTypeId(Guid typeId);

        DealBuilder WithStatusId(Guid statusId);

        DealBuilder WithCompanyId(Guid companyId);

        DealBuilder WithContactId(Guid contactId);

        DealBuilder WithCreateUserId(Guid createUserId);

        DealBuilder WithResponsibleUserId(Guid responsibleUserId);

        DealBuilder WithName(string name);

        DealBuilder WithStartDateTime(DateTime startDateTime);

        DealBuilder WithEndDateTime(DateTime endDateTime);

        DealBuilder WithSum(decimal sum);

        DealBuilder WithSumWithDiscount(decimal sumWithDiscount);

        DealBuilder WithFinishProbability(byte finishProbability);

        DealBuilder AsDeleted();

        DealBuilder WithAttributeLink(Guid attributeId, string value);

        Task<Deal> BuildAsync();
    }
}