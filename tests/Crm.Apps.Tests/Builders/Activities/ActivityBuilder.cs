using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.Models;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Activities
{
    public class ActivityBuilder : IActivityBuilder
    {
        private readonly IActivitiesClient _activitiesClient;
        private readonly Activity _activity;

        public ActivityBuilder(IActivitiesClient activitiesClient)
        {
            _activitiesClient = activitiesClient;
            _activity = new Activity
            {
                AccountId = Guid.Empty,
                TypeId = Guid.Empty,
                LeadId = Guid.Empty,
                StatusId = Guid.Empty,
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
                DeadLineDateTime = null,
                IsDeleted = false
            };
        }

        public ActivityBuilder WithAccountId(Guid accountId)
        {
            _activity.AccountId = accountId;

            return this;
        }

        public ActivityBuilder WithTypeId(Guid typeId)
        {
            _activity.TypeId = typeId;

            return this;
        }

        public ActivityBuilder WithLeadId(Guid leadId)
        {
            _activity.LeadId = leadId;

            return this;
        }

        public ActivityBuilder WithStatusId(Guid statusId)
        {
            _activity.StatusId = statusId;

            return this;
        }

        public ActivityBuilder WithCompanyId(Guid companyId)
        {
            _activity.CompanyId = companyId;

            return this;
        }

        public ActivityBuilder WithContactId(Guid contactId)
        {
            _activity.ContactId = contactId;

            return this;
        }

        public ActivityBuilder WithDealId(Guid dealId)
        {
            _activity.DealId = dealId;

            return this;
        }

        public ActivityBuilder WithCreateUserId(Guid createUserId)
        {
            _activity.CreateUserId = createUserId;

            return this;
        }

        public ActivityBuilder WithResponsibleUserId(Guid responsibleUserId)
        {
            _activity.ResponsibleUserId = responsibleUserId;

            return this;
        }

        public ActivityBuilder WithName(string name)
        {
            _activity.Name = name;

            return this;
        }

        public ActivityBuilder WithDescription(string description)
        {
            _activity.Description = description;

            return this;
        }

        public ActivityBuilder WithResult(string result)
        {
            _activity.Result = result;

            return this;
        }

        public ActivityBuilder WithPriority(ActivityPriority priority)
        {
            _activity.Priority = priority;

            return this;
        }

        public ActivityBuilder WithStartDateTime(DateTime startDateTime)
        {
            _activity.StartDateTime = startDateTime;

            return this;
        }

        public ActivityBuilder WithEndDateTime(DateTime endDateTime)
        {
            _activity.EndDateTime = endDateTime;

            return this;
        }

        public ActivityBuilder WithDeadLineDateTime(DateTime deadLineDateTime)
        {
            _activity.DeadLineDateTime = deadLineDateTime;

            return this;
        }

        public ActivityBuilder AsDeleted()
        {
            _activity.IsDeleted = true;

            return this;
        }

        public ActivityBuilder WithAttributeLink(Guid attributeId, string value)
        {
            if (_activity.AttributeLinks == null)
            {
                _activity.AttributeLinks = new List<ActivityAttributeLink>();
            }

            _activity.AttributeLinks.Add(new ActivityAttributeLink
            {
                ActivityAttributeId = attributeId,
                Value = value
            });

            return this;
        }

        public async Task<Activity> BuildAsync()
        {
            if (_activity.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_activity.AccountId));
            }

            if (_activity.TypeId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_activity.TypeId));
            }

            if (_activity.StatusId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_activity.StatusId));
            }

            var createdId = await _activitiesClient.CreateAsync(_activity).ConfigureAwait(false);

            return await _activitiesClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}