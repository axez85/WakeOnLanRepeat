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
            minutes = minutes * 60 * 1000;

            LogFile.Startfile();
                
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
            LogFile.RuningEnd();
        }

        private static async void WakeOnLan(object timerState)
        {
            string Mac = ""; //Mac-adress

            if (Mac == "")
                Console.WriteLine("Write you device Mac-address: ");

            LogFile.Append();

            var state = timerState as TimerState;
            Interlocked.Increment(ref state.Counter);

            EasyWakeOnLanClient WOLClient = new EasyWakeOnLanClient();
            await WOLClient.WakeAsync(Mac);
        }

        public class LogFile
        {
            public static string logPath = "last-run-log.txt";
                public static void Startfile()
                {
                    if (File.Exists(logPath))
                        File.Create(logPath).Close();
                }

                public static void Append()
                {
                    stringBuild.Append($"{DateTime.Now:yyyy-MM-d HH:mm:ss}: sending command to server.\n");
                }

                public static void RuningEnd()
                {
                    stringBuild.Append($"{DateTime.Now:yyyy-MM-d HH:mm:ss}: done.");
                    File.AppendAllText(LogFile.logPath, stringBuild.ToString());
                    stringBuild.Clear();
                }

        }

        class TimerState
        {
            public int Counter;
        }

    }
}
