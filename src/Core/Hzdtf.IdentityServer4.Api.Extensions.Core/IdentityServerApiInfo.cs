using IdentityServer4.AccessTokenValidation;
using System;

namespace Hzdtf.IdentityServer4.Api.Extensions.Core
{
    /// <summary>
    /// IdentityServer Api信息
    /// @ 黄振东
    /// </summary>
    public class IdentityServerApiInfo
    {
        /// <summary>
        /// IdentityServer Url地址
        /// </summary>
        public string IdentityServerUrl { get; set; }

        /// <summary>
        /// Https是否必须，默认为否
        /// </summary>
        public bool RequireHttpsMetadata { get; set; }

        /// <summary>
        /// 是否保存令牌
        /// </summary>
        public bool SaveToken { get; set; }

        /// <summary>
        /// 服务
        /// </summary>
        public ServiceInfo Service { get; set; } = new ServiceInfo();

        /// <summary>
        /// 授权方案Key，默认为Bearer
        /// </summary>
        public string AuthSchemeKey { get; set; } = IdentityServerAuthenticationDefaults.AuthenticationScheme;
    }

    /// <summary>
    /// 服务信息
    /// </summary>
    public class ServiceInfo
    {
        /// <summary>
        /// 服务名
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 策略名称，默认是ApiScope
        /// </summary>
        public string PolicyName { get; set; } = "ApiScope";
    }
}
