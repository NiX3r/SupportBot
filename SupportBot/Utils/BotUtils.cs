using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SupportBot.Events;
using SupportBot.Instances;
using SupportBot.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SupportBot.Utils
{
    class BotUtils
    {

        public static Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public static void RegisterGuildAvaible()
        {
            ProgramVariables._client.GuildAvailable += HandleGuildAvailableEvent.HandleCommandAsync;
        }

        public static void RegisterReactionAdded()
        {
            ProgramVariables._client.ReactionAdded += HandleReactionAddedEvent.HandleReactionAsync;
        }

        public static void RegisterDisconnectConnect()
        {
            ProgramVariables._client.Disconnected += HandleDisconnetConnectEvent.HandleDisconnect;
            ProgramVariables._client.Connected += HandleDisconnetConnectEvent.HandleConnect;
        }

        public static async Task RegisterCommandsAsync()
        {
            ProgramVariables._client.MessageReceived += HandleCommandAsyncEvent.HandleCommandAsync;
            await ProgramVariables._commands.AddModulesAsync(Assembly.GetEntryAssembly(), ProgramVariables._services);
        }

    }
}
