using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Exiled.API.Features;
using Exiled.API.Features.DamageHandlers;

namespace GGUtils
{
    internal class HealthObject
    {
        public static void DamageObject(Player player, float damage, RaycastHit hit)
        {
            string ObjectName = hit.transform.parent.name;
            int Health;
            if (!int.TryParse(hit.collider.name, out Health))
                return;
            int Damage = (int)damage;

            if (ObjectName.StartsWith("CustomSchematic-da_"))
            {
                Hitmarker.SendHitmarkerDirectly(player.ReferenceHub, 2f);

                if (Health <= Damage)
                    GameObject.Destroy(hit.collider.gameObject);

                else
                    hit.collider.name = (Health - Damage).ToString();
            }
        }
    }
}
