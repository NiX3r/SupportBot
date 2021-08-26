using Discord;
using Discord.WebSocket;
using SupportBot.Instances;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBot.Utils
{
    class ArchiveSupport
    {

        public static void Archive(ulong guildId, ulong channelId)
        {
            
            GuildInstance guild = ProgramVariables.guilds.First(x => x.GuildID == guildId);
            CaseInstance nCase = guild.ActiveCases.First(x => x.ChannelId == channelId);

            if (guild == null || nCase == null)
                return;

            if (!Directory.Exists("logs/" + guild.GuildName))
                Directory.CreateDirectory("logs/" + guild.GuildName);

            String filePath = "logs/" + guild.GuildName + "/" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm") + "_" + nCase.ChannelName + ".txt";
            StreamWriter sw = new StreamWriter(filePath);

            sw.WriteLine($"Question: {nCase.Question} | Author: {nCase.CreatorName} | Author ID: {nCase.CreatorId} | Channel: {nCase.ChannelName} | Channel ID: {nCase.ChannelId}");
            foreach(MessageInstance msg in nCase.Messages)
            {
                sw.Write("\n[" + msg.CreatedAt.ToString("HH:mm dd.MM.yyyy") + "] " + msg.Author + " >> " + msg.Content);
            }
            sw.Flush();
            sw.Close();

            List<EmbedFieldBuilder> embeds = new List<EmbedFieldBuilder>();

            nCase.Messages.ForEach(x => {
                EmbedFieldBuilder e = new EmbedFieldBuilder();
                e.Name = $"[{x.CreatedAt.ToString("HH:mm dd.MM.yyyy")}] {x.Author}";
                e.Value = x.Content;
                e.IsInline = false;
                embeds.Add(e);
            });

            var EmbedBuilder = new EmbedBuilder()
                .WithTitle($"Support log")
                .WithColor(Color.DarkPurple)
                .WithThumbnailUrl("https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fpngimg.com%2Fuploads%2Fquestion_mark%2Fquestion_mark_PNG134.png&f=1&nofb=1")
                .WithTimestamp(DateTime.Now)
                .WithFields(embeds)
                .WithFooter(footer =>
                {
                    footer
                    .WithText("Supporter by nCodes\n" +
                              "Question: " + nCase.Question + "\n" +
                              "Author: " + nCase.CreatorName + "\n" +
                              "Author ID: " + nCase.CreatorId + "\n" +
                              "Channel: " + nCase.ChannelName + "\n" +
                              "Channel ID: " + nCase.ChannelId);
                });
            Embed embed = EmbedBuilder.Build();

            nCase.SendLog.ForEach( x => {
                Task t = SendPrivateMessages(guild.GuildID, x, embed);
            });

            guild.ActiveCases.Remove(nCase);

        }

        private static async Task SendPrivateMessages(ulong guildId, ulong userId, Embed embed)
        {
            await ProgramVariables._client.GetGuild(guildId).GetUser(userId).SendMessageAsync(embed: embed);
        }

    }
}
