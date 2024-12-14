using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CommandSystem;
using Discord;
using Exiled.API.Features;
using PluginAPI.Roles;
using RemoteAdmin;
using ZXing;

namespace GGUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Badge : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            try
            {
                Player player = Player.Get(arguments.At(0));
                string color = arguments.At(1);
                string text = string.Join(" ", arguments.ToList().Skip(2));

                player.Group = new UserGroup { BadgeText = text, BadgeColor = color };
                response = $"Success!";
                return true;
            }
            catch (Exception e)
            {
                response = $"Error! {e.Message}";
                return false;
            }
        }

        public string Command { get; } = "badge";

        public string[] Aliases { get; } = { "bg" };

        public string Description { get; } = "특정 유저에게 커스텀 뱃지를 적용시키세요.";

        public bool SanitizeResponse { get; } = true;
    }

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ShowHint : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            try
            {
                Player player = Player.Get(arguments.At(0));
                float duration = float.Parse(arguments.At(1));
                string text = string.Join(" ", arguments.ToList().Skip(2));

                player.ShowHint(text, duration);
                response = $"Success!";
                return true;
            }
            catch (Exception e)
            {
                response = $"Error! {e.Message}";
                return false;
            }
        }

        public string Command { get; } = "showhint";

        public string[] Aliases { get; } = { "hint" };

        public string Description { get; } = "특정 유저에게 힌트를 보내세요!";

        public bool SanitizeResponse { get; } = true;
    }
}
