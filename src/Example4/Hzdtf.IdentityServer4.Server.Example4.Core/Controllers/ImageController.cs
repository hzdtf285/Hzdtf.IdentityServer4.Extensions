using Hzdtf.Utility.Standard.Attr;
using Hzdtf.Utility.Standard.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Hzdtf.IdentityServer4.Server.Example4.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        [HttpGet("BuilderCheckCode")]
        public FileContentResult BuilderCheckCode()
        {
            string checkCode = NumberUtil.Random();

            var imageBytes = ImageUtil.CreateCrossPlatformCodeImg(checkCode);
            HttpContext.Session.SetString("VerificationCode", checkCode);

            return File(imageBytes, "image/png");
        }
    }
}
