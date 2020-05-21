using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAdmin
{
    public partial class LoginForm : System.Web.UI.Page
    {
        public static bool menuVisible = false;
        public static bool loginVisible = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            Login1.DisplayRememberMe = false;
            Login1.RememberMeSet = false;

            Menu1.Visible = menuVisible;
            Login1.Visible = loginVisible;
        }

        protected void Login1_LoggedIn(object sender, EventArgs e)
        {
            menuVisible = true;
            loginVisible = false;
        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            Login login = sender as Login;
            if (login.UserName == "Zxc" && login.Password == "123")
            {
                e.Authenticated = true;
            }
        }

        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            switch (e.Item.Value)
            {
                case "Log":
                    break;

                case "ToAdminUsers":
                    break;

                case "Exit":
                    menuVisible = false;
                    loginVisible = true;
                    Page_Load(sender, e);
                    break;

                default:
                    break;
            }
        }
    }
}