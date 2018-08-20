using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using TotallyNotABot.commands;

namespace TotallyNotABot.audio
{
    class Audio2
    {
        public Process ffmpeg { get; set; }

        public async Task StreamAudio(string file)
        {
            // send a speaking indicator
            await Commands.Connection.SendSpeakingAsync();

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $@"-i ""{file}"" -ac 2 -f s16le -ar 48000 pipe:1",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            ffmpeg = Process.Start(psi);
            if (ffmpeg == null)
            {
                throw new Exception("Couldn't start ffmpeg with file: " + file);
            }
            Stream ffout = ffmpeg.StandardOutput.BaseStream;

            byte[] buff = new byte[3840];

            try
            {
                int br;
                while ((br = ffout.Read(buff, 0, buff.Length)) > 0)
                {
                    // not a full sample, mute the rest
                    if (br < buff.Length)
                    {
                        for (int i = br; i < buff.Length; i++)
                        {
                            buff[i] = 0;
                        }
                    }
                    for (int i = 0; i < buff.Length / 2; ++i)
                    {

                        // convert to 16-bit
                        // scale
                        short sample = (short)((buff[i * 2 + 1] << 8) | buff[i * 2]);
                        // value between 0 and 1.0
                        const double gain = 0.2;
                        sample = (short)(sample * gain);

                        // back to byte[]
                        buff[i * 2 + 1] = (byte)(sample >> 8);
                        buff[i * 2] = (byte)(sample & 0xff);
                    }

                    await Commands.Connection.SendAsync(buff, 20);
                }
                while ((br = ffout.Read(buff, 0, buff.Length)) > 0)
                {
                    // not a full sample, mute the rest
                    if (br < buff.Length)
                    {
                        for (int i = br; i < buff.Length; i++)
                        {
                            buff[i] = 0;
                        }
                    }
                    await Commands.Connection.SendAsync(buff, 20);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

           // we're not speaking anymore
            await Commands.Connection.SendSpeakingAsync(false);
        }
    }
}
