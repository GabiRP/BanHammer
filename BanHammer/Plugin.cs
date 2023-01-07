using System;
using Exiled.API.Features;
using Exiled.API.Interfaces;
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
        
        private EventHandlers _eventHandlers;
        
        public override void OnEnabled()
        {
            Singleton = this;
            _eventHandlers = new EventHandlers();
            JailbirdItem
            base.OnEnabled();
        }
        
        public override void OnDisabled()
        {
            _eventHandlers = null;
            Singleton = null;
            
            base.OnDisabled();
        }
    }
}