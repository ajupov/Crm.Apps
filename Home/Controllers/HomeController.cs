﻿using Ajupov.Infrastructure.All.ApiDocumentation.Attributes;
using Ajupov.Infrastructure.All.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Home.Controllers
{
    [IgnoreApiDocumentation]
    [Route("")]
    public class HomeController : DefaultMvcController
    {
        [HttpGet("")]
        public ActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}