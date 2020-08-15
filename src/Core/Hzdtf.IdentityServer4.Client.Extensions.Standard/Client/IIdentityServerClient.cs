using Hzdtf.Utility.Standard.Model.Return;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hzdtf.IdentityServer4.Client.Extensions.Standard.Client
{
    /// <summary>
    /// IdentityServer客户端接口
    /// @ 黄振东
    /// </summary>
    public interface IIdentityServerClient
    {
        /// <summary>
        /// 异步根据客户端ID获取验证客户端证书令牌
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <returns>返回信息，Data为令牌</returns>
        Task<ReturnInfo<IdentityModel.Client.TokenResponse>> GetClientCredentialsTokenAsync(string clientId);

        /// <summary>
        /// 异步根据客户端ID获取验证客户端证书令牌
        /// </summary>
        /// <param name="httpClient">http客户端</param>
        /// <param name="clientId">客户端ID</param>
        /// <returns>返回信息，Data为令牌</returns>
        Task<ReturnInfo<IdentityModel.Client.TokenResponse>> GetClientCredentialsTokenAsync(HttpClient httpClient, string clientId);

        /// <summary>
        /// 异步根据客户端ID获取验证密码令牌
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>返回信息，Data为令牌</returns>
        Task<ReturnInfo<IdentityModel.Client.TokenResponse>> GetPasswordTokenAsync(string clientId, string username, string password);

        /// <summary>
        /// 异步根据客户端ID获取验证密码令牌
        /// </summary>
        /// <param name="httpClient">http客户端</param>
        /// <param name="clientId">客户端ID</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>返回信息，Data为令牌</returns>
        Task<ReturnInfo<IdentityModel.Client.TokenResponse>> GetPasswordTokenAsync(HttpClient httpClient, string clientId, string username, string password);
    }
}
