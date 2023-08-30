using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using UnityEngine.VFX;
using System.Collections;

namespace Rasengan
{
    class CollisionStarter : MonoBehaviour
    {
        Item item;
        VisualEffect vfx;
        public bool callback = false;
        float timer = 0f;
        float maxSeconds = 6f;
        SpellCaster sc;
        Coroutine currentCoroutine;
        FixedJoint joint;
        float knockBack;
        bool instantKnockback;
        GameObject rasenganHit;
        public void DestroySPhere(SphereCollider c) {

            Destroy(c);

        }

        void OnCollisionEnter(Collision c)
        {

            if (c.gameObject.GetComponentInParent<Creature>() is Creature creature && !creature.isPlayer)
            {
                {
                    if (!instantKnockback)
                    {
                        rasenganHit.transform.position = c.transform.position;
                        rasenganHit.transform.forward = -sc.magic.forward;
                        GameObject.Instantiate(rasenganHit);
                        rasenganHit.GetComponent<VisualEffect>().Play();
                        currentCoroutine = StartCoroutine(StartStackingRasengan(creature));
                    }

                    else
                    {
                        rasenganHit.transform.position = c.transform.position;
                        rasenganHit.transform.forward = -sc.magic.forward;
                        var rasenganTemp = GameObject.Instantiate(rasenganHit);

                        var vfxTemp = rasenganTemp.GetComponent<VisualEffect>();
                        vfxTemp.playRate *= 2f;
                        GameManager.local.StartCoroutine(StartTimer(rasenganTemp));

                        creature.ragdoll.SetState(Ragdoll.State.Destabilized);

                        foreach (Rigidbody rigidbody in creature.ragdoll.parts.Select(part => part.rb))
                        {
                            rigidbody.AddForce(sc.magic.transform.forward * rigidbody.mass * knockBack * 2, ForceMode.Impulse);
                        }

                    }
                }
            }
        }

        void OnCollisionExit(Collision c) {
            if (c.gameObject.GetComponentInParent<Creature>() != Player.currentCreature)
            {
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);
                }

            }
        
        }

        public void Setup(Item item, VisualEffect vfx, SpellCaster sc, float knockBack, bool instantKnockback, GameObject rasenganHit) {


            this.item = item;
            this.vfx = vfx;
            this.sc = sc;
            this.knockBack = knockBack;
            this.instantKnockback = instantKnockback;
            this.rasenganHit = rasenganHit;
        }


        IEnumerator StartStackingRasengan(Creature creature) {

            yield return new WaitForSeconds(3f);
            foreach (Rigidbody rigidbody in creature.ragdoll.parts.Select(part => part.rb))
            {
                creature.ragdoll.SetState(Ragdoll.State.Destabilized);
                rigidbody.AddForce(sc.magic.transform.forward * rigidbody.mass * knockBack, ForceMode.Impulse);
            }
            item.Despawn();


        }

       public IEnumerator StartTimer(GameObject rasengan) {
            item.Despawn();

            yield return new WaitForSeconds(1.5f);
            rasengan.GetComponent<VisualEffect>().Stop();
            yield return new WaitForSeconds(3f);
            UnityEngine.Object.Destroy(rasengan);
        }

    }
}
