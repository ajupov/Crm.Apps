using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Orders.Models;
using Crm.Apps.Orders.Services;
using Crm.Apps.Orders.V1.Requests;
using Crm.Apps.Orders.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Orders.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireOrdersRole(JwtDefaults.AuthenticationScheme)]
    [Route("Orders/Attributes/v1")]
    public class OrderAttributesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IOrderAttributesService _orderAttributesService;

        public OrderAttributesController(IUserContext userContext, IOrderAttributesService orderAttributesService)
            : base(userContext)
        {
            _userContext = userContext;
            _orderAttributesService = orderAttributesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<OrderAttribute>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var attribute = await _orderAttributesService.GetAsync(id, false, ct);
            if (attribute == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(attribute, Roles.Orders, attribute.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<OrderAttribute>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var attributes = await _orderAttributesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(attributes, Roles.Orders, attributes.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<OrderAttributeGetPagedListResponse>> GetPagedList(
            OrderAttributeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _orderAttributesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(response, Roles.Orders, response.Attributes.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(OrderAttribute attribute, CancellationToken ct = default)
        {
            attribute.AccountId = _userContext.AccountId;

            var id = await _orderAttributesService.CreateAsync(_userContext.UserId, attribute, ct);

            return Created(nameof(Get), id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(OrderAttribute attribute, CancellationToken ct = default)
        {
            var oldAttribute = await _orderAttributesService.GetAsync(attribute.Id, true, ct);
            if (oldAttribute == null)
            {
                return NotFound(attribute.Id);
            }

            return await ActionIfAllowed(
                () => _orderAttributesService.UpdateAsync(_userContext.UserId, oldAttribute, attribute, ct),
                Roles.Orders,
                oldAttribute.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _orderAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _orderAttributesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Orders,
                attributes.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _orderAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _orderAttributesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Orders,
                attributes.Select(x => x.AccountId));
        }
    }
}
