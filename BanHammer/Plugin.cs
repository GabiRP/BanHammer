using System;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.CustomItems.API;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Jailbird;

namespace BanHammer
{
    public class Plugin : Plugin<Config>
    {
        public string Name { get; } = "BanHammer";

        public string Author { get; } = "GabiRP";

        public Version Version { get; } = new Version(1, 0, 0);

        public static Plugin Singleton;

        public Methods Methods;
        
        public override void OnEnabled()
        {
            Singleton = this;
            Methods = new Methods();
            
            Config.Item.Register();
            
            base.OnEnabled();
        }
        
        public override void OnDisabled()
        {
            Config.Item.Unregister();
            
            Methods = null;
            Singleton = null;
            
            base.OnDisabled();
        }
    }
}