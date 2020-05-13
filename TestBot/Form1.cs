using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace TestBot
{
    public partial class Form1 : Form
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
        private BotLinq botLinq;
        private Question question;
        private BotLinq.Usr usr = null;
        private string login = null;
        private string pas = null;

        #region Init Bot
        public Form1()
        {
            InitializeComponent();

            botLinq = new BotLinq();
            question = new Question(botLinq);
            botClient = new TelegramBotClient(token);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            botClient.OnMessage += BotClient_OnMessageReceived;
            botClient.OnMessageEdited += BotClient_OnMessageReceived;
            botClient.OnCallbackQuery += BotOnCallbackQueryReceived;
            botClient.OnInlineQuery += BotOnInlineQueryReceived;
            botClient.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            botClient.OnReceiveError += BotOnReceiveError;

            botClient.StartReceiving(Array.Empty<UpdateType>());
        }
        #endregion Init Bot

        #region Not implemented
        private void BotOnReceiveError(object sender, ReceiveErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs e)
        {
            throw new NotImplementedException(); 
        }

        private void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            throw new NotImplementedException();
            //textBox1.Text += $"Received inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}";
        }
        #endregion Not implemented

        #region States
        /// <summary>
        /// Конечный автомат
        /// </summary>
        /// <param name="text"></param>
        /// <returns>продолжить выполнение - true, прекратить выполнение - false</returns>
        private bool StateMachine(string text)
        {
            switch (St)
            {
                case States.enter_name:
                    login = text;
                    usr = botLinq.GetUserByLogin(login);
                    St = States.enter_pas;
                    botClient.SendTextMessageAsync(chat.Id, "Enter your password");
                    return false;

                case States.enter_pas:
                    pas = text;
                    if (usr != null && usr.NickName == login && usr.Psw == pas)
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
                    return false;
            }
            return true;
        }
        #endregion States

        #region Messages
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns>продолжить выполнение - true, прекратить выполнение - false</returns>
        private bool MessageMachine(string text)
        {
            switch (text)
            {
                case "/start":
                    St = States.start;
                    string welcome = "Добро пожаловать " + chat.FirstName + " " + chat.LastName;
                    botClient.SendTextMessageAsync(chat.Id, welcome, replyMarkup: LoginOrRegisterInlineKeyboard);
                    return false;

                default:
                    botClient.SendTextMessageAsync(chat.Id, "Sorry, error in the Bot", replyMarkup: new ReplyKeyboardRemove());
                    return false;
            }
        }
        #endregion Messages

        private void BotClient_OnMessageReceived(object sender, MessageEventArgs e)
        {
            if (chat == null)
            {
                chat = e.Message.Chat; 
            }
            if (e.Message.Type == MessageType.Text)
            {
                bool cont = StateMachine(e.Message.Text.Trim());
                if (!cont)
                {
                    return;
                }

                switch (e.Message.Text)
                {
                    #region start
                    case "/start":
                        St = States.start;
                        string welcome = "Добро пожаловать " + chat.FirstName + " " + chat.LastName;
                        InlineKeyboardMarkup ikm = question.CreateInlineKeyboard(1);
                        botClient.SendTextMessageAsync(chat.Id, welcome, replyMarkup: ikm);
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
            CallbackQuery callbackQuery = callbackQueryEventArgs.CallbackQuery;
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
    }
}
