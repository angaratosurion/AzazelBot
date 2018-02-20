using Discord;
using Discord.Commands;
using System;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord.Net.Providers.WS4Net;
using BotGear.Tools;
using BotGear.Interfaces;
using System.ComponentModel.Composition;
using BotGear.Attributes;
using BotGear.BotConfiguration;
using System.Text;
using System.IO;
using System.Reflection;

namespace AzazelBot.Core
{
    [Export( typeof(IBot))]
    [BotGearRunnerClass()]
    public  class AzazelBotCore : IBot
    {
       
        public static DateTime startTime = DateTime.Now;
        BotConfigurationCore confcore;
#if DEBUG
        const string token = "MzE5NjE0NTQyNzYwMTE2MjI0.DBDf4w.bpCjpj6Ty8jipAAR7oxCGfu2tC4";
#else

        const string token = "MzE5NjA3NjY2NDE2NjgwOTYw.DBDeHA.-izEleQhjwvTzWhhTQpTHrmvzjg";
#endif
        private  static  DiscordSocketClient client;
    
        //CommandsInitilizer comminit;
       // public static   CommandHandler handler;
        public static AZBCommandHandler handler;
        //private List<Assembly> Plugins = new List<Assembly>();
        CommonTools tls = new CommonTools();
       
       

        public AzazelBotCore()
        {
                   
          

        }
        public async Task Start()
        {




            //



            client.SetGameAsync("Type !help for help");
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
     
        
       

            await Task.Delay(-1);
        }

        public Task Log(LogMessage arg)
        {
            Console.WriteLine("{0} \n {1} \n {2} \n {3}", DateTime.Now.ToLongTimeString(), arg.Message
                , arg.Exception ,arg.Source);
#if DEBUG

#else
            string log = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Logs");
            string filename = startTime.ToLongTimeString().Replace('/', '-').Replace(':', '_') + ".txt";
            string oldcont = "";
            if ( Directory.Exists(log)==false)
            {
                Directory.CreateDirectory(log);

            }
            StringBuilder bld = new StringBuilder();
         
           
            if (File.Exists(filename))
            {
               oldcont = File.ReadAllText(filename);
            }

            bld.AppendLine(oldcont);
            bld.AppendLine(String.Format("{0} \n {1} \n {2} \n {3}", DateTime.Now.ToLongTimeString(), arg.Message
             , arg.Exception, arg.Source));

            string msg = bld.ToString();
            
            File.WriteAllText(Path.Combine(log, filename), msg);

#endif
            return Task.CompletedTask;
        }

        public async Task Stop()
        {
            await client.StopAsync();
         
        }

        public Task Configure()
        {
            
            DiscordSocketConfig config = new DiscordSocketConfig();
            if (CommonTools.IsWindows10() != true)
            {
                config.WebSocketProvider = WS4NetProvider.Instance;
                client = new DiscordSocketClient(config);
            }
            else
            {
                client = new DiscordSocketClient();
            }
            client.Log += Log;
            confcore = new BotConfigurationCore();
            handler = new AZBCommandHandler(null, client, null);
            client = confcore.ConfigureBot(client, handler).Result;

            return Task.CompletedTask;
        }

        





        //public static   void BootStrap()
        //{
        //    try
        //    {
        //        string pluginFolders;
        //        map.Add(client);

        //        var plugins = GetAssemblies();
        //        if ( plugins!=null)
        //        {
        //           foreach( var a in plugins)
        //            {
        //                map.Add(a);
        //            }


        //        }
        //        handler.Install(map);

        //    }
        //    catch (Exception ex)
        //    {

        //        //CommonTools.ErrorReporting(ex);
        //    }
        //}





    }
}
