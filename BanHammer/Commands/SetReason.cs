
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using NorthwoodLib.Pools;
using Utf8Json.Resolvers.Internal;

namespace BanHammer.Commands
{
    public class SetReason : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("banhammer.use"))
            {
                response = "You can't use this command!";
                return false;
            }
            bool flag = Plugin.Singleton.Methods.CustomReasons.TryGetValue(Player.Get(sender), out string reason);
            if (arguments.Count == 0)
            {
                if (flag)
                {
                    response = $"Your current custom reason is: {reason}";
                    return true;
                }
                else
                {
                    response = "You don't have a custom reason set!";
                    return true;
                }
            }
            
            string currentReason = "";
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            foreach (string argument in arguments)
            {
                stringBuilder.Append(argument);
                stringBuilder.Append(" ");
            }
            currentReason = StringBuilderPool.Shared.ToStringReturn(stringBuilder);

            if (flag)
                Plugin.Singleton.Methods.CustomReasons[Player.Get(sender)] = currentReason;
            else
                Plugin.Singleton.Methods.CustomReasons.Add(Player.Get(sender), currentReason);
            
            response = $"Custom reason set to: {currentReason}";
            return true;
        }

        public string Command { get; } = "setreason";

        public string[] Aliases { get; } = {"reason", "bhr"};

        public string Description { get; } = "Sets or tells you your ban reason.";
    }
}