using BotGear;
using BotGear.Data.Models;
using BotGear.Data.ViewModels;
using BotGear.Interfaces;
using BotGear.Managers;
using BotGear.Tools;
using BotGear.Tools.Bakatsuki;
using BotGear.Tools.Wikipedia;
using Discord;
using Discord.Commands;
using AzazelBot.Core.Data.ViewModels;
using AzazelBot.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BotGear.Tools.ImageDownloader;
using BotGear.Modules.CommandBuilder;

namespace AzazelBot.Core.Modules
{

    [Export(typeof(ModuleBase))]
    public class AzazelBotCommands : ModuleBase
    {
        [Command("Hello")]
        [Summary("Just a Hello World Command")]
        public async Task Hello()
        {
            
            await ReplyAsync("Hello World!");
        }
        [Command("version")]
        [Summary("Shows information about the Bot and it's plugins")]
        public async Task Version()
        {

            var appinf = await Context.Client.GetApplicationInfoAsync();
            Assembly execas = Assembly.GetExecutingAssembly();
            EmptyModuleInfo botinf = (EmptyModuleInfo) BotGearCore.GetAssemblyInfo(execas.GetName().CodeBase);

            string strappinfo = null, invurl = String.Format("https://discordapp.com/api/oauth2/authorize?client_id={0}&scope=bot&permissions=0",
                 Context.Client.CurrentUser.Id);
            StringBuilder str = new StringBuilder();

            strappinfo = String.Format("Application Name : {0} \n Version : {1} \n Description : {2}\n" +
                "Owner:  {3}   Created at : {4} \nSource Code: {5} \n  Invite Url:{6}"
                , appinf.Name, Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                appinf.Description, appinf.Owner, appinf.CreatedAt,botinf.SourceCode ,invurl);

            await ReplyAsync(strappinfo);
        }

        [Command("wiki")]
        [Summary("Searches the wikipedia")]

