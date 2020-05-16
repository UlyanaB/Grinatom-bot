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
        private const string token = "1211113358:AAEhRzhwrlNt2JL_13p9hrdUe9IjW7Ms6AQ";

        internal const string ContCmd    = "/ContinueCommand";
        internal const string ExitCmd    = "/ExitCommand";
        internal const string TimeoutCmd = "/TimeoutCommand";
        internal const string SkipCmd   = "/SkipCommand";
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

        #region Init Bot
        public BotForm()
        {
            InitializeComponent();

            botLinq = new BotLinq();
            question = new Question(botLinq);
            botClient = new TelegramBotClient(token);

            timer = new System.Timers.Timer();
            timer.Enabled = false;
            timer.Interval = 60 * 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;

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
                        st = States.Start;
                        guid = Guid.NewGuid();
                        string welcome = "Добро пожаловать " + chat.FirstName + " " + chat.LastName;
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
            string[] dataParts = callbackQuery.Data.Split(':');
            if ((dataParts.Length == 1) || (dataParts.Length == 2 && dataParts[1] == guid.ToString()))
            {
                guid = Guid.NewGuid();
                switch (dataParts[0])
                {
                    case ContCmd:
                        int numb = question.GetNextNumb();
                        if (numb == -1)
                        {
                            await botClient.SendTextMessageAsync(chat.Id, "У нас больше нет вопросов",
                                                                    replyMarkup: question.CreateExitInlineKeyboard(guid));
                            break;
                        }
                        string header = question.CreateHeader(numb);
                        ikm = question.CreateAnsInlineKeyboard(numb, guid);
                        ++AskNumb;
                        await botClient.SendTextMessageAsync(chat.Id, header, replyMarkup: ikm);
                        timer.Enabled = true;
                        break;

                    case YesCmd:
                        string trAns = string.Format("Правильный ответ! ( {0} из {1} )", ++TrueNumb, AskNumb);
                        await botClient.SendTextMessageAsync(chat.Id, trAns, replyMarkup: question.CreateContinueOrExitInlineKeyboard(guid));
                        break;

                    case NoCmd:
                        string flsAns = string.Format("Вы ошиблись ( {0} из {1} )", TrueNumb, AskNumb);
                        await botClient.SendTextMessageAsync(chat.Id, flsAns, replyMarkup: question.CreateContinueOrExitInlineKeyboard(guid));
                        break;

                    case SkipCmd:
                        string skpAns = string.Format("Вы отказались от ответа ( {0} из {1} )", TrueNumb, AskNumb);
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
            }
        }
    }
}
