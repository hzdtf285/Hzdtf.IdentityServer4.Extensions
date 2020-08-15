using Hzdtf.Utility.Standard.Utils;
using System;
using System.Linq;

namespace Hzdtf.IdentityServer4.Client.Extensions.Standard.Config
{
    /// <summary>
    /// IdentityServer客户端信息
    /// @ 黄振东
    /// </summary>
    public class IdentityServerClientInfo
    {
        /// <summary>
        /// IdentityServer Url地址
        /// </summary>
        public string IdentityServerUrl { get; set; }

        /// <summary>
        /// 客户端信息数组
        /// </summary>
        public ClientInfo[] Clients { get; set; }

        /// <summary>
        /// 根据客户端ID获取客户端信息
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <returns>客户端信息</returns>
        public ClientInfo GetClientById(string clientId)
        {
            if (string.IsNullOrWhiteSpace(clientId) || Clients.IsNullOrLength0())
            {
                return null;
            }

            return Clients.Where(p => p.ClientId == clientId).FirstOrDefault();
        }
    }

    /// <summary>
    /// 客户端信息
    /// @ 黄振东
    /// </summary>
    public class ClientInfo
    {
        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 客户端密钥
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 范围数组
        /// </summary>
        public string[] Scopes { get; set; }

        /// <summary>
        /// 保存令牌
        /// </summary>
        public bool SaveTokens { get; set; }

        /// <summary>
        /// 是否https是必须，默认是否
        /// </summary>
        public bool RequireHttpsMetadata { get; set; }
    }
}
