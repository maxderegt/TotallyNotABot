using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DSharpPlus.Entities;

namespace TotallyNotABot.core
{
    class InvalidSettingsException : Exception
    {
        public InvalidSettingsException(string message)
            : base(message)
        {
        }
    }

    class Settings
    {
        public static string SettingsFile = "discordbot\\settings.xml";
        public static string Token { get; set; }
        public static string Prefix { get; set; }
        public static string Spotify { get; set; }

        public static bool Load()
        {
            try
            {
                XDocument xml = XDocument.Load(SettingsFile);
                Token = xml.Descendants("token").First().Value;
                if (Token.Length == 0)
                {
                    throw new InvalidSettingsException("Missing \"token\" in settings!");
                }

                Prefix = xml.Descendants("prefix").First().Value;
                if (Prefix.Length == 0)
                {
                    throw new InvalidSettingsException("Missing \"prefix\" in settings!");
                }

                Spotify = xml.Descendants("spotify").First().Value;
                if (Spotify.Length > 0)
                {
                    Console.WriteLine("Loaded Spotify token");
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Couldn't read settings file: " + SettingsFile);
                return false;
            }

            return true;
        }


    }
}
