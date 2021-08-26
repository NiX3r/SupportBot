using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SupportBot.Timers
{
    class ActiveCasesTimer
    {

        private static Timer timer;

        public static void Setup()
        {
            timer = new Timer(60000);
            timer.Elapsed += OnTimedEvent;
        }

        public static void Start()
        {
            timer.Start();
        }

        public static void Stop()
        {
            timer.Stop();
        }

        private static void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            int cases = 0;
            ProgramVariables.guilds.ForEach(x => cases += x.ActiveCases.Count);
            ProgramVariables._client.SetGameAsync(cases + (cases==1? " case" : " cases"));
        }
    }
}
