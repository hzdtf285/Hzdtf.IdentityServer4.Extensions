using Hzdtf.IdentityServer4.Client.Extensions.Standard.Config;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Hzdtf.Utility.Standard.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;

namespace Hzdtf.IdentityServer4.Client.Extensions.Core
{
    /// <summary>
    /// IdentityServer客户端身份认证扩展类
    /// @ 黄振东
    /// </summary>
    public static class IdentityServerClientAuthenticationExtensions
    {
        /// <summary>
        /// 添加身份认证Cookie Oidc
        /// </summary>
        /// <param name="services">服务收藏</param>
        /// <param name="clientId">客户端ID</param>
        /// <param name="jsonFile">json文件</param>
        /// <returns>身份认证生成器</returns>
        public static AuthenticationBuilder AddAuthenticationCookieOidc(this IServiceCollection services, string clientId = null, string jsonFile = "Config/identityServer4_Client.json")
        {
            var reader = new IdentityServerClientConfigJsonFile(jsonFile);
            var idInfo = reader.Reader();
            if (idInfo == null)
            {
                throw new ArgumentNullException($"{jsonFile}:找不到任何配置信息");
            }
            if (idInfo.Clients.IsNullOrLength0())
            {
                throw new ArgumentNullException($"{jsonFile}:找不到任何客户端配置信息");
            }

            var clientInfo = string.IsNullOrWhiteSpace(clientId) ? idInfo.Clients[0] : idInfo.GetClientById(clientId);
            if (clientInfo == null)
            {
                throw new ArgumentNullException($"找不到客户端ID[{clientId}]客户端配置信息");
            }

            var config = new AuthenticationCookieOidcOptions()
            {
                IdentityServerUrl = idInfo.IdentityServerUrl,
                Client = clientInfo
            };

            return services.AddAuthenticationCookieOidc(config);
        }

        /// <summary>
        /// 添加身份认证Cookie Oidc
        /// </summary>
        /// <param name="services">服务收藏</param>
        /// <param name="options">身份认证Cookie Oidc选项配置回调</param>
        /// <returns>身份认证生成器</returns>
        public static AuthenticationBuilder AddAuthenticationCookieOidc(this IServiceCollection services, Action<AuthenticationCookieOidcOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("身份认证Cookie Oidc选项配置回调不能为null");
            }

            var config = new AuthenticationCookieOidcOptions();
            options(config);

            return services.AddAuthenticationCookieOidc(config);
        }

        /// <summary>
        /// 添加身份认证Cookie Oidc
        /// </summary>
        /// <param name="services">服务收藏</param>
        /// <returns>身份认证生成器</returns>
        private static AuthenticationBuilder AddAuthenticationCookieOidc(this IServiceCollection services, AuthenticationCookieOidcOptions config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("身份认证Cookie Oidc选项配置不能为null");
            }
            if (string.IsNullOrWhiteSpace(config.IdentityServerUrl))
            {
                throw new ArgumentNullException($"IdentityServer Url地址不能为空");
            }

            services.AddSingleton<IOptions<AuthenticationCookieOidcOptions>>(Options.Create<AuthenticationCookieOidcOptions>(config));
            var builder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = config.Scheme;
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie(config.Scheme)
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = config.IdentityServerUrl;

                options.ClientId = config.Client.ClientId;
                options.ClientSecret = config.Client.ClientSecret;
                options.ResponseType = "code";
                options.RequireHttpsMetadata = config.Client.RequireHttpsMetadata;

                if (!config.Client.Scopes.IsNullOrLength0())
                {
                    foreach (var s in config.Client.Scopes)
                    {
                        options.Scope.Add(s);
                    }
                }

                options.SaveTokens = config.Client.SaveTokens;
            });

            return builder;
        }

        /// <summary>
        /// 使用身份认证Cookie Oidc
        /// </summary>
        /// <param name="app">应用生成器</param>
        /// <returns>应用生成器</returns>
        public static IApplicationBuilder UseAuthenticationCookieOidc(this IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });

            return app;
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
        /// IdentityServer Url地址
        /// </summary>
        public string IdentityServerUrl { get; set; }

        /// <summary>
        /// 客户端信息
        /// </summary>
        public ClientInfo Client { get; set; }
    }
}
