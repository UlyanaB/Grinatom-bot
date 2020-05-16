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

        #region Predefined keyboard
        private const string ContCmd    = "/ContinueCommand";
        private const string ExitCmd    = "/ExitCommand";
        private const string TimeoutCmd = "/TimeoutCommand";
        private const string SkipCmd   = "/SkipCommand";
        internal const string NoCmd     = "/NoCommand";
        internal const string YesCmd    = "/YesCommand";

        private static readonly KeyValuePair<string, string> StartCommand 
                                                                = new KeyValuePair<string, string>("Начать", ContCmd);
        private static readonly KeyValuePair<string, string> ContinueCommand 
                                                                = new KeyValuePair<string, string>("Продолжить", ContCmd);
        private static readonly KeyValuePair<string, string> ExitCommand
                                                                = new KeyValuePair<string, string>("Выйти", ExitCmd);
        internal static readonly KeyValuePair<string, string> SkipCommand
                                                                = new KeyValuePair<string, string>("Пропустить", SkipCmd);

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
        private static readonly InlineKeyboardMarkup ExitInlineKeyboard
            = new InlineKeyboardMarkup(
                                            new[]
                                                {
                                                    new []
                                                          {
                                                            InlineKeyboardButton.WithCallbackData(ExitCommand.Key, ExitCommand.Value),
                                                          }
                                                }
                                        );
        #endregion Predefined keyboard

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

        private async void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Enabled = false;
            guid = Guid.NewGuid();
            string flsAns = string.Format("Вы не успели ответить ( {0} из {1} )", TrueNumb, AskNumb);
            await botClient.SendTextMessageAsync(chat.Id, flsAns, replyMarkup: ContinueOrExitInlineKeyboard);
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
                        string welcome = "Добро пожаловать " + chat.FirstName + " " + chat.LastName;
                        await botClient.SendTextMessageAsync(chat.Id, welcome, replyMarkup: StartOrExitInlineKeyboard);
                        return;
                }
            }
        }

        private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
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
                                                                    replyMarkup: ExitInlineKeyboard);
                            break;
                        }
                        string header = question.CreateHeader(numb);
                        ikm = question.CreateInlineKeyboard(numb, guid);
                        ++AskNumb;
                        await botClient.SendTextMessageAsync(chat.Id, header, replyMarkup: ikm);
                        timer.Enabled = true;
                        break;

                    case YesCmd:
                        string trAns = string.Format("Правильный ответ! ( {0} из {1} )", ++TrueNumb, AskNumb);
                        await botClient.SendTextMessageAsync(chat.Id, trAns, replyMarkup: ContinueOrExitInlineKeyboard);
                        break;

                    case NoCmd:
                        string flsAns = string.Format("Вы ошиблись ( {0} из {1} )", TrueNumb, AskNumb);
                        await botClient.SendTextMessageAsync(chat.Id, flsAns, replyMarkup: ContinueOrExitInlineKeyboard);
                        break;

                    case SkipCmd:
                        string skpAns = string.Format("Вы отказались от ответа ( {0} из {1} )", TrueNumb, AskNumb);
                        await botClient.SendTextMessageAsync(chat.Id, skpAns, replyMarkup: ContinueOrExitInlineKeyboard);
                        break;

                    case ExitCmd:
                        st = States.Stop;
                        await botClient.SendTextMessageAsync(chat.Id, "Заходите еще");
                        break;

                    default:
                        await botClient.SendTextMessageAsync(chat.Id, "Извините, ошибка в боте", replyMarkup: new ReplyKeyboardRemove());
                        break;
                }
            }
        }
    }
}
