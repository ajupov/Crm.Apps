using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crm.Clients.Deals.Clients;
using Crm.Clients.Deals.Models;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Deals
{
    public class DealBuilder : IDealBuilder
    {
        private readonly IDealsClient _dealsClient;
        private readonly Deal _deal;

        public DealBuilder(IDealsClient dealsClient)
        {
            _dealsClient = dealsClient;
            _deal = new Deal
            {
                AccountId = Guid.Empty,
                TypeId = Guid.Empty,
                StatusId = Guid.Empty,
                CompanyId = Guid.Empty,
                ContactId = Guid.Empty,
                CreateUserId = Guid.Empty,
                ResponsibleUserId = Guid.Empty,
                Name = "Test",
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now.AddDays(1),
                Sum = 1,
                SumWithoutDiscount = 1,
                FinishProbability = 50,
                IsDeleted = false
            };
        }

        public DealBuilder WithAccountId(Guid accountId)
        {
            _deal.AccountId = accountId;

            return this;
        }

        public DealBuilder WithTypeId(Guid typeId)
        {
            _deal.TypeId = typeId;

            return this;
        }

        public DealBuilder WithStatusId(Guid statusId)
        {
            _deal.StatusId = statusId;

            return this;
        }

        public DealBuilder WithCompanyId(Guid companyId)
        {
            _deal.CompanyId = companyId;

            return this;
        }

        public DealBuilder WithContactId(Guid contactId)
        {
            _deal.ContactId = contactId;

            return this;
        }

        public DealBuilder WithCreateUserId(Guid createUserId)
        {
            _deal.CreateUserId = createUserId;

            return this;
        }

        public DealBuilder WithResponsibleUserId(Guid responsibleUserId)
        {
            _deal.ResponsibleUserId = responsibleUserId;

            return this;
        }

        public DealBuilder WithName(string name)
        {
            _deal.Name = name;

            return this;
        }

        public DealBuilder WithStartDateTime(DateTime startDateTime)
        {
            _deal.StartDateTime = startDateTime;

            return this;
        }

        public DealBuilder WithEndDateTime(DateTime endDateTime)
        {
            _deal.EndDateTime = endDateTime;

            return this;
        }

        public DealBuilder WithSum(decimal sum)
        {
            _deal.Sum = sum;

            return this;
        }

        public DealBuilder WithSumWithoutDiscount(decimal sumWithoutDiscount)
        {
            _deal.SumWithoutDiscount = sumWithoutDiscount;

            return this;
        }

        public DealBuilder WithFinishProbability(byte finishProbability)
        {
            _deal.FinishProbability = finishProbability;

            return this;
        }

        public DealBuilder AsDeleted()
        {
            _deal.IsDeleted = true;

            return this;
        }

        public DealBuilder WithAttributeLink(Guid attributeId, string value)
        {
            if (_deal.AttributeLinks == null)
            {
                _deal.AttributeLinks = new List<DealAttributeLink>();
            }

            _deal.AttributeLinks.Add(new DealAttributeLink
            {
                DealAttributeId = attributeId,
                Value = value
            });

            return this;
        }

        public async Task<Deal> BuildAsync()
        {
            if (_deal.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_deal.AccountId));
            }

            if (_deal.TypeId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_deal.TypeId));
            }

            if (_deal.StatusId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_deal.StatusId));
            }

            var createdId = await _dealsClient.CreateAsync(_deal).ConfigureAwait(false);

            return await _dealsClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}