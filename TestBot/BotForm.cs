using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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
using TestBot.Properties;

namespace TestBot
{
    public partial class BotForm : Form
    {
        //private const string token = "1238975566:AAFltoIrHNZIOz-Z57hdqTTnHCJJHhWKUjE"; // вынесен в Settings
        
        internal const string ContCmd    = "/ContinueCommand";
        internal const string ExitCmd    = "/ExitCommand";
        internal const string TimeoutCmd = "/TimeoutCommand";
        internal const string SkipCmd = "/SkipCommand";
        internal const string FactCmd = "/FactCommand";
        internal const string NoCmd     = "/NoCommand";
        internal const string YesCmd    = "/YesCommand";

        internal ITelegramBotClient botClient = null;
        internal BotLinq botLinq;

        private InlineKeyboardMarkup ikm;
        private ConcurrentDictionary<long, BotChat> Chats = new ConcurrentDictionary<long, BotChat>();

        #region Init Bot
        public BotForm()
        {
            //  @GreenIT_bot    -   1238975566:AAFltoIrHNZIOz-Z57hdqTTnHCJJHhWKUjE    
            try
            {
                InitializeComponent();

                botLinq = new BotLinq();
                if (string.IsNullOrWhiteSpace(Properties.Settings.Default.Proxy))
                {
                    botClient = new TelegramBotClient(Properties.Settings.Default.BotToken);
                }
                else
                {
                    string[] proxyParts = Properties.Settings.Default.Proxy.Split(new[] { ':' }, 2);
                    WebProxy httpProxy = new WebProxy(proxyParts[0], int.Parse(proxyParts[1]));
                    botClient = new TelegramBotClient(Properties.Settings.Default.BotToken, httpProxy);
                }

                botClient.OnMessage += BotClient_OnMessageReceived;
                botClient.OnMessageEdited += BotClient_OnMessageReceived;
                botClient.OnCallbackQuery += BotOnCallbackQueryReceived;
                botClient.OnInlineQuery += BotOnInlineQueryReceived;
                botClient.OnInlineResultChosen += BotOnChosenInlineResultReceived;
                botClient.OnReceiveError += BotOnReceiveError;

                botClient.StartReceiving(Array.Empty<UpdateType>());
            }
            catch(Exception ex)
            {
                botLinq.AddToBotErrorLog("BotForm ctor exception - " + ex.Message);
            }
        }
        #endregion Init Bot

        #region Not implemented
        private void BotOnReceiveError(object sender, ReceiveErrorEventArgs e)
        {
            botLinq.AddToBotErrorLog("Unexpected error received - " + e.ApiRequestException.Message);
        }

        private void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs e)
        {
            botLinq.AddToBotErrorLog("Unexpected InlineQuery received - " + "From: '" + e.InlineQuery.From + "', Query: '" + e.InlineQuery.Query + "'");
        }

