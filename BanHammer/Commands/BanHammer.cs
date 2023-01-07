using System;
using Christmas.Scp2536;
using CommandSystem;
using Exiled.CustomItems.API.Features;
using Exiled.Permissions.Extensions;

namespace BanHammer.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class BanHammer : ParentCommand
    {
        public BanHammer() => LoadGeneratedCommands();
        
        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Give());
            RegisterCommand(new SetReason());
        }
        
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("banhammer.use"))
            {
                response = "You can't use this command!";
                return false;
            }
            
            response = "Usage:\n" +
                       "- banhammer give | Gives you a Ban Hammer\n" +
                       "- banhammer setreason | Tells you your current custom reason\n" +
                       "- banhammer setreason <reason> | Sets your custom reason";
            return true;
        }

        public override string Command { get; } = "banhammer";

        public override string[] Aliases { get; } = {"bhammer", "bh"};

        public override string Description { get; } = "Gives a Ban Hammer";

    }
}