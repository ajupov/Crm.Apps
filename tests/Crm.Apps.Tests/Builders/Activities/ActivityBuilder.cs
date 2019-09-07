using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.Models;
using Crm.Clients.Activities.RequestParameters;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Activities
{
    public class ActivityBuilder : IActivityBuilder
    {
        private readonly IActivitiesClient _activitiesClient;
        private readonly ActivityCreateRequest _request;

        public ActivityBuilder(IActivitiesClient activitiesClient)
        {
            _activitiesClient = activitiesClient;
            _request = new ActivityCreateRequest
            {
                AccountId = Guid.Empty,
                TypeId = Guid.Empty,
                LeadId = Guid.Empty,
                StatusId = Guid.Empty,
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
                DeadLineDateTime = null,
                IsDeleted = false
            };
        }

        public ActivityBuilder WithAccountId(Guid accountId)
        {
            _request.AccountId = accountId;

            return this;
        }

        public ActivityBuilder WithTypeId(Guid typeId)
        {
            _request.TypeId = typeId;

            return this;
        }

        public ActivityBuilder WithLeadId(Guid leadId)
        {
            _request.LeadId = leadId;

            return this;
        }

        public ActivityBuilder WithStatusId(Guid statusId)
        {
            _request.StatusId = statusId;

            return this;
        }

        public ActivityBuilder WithCompanyId(Guid companyId)
        {
            _request.CompanyId = companyId;

            return this;
        }

        public ActivityBuilder WithContactId(Guid contactId)
        {
            _request.ContactId = contactId;

            return this;
        }

        public ActivityBuilder WithDealId(Guid dealId)
        {
            _request.DealId = dealId;

            return this;
        }

        public ActivityBuilder WithResponsibleUserId(Guid responsibleUserId)
        {
            _request.ResponsibleUserId = responsibleUserId;

            return this;
        }

        public ActivityBuilder WithName(string name)
        {
            _request.Name = name;

            return this;
        }

        public ActivityBuilder WithDescription(string description)
        {
            _request.Description = description;

            return this;
        }

        public ActivityBuilder WithResult(string result)
        {
            _request.Result = result;

            return this;
        }

        public ActivityBuilder WithPriority(ActivityPriority priority)
        {
            _request.Priority = priority;

            return this;
        }

        public ActivityBuilder WithStartDateTime(DateTime startDateTime)
        {
            _request.StartDateTime = startDateTime;

            return this;
        }

        public ActivityBuilder WithEndDateTime(DateTime endDateTime)
        {
            _request.EndDateTime = endDateTime;

            return this;
        }

        public ActivityBuilder WithDeadLineDateTime(DateTime deadLineDateTime)
        {
            _request.DeadLineDateTime = deadLineDateTime;

            return this;
        }

        public ActivityBuilder AsDeleted()
        {
            _request.IsDeleted = true;

            return this;
        }

        public ActivityBuilder WithAttributeLink(Guid attributeId, string value)
        {
            if (_request.AttributeLinks == null)
            {
                _request.AttributeLinks = new List<ActivityAttributeLink>();
            }

            _request.AttributeLinks.Add(new ActivityAttributeLink
            {
                ActivityAttributeId = attributeId,
                Value = value
            });

            return this;
        }

        public async Task<Activity> BuildAsync()
        {
            if (_request.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_request.AccountId));
            }

            if (_request.TypeId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_request.TypeId));
            }

            if (_request.StatusId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_request.StatusId));
            }

            var createdId = await _activitiesClient.CreateAsync(_request);

            return await _activitiesClient.GetAsync(createdId);
        }
    }
}