using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    interface ILoginService
    {
        Task<string> LoginUsingWebApi(string username, string pasword);
        Task<string> LoginUsingWebService(string username, string pasword);
    }
}
