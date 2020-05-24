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
        private readonly Random random = new Random();

        private readonly KeyValuePair<string, string> StartCommand
                                                                = new KeyValuePair<string, string>("Начать", BotForm.ContCmd);
        private readonly KeyValuePair<string, string> ContinueCommand
                                                                = new KeyValuePair<string, string>("Продолжить", BotForm.ContCmd);
        private readonly KeyValuePair<string, string> ExitCommand
                                                                = new KeyValuePair<string, string>("Выйти", BotForm.ExitCmd);
        private readonly KeyValuePair<string, string> SkipCommand
                                                                = new KeyValuePair<string, string>("Пропустить", BotForm.SkipCmd);
        private readonly KeyValuePair<string, string> FactCommand
                                                                = new KeyValuePair<string, string>("Факты", BotForm.FactCmd);

        internal readonly IList<int> AskNumbLst = null;
        internal IList<int> FactNumbLst = null;

        internal Question(BotLinq botLinq)
        {
            BotLnq = botLinq;
            AskNumbLst = botLinq.GetAskList();
            FactNumbLst = botLinq.GetFactList();
        }

        /// <summary>
        /// получить номер следующего вопроса
        /// </summary>
        /// <returns>номер следующего вопроса, если вопросов больше нет то возвращает -1</returns>
        internal int GetNextNumb(IList<int> lst)
        {
            if (lst.Count == 0)
            {
                return -1;
            }
            int r = random.Next(lst.Count);
            int nextNumb = lst[r];
            lst.RemoveAt(r);
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
        internal InlineKeyboardMarkup CreateAnsInlineKeyboard(int ordNumb, Guid guid)
        {
            BotLinq.Ask ask = BotLnq.GetAskByOrd(ordNumb);
            IEnumerable<InlineKeyboardButton[]> ikm
                = BotLnq
                    .GetAnsByAsk(ask)
                    .Select(x => new[]
                               { InlineKeyboardButton.WithCallbackData(
                                                                         x.Ind.ToString() + ") " + x.AnsTxt,
                                                                        (x.TrueInd.ToString().ToUpper() == "Y"
                                                                            ? BotForm.YesCmd
                                                                            : BotForm.NoCmd)
                                                                         + ":" + guid.ToString() + ":" + x.Ind.ToString()
                                                                       )
                                }
                            );
            List<InlineKeyboardButton[]> ikmL = ikm.ToList();
            ikmL.Add(new[] { InlineKeyboardButton.WithCallbackData(SkipCommand.Key, SkipCommand.Value + ":" + guid.ToString()) });
            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(ikmL.ToArray());
            return inlineKeyboard;
        }

        internal InlineKeyboardMarkup CreateContinueOrExitInlineKeyboard(Guid guid)
        {
            InlineKeyboardMarkup ContinueOrExitInlineKeyboard
                = new InlineKeyboardMarkup(
                    new[]
                        {
                            new []
                                {
                                    InlineKeyboardButton.WithCallbackData(ContinueCommand.Key, ContinueCommand.Value + ":" + guid.ToString()),
                                    InlineKeyboardButton.WithCallbackData(ExitCommand.Key, ExitCommand.Value + ":" + guid.ToString()),
                                    InlineKeyboardButton.WithCallbackData(FactCommand.Key, FactCommand.Value + ":" + guid.ToString()),
                                }
                        }
                                            );
            return ContinueOrExitInlineKeyboard;
        }

        internal InlineKeyboardMarkup CreateStartOrExitInlineKeyboard(Guid guid)
        {
            InlineKeyboardMarkup StartOrExitInlineKeyboard
                = new InlineKeyboardMarkup(
                    new[]
                        {
                            new []
                                {
                                    InlineKeyboardButton.WithCallbackData(StartCommand.Key, StartCommand.Value + ":" + guid.ToString()),
                                    InlineKeyboardButton.WithCallbackData(ExitCommand.Key, ExitCommand.Value + ":" + guid.ToString()),
                                    InlineKeyboardButton.WithCallbackData(FactCommand.Key, FactCommand.Value + ":" + guid.ToString()),
                                }
                        }
                                        );
            return StartOrExitInlineKeyboard;
        }

        internal InlineKeyboardMarkup CreateExitInlineKeyboard(Guid guid)
        {
            InlineKeyboardMarkup ExitInlineKeyboard
                = new InlineKeyboardMarkup(
                    new[]
                        {
                            new []
                                {
                                    InlineKeyboardButton.WithCallbackData(ExitCommand.Key, ExitCommand.Value + ":" + guid.ToString()),
                                    InlineKeyboardButton.WithCallbackData(FactCommand.Key, FactCommand.Value + ":" + guid.ToString()),
                                }
                        }
                                        );
            return ExitInlineKeyboard;
        }

    }
}

