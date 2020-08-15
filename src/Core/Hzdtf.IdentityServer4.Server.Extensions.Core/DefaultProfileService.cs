using Hzdtf.Utility.Standard.Utils;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hzdtf.IdentityServer4.Server.Extensions.Core
{
    /// <summary>
    /// 默认配置服务
    /// @ 黄振东
    /// </summary>
    public class DefaultProfileService : IProfileService
    {
        /// <summary>
        /// 异步获取配置数据
        /// </summary>
        /// <param name="context">配置数据请求上下文</param>
        /// <returns>任务</returns>
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            return Task.Run(() =>
            {
                var scopes = context.RequestedResources.ParsedScopes;
                if (!scopes.IsNullOrCount0())
                {
                    var profile = scopes.Where(p => p.ParsedName == IdentityServerConstants.StandardScopes.Profile).FirstOrDefault();
                    if (profile != null)
                    {
                        context.IssuedClaims = context.Subject.Claims.ToList();
                    }
                }
            });
        }

        /// <summary>
        /// 异步是否激活
        /// </summary>
        /// <param name="context">激活上下文</param>
        /// <returns>任务</returns>
        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.Run(() =>
            {
                context.IsActive = true;
            });
        }
    }
}
