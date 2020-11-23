using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Services;

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
            string connStr = ConfigurationManager.ConnectionStrings["usersdbConnectionString"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "Select count(0) from Users where UserName = @username and Password = @password";
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                conn.Open();
                var count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0 ? "Login from Web Service, succesful" : "Login from Web Service failed";
            }
        }

        [WebMethod]
        public int CreateUser(string username, string password)
        {
            string connStr = ConfigurationManager.ConnectionStrings["usersdbConnectionString"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "Insert into Users(UserName, Password) values (@username, @password); Select SCOPE_IDENTITY()";
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                conn.Open();
                var newId = Convert.ToInt32(cmd.ExecuteScalar());
                return newId;
            }
        }
    }
}
