using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    interface ILoginService
    {
        Task<string> Login(string username, string pasword);
    }
}
