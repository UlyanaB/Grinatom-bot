using System;
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

namespace AdminBot
{
    internal class Question
    {
        private readonly KeyValuePair<string, string> AddCommand
                                                                = new KeyValuePair<string, string>("Добавить", AdminBotForm.AddCmd);
        private readonly KeyValuePair<string, string> EditCommand
                                                                = new KeyValuePair<string, string>("Редактировать", AdminBotForm.EditCmd);
        private readonly KeyValuePair<string, string> DeleteCommand
                                                                = new KeyValuePair<string, string>("Удалить", AdminBotForm.DeleteCmd);
        private readonly KeyValuePair<string, string> EnterNumbCommand
                                                                = new KeyValuePair<string, string>("Введите номер вопроса", AdminBotForm.EnterNumbCmd);
        private readonly KeyValuePair<string, string> ListCommand
                                                                = new KeyValuePair<string, string>("Вывести список", AdminBotForm.ListCmd);
        private readonly KeyValuePair<string, string> FirstCommand
                                                                = new KeyValuePair<string, string>("Следующие ...", AdminBotForm.FirstCmd);
        private readonly KeyValuePair<string, string> NextCommand
                                                                = new KeyValuePair<string, string>("Следующие ...", AdminBotForm.NextCmd);
        private readonly KeyValuePair<string, string> PrevCommand
                                                                = new KeyValuePair<string, string>("Предыдущие ...", AdminBotForm.PrevCmd);
        private readonly KeyValuePair<string, string> LastCommand
                                                                = new KeyValuePair<string, string>("Предыдущие ...", AdminBotForm.LastCmd);

        private readonly KeyValuePair<string, string> ExitCommand
                                                                = new KeyValuePair<string, string>("Выйти", AdminBotForm.ExitCmd);
        private readonly KeyValuePair<string, string> BackCommand
                                                                = new KeyValuePair<string, string>("Назад", AdminBotForm.BackCmd);

        internal Question()
        {
        }

        internal InlineKeyboardMarkup CreateEditInlineKeyboard(Guid guid)
        {
            InlineKeyboardMarkup EditInlineKeyboard
                = new InlineKeyboardMarkup(
                    new[]
                        {
                            new []
                                {
                                    InlineKeyboardButton.WithCallbackData(AddCommand.Key, AddCommand.Value + ":" + guid.ToString()),
                                    InlineKeyboardButton.WithCallbackData(EditCommand.Key, EditCommand.Value + ":" + guid.ToString()),
                                    InlineKeyboardButton.WithCallbackData(DeleteCommand.Key, DeleteCommand.Value + ":" + guid.ToString()),
                                },
                            new []
                                {
                                    InlineKeyboardButton.WithCallbackData(BackCommand.Key, BackCommand.Value + ":" + guid.ToString()),
                                    InlineKeyboardButton.WithCallbackData(ExitCommand.Key, ExitCommand.Value + ":" + guid.ToString()),
                                },
                        }
                                            );
            return EditInlineKeyboard;
        }

        internal InlineKeyboardMarkup CreateGetInlineKeyboard(Guid guid)
        {
            InlineKeyboardMarkup GetInlineKeyboard
                = new InlineKeyboardMarkup(
                    new[]
                        {
                            new []
                                {
                                    InlineKeyboardButton.WithCallbackData(ListCommand.Key, ListCommand.Value + ":" + guid.ToString()),
                                    InlineKeyboardButton.WithCallbackData(EnterNumbCommand.Key, EnterNumbCommand.Value + ":" + guid.ToString()),
                                },
                            new []
                                {
                                    InlineKeyboardButton.WithCallbackData(BackCommand.Key, BackCommand.Value + ":" + guid.ToString()),
                                    InlineKeyboardButton.WithCallbackData(ExitCommand.Key, ExitCommand.Value + ":" + guid.ToString()),
                                },
                        }
                                            );
            return GetInlineKeyboard;
        }

        internal InlineKeyboardMarkup CreateListInlineKeyboard(Guid guid)
        {
            InlineKeyboardMarkup GetInlineKeyboard
                = new InlineKeyboardMarkup(
                    new[]
                        {
                            new []
                                {
                                    InlineKeyboardButton.WithCallbackData(FirstCommand.Key, FirstCommand.Value + ":" + guid.ToString()),
                                    InlineKeyboardButton.WithCallbackData(PrevCommand.Key, PrevCommand.Value + ":" + guid.ToString()),
                                    InlineKeyboardButton.WithCallbackData(NextCommand.Key, NextCommand.Value + ":" + guid.ToString()),
                                    InlineKeyboardButton.WithCallbackData(LastCommand.Key, LastCommand.Value + ":" + guid.ToString()),
                                },
                            new []
                                {
                                    InlineKeyboardButton.WithCallbackData(BackCommand.Key, BackCommand.Value + ":" + guid.ToString()),
                                    InlineKeyboardButton.WithCallbackData(ExitCommand.Key, ExitCommand.Value + ":" + guid.ToString()),
                                },
                        }
                                            );
            return GetInlineKeyboard;
        }

    }
}

