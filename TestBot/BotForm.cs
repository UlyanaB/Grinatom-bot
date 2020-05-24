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
    public partial class BotForm : Form
    {                                 
        //Bor159_bot
        private const string token = "1211113358:AAEhRzhwrlNt2JL_13p9hrdUe9IjW7Ms6AQ";

        internal const string ContCmd    = "/ContinueCommand";
        internal const string ExitCmd    = "/ExitCommand";
        internal const string TimeoutCmd = "/TimeoutCommand";
        internal const string SkipCmd = "/SkipCommand";
        internal const string FactCmd = "/FactCommand";
        internal const string NoCmd     = "/NoCommand";
        internal const string YesCmd    = "/YesCommand";

        private ITelegramBotClient botClient = null;
        private Chat chat = null;
        private BotLinq botLinq;
        private Question question;
        private InlineKeyboardMarkup ikm;
        private Guid guid = Guid.Empty;
        private System.Timers.Timer timer;
        private States st;
        private int AskNumb = 0;    // вопросов задано
        private int TrueNumb = 0;   // правильных ответов
        private BotLinq.BotUsers botUsers = null;
        private int askId;

        #region Init Bot
        public BotForm()
        {
            InitializeComponent();

            botLinq = new BotLinq();
            botClient = new TelegramBotClient(token);

            timer = new System.Timers.Timer { Enabled = false, Interval = 60 * 1000, AutoReset = true };
            timer.Elapsed += Timer_Elapsed;

            botClient.OnMessage += BotClient_OnMessageReceived;
            botClient.OnMessageEdited += BotClient_OnMessageReceived;
            botClient.OnCallbackQuery += BotOnCallbackQueryReceived;
            botClient.OnInlineQuery += BotOnInlineQueryReceived;
            botClient.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            botClient.OnReceiveError += BotOnReceiveError;

            botClient.StartReceiving(Array.Empty<UpdateType>());

            st = States.None;
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
        }
        #endregion Not implemented

        private async void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Enabled = false;
            guid = Guid.NewGuid();
            string flsAns = string.Format("Вы не успели ответить ( {0} из {1} )", TrueNumb, AskNumb);
            await botClient.SendTextMessageAsync(chat.Id, flsAns, replyMarkup: question.CreateContinueOrExitInlineKeyboard(guid));
        }

        private async void BotClient_OnMessageReceived(object sender, MessageEventArgs e)
        {
            if (chat == null)
            {
                chat = e.Message.Chat;
            }
            if (e.Message.Type == MessageType.Text)
            {
                switch (e.Message.Text)
                {
                    case "/start":
                        int tlgUserId = e.Message.From.Id;
                        string tlgUserName = e.Message.From.FirstName + " " + e.Message.From.LastName;
                        botUsers = botLinq.AddOrUpdateBotUsers(tlgUserId, tlgUserName);

                        AskNumb = 0;    // вопросов задано
                        TrueNumb = 0;   // правильных ответов
                        question = new Question(botLinq);

                        question = new Question(botLinq);
                        st = States.Start;
                        guid = Guid.NewGuid();
                        string welcome = "Добро пожаловать " + botUsers.TlgUserName;
                        await botClient.SendTextMessageAsync(chat.Id, welcome, replyMarkup: question.CreateStartOrExitInlineKeyboard(guid));
                        return;
                }
            }
        }

        private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            timer.Enabled = false;
            if (st == States.Stop)
            {
                return;
            }

            CallbackQuery callbackQuery = callbackQueryEventArgs.CallbackQuery;
            string x = callbackQuery.Message.Text;
            string[] dataParts = callbackQuery.Data.Split(':');
            if ((dataParts.Length == 2 || dataParts.Length == 3) && dataParts[1] == guid.ToString())
            {
                guid = Guid.NewGuid();
                switch (dataParts[0])
                {
                    case ContCmd:
                        askId = question.GetNextNumb(question.AskNumbLst);
                        if (askId == -1)
                        {
                            await botClient.SendTextMessageAsync(chat.Id, "У нас больше нет вопросов",
                                                                    replyMarkup: question.CreateExitInlineKeyboard(guid));
                            break;
                        }
                        string header = question.CreateHeader(askId);
                        ikm = question.CreateAnsInlineKeyboard(askId, guid);
                        await botClient.SendTextMessageAsync(chat.Id, header, replyMarkup: ikm);
                        timer.Enabled = true;
                        break;

                    case FactCmd:
                        int id = question.GetNextNumb(question.FactNumbLst);
                        if (id == -1)
                        {
                            question.FactNumbLst = botLinq.GetFactList();
                            id = question.GetNextNumb(question.FactNumbLst);
                        }
                        string fact = botLinq.GetFactById(id);
                        InlineKeyboardMarkup ikm1 = askId == -1
                                                        ? question.CreateExitInlineKeyboard(guid)
                                                        : question.CreateContinueOrExitInlineKeyboard(guid);
                        await botClient.SendTextMessageAsync(chat.Id, fact, replyMarkup: ikm1);
                        break;

                    case YesCmd:
                        string trAns = string.Format("Правильный ответ! ( {0} из {1} )", ++TrueNumb, ++AskNumb);
                        await botClient.SendTextMessageAsync(chat.Id, trAns, replyMarkup: question.CreateContinueOrExitInlineKeyboard(guid));
                        break;

                    case NoCmd:
                        string flsAns = string.Format("Вы ошиблись ( {0} из {1} )", TrueNumb, ++AskNumb);
                        await botClient.SendTextMessageAsync(chat.Id, flsAns, replyMarkup: question.CreateContinueOrExitInlineKeyboard(guid));
                        break;

                    case SkipCmd:
                        string skpAns = string.Format("Вы отказались от ответа ( {0} из {1} )", TrueNumb, ++AskNumb);
                        await botClient.SendTextMessageAsync(chat.Id, skpAns, replyMarkup: question.CreateContinueOrExitInlineKeyboard(guid));
                        break;

                    case ExitCmd:
                        st = States.Stop;
                        await botClient.SendTextMessageAsync(chat.Id, "Заходите еще");
                        break;

                    default:
                        await botClient.SendTextMessageAsync(chat.Id, "Извините, ошибка в боте");
                        break;
                }
                if (dataParts[0] == YesCmd)
                {
                    if (botUsers.BestResult < TrueNumb)
                    {
                        botUsers.BestResult = TrueNumb;
                        botLinq.AddOrUpdateBotUsers(botUsers);
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
                    botLinq.AddToUsersLog(botUsers.Id, dataParts[0] == YesCmd ? 'Y' : 'N', AskNumb, TrueNumb, callbackQuery.Message.Text, txt);
                }
            }
        }
    }
}
