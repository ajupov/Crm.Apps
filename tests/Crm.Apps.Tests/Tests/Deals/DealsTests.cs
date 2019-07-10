using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Deals.Clients;
using Crm.Clients.Deals.Models;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Xunit;

namespace Crm.Apps.Tests.Tests.Deals
{
    public class DealTests
    {
        private readonly ICreate _create;
        private readonly IDealsClient _dealsClient;

        public DealTests(ICreate create, IDealsClient dealsClient)
        {
            _create = create;
            _dealsClient = dealsClient;
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var type = await _create.DealType.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var status = await _create.DealStatus.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var dealId = (await _create.Deal.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id)
                .BuildAsync().ConfigureAwait(false)).Id;

            var deal = await _dealsClient.GetAsync(dealId).ConfigureAwait(false);

            Assert.NotNull(deal);
            Assert.Equal(dealId, deal.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var type = await _create.DealType.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var status = await _create.DealStatus.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var dealIds = (await Task.WhenAll(
                    _create.Deal.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id).BuildAsync(),
                    _create.Deal.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id).BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            var deals = await _dealsClient.GetListAsync(dealIds).ConfigureAwait(false);

            Assert.NotEmpty(deals);
            Assert.Equal(dealIds.Count, deals.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attribute = await _create.DealAttribute.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var type = await _create.DealType.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var status = await _create.DealStatus.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            await Task.WhenAll(
                    _create.Deal.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id)
                        .WithAttributeLink(attribute.Id, "Test").BuildAsync(),
                    _create.Deal.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id)
                        .WithAttributeLink(attribute.Id, "Test").BuildAsync())
                .ConfigureAwait(false);
            var filterAttributes = new Dictionary<Guid, string> {{attribute.Id, "Test"}};
            var filterSourceIds = new List<Guid> {status.Id};

            var deals = await _dealsClient.GetPagedListAsync(account.Id, sortBy: "CreateDateTime", orderBy: "desc",
                allAttributes: false, attributes: filterAttributes, statusIds: filterSourceIds).ConfigureAwait(false);

