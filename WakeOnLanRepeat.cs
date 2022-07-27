using EasyWakeOnLan;
using System.Text;

namespace WakeOnLanRepeat
{
    class WakeOnLanRepeat
    {
        private static StringBuilder stringBuild = new StringBuilder();
        private static String Mac = "";

        static void Main(string[] args)
        {
            if (Mac != "")
                RepeatForMinutes(60);
            else
                Console.WriteLine("Mac-address is empty.");
        }

        static void RepeatForMinutes(int minutes = 0)
        {
            var timerState = new TimerState { Counter = 0 };
            int delay = minutes * 60 * 1000;
            delay = delay / 6; 

            LogFile.Startfile();
            var startTime = DateTime.UtcNow;

            while(DateTime.UtcNow - startTime < TimeSpan.FromMinutes(minutes))
            {
                while (timerState.Counter < 6)
                {
                    Interlocked.Increment(ref timerState.Counter);
                    WakeOnLan();
                    Task.Delay(delay).Wait();
                }
            }

            LogFile.RuningEnd();
        }


        private static async void WakeOnLan()
        {
            if (Mac == "") 
                Console.WriteLine("Write you device Mac-address. ");
            else
                LogFile.Append();     
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
            public int Counter = 0;
        }

    }
}
