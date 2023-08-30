using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using UnityEngine.VFX;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Rasengan
{
    public class Rasengan : SpellCastCharge
    {
        public string mainHand;
        public float knockback;
        public float chargeTime;
        public bool instantKnockback;
        Item rasengan;
        bool switcher = false;
        RagdollHand spawnHand;
        SphereCollider colliderTrigger;
        AudioSource startAudio;
        bool fullyFormed = false;
        VisualEffect vfx;
        ColliderTriggering vfxChange;
        AudioSource currentAudio;
        bool callBack = false;
        public float timer = 0f;
        AudioSource developedAudio;
        AudioSource loopAudio;
        float currentTime = 0;
        float previousTime = 0;
        ItemData normalRasengan;
        ItemData lightningRasengan;
        int maxItems = 2;
        CollisionStarter collisionHandler;
        GameObject rasenganHit;
        EffectData activateEffectData;
        int currentIndex = 0;
        List<Color> colors;
        int maxIterations;
        List<ItemData> itemDatas;
        int currentRasengan = 0;
        CoroutineManager coroutineManager;
        string rasenganName;
        Creature target;
        Rigidbody rasenganRigid;
        EffectData lightningEffect;

        public bool gripCheck = true;
        VanishingRasenganCollision vrc;
        

        public override void Load(SpellCaster spellCaster, Level level)
        {
            base.Load(spellCaster, level);

            if (mainHand == "Right") {

                spawnHand = Player.currentCreature.handRight;

            }

            else if (mainHand == "Left") {

                spawnHand = Player.currentCreature.handLeft;
            }

            Catalog.LoadAssetAsync<GameObject>("rasenganHitEffect.VFX.New", i => rasenganHit = i, "Rasengan");
            activateEffectData = Catalog.GetData<EffectData>("RasenganChargeFinger");
            colors = new List<Color>() { Color.white, Color.blue, Color.white, Color.gray};
            itemDatas = new List<ItemData>() {Catalog.GetData<ItemData>("RasenganItem"), Catalog.GetData<ItemData>("VanishingRasenganItem")};

            maxIterations = colors.Count - 1;
            coroutineManager = spellCaster.gameObject.AddComponent<CoroutineManager>();
            lightningEffect = Catalog.GetData<EffectData>("Bas.Particle.Spell.Lightning.ImbueHit");
        }

        void SetNewEffectInstance(Color colorStart, Color colorEnd) {

            var myClonedEffectData = activateEffectData.CloneJson<EffectData>();

            foreach (EffectModule effectModule in myClonedEffectData.modules)
            {
                if (effectModule is EffectModuleParticle effectModuleParticle)
                {
                    effectModuleParticle.mainNoHdrColorStart = colorStart;
                    effectModuleParticle.mainNoHdrColorEnd = colorEnd;
                }
            }
            var myGo = new GameObject();
            myGo.transform.position = spellCaster.magic.transform.position;
            myGo.transform.rotation = spellCaster.magic.transform.rotation;
            var effectInstance = myClonedEffectData.Spawn(myGo.transform);
            effectInstance.Play();
        }

        

        public override void UpdateCaster()
        {
            base.UpdateCaster();

            if (PlayerControl.GetHand(spawnHand.otherHand.side).gripPressed && gripCheck)
            {

                gripCheck = false;
                Debug.Log(itemDatas[currentRasengan].id);
                //SetNewEffectInstance(colors[currentIndex], colors[currentIndex + 1]);
                currentIndex += 2;

                if (currentIndex + 1 == maxIterations) currentIndex = 0;

                currentRasengan++;

                if (currentRasengan == itemDatas.Count) currentRasengan = 0;


                coroutineManager.StartGripCheck(this);
                

            }

            if (spellCaster.isFiring)
            {
                
                
                if (rasengan != null)
                {

                    rasengan.gameObject.transform.position = spellCaster.magic.transform.position;
                    rasengan.gameObject.transform.rotation = spellCaster.magic.transform.rotation;
                    if (startAudio != null) {

                        
                        previousTime = currentTime;
                        
                        currentTime = startAudio.time;
                        if (currentTime < previousTime)
                        {
                           startAudio.time = 2.4f;
                        }
                    }
                    
                }

                
                if (spellCaster.ragdollHand == spawnHand 
                    && Vector3.Distance(spawnHand.otherHand.transform.position, spawnHand.transform.position) < 0.3f 
                    && spawnHand.otherHand.Velocity().magnitude > 1f
                    && spellCaster.other.isFiring && spellCaster.other.spellInstance.id == spellCaster.spellInstance.id)
                {

                    float speed = spawnHand.otherHand.Velocity().magnitude;
                    timer += Time.deltaTime;
                    if (rasengan == null)
                    {
                        itemDatas[currentRasengan].SpawnAsync(rasenganTemp =>
                        {

                            if (rasengan != null)
                            {

                                rasengan.Despawn();

                            }
                            rasengan = rasenganTemp;

                            if (currentRasengan == 0)
                            {

                                rasenganName = "Rasengan";


                            }

                            else rasenganName = "VanishingRasengan";
                            foreach (AudioSource audio in rasengan.GetComponentsInChildren<AudioSource>()) {

                                if (audio.name == "RasenganStart")
                                {

                                    startAudio = audio;
                                    startAudio.loop = true;
                                    
                                }
                            }

                            fullyFormed = false;
                            vfx = rasengan.GetComponentInChildren<VisualEffect>();
                            vfxChange = rasengan.gameObject.AddComponent<ColliderTriggering>();
                            rasenganRigid = rasengan.GetComponent<Rigidbody>();


                            rasengan.gameObject.transform.position = spellCaster.magic.transform.position;
                            rasengan.gameObject.transform.rotation = spellCaster.magic.transform.rotation;
                        });
                    }

                    if (timer >= chargeTime && vfxChange != null)
                    {
                        if (rasenganName == "Rasengan")
                        {
                            timer = 0f;
                            fullyFormed = true;
                            SphereCollider collider;

                            collider = rasengan.gameObject.AddComponent<SphereCollider>();
                            collider.radius = 0.07f;
                            collider.center = Vector3.zero;
                            rasengan.gameObject.AddComponent<CollisionStarter>().Setup(rasengan, vfx, spellCaster, knockback, instantKnockback, rasenganHit);
                            collisionHandler = rasengan.gameObject.GetComponent<CollisionStarter>();


                        }

                        else if (rasenganName == "VanishingRasengan") {

                            timer = 0f;
                            fullyFormed = true;
                            SphereCollider collider;

                            collider = rasengan.gameObject.AddComponent<SphereCollider>();
                            collider.radius = 0.038f;
                            collider.center = Vector3.zero;
                            (vrc = rasengan.gameObject.AddComponent<VanishingRasenganCollision>()).Setup(rasengan, vfx, spellCaster, knockback, instantKnockback, rasenganHit);

                        }

                    }

                    else {

                        if (!fullyFormed) {

                            if (vfxChange != null)
                            {
                                vfxChange.ChangeGraph(vfx, timer,chargeTime, speed, rasenganName);
                            }

                        }
                    }
                }
            }

            else {

                if (rasengan != null)
                {
                    if (rasenganName == "Rasengan")
                    {
                        if (fullyFormed)
                        {

                            rasengan.GetComponent<CollisionStarter>().DestroySPhere(rasengan.GetComponent<SphereCollider>());

                            fullyFormed = false;

                        }

                        vfxChange.ChangeGraphDestroy(vfx);

                        vfxChange.StartDestroy(rasengan);

                    }

                    else {

                        if (fullyFormed)
                        {


                            
                            target = Creature.allActive.Where(i => !i.isPlayer && !i.isKilled).OrderBy(creature => (creature.ragdoll.headPart.transform.position - Player.currentCreature.transform.position).sqrMagnitude).First();

                            float currentDistance = Vector3.Distance(rasengan.transform.position, target.ragdoll.targetPart.transform.position);

                            

                            float strengthModifier = Mathf.Abs(Mathf.Min(currentDistance / 20f, 0.99f) - 1);

                            rasenganRigid.velocity = (target.ragdoll.targetPart.transform.position - rasengan.transform.position).normalized * 20f;

                            if (currentDistance <= Vector3.Distance(spellCaster.magic.transform.position,target.ragdoll.targetPart.transform.position) / 2f) {

                                vfxChange.SetAlphaVeanishing(vfx);
                                
                                var effectInstance = lightningEffect.Spawn(rasengan.transform);
                                
                                effectInstance.Play();

                            }

                        }

                        else {
                            
                            vfxChange.ChangeGraphDestroy(vfx);

                            vfxChange.StartDestroy(rasengan);

                        }

                    
                    
                    }

                }
            }

        }
        

    }
}