            var results = deals.Skip(1)
                .Zip(deals, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(deals);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attribute = await _create.DealAttribute.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var type = await _create.DealType.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var dealStatus = await _create.DealStatus.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var productStatus = await _create.ProductStatus.WithAccountId(account.Id).BuildAsync();
            var product = await _create.Product.WithAccountId(account.Id).WithStatusId(productStatus.Id).BuildAsync()
                .ConfigureAwait(false);

            var deal = new Deal
            {
                AccountId = account.Id,
                TypeId = type.Id,
                StatusId = dealStatus.Id,
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
                IsDeleted = true,
                Positions = new List<DealPosition>
                {
                    new DealPosition
                    {
                        ProductId = product.Id,
                        Count = 1,
                        Price = product.Price
                    }
                },
                AttributeLinks = new List<DealAttributeLink>
                {
                    new DealAttributeLink
                    {
                        DealAttributeId = attribute.Id,
                        Value = "Test"
                    }
                }
            };

            var createdDealId = await _dealsClient.CreateAsync(deal).ConfigureAwait(false);

            var createdDeal = await _dealsClient.GetAsync(createdDealId).ConfigureAwait(false);

            Assert.NotNull(createdDeal);
            Assert.Equal(createdDealId, createdDeal.Id);
            Assert.Equal(deal.AccountId, createdDeal.AccountId);
            Assert.Equal(deal.TypeId, createdDeal.TypeId);
            Assert.Equal(deal.StatusId, createdDeal.StatusId);
            Assert.Equal(deal.CompanyId, createdDeal.CompanyId);
            Assert.Equal(deal.ContactId, createdDeal.ContactId);
            Assert.True(!createdDeal.CreateUserId.IsEmpty());
            Assert.Equal(deal.ResponsibleUserId, createdDeal.ResponsibleUserId);
            Assert.Equal(deal.Name, createdDeal.Name);
            Assert.Equal(deal.StartDateTime.Date, createdDeal.StartDateTime.Date);
            Assert.Equal(deal.EndDateTime?.Date, createdDeal.EndDateTime?.Date);
            Assert.Equal(deal.Sum, createdDeal.Sum);
            Assert.Equal(deal.SumWithoutDiscount, createdDeal.SumWithoutDiscount);
            Assert.Equal(deal.FinishProbability, createdDeal.FinishProbability);
            Assert.Equal(deal.IsDeleted, createdDeal.IsDeleted);
            Assert.True(createdDeal.CreateDateTime.IsMoreThanMinValue());
            Assert.NotEmpty(createdDeal.Positions);
            Assert.NotEmpty(createdDeal.AttributeLinks);
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var type = await _create.DealType.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var dealStatus = await _create.DealStatus.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var productStatus =
                await _create.ProductStatus.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var product = await _create.Product.WithAccountId(account.Id).WithStatusId(productStatus.Id).BuildAsync()
                .ConfigureAwait(false);
            var attribute = await _create.DealAttribute.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var deal = await _create.Deal.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(dealStatus.Id)
                .BuildAsync().ConfigureAwait(false);

            deal.TypeId = type.Id;
            deal.StatusId = dealStatus.Id;
            deal.CompanyId = Guid.Empty;
            deal.ContactId = Guid.Empty;
            deal.ResponsibleUserId = Guid.Empty;
            deal.Name = "Test";
            deal.StartDateTime = DateTime.Now;
            deal.EndDateTime = DateTime.Now.AddDays(1);
            deal.Sum = 1;
            deal.SumWithoutDiscount = 1;
            deal.FinishProbability = 50;
            deal.IsDeleted = true;
            deal.Positions.Add(new DealPosition {ProductId = product.Id, Count = 1, Price = product.Price});
            deal.AttributeLinks.Add(new DealAttributeLink {DealAttributeId = attribute.Id, Value = "Test"});
            await _dealsClient.UpdateAsync(deal).ConfigureAwait(false);

            var updatedDeal = await _dealsClient.GetAsync(deal.Id).ConfigureAwait(false);

            Assert.Equal(deal.AccountId, updatedDeal.AccountId);
            Assert.Equal(deal.TypeId, updatedDeal.TypeId);
            Assert.Equal(deal.StatusId, updatedDeal.StatusId);
            Assert.Equal(deal.CompanyId, updatedDeal.CompanyId);
            Assert.Equal(deal.ContactId, updatedDeal.ContactId);
            Assert.Equal(deal.CreateUserId, updatedDeal.CreateUserId);
            Assert.Equal(deal.ResponsibleUserId, updatedDeal.ResponsibleUserId);
            Assert.Equal(deal.Name, updatedDeal.Name);
            Assert.Equal(deal.StartDateTime.Date, updatedDeal.StartDateTime.Date);
            Assert.Equal(deal.EndDateTime?.Date, updatedDeal.EndDateTime?.Date);
            Assert.Equal(deal.Sum, updatedDeal.Sum);
            Assert.Equal(deal.SumWithoutDiscount, updatedDeal.SumWithoutDiscount);
            Assert.Equal(deal.FinishProbability, updatedDeal.FinishProbability);
            Assert.Equal(deal.IsDeleted, updatedDeal.IsDeleted);
            Assert.Equal(deal.Positions.Single().ProductId, updatedDeal.Positions.Single().ProductId);
            Assert.Equal(deal.Positions.Single().Count, updatedDeal.Positions.Single().Count);
            Assert.Equal(deal.Positions.Single().Price, updatedDeal.Positions.Single().Price);
            Assert.Equal(deal.AttributeLinks.Single().DealAttributeId,
                updatedDeal.AttributeLinks.Single().DealAttributeId);
            Assert.Equal(deal.AttributeLinks.Single().Value, updatedDeal.AttributeLinks.Single().Value);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var type = await _create.DealType.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var status = await _create.DealStatus.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var dealIds = (await Task.WhenAll(
                    _create.Deal.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id).BuildAsync(),
                    _create.Deal.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id).BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _dealsClient.DeleteAsync(dealIds).ConfigureAwait(false);

            var deals = await _dealsClient.GetListAsync(dealIds).ConfigureAwait(false);

            Assert.All(deals, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var type = await _create.DealType.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var status = await _create.DealStatus.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var dealIds = (await Task.WhenAll(
                    _create.Deal.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id).BuildAsync(),
                    _create.Deal.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id).BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _dealsClient.RestoreAsync(dealIds).ConfigureAwait(false);

            var deals = await _dealsClient.GetListAsync(dealIds).ConfigureAwait(false);

            Assert.All(deals, x => Assert.False(x.IsDeleted));
        }
    }
}