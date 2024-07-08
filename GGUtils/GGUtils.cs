/* GGUtils (ver. Alpha 0.0.1) */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.API.Enums;
using PlayerRoles.FirstPersonControl;
using UnityEngine;
using MEC;
using Exiled.API.Features.Roles;

namespace GGUtils
{
    public class GGUtils : Plugin<Config>
    {
        public static GGUtils Instance;

        public List<Chracters> Chracters = new List<Chracters>();

        public override void OnEnabled()
        {
            Instance = this;

            Exiled.Events.Handlers.Player.FlippingCoin += OnFlippingCoin;
            Exiled.Events.Handlers.Player.Shot += OnShot;
            Exiled.Events.Handlers.Player.UsingMicroHIDEnergy += OnUsingMicroHIDEnergy;

            Exiled.Events.Handlers.Item.Swinging += OnSwining;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.FlippingCoin -= OnFlippingCoin;
            Exiled.Events.Handlers.Player.Shot -= OnShot;
            Exiled.Events.Handlers.Player.UsingMicroHIDEnergy -= OnUsingMicroHIDEnergy;

            Exiled.Events.Handlers.Item.Swinging -= OnSwining;

            Instance = null;
        }

        public void OnFlippingCoin(Exiled.Events.EventArgs.Player.FlippingCoinEventArgs ev)
        {
            ReferenceHub playerHub = ev.Player.ReferenceHub;
            FpcStandardRoleBase playerRole = playerHub.roleManager.CurrentRole as FpcStandardRoleBase;
            Vector3 playerForward = playerRole.transform.forward;

            ServerConsole.AddLog($"{ev.Player.Nickname}의 위치 : new Vector3({ev.Player.Position.x}f, {ev.Player.Position.y}f, {ev.Player.Position.z}f)", ConsoleColor.DarkMagenta);
            ServerConsole.AddLog($"{ev.Player.Nickname}의 방향 : new Vector3({playerForward.x}f, {playerForward.y}f, {playerForward.z}f)", ConsoleColor.Blue);

            if (Physics.Raycast(ev.Player.ReferenceHub.PlayerCameraReference.position + ev.Player.ReferenceHub.PlayerCameraReference.forward * 0.2f, ev.Player.ReferenceHub.PlayerCameraReference.forward, out RaycastHit hit, 99999, InventorySystem.Items.Firearms.Modules.StandardHitregBase.HitregMask))
                ServerConsole.AddLog($"{ev.Player.Nickname}의 전방 : {hit.transform.parent.parent.name} {hit.transform.parent.name} {hit.collider.name}", ConsoleColor.DarkYellow);
        }

        public void OnShot(Exiled.Events.EventArgs.Player.ShotEventArgs ev)
        {
            if (Physics.Raycast(ev.Player.ReferenceHub.PlayerCameraReference.position + ev.Player.ReferenceHub.PlayerCameraReference.forward * 0.2f, ev.Player.ReferenceHub.PlayerCameraReference.forward, out RaycastHit hit, 1000, InventorySystem.Items.Firearms.Modules.StandardHitregBase.HitregMask))
            {
                Dictionary<FirearmType, int> Firearm = new Dictionary<FirearmType, int>() { 
                    { FirearmType.Com15, 25 }, { FirearmType.Com18, 25 }, { FirearmType.FSP9, 22 }, { FirearmType.Crossvec, 23 } , { FirearmType.E11SR, 26 },
                    { FirearmType.FRMG0, 24 }, {FirearmType.Revolver, 51 }, {FirearmType.Shotgun, 8 }, {FirearmType.AK, 35 }, {FirearmType.Logicer, 25 },
                    { FirearmType.A7, 26 }, { FirearmType.Com45, 25 }, { FirearmType.ParticleDisruptor, 250 }
                };
                HealthObject.DamageObject(ev.Player, Firearm[ev.Firearm.FirearmType], hit);
            }
        }

        public void OnUsingMicroHIDEnergy(Exiled.Events.EventArgs.Player.UsingMicroHIDEnergyEventArgs ev)
        {
            if (ev.MicroHID.State == InventorySystem.Items.MicroHID.HidState.Firing)
            {
                if (Physics.Raycast(ev.Player.ReferenceHub.PlayerCameraReference.position + ev.Player.ReferenceHub.PlayerCameraReference.forward * 0.2f, ev.Player.ReferenceHub.PlayerCameraReference.forward, out RaycastHit hit, 5, InventorySystem.Items.Firearms.Modules.StandardHitregBase.HitregMask))
                    HealthObject.DamageObject(ev.Player, 120, hit);
            }
        }

        public async void OnSwining(Exiled.Events.EventArgs.Item.SwingingEventArgs ev)
        {
            await Task.Delay(300);

            if (Physics.Raycast(ev.Player.ReferenceHub.PlayerCameraReference.position + ev.Player.ReferenceHub.PlayerCameraReference.forward * 0.2f, ev.Player.ReferenceHub.PlayerCameraReference.forward, out RaycastHit hit, 3, InventorySystem.Items.Firearms.Modules.StandardHitregBase.HitregMask))
                HealthObject.DamageObject(ev.Player, 50, hit);
        }
    }
}

