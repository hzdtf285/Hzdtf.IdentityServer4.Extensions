using Hzdtf.IdentityServer4.Server.Extensions.Core;
using Hzdtf.Utility.Standard.Model;
using Hzdtf.Utility.Standard.Model.Return;
using IdentityServer4;
using IdentityServer4.Services;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hzdtf.IdentityServer4.Server.Example4.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly TestUserStore users;
        private readonly IIdentityServerInteractionService interactionService;
        private readonly IOptions<AuthenticationCookieOidcOptions> options;

        public AuthorizationController(TestUserStore users, IIdentityServerInteractionService interactionService, IOptions<AuthenticationCookieOidcOptions> options)
        {
            this.users = users;
            this.interactionService = interactionService;
            this.options = options;
        }

        [HttpPost("Login")]
        public async Task<ReturnInfo<LoginReturnInfo>> Login(LoginInfo loginInfo)
        {
            // 这里写自定义登录验证
            var context = await interactionService.GetAuthorizationContextAsync(loginInfo.ReturnUrl);

            var re = new ReturnInfo<LoginReturnInfo>()
            {
                Data = new LoginReturnInfo()
                {
                    ReturnUrl = loginInfo.ReturnUrl
                }
            };
            int num = HttpContext.Session.GetInt32("ErrLoginNum").GetValueOrDefault();
            num++;
            //错误登录超过3次则需要验证码
            if (num > 3)
            {
                if (string.IsNullOrWhiteSpace(loginInfo.VerificationCode))
                {
                    re.Data.IsVerificationCode = true;
                    re.SetFailureMsg("请输入验证码");

                    return re;
                }

                if (string.Compare(loginInfo.VerificationCode, HttpContext.Session.GetString("VerificationCode"), true) != 0)
                {
                    re.Data.IsVerificationCode = true;
                    re.SetFailureMsg("验证码不对，请输入正确的验证码");

                    return re;
                }
            }

            if (users.ValidateCredentials(loginInfo.LoginId, loginInfo.Password))
            {
                var user = users.FindByUsername(loginInfo.LoginId);
                var isuser = new IdentityServerUser(user.SubjectId)
                {
                    DisplayName = user.Username,
                };

                isuser.AdditionalClaims.Add(new Claim(ClaimTypes.Name, user.Username));
                isuser.AdditionalClaims.Add(new Claim("ProviderName", user.ProviderName));
                isuser.AdditionalClaims.Add(new Claim("ProviderSubjectId", user.ProviderSubjectId));
                isuser.AdditionalClaims.Add(new Claim("c1", user.Claims.Where(p => p.Type == "c1").FirstOrDefault().Value));


                isuser.AdditionalClaims.Add(new Claim(ClaimTypes.MobilePhone, "13726354556"));
                isuser.AdditionalClaims.Add(new Claim(ClaimTypes.HomePhone, "07556585412"));
                isuser.AdditionalClaims.Add(new Claim(ClaimTypes.OtherPhone, "599866685"));
                isuser.AdditionalClaims.Add(new Claim(ClaimTypes.Email, "123@qq.com"));

                isuser.AdditionalClaims.Add(new Claim(ClaimTypes.Country, "中国"));
                isuser.AdditionalClaims.Add(new Claim(ClaimTypes.StateOrProvince, "广东省"));
                isuser.AdditionalClaims.Add(new Claim(ClaimTypes.StreetAddress, "平湖街道"));

                re.SetSuccessMsg("登录成功");

                HttpContext.Session.Remove("VerificationCode");
                HttpContext.Session.Remove("ErrLoginNum");

                await HttpContext.SignInAsync(isuser);
            }
            else
            {
                HttpContext.Session.SetInt32("ErrLoginNum", num);
                re.Data.IsVerificationCode = IsNeedIsVerificationCode();

                re.SetFailureMsg("用户名或密码不对");
            }

            return re;
        }

        [HttpGet("GetIsVerificationCode")]
        public ReturnInfo<bool> GetIsVerificationCode()
        {
            ReturnInfo<bool> returnInfo = new ReturnInfo<bool>();
            returnInfo.Data = IsNeedIsVerificationCode();

            return returnInfo;
        }

        [HttpGet("Logout")]
        public async Task<ReturnInfo<bool>> Logout(string logoutId)
        {
            var vm = await interactionService.GetLogoutContextAsync(logoutId);
            SignOut(new AuthenticationProperties { RedirectUri = vm.PostLogoutRedirectUri }, options.Value.Scheme);
            Response.Redirect(vm.PostLogoutRedirectUri);

            return new ReturnInfo<bool>() { };
        }

        private bool IsNeedIsVerificationCode() => HttpContext.Session.GetInt32("ErrLoginNum").GetValueOrDefault() > 2;
    }
}
