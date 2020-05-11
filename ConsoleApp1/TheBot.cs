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


namespace ConsoleApp1
{
    class TheBot
    {
        private const string token = "1211113358:AAEhRzhwrlNt2JL_13p9hrdUe9IjW7Ms6AQ";

        private const string LoginCommand = "Login";
        private const string RegisterCommand = "Register";

        private ITelegramBotClient botClient = null;
        private Chat chat = null;
        private InlineKeyboardMarkup LoginOrRegisterInlineKeyboard = new InlineKeyboardMarkup(new[]
                                                                            {
                                                                                new [] // first row
                                                                                        {
                                                                                            InlineKeyboardButton.WithCallbackData(LoginCommand),
                                                                                            InlineKeyboardButton.WithCallbackData(RegisterCommand),
                                                                                        }
                                                                            });
        private States St = States.none;
        private BotDB botDb;


    }
}
