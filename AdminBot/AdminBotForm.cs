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

        internal const string ContCmd = "/ContinueCommand";
        internal const string ExitCmd = "/ExitCommand";
        internal const string TimeoutCmd = "/TimeoutCommand";
        internal const string SkipCmd = "/SkipCommand";
        internal const string NoCmd = "/NoCommand";
        internal const string YesCmd = "/YesCommand";

        private ITelegramBotClient botClient = null;
        private Chat chat = null;
        private Guid guid = Guid.Empty;
        private States st;
        private Question question;

        public AdminBotForm()
        {
            InitializeComponent();

            botClient = new TelegramBotClient(token);
            question = new Question();

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

        private void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs e)
        {
            throw new NotImplementedException();
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
    }
}
