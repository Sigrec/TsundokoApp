﻿using Microsoft.VisualBasic;
using DiscordRPC;

namespace Tsundoku.Helpers
{
    internal class DiscordRP
    {
        private static DiscordRpcClient client;

        public static void Initialize()
        {
            // == Create the client
            client = new DiscordRpcClient("1050229234674696252");

            // == Subscribe to some events
            client.OnReady += (sender, msg) =>
            {
                Tsundoku.Models.Constants.Logger.Info("Connected To Discord With User {0}", msg.User.Username);
            };

            // == Initialize
            client.Initialize();

            // == Set the presence
            client.SetPresence(new RichPresence()
            {
                Details = "Manga & Light Novel Collection App",
                State = "Browsing Collection",
                Timestamps = Timestamps.Now,
                Buttons = new Button[]
                {
                    new Button() 
                    { 
                        Label = "Download Tsundoku", 
                        Url = "https://github.com/Sigrec/TsundokuApp/blob/main/README.md"
                    }
                },
                Assets = new Assets()
                {
                    LargeImageKey = "rp_large_icon",
                    LargeImageText = "Tsundoku",
                }
            });
        }

        public static void Deinitialize() 
        {
            client.Dispose();
        }
    }
}
