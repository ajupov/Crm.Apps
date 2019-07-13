using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.Models;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Xunit;

namespace Crm.Apps.Tests.Tests.Activities
{
    public class ActivityTests
    {
        private readonly ICreate _create;
        private readonly IActivitiesClient _activitiesClient;

        public ActivityTests(ICreate create, IActivitiesClient activitiesClient)
        {
            _create = create;
            _activitiesClient = activitiesClient;
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var type = await _create.ActivityType.WithAccountId(account.Id).BuildAsync();
            var status = await _create.ActivityStatus.WithAccountId(account.Id).BuildAsync();
            var activityId = (await _create.Activity.WithAccountId(account.Id).WithTypeId(type.Id)
                .WithStatusId(status.Id)
                .BuildAsync()).Id;

            var activity = await _activitiesClient.GetAsync(activityId);

            Assert.NotNull(activity);
            Assert.Equal(activityId, activity.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var type = await _create.ActivityType.WithAccountId(account.Id).BuildAsync();
            var status = await _create.ActivityStatus.WithAccountId(account.Id).BuildAsync();
            var activityIds = (await Task.WhenAll(
                    _create.Activity.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id).BuildAsync(),
                    _create.Activity.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id).BuildAsync())
                ).Select(x => x.Id).ToList();

            var activities = await _activitiesClient.GetListAsync(activityIds);

            Assert.NotEmpty(activities);
            Assert.Equal(activityIds.Count, activities.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var attribute = await _create.ActivityAttribute.WithAccountId(account.Id).BuildAsync()
                ;
            var type = await _create.ActivityType.WithAccountId(account.Id).BuildAsync();
            var status = await _create.ActivityStatus.WithAccountId(account.Id).BuildAsync();
            await Task.WhenAll(
                    _create.Activity.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id)
                        .WithAttributeLink(attribute.Id, "Test").BuildAsync(),
                    _create.Activity.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id)
                        .WithAttributeLink(attribute.Id, "Test").BuildAsync())
                ;
            var filterAttributes = new Dictionary<Guid, string> {{attribute.Id, "Test"}};
            var filterSourceIds = new List<Guid> {status.Id};

            var activities = await _activitiesClient.GetPagedListAsync(account.Id, sortBy: "CreateDateTime",
                orderBy: "desc",
                allAttributes: false, attributes: filterAttributes, statusIds: filterSourceIds);

