using Hzdtf.Utility.Standard.Utils;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Hzdtf.IdentityServer4.Server.Extensions.Core
{
    /// <summary>
    /// IdentityServer服务端辅助类
    /// @ 黄振东
    /// </summary>
    public static class IdentityServerServerUtil
    {
        /// <summary>
        /// 转换为Identity资源数组
        /// </summary>
        /// <param name="identityResources">Identity资源信息数组</param>
        /// <returns>Identity资源数组</returns>
        public static IdentityResource[] ToIdentityResources(this IdentityResourceInfo[] identityResources)
        {
            if (identityResources.IsNullOrLength0())
            {
                return null;
            }

            var result = new IdentityResource[identityResources.Length];
            for (var i = 0; i < result.Length; i++)
            {
                var source = identityResources[i];
                var type = source.Type != null ? source.Type.ToLower() : null;
                if ("openid".Equals(type))
                {
                    result[i] = new IdentityResources.OpenId();
                }
                else if ("profile".Equals(type))
                {
                    result[i] = new IdentityResources.Profile();
                }
                else if ("address".Equals(type))
                {
                    result[i] = new IdentityResources.Address();
                }
                else if ("email".Equals(type))
                {
                    result[i] = new IdentityResources.Email();
                }
                else if ("phone".Equals(type))
                {
                    result[i] = new IdentityResources.Phone();
                }
                else
                {
                    result[i] = new IdentityResource(source.Name, source.DisplayName, source.UserClaims);
                }

                result[i].Description = source.Description;
                result[i].Enabled = source.Enabled;
                result[i].Emphasize = source.Emphasize;
                result[i].Required = source.Required;
                result[i].ShowInDiscoveryDocument = source.ShowInDiscoveryDocument;
            }

            return result;
        }

        /// <summary>
        /// 转换为Api范围数组
        /// </summary>
        /// <param name="apiScopes">Api范围信息数组</param>
        /// <returns>Api范围数组</returns>
        public static ApiScope[] ToApiScopes(this ApiScopeInfo[] apiScopes)
        {
            if (apiScopes.IsNullOrLength0())
            {
                return null;
            }

            var result = new ApiScope[apiScopes.Length];
            for (var i = 0; i < result.Length; i++)
            {
                var source = apiScopes[i];
                result[i] = new ApiScope(source.Name, source.DisplayName, source.UserClaims);

                result[i].Description = source.Description;
                result[i].Enabled = source.Enabled;
                result[i].Emphasize = source.Emphasize;
                result[i].Required = source.Required;
                result[i].ShowInDiscoveryDocument = source.ShowInDiscoveryDocument;
            }

            return result;
        }

        /// <summary>
        /// 转换为客户端数组
        /// </summary>
        /// <param name="clients">客户端信息数组</param>
        /// <returns>客户端数组</returns>
        public static Client[] ToClients(this ClientInfo[] clients)
        {
            if (clients.IsNullOrLength0())
            {
                return null;
            }

            var result = new Client[clients.Length];
            for (var i = 0; i < result.Length; i++)
            {
                var source = clients[i];
                result[i] = new Client();
                result[i].ClientId = source.ClientId;
                result[i].ClientName = source.ClientName;
                result[i].AlwaysIncludeUserClaimsInIdToken = source.AlwaysIncludeUserClaimsInIdToken;

                if (string.IsNullOrWhiteSpace(source.AllowedGrantType))
                {
                    throw new ArgumentException($"索引:{i},允许授权类型[AllowedGrantType]不能为空");
                }
                var grantType = source.AllowedGrantType.ToLower();
                switch (grantType)
                {
                    case "clientcredentials":
                        result[i].AllowedGrantTypes = GrantTypes.ClientCredentials;

                        break;

                    case "code":
                        result[i].AllowedGrantTypes = GrantTypes.Code;

                        break;

                    case "codeandclientcredentials":
                        result[i].AllowedGrantTypes = GrantTypes.CodeAndClientCredentials;

                        break;

                    case "deviceflow":
                        result[i].AllowedGrantTypes = GrantTypes.DeviceFlow;

                        break;

                    case "hybrid":
                        result[i].AllowedGrantTypes = GrantTypes.Hybrid;

                        break;

                    case "hybridandclientcredentials":
                        result[i].AllowedGrantTypes = GrantTypes.HybridAndClientCredentials;

                        break;

                    case "implicit":
                        result[i].AllowedGrantTypes = GrantTypes.Implicit;

                        break;

                    case "implicitandclientcredentials":
                        result[i].AllowedGrantTypes = GrantTypes.ImplicitAndClientCredentials;

                        break;

                    case "resourceownerpassword":
                        result[i].AllowedGrantTypes = GrantTypes.ResourceOwnerPassword;

                        break;

                    case "resourceownerpasswordandclientcredentials":
                        result[i].AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials;

                        break;

                    default:
                        throw new NotSupportedException($"索引:{i},不支持的授权类型[AllowedGrantTypes为:{source.AllowedGrantType}]");
                }
                
                if (!source.ClientSecrets.IsNullOrCount0())
                {
                    result[i].ClientSecrets = new List<Secret>(source.ClientSecrets.Count);
                    foreach (var cs in source.ClientSecrets)
                    {
                        result[i].ClientSecrets.Add(new Secret(cs.EncryptedValue, cs.Expiration));
                    }
                }

                if (!string.IsNullOrWhiteSpace(source.AccessTokenType))
                {
                    result[i].AccessTokenType =Enum.Parse<AccessTokenType>(source.AccessTokenType);
                }

                result[i].AllowedScopes = source.AllowedScopes;
                result[i].AllowOfflineAccess = source.AllowOfflineAccess;
                result[i].Enabled = source.Enabled;
                result[i].RedirectUris = source.RedirectUris;
                result[i].PostLogoutRedirectUris = source.PostLogoutRedirectUris;
                result[i].RequireClientSecret = source.RequireClientSecret;
                result[i].AllowedCorsOrigins = source.AllowedCorsOrigins;

                result[i].IdentityTokenLifetime = source.IdentityTokenLifetime;
                result[i].AllowedIdentityTokenSigningAlgorithms = source.AllowedIdentityTokenSigningAlgorithms;
                result[i].AccessTokenLifetime = source.AccessTokenLifetime;
                result[i].AuthorizationCodeLifetime = source.AuthorizationCodeLifetime;
                result[i].AbsoluteRefreshTokenLifetime = source.AbsoluteRefreshTokenLifetime;
                result[i].SlidingRefreshTokenLifetime = source.SlidingRefreshTokenLifetime;
                result[i].ConsentLifetime = source.ConsentLifetime;

                if (!string.IsNullOrWhiteSpace(source.RefreshTokenUsage))
                {
                    result[i].RefreshTokenUsage = Enum.Parse<TokenUsage>(source.RefreshTokenUsage);
                }

                result[i].UpdateAccessTokenClaimsOnRefresh = source.UpdateAccessTokenClaimsOnRefresh;

                if (!string.IsNullOrWhiteSpace(source.RefreshTokenUsage))
                {
                    result[i].RefreshTokenExpiration = Enum.Parse<TokenExpiration>(source.RefreshTokenExpiration);
                }

                result[i].EnableLocalLogin = source.EnableLocalLogin;
                result[i].IdentityProviderRestrictions = source.IdentityProviderRestrictions;

                result[i].IncludeJwtId = source.IncludeJwtId;

                if (!source.Claims.IsNullOrCount0())
                {
                    result[i].Claims = new List<ClientClaim>(source.Claims.Length);
                    for (var j = 0; j < source.Claims.Length; j++)
                    {
                        result[i].Claims.Add(new ClientClaim(source.Claims[j].Type, source.Claims[j].Value));
                    }
                }

                result[i].AlwaysSendClientClaims = source.AlwaysSendClientClaims;
                result[i].ClientClaimsPrefix = source.ClientClaimsPrefix;
                result[i].PairWiseSubjectSalt = source.PairWiseSubjectSalt;
                result[i].UserSsoLifetime = source.UserSsoLifetime;
                result[i].UserCodeType = source.UserCodeType;
                result[i].DeviceCodeLifetime = source.DeviceCodeLifetime;
                result[i].BackChannelLogoutSessionRequired = source.BackChannelLogoutSessionRequired;
                if (!string.IsNullOrWhiteSpace(source.ProtocolType))
                {
                    result[i].ProtocolType = source.ProtocolType;
                }
                result[i].Description = source.Description;
                result[i].ClientUri = source.ClientUri;
                result[i].LogoUri = source.LogoUri;
                result[i].RequirePkce = source.RequirePkce;
                result[i].RequireConsent = source.RequireConsent;
                result[i].AllowPlainTextPkce = source.AllowPlainTextPkce;
                result[i].RequireRequestObject = source.RequireRequestObject;
                result[i].AllowAccessTokensViaBrowser = source.AllowAccessTokensViaBrowser;
                result[i].FrontChannelLogoutUri = source.FrontChannelLogoutUri;
                result[i].FrontChannelLogoutSessionRequired = source.FrontChannelLogoutSessionRequired;
                result[i].BackChannelLogoutUri = source.BackChannelLogoutUri;
            }

            return result;
        }

        /// <summary>
        /// 转换为测试用户列表
        /// </summary>
        /// <param name="testUsers">测试用户信息数组</param>
        /// <returns>测试用户列表</returns>
        public static List<TestUser> ToTestUsers(this TestUserInfo[] testUsers)
        {
            if (testUsers.IsNullOrLength0())
            {
                return null;
            }

            var result = new List<TestUser>(testUsers.Length);
            for (var i = 0; i < testUsers.Length; i++)
            {
                var source = testUsers[i];
                var t = new TestUser();
                result.Add(t);

                t.SubjectId = source.SubjectId;
                t.Username = source.Username;
                t.Password = source.Password;
                t.ProviderName = source.ProviderName;
                t.ProviderSubjectId = source.ProviderSubjectId;
                t.IsActive = source.IsActive;

                if (!source.Claims.IsNullOrCount0())
                {
                    t.Claims = new List<Claim>(source.Claims.Length);
                    for (var j = 0; j < source.Claims.Length; j++)
                    {
                        t.Claims.Add(new Claim(source.Claims[j].Type, source.Claims[j].Value));
                    }
                }
            }

            return result;
        }
    }
}
