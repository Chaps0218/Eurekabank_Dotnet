using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace EUREKABANK_SOAP_DOTNET
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LoginService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select LoginService.svc or LoginService.svc.cs at the Solution Explorer and start debugging.
    public class LoginService : ILoginService
    {
        public bool Login(String username, String password)
        {
            return (username == "monster" && password == "774e993500f4027acfd72b7a7ee564b76ae43cf7c4c943ed0e0f364cca16b6ec") ||
                  (username == "admin" && password == "admin");
        }
    }
}
