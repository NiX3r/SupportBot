using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SupportBot.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBot
{
    class ProgramVariables
    {

        public static DiscordSocketClient _client { get; set; }
        public static CommandService _commands { get; set; }
        public static IServiceProvider _services { get; set; }
        public static List<GuildInstance> guilds { get; set; }

    }
}
