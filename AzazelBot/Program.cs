using AzazelBot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace AzazelBot
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        static  void Main(string[] args)
        {
            var handle = GetConsoleWindow();

#if DEBUG
          ShowWindow(handle, SW_SHOW);
#else
            ShowWindow(handle, SW_HIDE);
#endif
            AzazelBotCore bot = new AzazelBotCore();
             bot.Configure();
           // if (bot.Start().GetAwaiter() != null)
            {
               bot.Start().GetAwaiter().GetResult();
            }
            //Console.ReadLine();



        }
    }
}
