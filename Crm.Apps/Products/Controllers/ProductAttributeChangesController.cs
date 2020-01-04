using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.Models;
using Crm.Apps.Products.RequestParameters;
using Crm.Apps.Products.Services;
using Crm.Common.All.UserContext;
using Crm.Common.All.UserContext.Attributes;
using Crm.Common.All.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Products.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.ProductsManagement)]
    [Route("Api/Products/Attributes/Changes")]
    public class ProductAttributeChangesController : AllowingCheckControllerBase
    {
        private readonly IProductAttributesService _productAttributesService;
        private readonly IProductAttributeChangesService _productAttributeChangesService;

        public ProductAttributeChangesController(
            IUserContext userContext,
            IProductAttributesService productAttributesService,
            IProductAttributeChangesService productAttributeChangesService)
            : base(userContext)
        {
            _productAttributesService = productAttributesService;
            _productAttributeChangesService = productAttributeChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<ProductAttributeChange>>> GetPagedList(
            ProductAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var attribute = await _productAttributesService.GetAsync(request.AttributeId, ct);
            var changes = await _productAttributeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.ProductsManagement}, attribute.AccountId);
        }
    }
}