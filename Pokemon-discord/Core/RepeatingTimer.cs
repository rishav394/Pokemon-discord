using System;
using System.Threading.Tasks;
using System.Timers;

namespace Pokemon_discord.Core
{
    public static class RepeatingTimer
    {
        private static Timer _timer;

        public static Task StartTimer()
        {
            _timer = new Timer {Interval = 5000, AutoReset = true, Enabled = true};
            _timer.Elapsed += OnTimerTicked;
            return Task.CompletedTask;
        }

        private static void OnTimerTicked(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("ok");
        }
    }
}