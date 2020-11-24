using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebService
{
    public partial class UserWebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack)
            {
                var webService = new AuthWebService();
                webService.CreateUser(tbUsername.Text, tbPassword.Text);
            }
        }
    }
}