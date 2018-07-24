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
using BotGear.Data.JSON;
using Newtonsoft.Json;

namespace AzazelBot.Core
{
    [Export( typeof(IBot))]
    [BotGearRunnerClass()]
    public  class AzazelBotCore : IBot
    {
       
        public static DateTime startTime = DateTime.Now;
        BotConfigurationCore confcore;

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



            client.SetGameAsync("Type !?help for help");
            string token;
          
#if DEBUG
            token = Path.Combine(System.Windows.Forms.Application.StartupPath, "config_debug.token");
#else
  token = Path.Combine(System.Windows.Forms.Application.StartupPath, "config.token");

#endif
            var tokar = File.ReadAllLines(token);
            string tok="";
            foreach(var t in tokar)
            {
                tok += t;
            }
            BotToken bttoken = JsonConvert.DeserializeObject<BotToken>(tok);
          await client.LoginAsync(TokenType.Bot, bttoken.Token);
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
            client = confcore.ConfigureBot(client, handler,this).Result;

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
