using System;
using EasyWakeOnLan;
using System.Threading;
using System.Threading.Tasks;

namespace WakeOnLanRepeat
{
    class WakeOnLanRepeat
    {
        private static Timer timer;

        static void Main(string[] args)
        {
            RepeatEveryMinutes(10);
         
        }

        static void RepeatEveryMinutes(int minutes)
        {
            var timerState = new TimerState { Counter = 0 };
            minutes = minutes * 60 * 1000; 

            timer = new Timer(
                callback: new TimerCallback(TimerTask),
                state: timerState,
                dueTime: 1000,
                period: minutes);

            while (timerState.Counter < 6)
            {
                Task.Delay(1000).Wait();
            }

            timer.Dispose();
            Console.WriteLine($"{DateTime.Now:yyyy-MM-d HH:mm:ss}: done.");
        }

        private static void TimerTask(object timerState)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-d HH:mm:ss}: starting a new callback.");
            var state = timerState as TimerState;
            Interlocked.Increment(ref state.Counter);
        }

        class TimerState
        {
            public int Counter;
        }


        static void test()
        {
            Console.WriteLine("Running... " + DateTime.Now);
        }

        static async void WakeOnLan()
        {
            string Mac = "1C:1B:0D:95:F2:67";
            EasyWakeOnLanClient WOLClient = new EasyWakeOnLanClient();
            await WOLClient.WakeAsync(Mac);
        }

    }
}
