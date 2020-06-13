﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAdmin.WebForms
{
    public partial class BestUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!LoginForm.Authenticated)
            {
                throw new Exception("Forbidden");
            }
        }

        protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            GridView gv = sender as GridView;
            string w = gv.Rows[e.NewSelectedIndex].Cells[0].Text;
            SqlDataSourceLog.SelectParameters[0].DefaultValue = w;
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            Label2.Visible = true;
            Label2.Text = "Send button not implemented";
            //throw new NotImplementedException("Send button not implemented");
        }
    }
}