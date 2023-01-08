using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Permissions.Extensions;
using UnityEngine;

namespace BanHammer
{
    public class BanHammerItem : CustomItem
    {
        [Description("Change this if another CustomItem is already using this id")]
        public override uint Id { get; set; } = 6969;
        
        public override string Name { get; set; } = "BanHammer";

        [Description("Description shown when picking up the Ban Hammer")]
        public override string Description { get; set; } = "Ban Hammer.";

        [Description("Weight of the Ban Hammer")]
        public override float Weight { get; set; } = 0f;
        
        [Description("You shouldn't touch this if you don't want the Ban Hammer spawning somewhere in the map")]
        public override SpawnProperties SpawnProperties { get; set; }

        public override ItemType Type { get; set; } = ItemType.Jailbird;

        private int currentCustomTimeIndex;
        private int currentPredefinedReasonIndex;
        private bool usePredefinedReasons;
        
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.ProcessingHotkey += OnProcessingHotkey;
            Exiled.Events.Handlers.Player.DroppingItem += OnDroppingItem;
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
            
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.ProcessingHotkey -= OnProcessingHotkey;
            Exiled.Events.Handlers.Player.DroppingItem -= OnDroppingItem;
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
            
            base.UnsubscribeEvents();
        }

        private void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if(!Check(ev.Pickup)) return;

            if (!ev.Player.CheckPermission("banhammer.use"))
                ev.IsAllowed = false;
        }
        
        private void OnHurting(HurtingEventArgs ev)
        {
            if(!Check(ev.Attacker) || ev.Player == null || ev.Player == ev.Attacker) return;
            if(ev.Attacker.CurrentItem is not JailBird hammer) return;

            ev.IsAllowed = false;
            
            if (hammer.Base._charging)
            {
                int duration = 0;
                string reason = "";
                string nick = "";

                if (Physics.Raycast(ev.Attacker.CameraTransform.position + Vector3.forward * 1.5f, ev.Attacker.CameraTransform.forward, out RaycastHit hit, 5f))
                {
                    if(!hit.collider.TryGetComponent(out CharacterClassManager ccm))
                        return;
                    Player player = Player.Get(ccm.Hub);
                    nick = player.Nickname;
                    if (usePredefinedReasons)
                    {
                        KeyValuePair<long, string> preason = Plugin.Singleton.Config.PredefinedReasons.ElementAt(currentPredefinedReasonIndex);
                        player.Ban((int)preason.Key, preason.Value);
                    
                        duration = (int)preason.Key;
                        reason = preason.Value;
                    }
                    else
                    {
                        int d = (int)Plugin.Singleton.Config.BanDurations[currentCustomTimeIndex];
                        string r = Plugin.Singleton.Methods.GetCustomReason(ev.Attacker);
                        player.Ban(d, r);
                    
                        duration = d;
                        reason = r;
                    }
                }
                
                Map.Broadcast(Plugin.Singleton.Config.BroadcastDuration, Plugin.Singleton.Config.BanBroadcast
                                .Replace("%player%", nick)
                                .Replace("%duration%", duration.ToString())
                                .Replace("%reason%", reason));
            }
        }

        private void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if(!Check(ev.Player)) return;

            ev.IsAllowed = false;
            
            if (!usePredefinedReasons)
            {
                if(currentCustomTimeIndex >= Plugin.Singleton.Config.BanDurations.Length)
                    currentCustomTimeIndex = 0;
                else
                    currentCustomTimeIndex++;
            }
            else
            {
                if(currentPredefinedReasonIndex >= Plugin.Singleton.Config.PredefinedReasons.Count)
                    currentPredefinedReasonIndex = 0;
                else
                    currentPredefinedReasonIndex++;
            }

            string reason = usePredefinedReasons
                            ? Plugin.Singleton.Config.PredefinedReasons.ElementAt(currentPredefinedReasonIndex).Value
                            : Plugin.Singleton.Methods.GetCustomReason(ev.Player);
            string durationString = Plugin.Singleton.Methods.ParseTime(Plugin.Singleton.Config.BanDurations[currentCustomTimeIndex]);
            ev.Player.ShowHint($"Ban duration set to {durationString} with reason\n\"{reason}\"");
            
            /*usePredefinedReasons = !usePredefinedReasons;
            ev.Player.ShowHint($"The Ban Hammer now {(usePredefinedReasons ? "uses predefined reasons" : "uses custom reasons")}");*/
        }

        private void OnProcessingHotkey(ProcessingHotkeyEventArgs ev)
        {
            if (ev.Hotkey == HotkeyButton.Grenade)
            {
                
            }
        }
    }
}