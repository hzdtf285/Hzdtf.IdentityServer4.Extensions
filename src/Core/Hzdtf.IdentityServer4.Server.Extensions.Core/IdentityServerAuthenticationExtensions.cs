using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hzdtf.IdentityServer4.Server.Extensions.Core
{
    /// <summary>
    /// IdentityServer身份认证扩展类
    /// @ 黄振东
    /// </summary>
    public static class IdentityServerAuthenticationExtensions
    {
        /// <summary>
        /// 添加身份认证Cookie Oidc
        /// </summary>
        /// <param name="services">服务收藏</param>
        /// <param name="options">身份认证Cookie Oidc选项配置回调</param>
        /// <returns>身份认证生成器</returns>
        public static AuthenticationBuilder AddAuthenticationCookieOidc(this IServiceCollection services, Action<AuthenticationCookieOidcOptions> options = null)
        {
            var config = new AuthenticationCookieOidcOptions();
            if (options != null)
            {
                options(config);
            }
            
            services.AddSingleton<IOptions<AuthenticationCookieOidcOptions>>(Options.Create<AuthenticationCookieOidcOptions>(config));
            var builder = services.AddAuthentication(o =>
            {
                o.DefaultScheme = config.Scheme;
                o.DefaultChallengeScheme = "oidc";
            })
            .AddCookie(o =>
            {
                if (!string.IsNullOrWhiteSpace(config.LoginPath))
                {
                    o.LoginPath = config.LoginPath;
                }
                if (!string.IsNullOrWhiteSpace(config.LogoutPath))
                {
                    o.LogoutPath = config.LogoutPath;
                }
            });

            if (config.AllowClaimsToClient)
            {
                services.AddSingleton<IProfileService, DefaultProfileService>();
            }

            return builder;
        }
    }

    /// <summary>
    /// 身份认证Cookie Oidc选项配置
    /// @ 黄振东
    /// </summary>
    public class AuthenticationCookieOidcOptions
    {
        /// <summary>
        /// 方案，默认是CookieAuthenticationDefaults.AuthenticationScheme（Cookies）
        /// </summary>
        public string Scheme
        {
            get;
            set;
        } = CookieAuthenticationDefaults.AuthenticationScheme;

        /// <summary>
        /// 登录路径
        /// </summary>
        public string LoginPath
        {
            get;
            set;
        }

        /// <summary>
        /// 登出路径
        /// </summary>
        public string LogoutPath
        {
            get;
            set;
        }

        /// <summary>
        /// 是否允许额外的证件单元输出到客户端，默认为否。如果为是，请在验证登录时，在SignIn设置Claims
        /// </summary>
        public bool AllowClaimsToClient
        {
            get;
            set;
        }
    }
}
