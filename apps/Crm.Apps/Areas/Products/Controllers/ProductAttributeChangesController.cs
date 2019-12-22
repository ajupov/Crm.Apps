using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Products.Models;
using Crm.Apps.Areas.Products.Parameters;
using Crm.Apps.Areas.Products.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Products.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.ProductsManagement)]
    [Route("Api/Products/Attributes/Changes")]
    public class ProductAttributeChangesController : UserContextController
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
            ProductAttributeChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var attribute = await _productAttributesService.GetAsync(parameter.AttributeId, ct);
            var changes = await _productAttributeChangesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.ProductsManagement}, attribute.AccountId);
        }
    }
}