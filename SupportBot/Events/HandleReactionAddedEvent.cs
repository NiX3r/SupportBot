using Discord;
using Discord.WebSocket;
using SupportBot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBot.Events
{
    class HandleReactionAddedEvent
    {

        private static Emoji log = new Emoji("\U0001F4C4");
        private static Emoji close = new Emoji("\U0000274C");

        public static async Task HandleReactionAsync(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel originChannel, SocketReaction reaction)
        {
            if (reaction.User.Value.IsBot)
                return;

            if (reaction.Channel.Name.Contains("supporter-"))
            {
                // log emoji
                if(reaction.Emote.Equals(log))
                {
                    List<ulong> logs = ProgramVariables.guilds.FirstOrDefault(x => x.GuildID == ProgramVariables._client.Guilds
                                     .FirstOrDefault(y => y.TextChannels.Contains(reaction.Channel)).Id).ActiveCases
                                    .FirstOrDefault(x => x.ChannelId == reaction.Channel.Id).SendLog;

                    if (!logs.Contains(reaction.UserId))
                    {
                        logs.Add(reaction.UserId);
                    }
                    else
                    {
                        logs.Remove(reaction.UserId);
                    }
                }
                // close emoji
                if(reaction.Emote.Equals(close))
                {
                    int counter = reaction.Channel.GetMessageAsync(reaction.MessageId).Result.Reactions.FirstOrDefault(x => x.Key.Equals(close)).Value.ReactionCount;
                    if(counter == 3)
                    {
                        SocketGuild guild = ProgramVariables._client.Guilds.FirstOrDefault(x => x.TextChannels.Contains(reaction.Channel));
                        ArchiveSupport.Archive(guild.Id, reaction.Channel.Id);
                        await guild.TextChannels.FirstOrDefault(x => x.Id == reaction.Channel.Id).DeleteAsync();
                    }
                }
            }

        }
    }
}
