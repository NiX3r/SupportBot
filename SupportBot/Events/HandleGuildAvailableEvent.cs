using Discord.WebSocket;
using SupportBot.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBot.Events
{
    class HandleGuildAvailableEvent
    {

        public static async Task HandleCommandAsync(SocketGuild arg)
        {

            if (!arg.Name.Contains("nCodes"))
            {
                await arg.LeaveAsync();
                return;
            }

            bool alreadyInList = false;
            ProgramVariables.guilds.ForEach(x => { if (x.GuildID == arg.Id) { alreadyInList = true; return; } });
            if (alreadyInList)
            {
                var guild = ProgramVariables.guilds.FirstOrDefault(x => x.GuildID == arg.Id);
                foreach(CaseInstance nCase in guild.ActiveCases)
                {
                    if (arg.TextChannels.FirstOrDefault(x => x.Id == nCase.ChannelId) == null)
                        guild.ActiveCases.Remove(nCase);
                }
            }
            else
            {
                GuildInstance guild = new GuildInstance(arg.Id, arg.Name);

                if (arg.CategoryChannels.FirstOrDefault(x => x.Name.Equals("\\\\Supporter")) == null)
                    await arg.CreateCategoryChannelAsync("\\\\Supporter", x => { x.Position = 0; });

                if (ProgramVariables.guilds.FirstOrDefault(x => x.GuildID == arg.Id) != null)
                {
                    return;
                }

                guild.ActiveCases = new List<CaseInstance>();
                foreach (SocketRole role in arg.Roles)
                {
                    switch (role.Name)
                    {
                        case @"\\ATMember":
                            guild.AdminRoleId = role.Id;
                            break;
                        case "@everyone":
                            guild.EveryoneRoleId = role.Id;
                            break;
                    }
                }

                ProgramVariables.guilds.Add(guild);
            }

        }

    }
}
