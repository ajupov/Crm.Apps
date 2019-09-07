using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.Models;
using Crm.Clients.Activities.RequestParameters;
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
            var activityId = (await _create.Activity
                .WithAccountId(account.Id)
                .WithTypeId(type.Id)
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
                ).Select(x => x.Id).ToArray();

            var activities = await _activitiesClient.GetListAsync(activityIds);

            Assert.NotEmpty(activities);
            Assert.Equal(activityIds.Length, activities.Length);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var attribute = await _create.ActivityAttribute.WithAccountId(account.Id).BuildAsync();
            var type = await _create.ActivityType.WithAccountId(account.Id).BuildAsync();
            var status = await _create.ActivityStatus.WithAccountId(account.Id).BuildAsync();
            await Task.WhenAll(
                _create.Activity.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id)
                    .WithAttributeLink(attribute.Id, "Test").BuildAsync(),
                _create.Activity.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id)
                    .WithAttributeLink(attribute.Id, "Test").BuildAsync());
            var filterAttributes = new Dictionary<Guid, string> {{attribute.Id, "Test"}};
            var filterStatusIds = new List<Guid> {status.Id};

            var request = new ActivityGetPagedListRequest
            {
                AllAttributes = false,
                Attributes = filterAttributes,
                StatusIds = filterStatusIds
            };

            var activities = await _activitiesClient.GetPagedListAsync(request);

            var results = activities
                .Skip(1)
                .Zip(activities, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(activities);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var attribute = await _create.ActivityAttribute.WithAccountId(account.Id).BuildAsync();
            var type = await _create.ActivityType.WithAccountId(account.Id).BuildAsync();
            var activityStatus = await _create.ActivityStatus.WithAccountId(account.Id).BuildAsync();

            var request = new ActivityCreateRequest
            {
                AccountId = account.Id,
                TypeId = type.Id,
                StatusId = activityStatus.Id,
                LeadId = Guid.Empty,
                CompanyId = Guid.Empty,
                ContactId = Guid.Empty,
                DealId = Guid.Empty,
                ResponsibleUserId = Guid.Empty,
                Name = "Test",
                Description = "Test",
                Result = "Test",
                Priority = ActivityPriority.Medium,
                StartDateTime = DateTime.UtcNow,
                EndDateTime = DateTime.UtcNow.AddDays(1),
                DeadLineDateTime = DateTime.UtcNow.AddDays(2),
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

            var createdActivityId = await _activitiesClient.CreateAsync(request);

            var createdActivity = await _activitiesClient.GetAsync(createdActivityId);

            Assert.NotNull(createdActivity);
            Assert.Equal(createdActivityId, createdActivity.Id);
            Assert.Equal(request.AccountId, createdActivity.AccountId);
            Assert.Equal(request.TypeId, createdActivity.TypeId);
            Assert.Equal(request.StatusId, createdActivity.StatusId);
            Assert.Equal(request.LeadId, createdActivity.LeadId);
            Assert.Equal(request.CompanyId, createdActivity.CompanyId);
            Assert.Equal(request.ContactId, createdActivity.ContactId);
            Assert.Equal(request.DealId, createdActivity.DealId);
            Assert.True(!createdActivity.CreateUserId.IsEmpty());
            Assert.Equal(request.ResponsibleUserId, createdActivity.ResponsibleUserId);
            Assert.Equal(request.Name, createdActivity.Name);
            Assert.Equal(request.Description, createdActivity.Description);
            Assert.Equal(request.Result, createdActivity.Result);
            Assert.Equal(request.Priority, createdActivity.Priority);
            Assert.Equal(request.StartDateTime?.Date, createdActivity.StartDateTime?.Date);
            Assert.Equal(request.EndDateTime?.Date, createdActivity.EndDateTime?.Date);
            Assert.Equal(request.DeadLineDateTime?.Date, createdActivity.DeadLineDateTime?.Date);
            Assert.Equal(request.IsDeleted, createdActivity.IsDeleted);
            Assert.True(createdActivity.CreateDateTime.IsMoreThanMinValue());
            Assert.NotEmpty(createdActivity.AttributeLinks);
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var type = await _create.ActivityType.WithAccountId(account.Id).BuildAsync();
            var activityStatus = await _create.ActivityStatus.WithAccountId(account.Id).BuildAsync();
            var attribute = await _create.ActivityAttribute.WithAccountId(account.Id).BuildAsync();
            var activity = await _create.Activity.WithAccountId(account.Id).WithTypeId(type.Id)
                .WithStatusId(activityStatus.Id).BuildAsync();

            var request = new ActivityUpdateRequest
            {
                TypeId = type.Id,
                AccountId = account.Id,
                StatusId = activityStatus.Id,
                LeadId = null,
                CompanyId = null,
                ContactId = null,
                DealId = null,
                ResponsibleUserId = null,
                Name = "Test",
                Description = "Test",
                Result = "Test",
                Priority = ActivityPriority.Medium,
                StartDateTime = DateTime.UtcNow,
                EndDateTime = DateTime.UtcNow.AddDays(1),
                DeadLineDateTime = DateTime.UtcNow.AddDays(1),
                IsDeleted = true,
                AttributeLinks = new List<ActivityAttributeLink>
                {
                    new ActivityAttributeLink {ActivityAttributeId = attribute.Id, Value = "Test"}
                }
            };

            await _activitiesClient.UpdateAsync(request);

            var updatedActivity = await _activitiesClient.GetAsync(activity.Id);

            Assert.Equal(request.AccountId, updatedActivity.AccountId);
            Assert.Equal(request.TypeId, updatedActivity.TypeId);
            Assert.Equal(request.StatusId, updatedActivity.StatusId);
            Assert.Equal(request.LeadId, updatedActivity.LeadId);
            Assert.Equal(request.CompanyId, updatedActivity.CompanyId);
            Assert.Equal(request.ContactId, updatedActivity.ContactId);
            Assert.Equal(request.DealId, updatedActivity.DealId);
            Assert.Equal(request.ResponsibleUserId, updatedActivity.ResponsibleUserId);
            Assert.Equal(request.Name, updatedActivity.Name);
            Assert.Equal(request.Description, updatedActivity.Description);
            Assert.Equal(request.Result, updatedActivity.Result);
            Assert.Equal(request.Priority, updatedActivity.Priority);
            Assert.Equal(request.StartDateTime?.Date, updatedActivity.StartDateTime?.Date);
            Assert.Equal(request.EndDateTime?.Date, updatedActivity.EndDateTime?.Date);
            Assert.Equal(request.DeadLineDateTime?.Date, updatedActivity.DeadLineDateTime?.Date);
            Assert.Equal(request.IsDeleted, updatedActivity.IsDeleted);
            Assert.Equal(request.AttributeLinks.Single().ActivityAttributeId,
                updatedActivity.AttributeLinks.Single().ActivityAttributeId);
            Assert.Equal(request.AttributeLinks.Single().Value, updatedActivity.AttributeLinks.Single().Value);
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
                )
                .Select(x => x.Id)
                .ToArray();

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
                )
                .Select(x => x.Id)
                .ToArray();

            await _activitiesClient.RestoreAsync(activityIds);

            var activities = await _activitiesClient.GetListAsync(activityIds);

            Assert.All(activities, x => Assert.False(x.IsDeleted));
        }
    }
}