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
    public static class GlassObject
    {
        public static void DamageObject(Player player, float damage, RaycastHit hit)
        {
            string ObjectName = hit.transform.parent.name;
            int Health;
            Rigidbody rigid;

            if (!int.TryParse(hit.collider.name, out Health))
                return;

            int Damage = (int)damage;

            if (ObjectName.StartsWith("CustomSchematic-ga_"))
            {
                Hitmarker.SendHitmarkerDirectly(player.ReferenceHub, 2f);

                if (Health <= Damage)
                {
                    foreach (var _glass in Gtool.GetChild(hit.transform.parent))
                    {
                        rigid = _glass.gameObject.AddComponent<Rigidbody>();
                        rigid.isKinematic = false;
                        rigid.useGravity = true;
                        rigid.mass = 0.1f;
                        rigid.drag = 0.1f;
                    }
                }
                else
                {
                    hit.transform.name = (Health - Damage).ToString();
                }

                Log.Info(hit.transform.name);
            }
        }
    }
}
