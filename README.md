# Hzdtf.IdentityServer4.Extensions
IdentityServer4扩展库，支持.NET Core平台，语言：C#。适用于微服务。主要针对IdentityServer4进行功能上的封装，让使用更方便。通过集成配置文件，封装功能有：
1、客户端密钥认证
2、帐号和密码认证
3、授权码认证
4、与Ocelot网关集成、并与Consul集成实现动态路由

本框架必须运行在.NET Standard 2.0、.NET Core 3.1.5 以上。下载源码用Visual Studio 2019打开。
工程以Standard或Std结尾是标准库，以Framework或Frm结尾为Framework库，以Core结尾为Core库。
初始编译时会耗些时间，因为要从nuget下载包。
本库依赖类库是：
1、Hzdrtf.Utility
2、Hzdrtf.Utility.AspNet.Core
3、Hzdtf.Logger
4、Hzdtf.Platform
5、Hzdtf.Consul.Extensions
类库统一放在src/Library里。（依赖库源码都可在Hzdtf.Foundation.Framework/Hzdtf.Consul.Extensions里找到）

一、Hzdtf.IdentityServer4.Server.Extensions.Core包
IdentityServer4服务封装包

二、Hzdtf.IdentityServer4.Api.Extensions.Core包 
IdentityServer4 api资源包

三、Hzdtf.IdentityServer4.Client.Extensions.Standard包
IdentityServer4客户端包

四、Hzdtf.IdentityServer4.Client.Extensions.Core包
IdentityServer4 aspnet core客户端包

五、样例
默认的配置都会在Config文件夹下，server、api、客户端配置分别在identityServer4_Server.json、identityServer4_Api.json、identityServer4_Client.json下

通过配置可实现大部分ID4功能，详情请参参考Example
