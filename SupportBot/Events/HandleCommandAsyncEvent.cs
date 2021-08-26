using Discord.Commands;
using Discord.WebSocket;
using SupportBot.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBot.Events
{
    class HandleCommandAsyncEvent
    {
        public static async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(ProgramVariables._client, message);
            if (message.Author.IsBot) return;

            int argPos = 0;
            if (message.HasStringPrefix("nc?", ref argPos))
            {
                var result = await ProgramVariables._commands.ExecuteAsync(context, argPos, ProgramVariables._services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
                if (result.Error.Equals(CommandError.UnmetPrecondition)) await message.Channel.SendMessageAsync(result.ErrorReason);
            }
            else if (message.Channel.Name.Contains("supporter-"))
            {
                foreach (GuildInstance guild in ProgramVariables.guilds)
                {
                    foreach (CaseInstance nCase in guild.ActiveCases)
                    {
                        if (nCase.ChannelId == message.Channel.Id && nCase.ChannelName == message.Channel.Name)
                        {
                            nCase.Messages.Add(new MessageInstance(message.Author.Username, message.CreatedAt.UtcDateTime.AddHours(2), message.Content));
                        }
                    }
                }
            }
        }
    }
}
