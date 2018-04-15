using BotGear.Managers;
using BotGear.Modules;
using BotGear.Tools;
using Discord.WebSocket;
using AzazelBot.Core.Managers;
using System;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using BotGear.Tools.ImageDownloader;

namespace AzazelBot.Core
{
    public class AZBCommandHandler : CommandHandler
    {
        public AZBCommandHandler(IServiceProvider provider, DiscordSocketClient discord, CommandService tcommands) : base(provider, discord, tcommands)
        {
            
        }

        public override Task UserJoined(SocketGuildUser user)
        {
            try
            {
                base.UserJoined(user);
                //var channel = user.Guild.DefaultChannel;
                //if (channel != null && user.IsBot ==false)
                //{
                //    //channel.SendMessageAsync(String.Format("{0} Type !help for help ", user.Mention));
                //}
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return Task.CompletedTask;
            }
        }
        public override async Task GuildMemberUpdated(SocketGuildUser arg1, SocketGuildUser arg2)
        {
            try
            {
               await  base.GuildMemberUpdated(arg1, arg2);


                if (arg1.Status != arg2.Status && arg1.IsBot == false && arg2.IsBot == false)
                {
                    if (arg1.Status != Discord.UserStatus.Online && arg2.Status == Discord.UserStatus.Online)
                    // && arg2.Status!= Discord.UserStatus.DoNotDisturb && arg2.)
                    {
                        
                        UserManager usermngr = new UserManager();
                        var user = await usermngr.GetUserbyId(Convert.ToString(arg1.Id));
                        if (user != null)
                        {
                            DateTime date = DateTime.Now;
                            var birth = user.Birthday;
                            if (date.Day == birth.Day && date.Month == birth.Month)
                            {
                                FavoriteCharactersManager fvmngr = new FavoriteCharactersManager();
                                var fchar = fvmngr.GetUserFavorite(Convert.ToString(user.Id));
                                if (fchar != null)
                                {
                                    int i = 0;
                                    Random rnd = new Random();
                                    i = rnd.Next(fchar.Count - 1);
                                    if (i > 0)
                                    {
                                        DanbooruDownloader san = new DanbooruDownloader();
                                        string file = await san.DownloadRandomimage(fchar[i].CharacterName);
                                        var dmchannel = await arg1.GetOrCreateDMChannelAsync();
                                        if (dmchannel != null)
                                        {

                                            if (file == null)
                                            {
                                                //await dmchannel.SendMessageAsync(String.Format("Did not find a random image to send you {0} from danbooru complex for {1}.",
                                                //    arg2.Mention, fchar[i].CharacterName));
                                                return;
                                            }
                                            Stream fil = File.OpenRead(file);

                                            await dmchannel.SendFileAsync(fil, file, String.Format("{0} Happy Birthday ! Sending you a random image for : {1} from danbooru (based boards )  as a present!"
                                              , arg2.Mention, fchar[i].CharacterName));
                                        }
                                        File.Delete(file);

                                    }

                                }
                            }
                        }


                    }
                }




                //return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                //return Task.CompletedTask;
            }

        }
    }
}