        private void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs e)
        {
            botLinq.AddToBotErrorLog("Unexpected ChosenInlineResult received - " + "From: '" + e.ChosenInlineResult.From + "', Query: '" + e.ChosenInlineResult.Query + "'");
        }
        #endregion Not implemented

        private async void BotClient_OnMessageReceived(object sender, MessageEventArgs e)
        {
            BotChat botChat = null;
            try
            {
                if (e.Message.Type == MessageType.Text)
                {
                    switch (e.Message.Text)
                    {
                        case "/start":
                            if (Chats.ContainsKey(e.Message.Chat.Id))
                            {
                                botChat = Chats[e.Message.Chat.Id];
                            }
                            else
                            {
                                botChat = new BotChat() { id = e.Message.Chat.Id, AskNumb = 0, TrueNumb = 0, guid = Guid.NewGuid(), st = States.None };
                                botChat.timer = new System.Timers.Timer { Enabled = false, Interval = Settings.Default.AnswerTime.TotalMilliseconds, AutoReset = true };
                                botChat.timer.Elapsed += botChat.Timer_Elapsed;
                                botChat.botUsers = botLinq.AddOrUpdateBotUsers(e.Message.From.Id, e.Message.Chat.Id, e.Message.From.FirstName + " " + e.Message.From.LastName);
                                botChat.question = new Question(botLinq);
                                Chats[e.Message.Chat.Id] = botChat;

                                string welcome = "Добро пожаловать " + botChat.botUsers.TlgUserName;
                                await botClient.SendTextMessageAsync(botChat.id, welcome, replyMarkup: botChat.question.CreateStartOrExitInlineKeyboard(botChat.guid));

                                botLinq.AddToUsersLog(botChat.botUsers.Id, ' ', botChat.AskNumb, botChat.TrueNumb, "Вход", "Пользователь '" + botChat.botUsers.TlgUserName + "' вошел в чат");
                            }
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                botLinq.AddToBotErrorLog("OnMessageReceived exception - " + ex.Message);
            }

        }

        private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            try
            {
                BotChat botChat = Chats[callbackQueryEventArgs.CallbackQuery.Message.Chat.Id];

                botChat.timer.Enabled = false;
                if (botChat.st == States.Stop)
                {
                    botChat.timer.Elapsed -= botChat.Timer_Elapsed;
                    botLinq.AddToUsersLog(botChat.botUsers.Id, ' ', botChat.AskNumb, botChat.TrueNumb, "Выход", "Пользователь '" + botChat.botUsers.TlgUserName + "' вышел из чата");
                    Chats.TryRemove(botChat.id, out botChat);
                    return;
                }

                CallbackQuery callbackQuery = callbackQueryEventArgs.CallbackQuery;
                string x = callbackQuery.Message.Text;
                string[] dataParts = callbackQuery.Data.Split(':');
                if ((dataParts.Length == 2 || dataParts.Length == 3) && dataParts[1] == botChat.guid.ToString())
                {
                    botChat.guid = Guid.NewGuid();
                    switch (dataParts[0])
                    {
                        case ContCmd:
                            botChat.askId = botChat.question.GetNextNumb(botChat.question.AskNumbLst);
                            if (botChat.askId == -1)
                            {
                                await botClient.SendTextMessageAsync(botChat.id, "У нас больше нет вопросов",
                                                                        replyMarkup: botChat.question.CreateExitInlineKeyboard(botChat.guid));
                                break;
                            }
                            string header = botChat.question.CreateHeader(botChat.askId);
                            ikm = botChat.question.CreateAnsInlineKeyboard(botChat.askId, botChat.guid);
                            await botClient.SendTextMessageAsync(botChat.id, header, replyMarkup: ikm);
                            botChat.askTxt = header;
                            botChat.timer.Enabled = true;
                            break;

                        case FactCmd:
                            int id = botChat.question.GetNextNumb(botChat.question.FactNumbLst);
                            if (id == -1)
                            {
                                botChat.question.FactNumbLst = botLinq.GetFactList();
                                id = botChat.question.GetNextNumb(botChat.question.FactNumbLst);
                            }
                            string fact = botLinq.GetFactById(id);
                            InlineKeyboardMarkup ikm1 = botChat.askId == -1
                                                            ? botChat.question.CreateExitInlineKeyboard(botChat.guid)
                                                            : botChat.question.CreateContinueOrExitInlineKeyboard(botChat.guid);
                            await botClient.SendTextMessageAsync(botChat.id, fact, replyMarkup: ikm1);
                            break;

                        case YesCmd:
                            string trAns = string.Format("Правильный ответ! ( {0} из {1} )", ++botChat.TrueNumb, ++botChat.AskNumb);
                            await botClient.SendTextMessageAsync(botChat.id, trAns, replyMarkup: botChat.question.CreateContinueOrExitInlineKeyboard(botChat.guid));
                            break;

                        case NoCmd:
                            string flsAns = string.Format("Вы ошиблись ( {0} из {1} )", botChat.TrueNumb, ++botChat.AskNumb);
                            await botClient.SendTextMessageAsync(botChat.id, flsAns, replyMarkup: botChat.question.CreateContinueOrExitInlineKeyboard(botChat.guid));
                            break;

                        case SkipCmd:
                            string skpAns = string.Format("Вы отказались от ответа ( {0} из {1} )", botChat.TrueNumb, ++botChat.AskNumb);
                            await botClient.SendTextMessageAsync(botChat.id, skpAns, replyMarkup: botChat.question.CreateContinueOrExitInlineKeyboard(botChat.guid));
                            break;

                        case ExitCmd:
                            botChat.st = States.Stop;
                            await botClient.SendTextMessageAsync(botChat.id, "Заходите еще");
                            botLinq.AddToUsersLog(botChat.botUsers.Id, ' ', botChat.AskNumb, botChat.TrueNumb, "Выход", "Пользователь '" + botChat.botUsers.TlgUserName + "' вышел из чата");
                            botChat.timer.Elapsed -= botChat.Timer_Elapsed;
                            Chats.TryRemove(botChat.id, out botChat);
                            break;

                        default:
                            await botClient.SendTextMessageAsync(botChat.id, "Извините, ошибка в боте");
                            botLinq.AddToBotErrorLog("Unexpected CallbackQuery received - " + dataParts[0]);
                            break;
                    }
                    if (dataParts[0] == YesCmd)
                    {
                        if (botChat.botUsers.BestResult < botChat.TrueNumb)
                        {
                            botChat.botUsers.BestResult = botChat.TrueNumb;
                            botLinq.AddOrUpdateBotUsers(botChat.botUsers);
                        }
                    }
                    if (dataParts[0] == YesCmd || dataParts[0] == NoCmd || dataParts[0] == SkipCmd)
                    {
                        string txt = "";
                        if (dataParts.Length > 2)
                        {
                            IEnumerable<IEnumerable<InlineKeyboardButton>> ikb = callbackQuery.Message.ReplyMarkup.InlineKeyboard;
                            foreach (IEnumerable<InlineKeyboardButton> oneButton in ikb)
                            {
                                if (oneButton.First().Text.StartsWith(dataParts[2]))
                                {
                                    txt = oneButton.First().Text;
                                    break;
                                }
                            }
                        }
                        string msgTxt = callbackQuery.Message.Text;
                        char ansType;
                        switch(dataParts[0])
                        {
                            case YesCmd:
                                ansType = 'Y';
                                break;

                            case NoCmd:
                                ansType = 'N';
                                break;

                            case SkipCmd:
                                ansType = 'S';
                                break;

                            default:
                                ansType = 'U';
                                break;
                        }
                        botLinq.AddToUsersLog(botChat.botUsers.Id, ansType, botChat.AskNumb, botChat.TrueNumb, msgTxt, txt);
                    }
                }
            }
            catch (Exception ex)
            {
                botLinq.AddToBotErrorLog("OnCallbackQueryReceived exception - " + ex.Message);
            }
        }
    }
}
