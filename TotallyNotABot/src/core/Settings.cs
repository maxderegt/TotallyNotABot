using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TotallyNotABot.src.core
{
    class Settings
    {
        public static string SettingsFile = "discordbot\\settings.xml";
        public string Token { get; }
        public string Prefix { get;  }

        public static Settings load()
        {
            XDocument xml = XDocument.Load(SettingsFile);
            string token = xml.Descendants("token").First().Value;
            string prefix = xml.Descendants("prefix").First().Value;
            return new Settings(token, prefix);
        }

        public Settings(string token, string prefix)
        {
            Token = token;
            Prefix = prefix;
        }

    }
}
