using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebAdmin.TestBotSrv;

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
            TestBotServiceClient cl = null;
            try
            {
                cl = new TestBotServiceClient();
                cl.Open();

                GridView gv = GridView1;
                for (int ordNmb = 0; ordNmb < Math.Min(gv.Rows.Count, 5); ordNmb++)
                {
                    GridViewRow grv = gv.Rows[ordNmb];
                    int chatId = int.Parse(grv.Cells[1].Text);
                    string fio = grv.Cells[2].Text;
                    string lastEnter = grv.Cells[4].Text;
                    string bestResult = grv.Cells[5].Text;
                    bool flg = cl.GetUser(ordNmb + 1, chatId, fio, lastEnter, int.Parse(bestResult));
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                cl.Close();
            }
        }
    }
}