using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using SupportBot.Instances;
using SupportBot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SupportBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {

        [Command("ping"), RequireUserPermission(GuildPermission.Administrator)]
        public async Task Ping()
        {

            EmbedFieldBuilder close = new EmbedFieldBuilder();
            close.Name = "Processor";
            close.Value = HardwareUsage.getCurrentCpuUsage();
            close.IsInline = true;

            EmbedFieldBuilder log = new EmbedFieldBuilder();
            log.Name = "RAM";
            log.Value = HardwareUsage.getAvailableRAM();
            log.IsInline = true;

            EmbedFieldBuilder guilds = new EmbedFieldBuilder();
            guilds.Name = "Guilds";
            guilds.Value = ProgramVariables._client.Guilds.Count;
            guilds.IsInline = false;

            EmbedFieldBuilder latency = new EmbedFieldBuilder();
            latency.Name = "Latency";
            latency.Value = ProgramVariables._client.Latency;
            latency.IsInline = true;
            
            float cpu = (float)Convert.ToDecimal(close.Value.ToString().Replace("%", ""));

            var EmbedBuilder = new EmbedBuilder()
                .WithTitle($"Server response")
                .WithColor(cpu < 50.0 ? Color.Green : Color.Red)
                .WithThumbnailUrl(cpu < 50.0 ? "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi.ytimg.com%2Fvi%2FgTrvi5MI9NY%2Fmaxresdefault.jpg&f=1&nofb=1" : "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi.imgflip.com%2F4qbswj.jpg&f=1&nofb=1")
                .WithTimestamp(DateTime.Now)
                .WithFields(close, log, guilds, latency);
            Embed embed = EmbedBuilder.Build();
            await Context.Channel.SendMessageAsync(embed: embed);
            return;
        }

        [Command("help")]
        public async Task Test([Remainder] string reason = "")
        {
            
            foreach (GuildInstance guild in ProgramVariables.guilds)
            {
                if(guild.GuildID == Context.Guild.Id)
                {
                    if (guild.ActiveCases.FirstOrDefault(x => x.CreatorId == Context.User.Id) != null)
                    {
                        await ReplyAsync(Context.User.Mention + ", you already have a support request!");
                        return;
                    }

                    RestTextChannel ch = await Context.Guild.CreateTextChannelAsync("supporter-" + Context.User.Username.ToLower(), x =>
                    {
                        OverwritePermissions permission = new OverwritePermissions(117760, 1);
                        Overwrite user = new Overwrite(Context.User.Id, PermissionTarget.User, permission);
                        List<Overwrite> overwrites = new List<Overwrite>();
                        overwrites.Add(user);
                        user = new Overwrite(guild.AdminRoleId, PermissionTarget.Role, permission);
                        overwrites.Add(user);
                        permission = new OverwritePermissions(0, 117760);
                        user = new Overwrite(guild.EveryoneRoleId, PermissionTarget.Role, permission);
                        overwrites.Add(user);
                        IEnumerable<Overwrite> temp = overwrites;
                        x.CategoryId = Context.Guild.CategoryChannels.FirstOrDefault(y => y.Name.Equals("\\\\Supporter")).Id;
                        x.Topic = reason;
                        x.PermissionOverwrites = new Optional<IEnumerable<Overwrite>>(temp);
                    });
                    CaseInstance sCase = new CaseInstance(ch.Id);
                    sCase.ChannelName = "supporter-" + Context.User.Username.ToLower();
                    sCase.CreatorId = Context.User.Id;
                    sCase.CreatorName = Context.User.Username;
                    sCase.Question = reason;
                    sCase.Messages = new List<MessageInstance>();
                    sCase.SendLog = new List<ulong>();
                    guild.ActiveCases.Add(sCase);

                    EmbedFieldBuilder close = new EmbedFieldBuilder();
                    close.Name = "For close";
                    close.Value = "emote with :x:";
                    close.IsInline = true;

                    EmbedFieldBuilder log = new EmbedFieldBuilder();
                    log.Name = "For send log";
                    log.Value = "emote with :page_facing_up:";
                    log.IsInline = true;

                    var EmbedBuilder = new EmbedBuilder()
                        .WithTitle($"Please insert some more details to your problem!")
                        .WithColor(Color.Purple)
                        .WithThumbnailUrl("https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fpngimg.com%2Fuploads%2Fquestion_mark%2Fquestion_mark_PNG134.png&f=1&nofb=1")
                        .WithTimestamp(DateTime.Now)
                        .WithFields(close, log)
                        .WithFooter(footer =>
                        {
                            footer
                            .WithText("Supporter by nCodes\n" +
                                      "Question: " + sCase.Question + "\n" +
                                      "Author: " + sCase.CreatorName + "\n" +
                                      "Author ID: " + sCase.CreatorId + "\n" +
                                      "Channel: " + sCase.ChannelName + "\n" +
                                      "Channel ID: " + sCase.ChannelId);
                        });
                    Embed embed = EmbedBuilder.Build();
                    RestUserMessage msg = await ch.SendMessageAsync(embed: embed);
                    //await ch.SendMessageAsync(Context.User.Mention + " " + Context.Guild.GetRole(guild.AdminRoleId).Mention);
                    await msg.AddReactionAsync(new Emoji("\U0000274C"));
                    await msg.AddReactionAsync(new Emoji("\U0001F4C4"));
                    return;
                }
            }

            await ReplyAsync(Context.User.Mention + ", bot is not ready for use!");

        }

        [Command("stop"), RequireUserPermission(GuildPermission.Administrator)]
        public async Task Stop()
        {

            if (!Context.Channel.Name.Contains("supporter-"))
                return;
            ArchiveSupport.Archive(Context.Guild.Id, Context.Channel.Id);
            await Context.Guild.TextChannels.FirstOrDefault(x => x.Id == Context.Channel.Id).DeleteAsync();

        }

        [Command("at"), RequireUserPermission(GuildPermission.Administrator)]
        public async Task AT(ulong newAT)
        {
            var guild = ProgramVariables.guilds.FirstOrDefault(x => x.GuildID == Context.Guild.Id);
            guild.AdminRoleId = newAT;
            await ReplyAsync(Context.User.Mention + ", you successfully set new role as administrators!");
        }

    }
}
