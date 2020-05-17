using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AdminBot
{
    public partial class AdminBotForm : Form
    {
        //Bor159Admin_bot
        private const string token = "992461240:AAEAZGMF0LT5XDQRLxHz3KXbGYCQG31r0QA";

        internal const string AddCmd = "/AddCommand";
        internal const string EditCmd = "/EditCommand";
        internal const string DeleteCmd = "/DeleteCommand";
        internal const string ListCmd = "/ListCommand";
        internal const string EnterNumbCmd = "/EnterNumbCommand";
        internal const string FirstCmd = "/FirstCommand";
        internal const string NextCmd = "/NextCommand";
        internal const string PrevCmd = "/PrevCommand";
        internal const string LastCmd = "/LastCommand";
        internal const string NumberCmd = "/NumberCommand";

        internal const string ExitCmd = "/ExitCommand";
        internal const string BackCmd = "/BackCommand";

        internal const string TimeoutCmd = "/TimeoutCommand";

        private ITelegramBotClient botClient = null;
        private Chat chat = null;
        private Guid guid = Guid.Empty;
        private States st;
        private Question question;
        private BotLinq botLinq;

        public AdminBotForm()
        {
            InitializeComponent();

            botClient = new TelegramBotClient(token);
            botLinq = new BotLinq();
            question = new Question(botLinq);

            botClient.OnMessage += BotClient_OnMessageReceived;
            botClient.OnMessageEdited += BotClient_OnMessageReceived;
            botClient.OnCallbackQuery += BotOnCallbackQueryReceived;
            botClient.OnInlineQuery += BotOnInlineQueryReceived;
            botClient.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            botClient.OnReceiveError += BotOnReceiveError;

            botClient.StartReceiving(Array.Empty<UpdateType>());

        }

        #region not implemented
        private void BotOnReceiveError(object sender, ReceiveErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion not implemented

        private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs e)
        {
            int askNumb = -1;

            if (st == States.Stop)
            {
                return;
            }

            CallbackQuery callbackQuery = e.CallbackQuery;
            string[] dataParts = callbackQuery.Data.Split(':');
            if (dataParts.Length == 2 && dataParts[1] == guid.ToString())
            {
                guid = Guid.NewGuid();
                if (dataParts[0].StartsWith(NumberCmd))
                {
                    askNumb = int.Parse(dataParts[0].Substring(NumberCmd.Length));
                    dataParts[0] = NumberCmd;
                }
                switch (dataParts[0])
                {
                    case AddCmd:
                        break;

                    case EditCmd:
                        await botClient.SendTextMessageAsync(chat.Id, "Выберите вопрос", replyMarkup: question.CreateGetInlineKeyboard(guid));
                        break;

                    case DeleteCmd:
                        break;

                    case ListCmd:
                        await botClient.SendTextMessageAsync(chat.Id, "Выберите вопрос", replyMarkup: question.CreateListInlineKeyboard(guid));
                        break;

                    case EnterNumbCmd:
                        await botClient.SendTextMessageAsync(chat.Id, "Введите номер вопроса");
                        break;

                    case NumberCmd:
                        await botClient.SendTextMessageAsync(chat.Id, botLinq.GetAskByOrd(askNumb).AskTxt);
                        break;

                    case BackCmd:
                        await botClient.SendTextMessageAsync(chat.Id, "Вы отказались от этого меню", replyMarkup: question.CreateEditInlineKeyboard(guid));
                        break;

                    case ExitCmd:
                        st = States.Stop;
                        await botClient.SendTextMessageAsync(chat.Id, "Сеанс завершен");
                        break;

                    default:
                        await botClient.SendTextMessageAsync(chat.Id, "Извините, ошибка в боте");
                        break;
                }
            }
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
                        await botClient.SendTextMessageAsync(chat.Id, welcome, replyMarkup: question.CreateEditInlineKeyboard(guid));
                        return;
                }
            }
        }
    }
}
