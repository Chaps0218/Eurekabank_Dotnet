using LoginService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.ServiceModel;

namespace EUREKABANK_SOAP_DOTNET_CLIMOV.Controller
{
    public class LoginController
    {
        public async Task<bool> LoginAsync(string username, string password)
        {
            using (var loginService = new LoginServiceClient())
            {
                var hashedPassword = HashPassword(password);
                return await loginService.LoginAsync(username, hashedPassword);
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);

                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
