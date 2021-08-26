using Newtonsoft.Json;
using SupportBot.Instances;
using SupportBot.Timers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBot.Events
{
    class HandleDisconnetConnectEvent
    {
        public static async Task HandleDisconnect(Exception ex)
        {

            ActiveCasesTimer.Stop();

            String json = JsonConvert.SerializeObject(ProgramVariables.guilds);
            StreamWriter sw = new StreamWriter("logs/temp.json");
            sw.Write(json);
            sw.Flush();
            sw.Close();

        }

        public static async Task HandleConnect()
        {
            if (!File.Exists("logs/temp.json"))
            {
                ProgramVariables.guilds = new List<GuildInstance>();
                return;
            }

            StreamReader sr = new StreamReader("logs/temp.json");
            String json = sr.ReadToEnd();
            sr.Close();
            ProgramVariables.guilds = JsonConvert.DeserializeObject<List<GuildInstance>>(json);
        }

    }
}
