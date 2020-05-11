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
        private BotDB botDb;

        public Form1()
        {
            InitializeComponent();

            new BotLinq();

            botClient = new TelegramBotClient(token);
            botDb = new BotDB();
        }

        private void BotOnReceiveError(object sender, ReceiveErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs e)
        {
            throw new NotImplementedException(); 
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            botClient.SendTextMessageAsync(chat.Id, "456");
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

                    #region inline
                    case "/inline":
                        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new [] // first row
                        {
                            InlineKeyboardButton.WithCallbackData("1.1"),
                            InlineKeyboardButton.WithCallbackData("1.2"),
                        },
                        new [] // second row
                        {
                            InlineKeyboardButton.WithCallbackData("2.1"),
                            InlineKeyboardButton.WithCallbackData("2.2"),
                        }
                    });
                        botClient.SendTextMessageAsync(chat.Id, "Choose", replyMarkup: inlineKeyboard);
                        break;
                    #endregion inline

                    #region keyboard
                    case "/keyboard":
                        ReplyKeyboardMarkup z = new ReplyKeyboardMarkup();
                        ReplyKeyboardMarkup ReplyKeyboard = new ReplyKeyboardMarkup(new[]
                        {
                        new[] { new KeyboardButton("1.1"), new KeyboardButton("1.2") },
                        new[] { new KeyboardButton("2.1"), new KeyboardButton("2.2") },
                    });

                        botClient.SendTextMessageAsync(chat.Id, "Choose", replyMarkup: ReplyKeyboard);
                        break;
                    #endregion keyboard

                    #region photo
                    case "/photo":
                        botClient.SendChatActionAsync(chat.Id, ChatAction.UploadPhoto);

                        const string file = @"Chrysanthemum.jpg";

                        var fileName = file.Split(Path.DirectorySeparatorChar).Last();

                        using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            //                            FileToSend fileToSend = new FileToSend("Nice Picture", fileStream);
                            Telegram.Bot.Types.InputFiles.InputOnlineFile fileToSend = new Telegram.Bot.Types.InputFiles.InputOnlineFile(fileStream);
                            botClient.SendPhotoAsync(chat.Id, fileToSend);
                        }
                        break;
                    #endregion photo

                    #region request
                    case "/request":
                        var RequestReplyKeyboard = new ReplyKeyboardMarkup(new[]
                        {
                            new KeyboardButton("Location"),
                            new KeyboardButton("Contact"),
                        });

                        botClient.SendTextMessageAsync(chat.Id, "Who or Where are you?", replyMarkup: RequestReplyKeyboard);
                        break;
                    #endregion request

                    #region default
                    default:
                        const string usage = @"
Usage:
/inline   - send inline keyboard
/keyboard - send custom keyboard
/photo    - send a photo
/request  - request location or contact";

                        botClient.SendTextMessageAsync(chat.Id, usage, replyMarkup: new ReplyKeyboardRemove());
                        break;
                        #endregion default
                }
            }
            //await botClient.SendTextMessageAsync(e.Message.Chat.Id, "123");
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
            textBox1.Text += $"Received inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            botClient.StartReceiving(Array.Empty<UpdateType>());
            botClient.OnMessage += BotClient_OnMessageReceived;
            botClient.OnMessageEdited += BotClient_OnMessageReceived;
            botClient.OnCallbackQuery += BotOnCallbackQueryReceived;
            botClient.OnInlineQuery += BotOnInlineQueryReceived;
            botClient.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            botClient.OnReceiveError += BotOnReceiveError;
        }
    }
}
