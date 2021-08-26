using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBot.Instances
{
    class GuildInstance
    {

        public ulong GuildID { get; set; }
        public String GuildName { get; set; }
        public ulong EveryoneRoleId { get; set; }
        public ulong AdminRoleId { get; set; }
        public List<CaseInstance> ActiveCases { get; set; }

        public GuildInstance(ulong guildId, String guildName)
        {
            GuildID = guildId;
            GuildName = guildName;
        }

    }
}
