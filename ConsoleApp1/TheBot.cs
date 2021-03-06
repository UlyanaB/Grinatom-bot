﻿using System;
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
        private InlineKeyboardMarkup LoginOrRegisterInlineKeyboard = 
            new InlineKeyboardMarkup(new[]
                {
                    new [] // first row
                        {
                            InlineKeyboardButton.WithCallbackData(LoginCommand),
                            InlineKeyboardButton.WithCallbackData(RegisterCommand),
                        }
                });
        private States St = States.none;
        private BotDB botDb;

        public TheBot()
        {
            botClient = new TelegramBotClient(token);
            // new BotLinq();
            botDb = new BotDB();

            botClient.OnMessage += BotClient_OnMessageReceived;
            botClient.OnMessageEdited += BotClient_OnMessageReceived;
            botClient.OnCallbackQuery += BotOnCallbackQueryReceived;
            botClient.OnInlineQuery += BotOnInlineQueryReceived;
            botClient.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            botClient.OnReceiveError += BotOnReceiveError;

            botClient.StartReceiving(Array.Empty<UpdateType>());

        }

        private void BotOnReceiveError(object sender, ReceiveErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BotClient_OnMessageReceived(object sender, MessageEventArgs e)
        {
            if (chat == null)
            {
                chat = e.Message.Chat;
            }
            if (e.Message.Type == MessageType.Text)
            {
                BotDB.Usr usr = null;
                string login = null;
                string pas = null;

                switch (St)
                {
                    case States.enter_name:
                        login = e.Message.Text.Trim();
                        usr = botDb.GetUserByLogin(login);
                        St = States.enter_pas;
                        botClient.SendTextMessageAsync(chat.Id, "Enter your password");
                        return;

                    case States.enter_pas:
                        pas = e.Message.Text.Trim();
                        if (usr != null && usr.nickName == login && usr.psw == pas)
                        {
                            login = pas = null;
                            St = States.login;
                            string welcome1 = "Приветствуем Вас в игре";
                            botClient.SendTextMessageAsync(chat.Id, welcome1, replyMarkup: LoginOrRegisterInlineKeyboard);
                        }
                        else
                        {
                            St = States.enter_name;
                            botClient.SendTextMessageAsync(chat.Id, "Mistake. Try again.");
                            botClient.SendTextMessageAsync(chat.Id, "", replyMarkup: LoginOrRegisterInlineKeyboard);
                        }
                        return;
                }
                switch (e.Message.Text)
                {
                    #region start
                    case "/start":
                        St = States.start;
                        string welcome = "Добро пожаловать " + chat.FirstName + " " + chat.LastName;
                        botClient.SendTextMessageAsync(chat.Id, welcome, replyMarkup: LoginOrRegisterInlineKeyboard);
                        break;
                    #endregion start

                    #region default
                    default:
                        botClient.SendTextMessageAsync(chat.Id, "Sorry, error in the Bot", replyMarkup: new ReplyKeyboardRemove());
                        break;
                        #endregion default
                }
            }
        }

        private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;
            switch (callbackQuery.Data)
            {
                case LoginCommand:
                    St = States.enter_name;
                    await botClient.SendTextMessageAsync(chat.Id, "Enter your login");
                    break;

                case RegisterCommand:
                    break;

                default:
                    break;
            }
        }

        private void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            throw new NotImplementedException();
            //textBox1.Text += $"Received inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}";
        }

    }
}
