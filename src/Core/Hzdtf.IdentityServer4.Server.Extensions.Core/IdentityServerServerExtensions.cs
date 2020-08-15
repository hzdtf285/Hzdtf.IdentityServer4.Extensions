using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Hzdtf.Utility.Standard.Utils;

namespace Hzdtf.IdentityServer4.Server.Extensions.Core
{
    /// <summary>
    /// IdentityServer服务端扩展类
    /// @ 黄振东
    /// </summary>
    public static class IdentityServerServerExtensions
    {
        /// <summary>
        /// 添加IdentityServer服务到内存里
        /// </summary>
        /// <param name="services">服务收藏</param>
        /// <param name="configJsonFileName">配置Json文件名</param>
        /// <returns>IdentityServer生成器</returns>
        public static IIdentityServerBuilder AddIdentityServerInMemory(this IServiceCollection services, string configJsonFileName = "Config/identityServer4_Server.json")
        {
            if (string.IsNullOrWhiteSpace(configJsonFileName))
            {
                throw new ArgumentException("配置Json文件名不能为空");
            }

            var config = JsonUtil.DeserializeFromFile<IdentityServerServerInfo>(configJsonFileName);

            return services.AddIdentityServerInMemory(config);
        }

        /// <summary>
        /// 添加IdentityServer服务到内存里
        /// </summary>
        /// <param name="services">服务收藏</param>
        /// <param name="options">IdentityServer服务端信息配置回调</param>
        /// <returns>IdentityServer生成器</returns>
        public static IIdentityServerBuilder AddIdentityServerInMemory(this IServiceCollection services, Action<IdentityServerServerInfo> options)
        {
            if (options == null)
            {
                throw new ArgumentException("IdentityServer服务端信息配置回调不能为空");
            }

            var config = new IdentityServerServerInfo();
            if (options != null)
            {
                options(config);
            }

            return services.AddIdentityServerInMemory(config);
        }

        /// <summary>
        /// 添加IdentityServer服务到内存里
        /// </summary>
        /// <param name="services">服务收藏</param>
        /// <param name="config">IdentityServer服务端信息配置</param>
        /// <returns>IdentityServer生成器</returns>
        private static IIdentityServerBuilder AddIdentityServerInMemory(this IServiceCollection services, IdentityServerServerInfo config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("IdentityServer服务端信息配置不能为null");
            }

            var irs = config.IdentityResources.ToIdentityResources();
            var apiScopes = config.ApiScopes.ToApiScopes();
            var clients = config.Clients.ToClients();
            var users = config.TestUsers.ToTestUsers();

            var builder = services.AddIdentityServer();
            if (!irs.IsNullOrLength0())
            {
                builder.AddInMemoryIdentityResources(irs);
            }
            if (!apiScopes.IsNullOrLength0())
            {
                builder.AddInMemoryApiScopes(apiScopes);
            }
            if (!clients.IsNullOrLength0())
            {
                builder.AddInMemoryClients(clients);
            }
            if (!users.IsNullOrCount0())
            {
                builder.AddTestUsers(users);
            }

            if (config.UseDeveloperSigningCredential)
            {
                if (string.IsNullOrWhiteSpace(config.DeveloperSigningCredentialFile))
                {
                    builder.AddDeveloperSigningCredential();
                }
                else
                {
                    builder.AddDeveloperSigningCredential(filename: config.DeveloperSigningCredentialFile);
                }
            }

            return builder;
        }
    }
}
