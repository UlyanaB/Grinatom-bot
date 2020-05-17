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

namespace AdminBot
{
    internal class Question
    {
        private readonly KeyValuePair<string, string> StartCommand
                                                                = new KeyValuePair<string, string>("Начать", AdminBotForm.ContCmd);
        private readonly KeyValuePair<string, string> ContinueCommand
                                                                = new KeyValuePair<string, string>("Продолжить", AdminBotForm.ContCmd);
        private readonly KeyValuePair<string, string> ExitCommand
                                                                = new KeyValuePair<string, string>("Выйти", AdminBotForm.ExitCmd);
        private readonly KeyValuePair<string, string> SkipCommand
                                                                = new KeyValuePair<string, string>("Пропустить", AdminBotForm.SkipCmd);

        internal Question()
        {
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
                                }
                        }
                                        );
            return ExitInlineKeyboard;
        }

    }
}