        public async Task Wiki(string querry)
        {
            try
            {
                WikipediaClient wkclient = new WikipediaClient();

                string arg = querry;
                if (arg != null)
                {
                    string cont = await wkclient.Search(arg);
                    string clncont = Regex.Replace(cont, @"([^a-zA-Z0-9_]|^\s)", string.Empty);
                    string res = String.Format("You asked for  :{0 }\n Answer: \n {1}", arg, cont);
                    //await e.Channel.SendMessage(String.Format("You asked for  :{0}\n",arg));
                    //await e.Channel.SendMessage(String.Format("Answer: \n {0}", cont));
                    string temp = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Temp");
                    if (Directory.Exists(temp) == true)
                    {
                        Directory.Delete(temp, true);
                    }
                    Directory.CreateDirectory(temp);
                    string path = Path.Combine(temp, arg + ".txt");
                    File.WriteAllText(path, cont);
                    Stream fil = File.OpenRead(path);
                    // var stat =await e.Channel.SendMessage(res);
                    if (cont.Length > 2000)
                    {



                        await Context.Channel.SendFileAsync(fil, path, String.Format("{0} You asked for  :{1} " +
                            "\n The answer exceeds the 2000 characters limit so it is saved on a plain text file"
                            , Context.User.Mention, arg));

                        File.Delete(path);

                    }
                    else
                    {
                        await ReplyAsync(res);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

            }

            // Console.WriteLine("Message State : {0} \n Message Text {1}", stat.State.ToString(), stat.Text);

        
        }
        [Command("baka")]
        [Summary("Searches the bakatsuki for information about a noveland manga")]
        public async Task Baka(string querry)
        {


            try
            {
                BakatsukiClient bkclient = new BakatsukiClient();

                string arg = querry;
                if (arg != null)
                {
                    string cont = await bkclient.Search(arg);
                    string clncont = Regex.Replace(cont, @"([^a-zA-Z0-9_]|^\s)", string.Empty);
                    string res = String.Format("You asked for  :{0 }\n Answer: \n {1}", arg, cont);
                    //await e.Channel.SendMessage(String.Format("You asked for  :{0}\n",arg));
                    //await e.Channel.SendMessage(String.Format("Answer: \n {0}", cont));
                    string temp = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Temp");
                    if (Directory.Exists(temp) == true)
                    {
                        Directory.Delete(temp, true);
                    }
                    Directory.CreateDirectory(temp);
                    string path = Path.Combine(temp, arg + ".txt");
                    File.WriteAllText(path, cont);
                    Stream fil = File.OpenRead(path);
                    // var stat =await e.Channel.SendMessage(res);
                    if (cont.Length > 2000)
                    {



                        await Context.Channel.SendFileAsync(fil, path, String.Format("{0} You asked for  :{1} " +
                            "\n The answer exceeds the 2000 characters limit so it is saved on a plain text file"
                            , Context.User.Mention, arg));

                        File.Delete(path);

                    }
                    else
                    {
                        await ReplyAsync(res);
                    }

                }
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("list")]
        [Summary("Shows the existing Channels on the Server")]
        public async Task List()
        {


            try
            {
                if (Context.Guild == null)
                {
                    await ReplyAsync("You are not conencted to Guild . Probably you are on a DM ");
                    return;
                }
                var chans = await Context.Guild.GetChannelsAsync();
                string cms = null;
                if (chans != null)
                {
                    StringBuilder strbld = new StringBuilder();
                    var txtchans = await Context.Guild.GetTextChannelsAsync();
                    if (txtchans != null)
                    {
                        strbld.AppendLine("== Text Cahnnels ===");
                        foreach (var c in txtchans)
                        {
                            strbld.AppendLine(String.Format("Channel Name : {0}", c.Name));
                        }



                    }
                    var vcchans = await Context.Guild.GetVoiceChannelsAsync();
                    if (vcchans != null)
                    {
                        strbld.AppendLine("==  Voice Cahnnels ===");
                        foreach (var c in vcchans)
                        {
                            strbld.AppendLine(String.Format("Channel Name : {0}", c.Name));
                        }
                    }
                    cms = strbld.ToString();
                }


                await ReplyAsync(cms);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }

        }
        [Command("help")]
        [Summary("Shows Help about a command")]
        public async Task Help(string command)
        {


            try
            {
                string help = null;

                var cmd = AzazelBotCore.handler.Commands;
                if (cmd != null && command != null)
                {
                    var dmChannel = Context.Channel;/* A channel is created so that the commands will be privately sent to the user, and not flood the chat. */

                    // var cmds = StrikeWitchesBot.handler.Commands.Commands.ToList();
                    HelpCommand helpCommand = new HelpCommand(cmd);
                    var builder = await helpCommand.Help(Context, command);
                    await dmChannel.SendMessageAsync("", false, builder.Build());
                }




            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }

        }
        [Command("help")]
        [Summary("Shows Help")]
        public async Task Help()
        {

            try
            {
                var dmChannel = Context.Channel;/* A channel is created so that the commands will be privately sent to the user, and not flood the chat. */

                // var cmds = StrikeWitchesBot.handler.Commands.Commands.ToList();
             
                HelpCommand helpCommand = new HelpCommand(AzazelBotCore.handler.Commands);
                var builder = await helpCommand.Help(Context);

                await dmChannel.SendMessageAsync("", false, builder.Build());
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("plugins")]
        [Summary("Shows all the plugins the bot has")]
        public async Task plugins()
        {
            try
            {
                string plugins = "";
                List<IModuleInfo> inf = BotGearCore.GetAllModulesInfo().ToList();
                if (inf != null)
                {
                    StringBuilder strbld = new StringBuilder();
                    foreach (var i in inf)
                    {
                        strbld.AppendLine(String.Format("Name : {0} - {1}\nDescription : \n {2} \n Web Site : {3} \n Source Code : {4}\n",
                            i.Name, i.Version, i.Description, i.WebSite, i.SourceCode));
                    }
                    plugins = strbld.ToString();

                }

                await ReplyAsync(plugins);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("danboorudm")]
        [Summary("Download a  random image from danbooru based on a character you want and get image to a DM ")]
        public async Task danbooruDM(string character)
        {

            try
            {
                string arg = character;
                if (arg != null)
                {

                    DanbooruDownloader don = new DanbooruDownloader();
                    string file = await don.DownloadRandomimage(arg);
                    if ( file == null)
                    {
                        await ReplyAsync(String.Format("Did not find a random image to send you {0} from danbooru (based boards )  for {1}.", Context.User.Mention,character));
                        return;
                    }
                    Stream fil = File.OpenRead(file);
                    var channel = await Context.User.GetOrCreateDMChannelAsync();
                    if (channel != null)
                    {
                        await channel.SendFileAsync(fil, file, String.Format("{0} Sending you a random image for : {1} from danbooru (based boards ) "
                          , Context.User.Mention, arg));
                    }
                    File.Delete(file);


                }
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }

        [Command("danbooru")]
        [Summary("Download a  random image from danbooru (based boards )  based on a character you want ")]
        [RequireNsfw]
        public async Task danbooru(string character)
        {

            try
            {
                string arg = character;
                if (arg != null)
                {

                    DanbooruDownloader don = new DanbooruDownloader();
                    string file = await don.DownloadRandomimage(arg);
                    if (file == null)
                    {
                        await ReplyAsync(String.Format("Did not find a random image to send you {0} from danbooru (based boards )  for {1}.", Context.User.Mention, character));
                        return;
                    }
                    Stream fil = File.OpenRead(file);
                   
                        await Context.Channel.SendFileAsync(fil, file, String.Format("{0} got  a random image for : {1} from danbooru (based boards ) "
                          , Context.User.Mention, arg));
                   
                    File.Delete(file);


                }
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("danboorurnd")]
        [Summary("Download a  random image from danbooru (based boards )  based on a character you want  and sent it to a random user")]
        [RequireNsfw]
        public async Task danbooruRnd(string character)
        {

            try
            { 
            string arg = character;
                if (arg != null)
                {
                    Stream fil=null;
                      int i=0;
                    Random rnd = new Random();
                    DanbooruDownloader don = new DanbooruDownloader();
                    string file = await don.DownloadRandomimage(arg);
                    if (file != null)
                    {
                       fil = File.OpenRead(file);
                    }
                    IAsyncEnumerable<IReadOnlyCollection<IUser>> tusers = Context.Channel.GetUsersAsync(CacheMode.AllowDownload);
                    var susers = tusers.ToList();


                    List<IUser> ausers = tusers.Flatten().Result.ToList();
                    List<IUser> users = ausers.FindAll(x => x.IsBot == false);
                    i = rnd.Next(users.Count);
                    IUser user = users[i];
                    if (fil != null)
                    {


                        var channel = await user.GetOrCreateDMChannelAsync();
                        if (channel != null )
                        {
                            await ReplyAsync(String.Format("Sending a random image for : {0} from danbooru (based boards )   to {1}", arg, user.Mention));


                            await channel.SendFileAsync(fil, file, String.Format("{0} Get a random image for : {0} from danbooru (based boards ) "
                                 , user.Mention, arg));

                        }

                        ///CreatePrivateChannelAsync(user.Id).Result.SendFile(file);
                        // await e.Channel.SendFile(file);
                    }
                   File.Delete(file);
                }
                


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }

        [Command("uptime")]
        [Summary("Shows the bot's uptime ")]
        public async Task Uptime()
        {

            try
            {
                var delta = DateTime.Now - AzazelBotCore.startTime;

                await ReplyAsync(string.Format("Current Uptime: Days: {0} Hours: {1} Minutes: {2} seconds:{3}"
                    , delta.Days.ToString("n0"), delta.Hours.ToString("n0"), Math.Floor((decimal)delta.Minutes).ToString("n0"),delta.Seconds.ToString("n0")));
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("addfavoritechars")]
        [Summary("Adds favorite characters . Just seperate them with commas. ")]
        public async Task  AddfavoriteChars(string chars)
        {
            try
            {
                if (chars != null)
                {
                    UserManager urmngr = new UserManager();
                    
                    var usr = await urmngr.GetUserbyId(Convert.ToString(Context.User.Id));
                    FavoriteCharactersManager fvmngr = new FavoriteCharactersManager();

                     if (usr!=null)
                    {
                        
                        await fvmngr.AddFavorite(usr.Id, chars, Convert.ToString(Context.Guild.Id));
                        await ReplyAsync(Context.User.Mention + "You succesfully registered yor favorite Character/s");

                    }
                    else
                    {
                        await ReplyAsync(Context.User.Mention + "You arre nto registered with this server");
                    }

                    
                }

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }

        }
        [Command("editfavoritechars")]
        [Summary("Edits your favorite character . ")]
        public async Task EditfavoriteChars(int id ,string char2)
        {
            try
            {
                if (id>0 && char2 !=null)
                {
                    UserManager urmngr = new UserManager();
                    var usr = await urmngr.GetUserbyId(Convert.ToString(Context.User.Id));
                    FavoriteCharactersManager fvmngr = new FavoriteCharactersManager();
                    if (usr != null)
                    {
                        await fvmngr.EditFavorite(Convert.ToString(usr.Id),id,char2);
                        await ReplyAsync(Context.User.Mention + "You modified  yor favorite Character/s");

                    }
                    else
                    {
                        await ReplyAsync(Context.User.Mention + "You are not registered with this server");
                    }


                }

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }

        }

        [Command("delfavoritechars")]
        [Summary( "deletes a favoritre character with the given id number")]
        public async Task RemovefavoriteChars(int  id)
        {
            try
            {
                if (id>0)
                {
                    UserManager urmngr = new UserManager();
                    var usr = await urmngr.GetUserbyId(Convert.ToString(Context.User.Id));
                    FavoriteCharactersManager fvmngr = new FavoriteCharactersManager();
                    if (usr != null)
                    {
                         await fvmngr.DelFavoritesByid(Convert.ToString(usr.Id), id);
                        await ReplyAsync(Context.User.Mention + "You succesfully removed  yor favorite Character/s");

                    }
                    else
                    {
                        await ReplyAsync(Context.User.Mention + "You arre nto registered with this server");
                    }


                }

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }

        }
        [Command("getfavoritechars")]
        [Summary("Registers a user to the database. ")]
        public async Task GetfavoriteChars()
        {
            try
            {
               
                    UserManager urmngr = new UserManager();
                    var usr = await urmngr.GetUserbyId(Convert.ToString(Context.User.Id));
                    FavoriteCharactersManager fvmngr = new FavoriteCharactersManager();
                    if (usr != null)
                    {
                      var favs=fvmngr.GetUserFavorite(usr.Id);


                    string msg = "";

                    StringBuilder bld = new StringBuilder();
                   

                     if ( favs !=null)
                    {
                        bld.AppendLine(String.Format("{0} your favorite characters : ", Context.User.Mention));


                        foreach( var f in favs)
                        {
                            bld.AppendLine(String.Format("Id:{0}", f.Id));
                            bld.AppendLine(String.Format("Name :{0}",Format.Sanitize(f.CharacterName)));
                            bld.AppendLine(String.Format("Date :{0} ", f.AddedAt.ToLongDateString()));
                            ViewFavoriteCharacter vchar = new ViewFavoriteCharacter();
                           vchar.ImportModel(f);
                            bld.AppendLine(String.Format("ServerName :{0} ", vchar.ServerName));

                        }
                       // var t = bld.ToString();
                        msg = bld.ToString();
                       // await ReplyAsync(Context.User.Mention + msg);
                        var channel = await Context.User.GetOrCreateDMChannelAsync();
                        if (channel != null)
                        {
                            await channel.SendMessageAsync(msg);
                        }
                    }
                     else
                    {
                        await ReplyAsync(Context.User.Mention + "You dont have added favorite characters");
                    }

                    

                    }
                    else
                    {
                        await ReplyAsync("You are not registered with this server or you dont have added favorite characters");
                    }


                }

            
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }

        }

        [Command("register")]
        [Summary("Registers a user to the database. ")]
        public async Task Register(string birthday)
        {
            try
            {
                if ( birthday!=null)
                {
                    DateTime date = DateTime.Parse(birthday);
                    UserManager urmngr = new UserManager();
                    await urmngr.addUser(Context.User, date,Context.Guild);
                   await  ReplyAsync(Context.User.Mention +"You succesfully registered with the server");
                }
                
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }

        }

        [Command("unregister")]
        [Summary("UnRegisters a user to the database. ")]
        
        public async Task UnRegister( )
        {
            try
            {
              
                    
                    UserManager urmngr = new UserManager();
                ServerManager srvmngr = new ServerManager();
                    FavoriteCharactersManager favmngr = new FavoriteCharactersManager();
                    await favmngr.DelFavorites(Convert.ToString(Context.User.Id));
                await srvmngr.deleteMemberfromServer(Context.Guild, Context.User);

                    await ReplyAsync("You succesfully unregistered with the server");
              

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }

        }
        [Command("unregisterbot")]
        [Summary("UnRegisters a user to the database. ")]
        [RequireOwner]

        public async Task UnRegisterBot()
        {
            try
            {


                UserManager urmngr = new UserManager();

                FavoriteCharactersManager favmngr = new FavoriteCharactersManager();
                await favmngr.DelFavorites(Convert.ToString(Context.User.Id));
                await urmngr.DeleteUser(Context.User);

                await ReplyAsync("You succesfully unregistered with the server");


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }

        }

        [Command("listusers")]
        [Summary("getusers. ")]
        [RequireUserPermission(GuildPermission.Administrator)]
        

        public async Task GetUserList()
        {
            try
            {
                string msg = "";

                UserManager usrmngr = new UserManager();
               var tusers= usrmngr.GetUsers();
                ModuleConverter conv = new ModuleConverter();
                BotGearServer srv = conv.IGuildToBotGearServer(Context.Guild);
                ServerManager srvMngr = new ServerManager();
                if (tusers != null && srv != null)
                {


                    var users = await srvMngr.getServerMembersbyServerId(srv.Id);
                    if (users != null)
                    {
                        StringBuilder bld = new StringBuilder();
                        bld.AppendLine("Registered users with the bot in server : " + Context.Guild.Name);
                        foreach (var c in users)
                        {
                            bld.AppendFormat("\nId :{0}", c.Id);
                            bld.AppendFormat("\nUserName :{0}", c.Username);
                            bld.AppendFormat("\nBirthday  :{0}", c.Birthday);
                            bld.AppendFormat("\nCreated at :{0}", c.CreatedAt);
                            bld.AppendFormat("\nRegisted at: :{0}", c.RegisteredAt);
                            bld.AppendFormat("\nDiscriminator :{0}", c.Discriminator);
                            bld.AppendFormat("\nDiscriminator Value :{0}", c.DiscriminatorValue);

                            bld.AppendLine();
                        }
                        msg = bld.ToString();
                    }
                }
                else
                {
                    StringBuilder bld = new StringBuilder();
                    bld.AppendLine("Registered users with the bot in server : " + Context.Guild.Name);
                    bld.AppendLine("There are no users  ");
                    msg = bld.ToString();
                }
               

                var channel = await Context.User.GetOrCreateDMChannelAsync();
                    if (channel != null)
                    {
                        await channel.SendMessageAsync(msg);
                    }
                
                
                //await ReplyAsync(msg);


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }

        }
        [Command("listbotusers")]
        [Summary("getbot users. ")]
        [RequireOwner]
        public async Task GetBotUserList()
        {
            try
            {
                string msg = "";

                UserManager usrmngr = new UserManager();

                var users = usrmngr.GetUsers();
                if (users != null)
                {
                    StringBuilder bld = new StringBuilder();
                    bld.AppendLine("Registered users with the bot");
                    foreach (var c in users)
                    {
                        bld.AppendFormat("\nId :{0}", c.Id);
                        bld.AppendFormat("\nUserName :{0}", c.Username);
                        bld.AppendFormat("\nBirthday  :{0}", c.Birthday);
                        bld.AppendFormat("\nCreated at :{0}", c.CreatedAt);
                        bld.AppendFormat("\nRegisted at: :{0}", c.RegisteredAt);
                        bld.AppendFormat("\nDiscriminator :{0}", c.Discriminator);
                        bld.AppendFormat("\nDiscriminator Value :{0}", c.DiscriminatorValue);
                        bld.AppendLine("It's Registered in servers:");
                        bld.AppendLine();
                        BotGearViewUser vuser = new BotGearViewUser();
                        vuser.ImportFromModel(c);
                        var servs = vuser.Servers;
                        if (servs != null)
                        {
                            foreach (var s in servs)
                            {
                                bld.AppendFormat("\nServer Id : {0}",s.Id);
                                bld.AppendFormat("\nServer Name : {0}", s.Name);
                                bld.AppendFormat("\nOwner Id : {0}", s.OwnerId);
                                bld.AppendFormat("\nCreated At: {0}", s.CreatedAt);
                                bld.AppendLine();


                            }
                        }
                        else
                        {
                            bld.AppendLine("User is registered in no servers");
                        }

                      bld.AppendLine();
                    }
                    msg = bld.ToString();
                }
                else
                {
                    StringBuilder bld = new StringBuilder();
                    bld.AppendLine("Registered users with the bot");
                    bld.AppendLine("There are no users  ");
                    msg = bld.ToString();
                }

                var channel = await Context.User.GetOrCreateDMChannelAsync();
                if (channel != null)
                {
                    await channel.SendMessageAsync(msg);
                }
                //await ReplyAsync(msg);


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }

        }


    }
}
