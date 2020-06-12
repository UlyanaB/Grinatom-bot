using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestBot
{
    internal class BotChat
    {
        internal long id;
        internal BotChat botChat = null;
        internal Guid guid = Guid.Empty;
        internal System.Timers.Timer timer;
        internal States st;
        internal int AskNumb = 0;    // вопросов задано
        internal int TrueNumb = 0;   // правильных ответов
        internal BotLinq.BotUsers botUsers = null;
        internal int askId;
        internal Question question;

        internal async void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                timer.Enabled = false;
                guid = Guid.NewGuid();
                string flsAns = string.Format("Вы не успели ответить ( {0} из {1} )", TrueNumb, AskNumb);
                await Program.BotForm.botClient.SendTextMessageAsync(id, flsAns, replyMarkup: question.CreateContinueOrExitInlineKeyboard(guid));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
