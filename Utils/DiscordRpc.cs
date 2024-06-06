using DiscordRPC;
using DiscordRPC.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.VisualBasic.CompilerServices;
using Timer = System.Timers.Timer;

namespace Kurai.Launcher.Utils;

public static class DiscordRpc
    {
        public static DiscordRpcClient client;
        private static string _discordTime = "";
        private static string previousContent = "";
        private static Timer _timer;
        private static string version = "";

        public static async Task Initialize()
        {
            await Task.Run(() =>
            {
                InitializeDiscordClient();
            });

            _discordTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

            _timer = new Timer(100); // 100 milliseconds
            _timer.Elapsed += TimerElapsed;
            _timer.Enabled = true;

            InLauncher();
        }

        private static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (Minecraft.IsGameOpen())
            {
                SetPresence("Playing Minecraft " + MainWindow.CurrentVersion, "None", "kurai-logo", "Kurai Client");
            }
            else
            {
                InLauncher();
            }
        }
        
        public static void InLauncher()
        {
            SetPresence("In Launcher", "None", "kurai-logo", "Kurai Launcher");
        }

        private static void InitializeDiscordClient()
        {
            client = new DiscordRpcClient("1247740086443966474");
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };
            client.OnReady += (sender, e) => Trace.WriteLine("Received Ready from user {0}", e.User.Username);
            client.OnPresenceUpdate += (sender, e) => Trace.WriteLine("Received Update! {0}", e.Presence.ToString());
            client.Initialize();
        }

        private static RichPresence currentPresence = null;

        private static void SetPresence(string details, string smallKey, string bigKey, string bigImgText)
        {
            DateTime? dateTimestampEnd = null;
            if (!string.IsNullOrEmpty(_discordTime) && int.TryParse(_discordTime, out int timestampEnd))
            {
                dateTimestampEnd = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestampEnd);
            }

            var assets = new Assets
            {
                LargeImageKey = bigKey,
                LargeImageText = bigImgText,
                SmallImageKey = smallKey != "None" ? smallKey : null
            };

            if (currentPresence == null)
            {
                // Add the "Download" button
                var button = new Button
                {
                    Label = "Download",
                    Url = "https://discord.gg/kuraiclient"
                    
                };

                currentPresence = new RichPresence();
                currentPresence.Timestamps = new Timestamps
                {
                    Start = _discordTime != "" && int.TryParse(_discordTime, out int timestampStart)
                        ? new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestampStart)
                        : DateTime.UtcNow,
                    End = dateTimestampEnd
                };
                
                currentPresence.Buttons = new Button[] { button };
            }

            currentPresence.Details = details;
            currentPresence.Assets = assets;

            client.SetPresence(currentPresence);
        }
    }