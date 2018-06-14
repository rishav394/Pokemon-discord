using System.Timers;
using System.Threading.Tasks;
using System;

namespace Pokemon_discord.Core
{
    public static class RepeatingTimer
    {

        private static Timer timer;
        public static Task StartTimer()
        {
            timer = new Timer()
            {
                Interval = 5000,
                AutoReset = true,
                Enabled = true
            };

            timer.Elapsed += OnTimerTicked;
            return Task.CompletedTask;
        }

        private static void OnTimerTicked(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("ok");
        }
    }
}
