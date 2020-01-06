using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Clients.Leads.Clients;
using Crm.Apps.Clients.Leads.Models;

namespace Crm.Apps.Tests.Builders.Leads
{
    public class LeadBuilder : ILeadBuilder
    {
        private readonly ILeadsClient _leadsClient;
        private readonly Lead _lead;

        public LeadBuilder(ILeadsClient leadsClient)
        {
            _leadsClient = leadsClient;
            _lead = new Lead
            {
                AccountId = Guid.Empty,
                SourceId = Guid.Empty,
                CreateUserId = Guid.Empty,
                ResponsibleUserId = Guid.Empty,
                Surname = "Test",
                Name = "Test",
                Patronymic = "Test",
                Phone = "9999999999",
                Email = "test@test",
                CompanyName = "Test",
                Post = "Test",
                Postcode = "000000",
                Country = "Test",
                Region = "Test",
                Province = "Test",
                City = "Test",
                Street = "Test",
                House = "1",
                Apartment = "1",
                OpportunitySum = 1,
                IsDeleted = false
            };
        }

        public LeadBuilder WithSourceId(Guid sourceId)
        {
            _lead.SourceId = sourceId;

            return this;
        }

        public LeadBuilder WithCreateUserId(Guid createUserId)
        {
            _lead.CreateUserId = createUserId;

            return this;
        }

        public LeadBuilder WithResponsibleUserId(Guid responsibleUserId)
        {
            _lead.ResponsibleUserId = responsibleUserId;

            return this;
        }

        public LeadBuilder WithSurname(string surname)
        {
            _lead.Surname = surname;

            return this;
        }

        public LeadBuilder WithName(string name)
        {
            _lead.Name = name;

            return this;
        }

        public LeadBuilder WithPatronymic(string patronymic)
        {
            _lead.Patronymic = patronymic;

            return this;
        }

        public LeadBuilder WithPhone(string phone)
        {
            _lead.Phone = phone;

            return this;
        }

        public LeadBuilder WithEmail(string email)
        {
            _lead.Email = email;

            return this;
        }

        public LeadBuilder WithCompanyName(string companyName)
        {
            _lead.CompanyName = companyName;

            return this;
        }

        public LeadBuilder WithPost(string post)
        {
            _lead.Post = post;

            return this;
        }

        public LeadBuilder WithPostcode(string postcode)
        {
            _lead.Postcode = postcode;

            return this;
        }

        public LeadBuilder WithCountry(string country)
        {
            _lead.Country = country;

            return this;
        }

        public LeadBuilder WithRegion(string region)
        {
            _lead.Region = region;

            return this;
        }

        public LeadBuilder WithProvince(string province)
        {
            _lead.Province = province;

            return this;
        }

        public LeadBuilder WithCity(string city)
        {
            _lead.City = city;

            return this;
        }

        public LeadBuilder WithStreet(string street)
        {
            _lead.Street = street;

            return this;
        }

        public LeadBuilder WithHouse(string house)
        {
            _lead.House = house;

            return this;
        }

        public LeadBuilder WithApartment(string apartment)
        {
            _lead.Apartment = apartment;

            return this;
        }

        public LeadBuilder WithOpportunitySum(decimal opportunitySum)
        {
            _lead.OpportunitySum = opportunitySum;

            return this;
        }

        public LeadBuilder AsDeleted()
        {
            _lead.IsDeleted = true;

            return this;
        }

        public LeadBuilder WithAttributeLink(Guid attributeId, string value)
        {
            if (_lead.AttributeLinks == null)
            {
                _lead.AttributeLinks = new List<LeadAttributeLink>();
            }

            _lead.AttributeLinks.Add(new LeadAttributeLink
            {
                LeadAttributeId = attributeId,
                Value = value
            });

            return this;
        }

        public async Task<Lead> BuildAsync()
        {
            if (_lead.SourceId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_lead.SourceId));
            }

            var id = await _leadsClient.CreateAsync(_lead);

            return await _leadsClient.GetAsync(id);
        }
    }
}