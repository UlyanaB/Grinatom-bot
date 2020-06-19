using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBot
{
    public class TestBotService : ITestBotService
    {

        public bool GetUser(int position, long tlgUserId, string tlgUserName, string lastEnter, int bestResult)
        {
            Program.BotForm.botClient.SendTextMessageAsync(tlgUserId, "Поздравляем Вас " + tlgUserName +
                "! Вы на " + position + " месте с " + bestResult + " баллами. Последний раз Вы были здесь " + lastEnter);
            return true;
        }
    }
}
