using System;
using EasyWakeOnLan;

namespace WakeOnLanRepeat
{
    class WakeOnLanRepeat
    {
        static void Main(string[] args)
        {
            RepeatEveryMinutes(10);
         
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
