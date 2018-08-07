using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.VoiceNext;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.IO;
using System.Diagnostics;
using VideoLibrary;
using YoutubeExplode;
using System.Collections.Generic;

namespace TotallyNotABot
{
    class Program
    {
        static DiscordClient discord;
        static VoiceNextClient voice;
        static CommandsNextModule commands;
        static MyCommands myCommands = new MyCommands();
        //bool EnableDms = false;


        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "NDIwMzMzNjcyNDU4MzU0Njk4.DX9J-A.npSAUCUOYs-vOSJNwHNQTE3gKKw",
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });
            voice = discord.UseVoiceNext();

            myCommands.setdiscord(discord, voice);

            discord.MessageCreated += async e =>
            {
                string message = e.Message.Content.ToLower();

                if (e.Message.Author.Id != 420333672458354698)
                {
                    Console.WriteLine("recieved message : {0}", e.Message);
                }
            };

            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = "!",
                EnableDms = false
            });

            commands.RegisterCommands<MyCommands>();
            //commands.CommandErrored += Commands_CommandErrored;
            
            
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        private static Task Commands_CommandErrored(CommandErrorEventArgs e)
        {
            Console.WriteLine(e);
            throw new NotImplementedException();
        }
        
        
    }

    public class MyCommands
    {

        static DiscordClient discord;
        static VoiceNextClient voice;
        static VoiceNextConnection connection;
        List<YoutubeExplode.Models.Video> list = new List<YoutubeExplode.Models.Video>();
        List<YoutubeExplode.Models.Video> PlayList = new List<YoutubeExplode.Models.Video>();
        Process ffmpeg;
        Queue<YoutubeExplode.Models.Video> QueueList = new Queue<YoutubeExplode.Models.Video>();
        

        public MyCommands()
        {
        }

        public void setdiscord(DiscordClient client, VoiceNextClient Voice)
        {
            discord = client;
            voice = Voice;
        }

        [Command("hi")]
        public async Task Hi(CommandContext ctx)
        {
            await ctx.RespondAsync($"👋 Hi, {ctx.User.Mention}!");
        }

        [Command("search")]
        public async Task Search(CommandContext ctx)
        {
            list.Clear();

            string[] msg = ctx.Message.Content.Split(" ");
            string input = "";
            bool play = false;
            if (msg[1].Equals("-p") && msg.Length != 2)
            {
                play = true;
                input = input + msg[2];
                for(int i = 3; i < msg.Length; i++)
                {
                    input = input + " " + msg[i];
                }
            }
            else
            {
                input = input + msg[1];
                for (int i = 2; i < msg.Length; i++)
                {
                    input = input + " " + msg[i];
                }
            }
            var client = new YoutubeClient();
            var temp = await client.SearchVideosAsync(input, 1);
            for (int i = 0; i < 5; i++)
            {
                list.Add(temp[i]);
            }
            await ctx.RespondAsync($"{String.Join("\n", list)}");
            if (play)
            {
                PlayFromList(1);
                PlayAudio("C:\\discordbot\\video.webm");
            }
        }

        public void PlayFromList(int number)
        {
            string url = "https://www.youtube.com/watch?v=" + list[number-1].Id;
            DownloadURL(url);
        }

        [Command("goodbot")]
        public async Task GoodBot(CommandContext ctx)
        {
            await ctx.RespondAsync($"Thank You {ctx.User.Mention}!");
        }

        //[Command("download")]
        //public async Task Download(CommandContext ctx)
        //{
        //    string[] msg = ctx.Message.Content.Split(" ");
        //    DownloadURL(msg[1]);
        //    await ctx.RespondAsync("👌");
        //}

        public async void DownloadURL(string url)
        {
            var youTube = YouTube.Default; // starting point for YouTube actions
            var video = youTube.GetVideo(url); // gets a Video object with info about the video

            try
            {
                await File.WriteAllBytesAsync(@"C:\discordbot\video.webm", video.GetBytes());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [Command("spam")]
        public async Task Spam(CommandContext ctx)
        {
            string[] msg = ctx.Message.Content.Split(" ");
            if (msg.Length == 1)
            {
                await ctx.Message.RespondAsync($"Spam who? {ctx.User.Mention}");
            }
            else
            {
                if (msg[1].StartsWith("<@"))
                {
                    char[] remove = { '<', '@', '>', '!' };
                    string idstring = msg[1].Trim(remove);
                    ulong id = UInt64.Parse(idstring);

                    DiscordUser user = await discord.GetUserAsync(id);
                    DiscordDmChannel channel = await discord.CreateDmAsync(user);

                    int j = 1;
                    Int32.TryParse(msg[2], out j);
                    if (j < 6 || ctx.Member.IsOwner)
                        for (int i = 0; i < j; i++)
                        {
                            await channel.SendMessageAsync("Spam");
                        }
                    else
                        await ctx.Message.RespondAsync("NO");
                }
                else
                    await ctx.Message.RespondAsync($"Spam who? {ctx.User.Mention}");
            }
        }

        [Command("start")]
        public async Task Start(CommandContext ctx, [RemainingText] string file)
        {
            if (ffmpeg == null)
            {
                CheckQueue();
            }
            else if (ffmpeg.HasExited)
            {
                CheckQueue();
            }
            else
            {
                await ctx.RespondAsync("Music is already being played");
            }
        
        }

        [Command("play")]
        public async Task Play(CommandContext ctx)
        {
            int number;
            string[] msg = ctx.Message.Content.Split(" ");
            if (msg.Length > 1) {
                if (list.Count > 0) {
                    if (int.TryParse(msg[1], out number))
                    {
                        string url = "https://www.youtube.com/watch?v=" + list[number - 1].Id;
                        QueueList.Enqueue(list[number - 1]);
                    }
                    await ctx.RespondAsync($"Added to queue");

                    List<string> templist = new List<string>();

                    foreach (YoutubeExplode.Models.Video item in QueueList)
                    {
                        templist.Add(item.Title);
                    }
                    await ctx.RespondAsync($"{String.Join("\n", templist)}");

                    if (ffmpeg == null)
                        CheckQueue();
                    else if (ffmpeg.HasExited)
                        CheckQueue();
                }
                else
                {
                    await ctx.RespondAsync($"Please use the command !search [name of a song] first");
                }
            }
            else
            {
                await ctx.RespondAsync($"Please use the command !play [1-5] to select a song from the !search command");
            }
        }
            
        public async void PlayAudio(string file)
        {            
            await connection.SendSpeakingAsync(true); // send a speaking indicator
            int number;
            if (int.TryParse(file, out number))
            {
                if (number > 0 && number < list.Count)
                {
                    PlayFromList(number);
                    file = "C:\\discordbot\\video.webm";
                }
            }
            
            var psi = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $@"-i ""{file}"" -ac 2 -f s16le -ar 48000 pipe:1",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            ffmpeg = Process.Start(psi);
            var ffout = ffmpeg.StandardOutput.BaseStream;

            var buff = new byte[3840];
            var br = 0;

            try
            {
                while ((br = ffout.Read(buff, 0, buff.Length)) > 0)
                {
                    if (br < buff.Length) // not a full sample, mute the rest
                        for (var i = br; i < buff.Length; i++)
                            buff[i] = 0;

                    for (int i = 0; i < buff.Length / 2; ++i)
                    {

                        // convert to 16-bit
                        short sample = (short)((buff[i * 2 + 1] << 8) | buff[i * 2]);// scale
                        const double gain = 0.2; // value between 0 and 1.0
                        sample = (short)(sample * gain);

                        // back to byte[]
                        buff[i * 2 + 1] = (byte)(sample >> 8);
                        buff[i * 2] = (byte)(sample & 0xff);
                    }


                    await connection.SendAsync(buff, 20);
                }
                while ((br = ffout.Read(buff, 0, buff.Length)) > 0)
                {
                    if (br < buff.Length) // not a full sample, mute the rest
                        for (var i = br; i < buff.Length; i++)
                            buff[i] = 0;

                    await connection.SendAsync(buff, 20);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                await connection.SendSpeakingAsync(false); // we're not speaking anymore
                CheckQueue();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void CheckQueue()
        {
            if (connection != null)
                if (QueueList.Count > 0)
                {
                    var video = QueueList.Dequeue();                 
                    DownloadURL("https://www.youtube.com/watch?v=" + video.Id);
                    DiscordGame test = new DiscordGame
                    {
                        Name = video.Title,
                        Details = "",
                        State = "playing music"
                    };
                    discord.UpdateStatusAsync(game: test);
                    PlayAudio("C:\\discordbot\\video.webm");
                }
                else
                {
                    discord.UpdateStatusAsync(null);
                }
        }

        [Command("stop")]
        public async Task Stop(CommandContext ctx)
        {
            try
            {
                ffmpeg.Kill();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            await ctx.RespondAsync($"👌");
        }

        [Command("join")]
        public async Task Join(CommandContext ctx)
        {
            if (connection == null)
            {
                try
                {
                    ffmpeg.Kill();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                var channel = ctx.Member.VoiceState.Channel;
                if (channel == null)
                    Console.WriteLine("You need to be in a voice channel.");
                else
                {
                    connection = await voice.ConnectAsync(channel);
                    Console.WriteLine("connection established");
                }
            }
        }

        [Command("leave")]
        public async Task Leave(CommandContext ctx)
        {
            Task task = Stop(ctx);

            var channel = ctx.Member.VoiceState.Channel;
            if (channel == null)
                Console.WriteLine("Not connected in this guild.");

            await ctx.RespondAsync("Bye Bye");
            connection.Disconnect();
            connection = null;
        }

        [Command("ping")]
        public async Task Ping(CommandContext ctx)
        {
            string[] msg = ctx.Message.Content.Split(" ");
            if (msg.Length == 1)
            {
                await ctx.Message.RespondAsync("pong!");
            }
            else
            {
                int j = 1;
                Int32.TryParse(msg[1], out j);
                if (j < 6)
                    for (int i = 0; i < j; i++)
                    {
                        await ctx.Message.RespondAsync("pong!");
                    }
                else
                    await ctx.Message.RespondAsync("NO");
            }
        }
    }
}
