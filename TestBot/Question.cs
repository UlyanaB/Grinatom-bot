using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace TestBot
{
    internal class Question
    {
        private BotLinq BotLnq;

        internal Question(BotLinq botLinq)
        {
            BotLnq = botLinq;
        }

        internal InlineKeyboardMarkup CreateInlineKeyboard(int ordNumb)
        {
            BotLinq.Ask ask = BotLnq.GetAskByOrd(ordNumb);
            IEnumerable<InlineKeyboardButton[]> ikm 
                = BotLnq
                    .GetAnsByAsk(ask)
                    .Select(x => new[] { InlineKeyboardButton.WithCallbackData(x.Ind.ToString() + " " + x.AnsTxt) });
            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(ikm.ToArray());
            return inlineKeyboard;
        }
    }
}
