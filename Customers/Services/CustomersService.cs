using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Decimal;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Customers.Helpers;
using Crm.Apps.Customers.Mappers;
using Crm.Apps.Customers.Models;
using Crm.Apps.Customers.Storages;
using Crm.Apps.Customers.V1.Requests;
using Crm.Apps.Customers.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Customers.Services
{
    public class CustomersService : ICustomersService
    {
        private readonly CustomersStorage _storage;

        public CustomersService(CustomersStorage storage)
        {
            _storage = storage;
        }

        public Task<Customer> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct)
        {
            return _storage.Customers
                .AsTracking(isTrackChanges ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
                .Include(x => x.Source)
                .Include(x => x.AttributeLinks)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Customer>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Customers
                .AsNoTracking()
                .Include(x => x.Source)
                .Include(x => x.AttributeLinks)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<CustomerGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            CustomerGetPagedListRequest request,
            CancellationToken ct)
        {
            var customers = await _storage.Customers
                .AsNoTracking()
                .Include(x => x.Source)
                .Include(x => x.AttributeLinks)
                .Where(x =>
                    x.AccountId == accountId &&
                    (request.Surname.IsEmpty() || EF.Functions.ILike(x.Surname, $"{request.Surname}%")) &&
                    (request.Name.IsEmpty() || EF.Functions.ILike(x.Name, $"{request.Name}%")) &&
                    (request.Patronymic.IsEmpty() || EF.Functions.ILike(x.Patronymic, $"{request.Patronymic}%")) &&
                    (request.Phone.IsEmpty() || x.Phone == request.Phone) &&
                    (request.Email.IsEmpty() || x.Email == request.Email) &&
                    (!request.MinBirthDate.HasValue || x.BirthDate >= request.MinBirthDate) &&
                    (!request.MaxBirthDate.HasValue || x.BirthDate <= request.MaxBirthDate) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .ToListAsync(ct);

            return new CustomerGetPagedListResponse
            {
                TotalCount = customers
                    .Count(x => x.FilterByAdditional(request)),
                LastModifyDateTime = customers
                    .Max(x => x.ModifyDateTime),
                Customers = customers
                    .Where(x => x.FilterByAdditional(request))
                    .AsQueryable()
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToList()
            };
        }

        public async Task<Guid> CreateAsync(Guid userId, Customer customer, CancellationToken ct)
        {
            var newCustomer = new Customer();
            var source = await _storage.CustomerSources.FirstAsync(t => t.Id == customer.SourceId, ct);

            var change = newCustomer.CreateWithLog(userId, x =>
            {
                x.Id = customer.Id;
                x.AccountId = customer.AccountId;
                x.SourceId = customer.SourceId;
                x.CreateUserId = userId;
                x.ResponsibleUserId = customer.ResponsibleUserId;
                x.Surname = customer.Surname;
                x.Name = customer.Name;
                x.Patronymic = customer.Patronymic;
                x.Phone = customer.Phone;
                x.Email = customer.Email;
                x.BirthDate = customer.BirthDate;
                x.Image = customer.Image;
                x.IsDeleted = customer.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
                x.Source = source;
                x.AttributeLinks = customer.AttributeLinks.Map(x.Id);
            });

            var entry = await _storage.AddAsync(newCustomer, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, Customer oldCustomer, Customer newCustomer, CancellationToken ct)
        {
            var change = oldCustomer.UpdateWithLog(userId, x =>
            {
                x.AccountId = newCustomer.AccountId;
                x.SourceId = newCustomer.SourceId;
                x.ResponsibleUserId = newCustomer.ResponsibleUserId;
                x.Surname = newCustomer.Surname;
                x.Name = newCustomer.Name;
                x.Patronymic = newCustomer.Patronymic;
                x.Phone = newCustomer.Phone;
                x.Email = newCustomer.Email;
                x.BirthDate = newCustomer.BirthDate;
                x.Image = newCustomer.Image;
                x.IsDeleted = newCustomer.IsDeleted;
                x.ModifyDateTime = DateTime.UtcNow;
                x.AttributeLinks = newCustomer.AttributeLinks.Map(x.Id);
            });

            _storage.Update(oldCustomer);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<CustomerChange>();

            await _storage.Customers
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, l =>
                {
                    l.IsDeleted = true;
                    l.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<CustomerChange>();

            await _storage.Customers
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, l =>
                {
                    l.IsDeleted = false;
                    l.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}
