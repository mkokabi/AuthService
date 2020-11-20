using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using UserService;

namespace WebService
{
    /// <summary>
    /// Summary description for AuthWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AuthWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string Login(string username, string password)
        {
            var result = new AuthService().Login(username, password).Result;
            return $"Login from Web Service {result.Message}";
        }
    }
}
