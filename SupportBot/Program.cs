using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SupportBot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SupportBot.Instances;
using SupportBot.Timers;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.InteropServices;

namespace SupportBot
{
    class Program
    {
        static void Main(string[] args)
        {
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);
            var bot = RunBotAsync();
            Console.ReadLine();
        }

        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                ActiveCasesTimer.Stop();
                String json = JsonConvert.SerializeObject(ProgramVariables.guilds);
                StreamWriter sw = new StreamWriter("logs/temp.json");
                sw.Write(json);
                sw.Flush();
                sw.Close();
            }
            return false;
        }
        static ConsoleEventDelegate handler;   // Keeps it from getting garbage collected
                                               // Pinvoke
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);


        private static async Task RunBotAsync()
        {

            ActiveCasesTimer.Setup();
            ProgramVariables._client = new DiscordSocketClient();
            ProgramVariables._commands = new CommandService();
            ProgramVariables._services = new ServiceCollection().AddSingleton(ProgramVariables._client).AddSingleton(ProgramVariables._commands).BuildServiceProvider();
            ProgramVariables._client.Log += BotUtils._client_Log;
            BotUtils.RegisterDisconnectConnect();
            BotUtils.RegisterGuildAvaible();
            BotUtils.RegisterReactionAdded();
            ActiveCasesTimer.Start();
            await BotUtils.RegisterCommandsAsync();
            await ProgramVariables._client.LoginAsync(TokenType.Bot, "ODUyMjU0OTAwNDY1MDQxNDI4.YMEKOg.aOQ1TwTzfzVdt4TWKoWubKeIIc8");
            await ProgramVariables._client.StartAsync();
            await Task.Delay(-1);

        }

    }
}
