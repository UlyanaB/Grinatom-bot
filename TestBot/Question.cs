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

        /// <summary>
        /// возвращает текст вопроса
        /// </summary>
        /// <param name="ordNumb">номер вопроса</param>
        /// <returns>текст вопроса</returns>
        internal string CreateHeader(int ordNumb)
        {
            BotLinq.Ask ask = BotLnq.GetAskByOrd(ordNumb);
            return ask.AskTxt;
        }

        /// <summary>
        /// возвращает варианты ответов
        /// </summary>
        /// <param name="ordNumb">номер вопроса</param>
        /// <returns>варианты ответов</returns>
        internal InlineKeyboardMarkup CreateInlineKeyboard(int ordNumb)
        {
            BotLinq.Ask ask = BotLnq.GetAskByOrd(ordNumb);
            IEnumerable<InlineKeyboardButton[]> ikm 
                = BotLnq
                    .GetAnsByAsk(ask)
                    .Select (x => new[] 
                                { InlineKeyboardButton.WithCallbackData(
                                                                        x.Ind.ToString() + ") " + x.AnsTxt, 
                                                                        x.TrueInd.ToString().ToUpper() == "Y" 
                                                                            ? Form1.YesCmd
                                                                            : Form1.NoCmd
                                                                       )
                                }
                            );
            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(ikm.ToArray());
            return inlineKeyboard;
        }
    }
}
