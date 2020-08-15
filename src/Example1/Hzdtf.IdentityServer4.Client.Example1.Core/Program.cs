using Hzdtf.IdentityServer4.Client.Extensions.Standard.Client;
using Hzdtf.Utility.Standard.Utils;
using IdentityModel.Client;
using System;
using System.Net.Http;

namespace Hzdtf.IdentityServer4.Client.Example1.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1();
        }

        private static void Test1()
        {
            var httpClient = new HttpClient();
            var idClient = new IdentityServerClient();
            var re = idClient.GetClientCredentialsTokenAsync(httpClient, "client1").Result;

            Console.WriteLine("token:" + JsonUtil.SerializeIgnoreNull(re));

            if (re.Success())
            {
                httpClient.SetBearerToken(re.Data.AccessToken);
                var url = "http://localhost:5003/identity";

                var response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(content);
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                }
            }            
        }
    }
}
