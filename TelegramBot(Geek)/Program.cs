using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot_Geek_
{
    class Program
    {
        static TelegramBotClient Bot;

        static void Main(string[] args)
        {
            Bot = new TelegramBotClient("1247577567:AAE8KYPLeY1KPjRe7B_yKiozZNYjydJ4UcM");

            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;

            var me = Bot.GetMeAsync().Result;

            Console.WriteLine(me.FirstName);

            Bot.StartReceiving();

            Console.ReadLine();

            Bot.StopReceiving();
        }

        private static async void BotOnCallbackQueryReceived(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            string buttonText = e.CallbackQuery.Data;
            string name = $"{e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName}";
            Console.WriteLine($"{name} нажал кнопку {buttonText}");

            await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Ві нажали кнопку {buttonText}");
        }

        private static async void BotOnMessageReceived(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;

            if (message.Type != MessageType.Text)
                return;

            string name = $"{message.From.FirstName} {message.From.LastName}";

            Console.WriteLine($"{name} send message: '{message.Text}'");

            switch (message.Text)
            {
                case "/start":
                    string text =
@"Список команд:
/start - запуск бота
/inline - вывод меню
/keyboard- вывод клавиатуры";
                    await Bot.SendTextMessageAsync(message.From.Id,text);
                    break;
                case "/keyboard":
                    var replyKeyboard = new ReplyKeyboardMarkup(new[] 
                    {
                        new[]
                        {
                            new KeyboardButton("Hello"),
                            new KeyboardButton("How are you")
                        },
                        new[]
                        {
                            new KeyboardButton("Contact") { RequestContact = true},
                            new KeyboardButton("Geolocation") {RequestLocation = true}
                        }
                    });
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Message",
                        replyMarkup: replyKeyboard);
                    break;
                case "/inline":
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("VK", "https://vk.com"),
                            InlineKeyboardButton.WithUrl("FB", "https://fb.com")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Пункт 1"),
                            InlineKeyboardButton.WithCallbackData("Пункт 2")
                        }
                    });
                    await Bot.SendTextMessageAsync(message.From.Id, "Выберите пункт меню", 
                        replyMarkup: inlineKeyboard);
                    break;
                default:
                    
                    break;
            }
        }
    }
}
