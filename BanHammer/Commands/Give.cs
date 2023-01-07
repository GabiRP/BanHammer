using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.Permissions.Extensions;
using Utf8Json.Resolvers.Internal;

namespace BanHammer.Commands
{
    public class Give : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("banhammer.use"))
            {
                response = "You can't use this command!";
                return false;
            }
            
            if(!CustomItem.TryGet((int)Plugin.Singleton.Config.Item.Id, out CustomItem item))
            {
                response = "BanHammer item not found!";
                return false;
            }
            item.Give(Player.Get(sender));
            
            response = "Done!";
            return true;
        }

        public string Command { get; } = "give";

        public string[] Aliases { get; } = {"g"};

        public string Description { get; } = "Gives you a BanHammer";
    }
}