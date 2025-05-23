﻿using System;
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

            if (ev.Player.CurrentRoom != null)
            {
                Vector3 pos = ev.Player.Position - ev.Player.CurrentRoom.Position;
                ServerConsole.AddLog($"{ev.Player.Nickname}의 상대적 위치 : RoomType.{ev.Player.CurrentRoom.Type} new Vector3({pos.x}f, {pos.y}f, {pos.z}f)");
            }

            ServerConsole.AddLog($"{ev.Player.Nickname}의 위치 : new Vector3({ev.Player.Position.x}f, {ev.Player.Position.y}f, {ev.Player.Position.z}f)", ConsoleColor.DarkMagenta);
            ServerConsole.AddLog($"{ev.Player.Nickname}의 방향 : new Vector3({playerForward.x}f, {playerForward.y}f, {playerForward.z}f)", ConsoleColor.Blue);

            if (Physics.Raycast(ev.Player.ReferenceHub.PlayerCameraReference.position + ev.Player.ReferenceHub.PlayerCameraReference.forward * 0.2f, ev.Player.ReferenceHub.PlayerCameraReference.forward, out RaycastHit hit, 99999, (LayerMask)1))
                ServerConsole.AddLog($"{ev.Player.Nickname}의 전방 : {hit.transform.parent.parent.name}ㅣ{hit.transform.parent.name}ㅣ{hit.collider.name}", ConsoleColor.DarkYellow);
        }

        public void OnShot(Exiled.Events.EventArgs.Player.ShotEventArgs ev)
        {
            if (Physics.Raycast(ev.Player.ReferenceHub.PlayerCameraReference.position + ev.Player.ReferenceHub.PlayerCameraReference.forward * 0.2f, ev.Player.ReferenceHub.PlayerCameraReference.forward, out RaycastHit hit, 1000, (LayerMask)1))
            {
                Dictionary<FirearmType, int> Firearm = new Dictionary<FirearmType, int>() {
                    { FirearmType.Com15, 25 }, 
                    { FirearmType.Com18, 25 }, 
                    { FirearmType.FSP9, 22 }, 
                    { FirearmType.Crossvec, 23 }, 
                    { FirearmType.E11SR, 26 },
                    { FirearmType.FRMG0, 24 }, 
                    { FirearmType.Revolver, 51 }, 
                    { FirearmType.Shotgun, 8 }, 
                    { FirearmType.AK, 35 }, 
                    { FirearmType.Logicer, 25 },
                    { FirearmType.A7, 26 }, 
                    { FirearmType.Com45, 25 }, 
                    { FirearmType.ParticleDisruptor, 250 },
                    { FirearmType.Scp127, 30 }
                };

                HealthObject.DamageObject(ev.Player, Firearm[ev.Firearm.FirearmType], hit);

            }
        }

        public void OnUsingMicroHIDEnergy(Exiled.Events.EventArgs.Player.UsingMicroHIDEnergyEventArgs ev)
        {
            if (ev.MicroHID.State == InventorySystem.Items.MicroHID.Modules.MicroHidPhase.Firing)
            {
                if (Physics.Raycast(ev.Player.ReferenceHub.PlayerCameraReference.position + ev.Player.ReferenceHub.PlayerCameraReference.forward * 0.2f, ev.Player.ReferenceHub.PlayerCameraReference.forward, out RaycastHit hit, 5, (LayerMask)1))
                    HealthObject.DamageObject(ev.Player, 120, hit);
            }
        }

        public async void OnSwining(Exiled.Events.EventArgs.Item.SwingingEventArgs ev)
        {
            await Task.Delay(300);

            if (Physics.Raycast(ev.Player.ReferenceHub.PlayerCameraReference.position + ev.Player.ReferenceHub.PlayerCameraReference.forward * 0.2f, ev.Player.ReferenceHub.PlayerCameraReference.forward, out RaycastHit hit, 3, (LayerMask)1))
                HealthObject.DamageObject(ev.Player, 50, hit);
        }
    }
}

