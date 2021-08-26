using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBot.Instances
{
    class CaseInstance
    {
        public String ChannelName { get; set; }
        public ulong ChannelId { get; set; }
        public String CreatorName { get; set; }
        public ulong CreatorId { get; set; }
        public String Question { get; set; }
        public List<MessageInstance> Messages { get; set; }
        public List<ulong> SendLog { get; set; }

        public CaseInstance(ulong channelId)
        {
            ChannelId = channelId;
        }

    }
}
