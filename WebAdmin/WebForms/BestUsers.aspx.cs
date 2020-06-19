using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Web.UI.WebControls;
using TestBot;

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
            // @GreenIT_Congratulations_bot -   1247571431:AAHo8IqotVqCyvIKe9QGDy12C5CgB_fXo9U
            try
            {
                using (ChannelFactory<IAdminService> cf = new ChannelFactory<IAdminService>(new WebHttpBinding(), "http://localhost:8000"))
                {
                    cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                    IAdminService channel = cf.CreateChannel();
                    string s;
                    s = channel.EchoWithGet("Hello, world");
                    s = channel.EchoWithPost("Hello, world");
                }
/*
                    GridView gv = GridView1;
                for (int ordNmb = 0; ordNmb < Math.Min(gv.Rows.Count, 5); ordNmb++)
                {
                    GridViewRow grv = gv.Rows[ordNmb];
                    int chatId = int.Parse(grv.Cells[1].Text);
                    string fio = grv.Cells[2].Text;
                    string lastEnter = grv.Cells[4].Text;
                    string bestResult = grv.Cells[5].Text;
                    string congratulations = "Поздравляем Вас " + fio + "! Вы на " + (ordNmb + 1) + " месте с " + bestResult + " балами. " +
                                             "Последний раз Вы заходили " + lastEnter;
                    botClient.SendTextMessageAsync(chatId, congratulations);
                }
*/
            }
            catch (Exception ex)
            {

            }
        }
    }
}