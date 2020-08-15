using System;

namespace Hzdtf.IdentityServer4.MvcClient.Example4.Core.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
