using Hzdtf.Utility.Standard.Utils;
using IdentityServer4.AccessTokenValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hzdtf.IdentityServer4.Api.Extensions.Core
{
    /// <summary>
    /// IdentityServer Api扩展类库
    /// @ 黄振东
    /// </summary>
    public static class IdentityServerApiExtensions
    {
        /// <summary>
        /// 添加IdentityServer Api
        /// </summary>
        /// <param name="services">服务收藏</param>
        /// <param name="appConfig">应用配置</param>
        /// <param name="configJsonFileName">配置Json文件名</param>
        /// <returns>服务收藏</returns>
        public static IServiceCollection AddIdentityServerApi(this IServiceCollection services, IConfiguration appConfig, string configJsonFileName = "Config/identityServer4_Api.json")
        {
            if (string.IsNullOrWhiteSpace(configJsonFileName))
            {
                throw new ArgumentException("配置Json文件名不能为空");
            }

            var config = JsonUtil.DeserializeFromFile<IdentityServerApiInfo>(configJsonFileName);

            return services.AddIdentityServerApi(appConfig, config);
        }

        /// <summary>
        /// 添加IdentityServer Api
        /// </summary>
        /// <param name="services">服务收藏</param>
        /// <param name="appConfig">应用配置</param>
        /// <param name="options">IdentityServer Api配置信息回调</param>
        /// <returns>服务收藏</returns>
        public static IServiceCollection AddIdentityServerApi(this IServiceCollection services, IConfiguration appConfig, Action<IdentityServerApiInfo> options)
        {
            if (options == null)
            {
                throw new ArgumentException("IdentityServer Api配置信息回调不能为空");
            }

            var config = new IdentityServerApiInfo();
            if (options != null)
            {
                options(config);
            }

            services.AddIdentityServerApi(appConfig, config);

            return services;
        }

        /// <summary>
        /// 添加IdentityServer Api
        /// </summary>
        /// <param name="services">服务收藏</param>
        /// <param name="appConfig">应用配置</param>
        /// <param name="config">IdentityServer Api配置信息</param>
        /// <returns>服务收藏</returns>
        private static IServiceCollection AddIdentityServerApi(this IServiceCollection services, IConfiguration appConfig, IdentityServerApiInfo config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("IdentityServer Api配置信息不能为null");
            }
            if (appConfig == null)
            {
                throw new ArgumentNullException("应用配置不能为null");
            }
            if (string.IsNullOrWhiteSpace(config.IdentityServerUrl))
            {
                throw new ArgumentNullException("IdentityServer Url地址不能为空");
            }

            if (string.IsNullOrWhiteSpace(config.Service.ServiceName))
            {
                config.Service.ServiceName = appConfig["ServiceName"];
            }

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddJwtBearer(config.AuthSchemeKey, options =>
                {
                    options.Authority = config.IdentityServerUrl;
                    options.RequireHttpsMetadata = config.RequireHttpsMetadata;
                    options.SaveToken = config.SaveToken;
                    options.Audience = config.Service.ServiceName;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(config.Service.PolicyName, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", config.Service.ServiceName);
                });
            });

            return services;
        }
    }
}
