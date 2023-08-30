using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using UnityEngine.VFX;

namespace Rasengan
{
    class VanishingRasenganCollision : MonoBehaviour
    {

        Item item;

        VisualEffect vfx;
        SpellCaster sc;
        float knockBack;
        bool instantKnockback;
        GameObject rasenganHit;
        Vector3 velocityDirection;

        void OnCollisionEnter(Collision c) {


            if (c.gameObject.GetComponentInParent<Creature>() is Creature creature && !creature.isPlayer) {


                creature.ragdoll.SetState(Ragdoll.State.Destabilized);

                foreach (Rigidbody rigidbody in creature.ragdoll.parts.Select(part => part.rb))
                {
                    rigidbody.AddForce(item.GetComponent<Rigidbody>().velocity.normalized * rigidbody.mass * knockBack * 2, ForceMode.Impulse);
                }

                item.Despawn();
            }
        
        
        }




        public void Setup(Item item, VisualEffect vfx, SpellCaster sc, float knockBack, bool instantKnockback, GameObject rasenganHit)
        {


            this.item = item;
            this.vfx = vfx;
            this.sc = sc;
            this.knockBack = knockBack;
            this.instantKnockback = instantKnockback;
            this.rasenganHit = rasenganHit;
        }



        public void SetVelocityDirection(Vector3 velocityDirection) {

            this.velocityDirection = velocityDirection;
        
        }

    }
}
