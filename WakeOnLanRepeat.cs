using System;
using EasyWakeOnLan;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.IO;

namespace WakeOnLanRepeat
{
    class WakeOnLanRepeat
    {
        private static Timer timer;
        private static StringBuilder stringBuild = new StringBuilder();

        static void Main(string[] args)
        {
            RepeatEveryMinutes(1);
        
        }

        static void RepeatEveryMinutes(int minutes)
        {
            var timerState = new TimerState { Counter = 0 };
            var logPath = "runtime-log.txt";
            minutes = minutes * 60 * 1000;

            if (File.Exists(logPath))
                File.Create(logPath).Close();
                
            timer = new Timer(
                callback: new TimerCallback(WakeOnLan),
                state: timerState,
                dueTime: 1000,
                period: minutes);

            while (timerState.Counter < 6)
            {
                Task.Delay(1000).Wait();
            }

            timer.Dispose();
            stringBuild.Append($"{DateTime.Now:yyyy-MM-d HH:mm:ss}: done.");
            File.AppendAllText(logPath, stringBuild.ToString());
            stringBuild.Clear();
        }

        private static async void WakeOnLan(object timerState)
        {
            string Mac = ""; //Mac-adress

            stringBuild.Append($"{DateTime.Now:yyyy-MM-d HH:mm:ss}: sending command to server.\n");

            var state = timerState as TimerState;
            Interlocked.Increment(ref state.Counter);

            EasyWakeOnLanClient WOLClient = new EasyWakeOnLanClient();
            await WOLClient.WakeAsync(Mac);
        }

        class TimerState
        {
            public int Counter;
        }

    }
}
