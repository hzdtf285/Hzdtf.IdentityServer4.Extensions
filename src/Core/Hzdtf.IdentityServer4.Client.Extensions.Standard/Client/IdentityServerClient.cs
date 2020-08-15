using Hzdtf.IdentityServer4.Client.Extensions.Standard.Config;
using Hzdtf.Utility.Standard.Attr;
using Hzdtf.Utility.Standard.Model.Return;
using Hzdtf.Utility.Standard.Utils;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hzdtf.IdentityServer4.Client.Extensions.Standard.Client
{
    /// <summary>
    /// IdentityServer客户端
    /// @ 黄振东
    /// </summary>
    [Inject]
    public class IdentityServerClient : IIdentityServerClient
    {
        /// <summary>
        /// 配置读取
        /// </summary>
        private readonly IIdentityServerClientConfigReader configReader;

        /// <summary>
        /// 构造方法
        /// </summary>
        public IdentityServerClient()
            : this(new IdentityServerClientConfigJsonFile())
        {
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="configReader">配置读取</param>
        public IdentityServerClient(IIdentityServerClientConfigReader configReader)
        {
            this.configReader = configReader;
        }

        /// <summary>
        /// 异步根据客户端ID获取验证客户端证书令牌
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <returns>返回信息，Data为令牌</returns>
        public Task<ReturnInfo<IdentityModel.Client.TokenResponse>> GetClientCredentialsTokenAsync(string clientId)
        {
            using (var client = new HttpClient())
            {
                return GetClientCredentialsTokenAsync(clientId);
            }
        }

        /// <summary>
        /// 异步根据客户端ID获取验证客户端证书令牌
        /// </summary>
        /// <param name="httpClient">http客户端</param>
        /// <param name="clientId">客户端ID</param>
        /// <returns>返回信息，Data为令牌</returns>
        public async Task<ReturnInfo<IdentityModel.Client.TokenResponse>> GetClientCredentialsTokenAsync(HttpClient httpClient, string clientId)
        {
            return await RequestIdentityServer(httpClient, clientId, (re, clientInfo, disco) =>
            {
                var tokenResponse = httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = clientInfo.ClientId,
                    ClientSecret = clientInfo.ClientSecret,
                    Scope = clientInfo.Scopes[0]
                }).Result;

                if (tokenResponse.IsError)
                {
                    re.SetFailureMsg(tokenResponse.Error);
                }
                else
                {
                    re.Data = tokenResponse;
                }
            });
        }

        /// <summary>
        /// 异步根据客户端ID获取验证密码令牌
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>返回信息，Data为令牌</returns>
        public Task<ReturnInfo<IdentityModel.Client.TokenResponse>> GetPasswordTokenAsync(string clientId, string username, string password)
        {
            using (var client = new HttpClient())
            {
                return GetPasswordTokenAsync(clientId, username, password);
            }
        }

        /// <summary>
        /// 异步根据客户端ID获取验证密码令牌
        /// </summary>
        /// <param name="httpClient">http客户端</param>
        /// <param name="clientId">客户端ID</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>返回信息，Data为令牌</returns>
        public async Task<ReturnInfo<IdentityModel.Client.TokenResponse>> GetPasswordTokenAsync(HttpClient httpClient, string clientId, string username, string password)
        {
            return await RequestIdentityServer(httpClient, clientId, (re, clientInfo, disco) =>
            {
                var tokenResponse = httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest()
                {
                    Address = disco.TokenEndpoint,
                    ClientId = clientInfo.ClientId,
                    ClientSecret = clientInfo.ClientSecret,
                    Scope = clientInfo.Scopes[0],
                    UserName = username,
                    Password = password
                }).Result;

                if (tokenResponse.IsError)
                {
                    re.SetFailureMsg(tokenResponse.Error);
                }
                else
                {
                    re.Data = tokenResponse;
                }
            });
        }

        /// <summary>
        /// 请求IdentityServer
        /// </summary>
        /// <param name="httpClient">http客户端</param>
        /// <param name="clientId">客户端ID</param>
        /// <param name="successCallback">请求成功后回调</param>
        /// <returns>返回令牌</returns>
        private async Task<ReturnInfo<IdentityModel.Client.TokenResponse>> RequestIdentityServer(HttpClient httpClient, string clientId, Action<ReturnInfo<IdentityModel.Client.TokenResponse>, ClientInfo, DiscoveryDocumentResponse> successCallback)
        {
            var re = new ReturnInfo<IdentityModel.Client.TokenResponse>();
            string identityServerUrl;
            var clientInfo = GetValiParam(re, clientId, out identityServerUrl);
            if (re.Failure())
            {
                return re;
            }

            var disco = await httpClient.GetDiscoveryDocumentAsync(identityServerUrl);
            if (disco.IsError)
            {
                re.SetFailureMsg(disco.Error);

                return re;
            }

            successCallback(re, clientInfo, disco);

            return re;
        }

        /// <summary>
        /// 获取并验证参数
        /// </summary>
        /// <param name="re">返回令牌</param>
        /// <param name="clientId">客户端ID</param>
        /// <param name="identityServerUrl">IdentityServer Url地址</param>
        private ClientInfo GetValiParam(ReturnInfo<IdentityModel.Client.TokenResponse> re, string clientId, out string identityServerUrl)
        {
            identityServerUrl = null;
            if (string.IsNullOrWhiteSpace(clientId))
            {
                re.SetFailureMsg("客户端不能为空");

                return null;
            }
            var configInfo = configReader.Reader();
            if (configInfo == null)
            {
                re.SetFailureMsg("找不到任何配置信息");

                return null;
            }

            if (string.IsNullOrWhiteSpace(configInfo.IdentityServerUrl))
            {
                re.SetFailureMsg("IdentityServer地址不能为空");

                return null;
            }
            identityServerUrl = configInfo.IdentityServerUrl;

            var clientInfo = configInfo.GetClientById(clientId);
            if (clientInfo == null)
            {
                re.SetFailureMsg($"找不到{clientId}的配置信息");

                return null;
            }
            if (clientInfo.Scopes.IsNullOrLength0())
            {
                re.SetFailureMsg($"范围数组不能为空");

                return null;
            }

            return clientInfo;
        }
    }
}
