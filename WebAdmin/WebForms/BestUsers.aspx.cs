using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telegram.Bot;

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
                TelegramBotClient botClient = null;
                if (string.IsNullOrWhiteSpace(Properties.Settings.Default.Proxy))
                {
                    botClient = new TelegramBotClient(Properties.Settings.Default.BotToken);
                }
                else
                {
                    string[] proxyParts = Properties.Settings.Default.Proxy.Split(new[] { ':' }, 2);
                    WebProxy httpProxy = new WebProxy(proxyParts[0], int.Parse(proxyParts[1]));
                    botClient = new TelegramBotClient(Properties.Settings.Default.BotToken, httpProxy);
                }
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
            }
            catch (Exception ex)
            {

            }
        }
    }
}