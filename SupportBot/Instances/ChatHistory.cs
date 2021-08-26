using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBot.Instances
{
    class ChatHistory
    {

        public String ChannelName { get; set; }
        public ulong ChannelID { get; set; }
        public List<MessageInstance> Messages { get; set; }
        public String Question { get; set; }

        public ChatHistory(String Name, ulong ID, String question)
        {
            ChannelName = Name;
            ChannelID = ID;
            Question = question;
        }

    }
}
