using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace Hzdtf.IdentityServer4.Server.Extensions.Core
{
    /// <summary>
    /// IdentityServer服务端信息
    /// @ 黄振东
    /// </summary>
    public class IdentityServerServerInfo
    {
        /// <summary>
        /// Identity资源数组
        /// </summary>
        public IdentityResourceInfo[] IdentityResources { get; set; }

        /// <summary>
        /// Identity资源数组
        /// </summary>
        public ApiScopeInfo[] ApiScopes { get; set; }

        /// <summary>
        /// 客户端数组
        /// </summary>
        public ClientInfo[] Clients { get; set; }

        /// <summary>
        /// 测试用户数组
        /// </summary>
        public TestUserInfo[] TestUsers { get; set; }

        /// <summary>
        /// 使用开发签名证书，默认为是。如果为否，则手工设置证书（执行AddSigningCredential）
        /// </summary>
        public bool UseDeveloperSigningCredential { get; set; } = true;

        /// <summary>
        /// 开发签名证书文件
        /// </summary>
        public string DeveloperSigningCredentialFile { get; set; }
    }

    /// <summary>
    /// Identity资源信息
    /// @ 黄振东
    /// </summary>
    public class IdentityResourceInfo : ResourceInfo
    {
        /// <summary>
        /// 类型，如OpenId/Profile等，可在IdentityResources里找
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 用户证件单元数组
        /// </summary>
        public string[] UserClaims { get; set; }

        /// <summary>
        /// 是否在用户屏幕上取消选择范围，默认为否
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// 是否在用户屏幕上强调此范围，默认为否
        /// </summary>
        public bool Emphasize { get; set; }
    }

    /// <summary>
    /// 资源信息
    /// @ 黄振东
    /// </summary>
    public class ResourceInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 是否启用，默认为是
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否显示发现文档范围，默认为是
        /// </summary>
        public bool ShowInDiscoveryDocument { get; set; } = true;
    }

    /// <summary>
    /// Api范围信息
    /// @ 黄振东
    /// </summary>
    public class ApiScopeInfo : ResourceInfo
    {
        /// <summary>
        /// 是否在用户屏幕上取消选择范围，默认为否
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// 是否在用户屏幕上强调此范围，默认为否
        /// </summary>
        public bool Emphasize { get; set; }

        /// <summary>
        /// 用户证件单元数组
        /// </summary>
        public string[] UserClaims { get; set; }
    }

    /// <summary>
    /// 客户端信息
    /// @ 黄振东
    /// </summary>
    public class ClientInfo
    {
        /// <summary>
        /// 客户端ID，必须
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 客户端名称
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// 允许授权类型，默认是ClientCredentials，如Code/Implicit/ClientCredentials/ResourceOwnerPassword等，可在GrantTypes里找
        /// </summary>
        public string AllowedGrantType { get; set; } = "ClientCredentials";

        /// <summary>
        /// 客户端密钥令牌列表
        /// </summary>
        public IList<ClientSecretInfo> ClientSecrets { get; set; } = new List<ClientSecretInfo>();

        /// <summary>
        /// 允许范围数组
        /// </summary>
        public string[] AllowedScopes { get; set; }

        /// <summary>
        /// 访问令牌类型，有Jwt/Reference，可在AccessTokenType找到
        /// </summary>
        public string AccessTokenType { get; set; }

        /// <summary>
        /// 是否启用，默认为是
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 重定向URI数组
        /// </summary>
        public string[] RedirectUris { get; set; }

        /// <summary>
        /// 登出重定向URI数组
        /// </summary>
        public string[] PostLogoutRedirectUris { get; set; }

        /// <summary>
        /// 客户端密钥是否必须，默认为是
        /// </summary>
        public bool RequireClientSecret { get; set; } = true;

        /// <summary>
        /// 允许跨域的组织数组
        /// </summary>
        public string[] AllowedCorsOrigins { get; set; }

        /// <summary>
        /// 是否允许离线令牌，默认为否
        /// </summary>
        public bool AllowOfflineAccess { get; set; }

        /// <summary>
        /// 允许包含用户证件令牌到令牌
        /// </summary>
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

        /// <summary>
        /// Identity令牌生命周期，单位为秒。默认为300秒
        /// </summary>
        public int IdentityTokenLifetime { get; set; } = 300;

        /// <summary>
        /// Identity签名算法，如果为空，使用默认的算法
        /// </summary>
        public string[] AllowedIdentityTokenSigningAlgorithms { get; set; }

        /// <summary>
        /// 访问令牌，单位为秒，默认为3600秒
        /// </summary>
        public int AccessTokenLifetime { get; set; } = 3600;

        /// <summary>
        /// 授权码生命周期，单位为秒，默认为300秒
        /// </summary>
        public int AuthorizationCodeLifetime { get; set; } = 300;

        /// <summary>
        /// 绝对刷新令牌生命周期，单位为秒，默认为30天
        /// </summary>
        public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;

        /// <summary>
        /// 滑动刷新令牌生命周期，单位为秒，默认为15天
        /// </summary>
        public int SlidingRefreshTokenLifetime { get; set; } = 1296000;

        /// <summary>
        /// 用户同意生命周期，单位为秒
        /// </summary>
        public int? ConsentLifetime { get; set; }

        /// <summary>
        /// 刷新令牌方式，使用TokenUsage枚举的字符串
        /// </summary>
        public string RefreshTokenUsage { get; set; }

        /// <summary>
        /// 访问令牌是否在刷新令牌请求上更新，默认为否
        /// </summary>
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

        /// <summary>
        /// 刷新令牌过期方式，使用TokenExpiration枚举字符串
        /// </summary>
        public string RefreshTokenExpiration { get; set; }

        /// <summary>
        /// 是否启用本地登录，默认为是
        /// </summary>
        public bool EnableLocalLogin { get; set; } = true;

        /// <summary>
        /// 指定哪些外部idp可以与此客户端一起使用
        /// </summary>
        public string[] IdentityProviderRestrictions { get; set; }

        /// <summary>
        /// Jwt是否应包含标识符，默认为是
        /// </summary>
        public bool IncludeJwtId { get; set; } = true;

        /// <summary>
        /// 允许客户端证件单元包含在令牌中
        /// </summary>
        public ClaimInfo[] Claims { get; set; }

        /// <summary>
        /// 允许发送客户端证件单元，默认为否
        /// </summary>
        public bool AlwaysSendClientClaims { get; set; }

        /// <summary>
        /// 客户端证件单元前辍
        /// </summary>
        public string ClientClaimsPrefix { get; set; }

        /// <summary>
        /// 设置成对的SubjectID
        /// </summary>
        public string PairWiseSubjectSalt { get; set; }

        /// <summary>
        /// 用户上次身份验证以来的最大持续时间(以秒为单位)
        /// </summary>
        public int? UserSsoLifetime { get; set; }

        /// <summary>
        /// 流用户设备代码类型
        /// </summary>
        public string UserCodeType { get; set; }

        /// <summary>
        /// 设备代码生命周期
        /// </summary>
        public int DeviceCodeLifetime { get; set; }

        /// <summary>
        /// 用户会话ID是否发送到登出URI，默认为是
        /// </summary>
        public bool BackChannelLogoutSessionRequired { get; set; } = true;

        /// <summary>
        /// 协议类型
        /// </summary>
        public string ProtocolType { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 客户端URI，在同意屏幕上使用
        /// </summary>
        public string ClientUri { get; set; }

        /// <summary>
        /// Logo URI
        /// </summary>
        public string LogoUri { get; set; }

        /// <summary>
        /// 是否需要同意屏幕，默认为否
        /// </summary>
        public bool RequireConsent { get; set; }

        /// <summary>
        /// 授权码是否需要证书密钥，默认为是
        /// </summary>
        public bool RequirePkce { get; set; } = true;

        /// <summary>
        /// 证书密钥是否可以在普通方法里发送，默认为否
        /// </summary>
        public bool AllowPlainTextPkce { get; set; }

        /// <summary>
        /// 客户端是否必须对授权请求使用请求对象，默认为否
        /// </summary>
        public bool RequireRequestObject { get; set; }

        /// <summary>
        /// 该客户端是否通过浏览器传输访问令牌，如果为否，可以防止意外泄漏访问令牌，默认为否
        /// </summary>
        public bool AllowAccessTokensViaBrowser { get; set; }

        /// <summary>
        /// 客户端为基于HTTP前端通道的注销指定注销URI
        /// </summary>
        public string FrontChannelLogoutUri { get; set; }

        /// <summary>
        /// 用户的会话id应该被发送到FrontChannelLogoutUri，默认为是
        /// </summary>
        public bool FrontChannelLogoutSessionRequired { get; set; } = true;

        /// <summary>
        /// 客户端为基于HTTP回通道的注销指定注销URI
        /// </summary>
        public string BackChannelLogoutUri { get; set; }
    }

    /// <summary>
    /// 客户端密钥信息
    /// @ 黄振东
    /// </summary>
    public class ClientSecretInfo
    {
        /// <summary>
        /// 密钥值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 加密类型，有Sha256和Sha512。默认为Sha256
        /// </summary>
        public string EncryptionType { get; set; } = "Sha256";

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? Expiration { get; set; }

        /// <summary>
        /// 加密后的值
        /// </summary>
        public string EncryptedValue
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Value))
                {
                    return Value;
                }

                var type = EncryptionType.ToLower();
                switch (type)
                {
                    case "sha256":

                        return Value.Sha256();

                    case "sha512":

                        return Value.Sha512();

                    default:

                        return Value;
                }
            }
        }
    }

    /// <summary>
    /// 测试用户信息
    /// @ 黄振东
    /// </summary>
    public class TestUserInfo
    {
        /// <summary>
        /// 主题ID（IdentityServer4区分用户唯一）
        /// </summary>
        public string SubjectId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 提供者名称
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// 提供者主题ID
        /// </summary>
        public string ProviderSubjectId { get; set; }

        /// <summary>
        /// 是否激活，默认为是
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 证件单元信息数组
        /// </summary>
        public ClaimInfo[] Claims { get; set; }
    }

    /// <summary>
    /// 证件单元信息
    /// </summary>
    public class ClaimInfo
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}