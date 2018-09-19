using System;
using System.Collections.Generic;
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
                List<XElement> xmlToken = xml.Descendants("token").ToList();
                if (xmlToken.Any())
                {
                    Token = xmlToken.First().Value;
                }
                else
                {
                    throw new InvalidSettingsException("Missing \"token\" in settings!");
                }

                List<XElement> xmlPrefix = xml.Descendants("prefix").ToList();
                if (xmlPrefix.Any())
                {
                    Prefix = xmlPrefix.First().Value;
                }
                else
                {
                    throw new InvalidSettingsException("Missing \"prefix\" in settings!");
                }


                List<XElement> xmlSpotify = xml.Descendants("spotify").ToList();
                if (xmlSpotify.Any())
                {
                    Spotify = xmlSpotify.First().Value;
                }
                else
                {
                    Console.Error.WriteLine("No spotify token loaded!");
                }

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Couldn't read settings file: " + SettingsFile);
                return false;
            }
            catch (InvalidSettingsException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }


    }
}
