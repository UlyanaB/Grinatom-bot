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

        private const string ContCmd = "/ContinueCommand";
        private const string ExitCmd = "/ExitCommand";

        private static readonly KeyValuePair<string, string> StartCommand 
                                                                = new KeyValuePair<string, string>("Начать", ContCmd);
        private static readonly KeyValuePair<string, string> ContinueCommand 
                                                                = new KeyValuePair<string, string>("Продолжить", ContCmd);
        private static readonly KeyValuePair<string, string> ExitCommand        
                                                                = new KeyValuePair<string, string>("Выйти", ExitCmd);

        private static readonly InlineKeyboardMarkup StartOrExitInlineKeyboard 
            = new InlineKeyboardMarkup  (
                                            new[]
                                                {
                                                    new [] 
                                                          {
                                                            InlineKeyboardButton.WithCallbackData(StartCommand.Key, StartCommand.Value),
                                                            InlineKeyboardButton.WithCallbackData(ExitCommand.Key, ExitCommand.Value),
                                                          }
                                                }
                                        );
        private static readonly InlineKeyboardMarkup ContinueOrExitInlineKeyboard
            = new InlineKeyboardMarkup(
                                            new[]
                                                {
                                                    new []
                                                          {
                                                            InlineKeyboardButton.WithCallbackData(ContinueCommand.Key, ContinueCommand.Value),
                                                            InlineKeyboardButton.WithCallbackData(ExitCommand.Key, ExitCommand.Value),
                                                          }
                                                }
                                        );

        private ITelegramBotClient botClient = null;
        private Chat chat = null;
        private States St = States.none;
        private BotLinq botLinq;
        private Question question;

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
                    string welcome = "Добро пожаловать " + chat.FirstName + " " + chat.LastName;
                    botClient.SendTextMessageAsync(chat.Id, welcome, replyMarkup: StartOrExitInlineKeyboard);
                    return false;

                default:
                    botClient.SendTextMessageAsync(chat.Id, "Извините, ошибка в боте", replyMarkup: new ReplyKeyboardRemove());
                    return false;
            }
        }
        #endregion Messages

        private void BotClient_OnMessageReceived(object sender, MessageEventArgs e)
        {
            bool cont = false;

            if (chat == null)
            {
                chat = e.Message.Chat; 
            }
            if (e.Message.Type == MessageType.Text)
            {
                cont = MessageMachine(e.Message.Text);
                if (!cont)
                {
                    return;
                }
            }
        }

        private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            CallbackQuery callbackQuery = callbackQueryEventArgs.CallbackQuery;
            switch (callbackQuery.Data)
            {
                case ContCmd:
                    string header = question.CreateHeader(1);
                    InlineKeyboardMarkup ikm = question.CreateInlineKeyboard(1);
                    await botClient.SendTextMessageAsync(chat.Id, header, replyMarkup: ikm);
                    break;

                case ExitCmd:
                    await botClient.SendTextMessageAsync(chat.Id, "Заходите еще");
                    break;

                default:
                    await botClient.SendTextMessageAsync(chat.Id, "Извините, ошибка в боте", replyMarkup: new ReplyKeyboardRemove());
                    break;
            }
        }
    }
}