            var results = activities.Skip(1)
                .Zip(activities, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(activities);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var attribute = await _create.ActivityAttribute.WithAccountId(account.Id).BuildAsync()
                ;
            var type = await _create.ActivityType.WithAccountId(account.Id).BuildAsync();
            var activityStatus =
                await _create.ActivityStatus.WithAccountId(account.Id).BuildAsync();
            var productStatus = await _create.ProductStatus.WithAccountId(account.Id).BuildAsync();
            var product = await _create.Product.WithAccountId(account.Id).WithStatusId(productStatus.Id).BuildAsync()
                ;

            var activity = new Activity
            {
                AccountId = account.Id,
                TypeId = type.Id,
                StatusId = activityStatus.Id,
                LeadId = Guid.Empty,
                CompanyId = Guid.Empty,
                ContactId = Guid.Empty,
                DealId = Guid.Empty,
                CreateUserId = Guid.Empty,
                ResponsibleUserId = Guid.Empty,
                Name = "Test",
                Description = "Test",
                Result = "Test",
                Priority = ActivityPriority.None,
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now.AddDays(1),
                DeadLineDateTime = DateTime.Now.AddDays(2),
                IsDeleted = true,
                AttributeLinks = new List<ActivityAttributeLink>
                {
                    new ActivityAttributeLink
                    {
                        ActivityAttributeId = attribute.Id,
                        Value = "Test"
                    }
                }
            };

            var createdActivityId = await _activitiesClient.CreateAsync(activity);

            var createdActivity = await _activitiesClient.GetAsync(createdActivityId);

            Assert.NotNull(createdActivity);
            Assert.Equal(createdActivityId, createdActivity.Id);
            Assert.Equal(activity.AccountId, createdActivity.AccountId);
            Assert.Equal(activity.TypeId, createdActivity.TypeId);
            Assert.Equal(activity.StatusId, createdActivity.StatusId);
            Assert.Equal(activity.LeadId, createdActivity.LeadId);
            Assert.Equal(activity.CompanyId, createdActivity.CompanyId);
            Assert.Equal(activity.ContactId, createdActivity.ContactId);
            Assert.Equal(activity.DealId, createdActivity.DealId);
            Assert.True(!createdActivity.CreateUserId.IsEmpty());
            Assert.Equal(activity.ResponsibleUserId, createdActivity.ResponsibleUserId);
            Assert.Equal(activity.Name, createdActivity.Name);
            Assert.Equal(activity.Description, createdActivity.Description);
            Assert.Equal(activity.Result, createdActivity.Result);
            Assert.Equal(activity.Priority, createdActivity.Priority);
            Assert.Equal(activity.StartDateTime.Date, createdActivity.StartDateTime.Date);
            Assert.Equal(activity.EndDateTime?.Date, createdActivity.EndDateTime?.Date);
            Assert.Equal(activity.DeadLineDateTime?.Date, createdActivity.DeadLineDateTime?.Date);
            Assert.Equal(activity.IsDeleted, createdActivity.IsDeleted);
            Assert.True(createdActivity.CreateDateTime.IsMoreThanMinValue());
            Assert.NotEmpty(createdActivity.AttributeLinks);
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var type = await _create.ActivityType.WithAccountId(account.Id).BuildAsync();
            var activityStatus =
                await _create.ActivityStatus.WithAccountId(account.Id).BuildAsync();
            var attribute = await _create.ActivityAttribute.WithAccountId(account.Id).BuildAsync()
                ;
            var activity = await _create.Activity.WithAccountId(account.Id).WithTypeId(type.Id)
                .WithStatusId(activityStatus.Id)
                .BuildAsync();

            activity.TypeId = type.Id;
            activity.StatusId = activityStatus.Id;
            activity.LeadId = Guid.Empty;
            activity.CompanyId = Guid.Empty;
            activity.ContactId = Guid.Empty;
            activity.DealId = Guid.Empty;
            activity.ResponsibleUserId = Guid.Empty;
            activity.Name = "Test";
            activity.Description = "Test";
            activity.Result = "Test";
            activity.Priority = ActivityPriority.Medium;
            activity.StartDateTime = DateTime.Now;
            activity.EndDateTime = DateTime.Now.AddDays(1);
            activity.DeadLineDateTime = DateTime.Now.AddDays(1);
            activity.IsDeleted = true;
            activity.AttributeLinks.Add(new ActivityAttributeLink {ActivityAttributeId = attribute.Id, Value = "Test"});
            await _activitiesClient.UpdateAsync(activity);

            var updatedActivity = await _activitiesClient.GetAsync(activity.Id);

            Assert.Equal(activity.AccountId, updatedActivity.AccountId);
            Assert.Equal(activity.TypeId, updatedActivity.TypeId);
            Assert.Equal(activity.StatusId, updatedActivity.StatusId);
            Assert.Equal(activity.LeadId, updatedActivity.LeadId);
            Assert.Equal(activity.CompanyId, updatedActivity.CompanyId);
            Assert.Equal(activity.ContactId, updatedActivity.ContactId);
            Assert.Equal(activity.DealId, updatedActivity.DealId);
            Assert.Equal(activity.CreateUserId, updatedActivity.CreateUserId);
            Assert.Equal(activity.ResponsibleUserId, updatedActivity.ResponsibleUserId);
            Assert.Equal(activity.Name, updatedActivity.Name);
            Assert.Equal(activity.Description, updatedActivity.Description);
            Assert.Equal(activity.Result, updatedActivity.Result);
            Assert.Equal(activity.Priority, updatedActivity.Priority);
            Assert.Equal(activity.StartDateTime.Date, updatedActivity.StartDateTime.Date);
            Assert.Equal(activity.EndDateTime?.Date, updatedActivity.EndDateTime?.Date);
            Assert.Equal(activity.DeadLineDateTime?.Date, updatedActivity.DeadLineDateTime?.Date);
            Assert.Equal(activity.IsDeleted, updatedActivity.IsDeleted);
            Assert.Equal(activity.AttributeLinks.Single().ActivityAttributeId,
                updatedActivity.AttributeLinks.Single().ActivityAttributeId);
            Assert.Equal(activity.AttributeLinks.Single().Value, updatedActivity.AttributeLinks.Single().Value);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var type = await _create.ActivityType.WithAccountId(account.Id).BuildAsync();
            var status = await _create.ActivityStatus.WithAccountId(account.Id).BuildAsync();
            var activityIds = (await Task.WhenAll(
                    _create.Activity.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id).BuildAsync(),
                    _create.Activity.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id).BuildAsync())
                ).Select(x => x.Id).ToList();

            await _activitiesClient.DeleteAsync(activityIds);

            var activities = await _activitiesClient.GetListAsync(activityIds);

            Assert.All(activities, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var type = await _create.ActivityType.WithAccountId(account.Id).BuildAsync();
            var status = await _create.ActivityStatus.WithAccountId(account.Id).BuildAsync();
            var activityIds = (await Task.WhenAll(
                    _create.Activity.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id).BuildAsync(),
                    _create.Activity.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id).BuildAsync())
                ).Select(x => x.Id).ToList();

            await _activitiesClient.RestoreAsync(activityIds);

            var activities = await _activitiesClient.GetListAsync(activityIds);

            Assert.All(activities, x => Assert.False(x.IsDeleted));
        }
    }
}