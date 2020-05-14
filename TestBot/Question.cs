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
        private readonly IList<int> AskNumbLst = null;
        private readonly Random random = new Random();

        internal Question(BotLinq botLinq)
        {
            BotLnq = botLinq;
            AskNumbLst = botLinq.GetAskList();
        }

        /// <summary>
        /// получить номер следующего вопроса
        /// </summary>
        /// <returns>номер следующего вопроса, если вопросов больше нет то возвращает -1</returns>
        internal int GetNextNumb()
        {
            if (AskNumbLst.Count == 0)
            {
                return -1;
            }
            int r = random.Next(AskNumbLst.Count);
            int nextNumb = AskNumbLst[r];
            AskNumbLst.RemoveAt(r);
            return nextNumb;
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
                                                                            ? BotForm.YesCmd
                                                                            : BotForm.NoCmd
                                                                       )
                                }
                            );
            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(ikm.ToArray());
            return inlineKeyboard;
        }
    }
}
