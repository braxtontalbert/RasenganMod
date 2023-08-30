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

    class ColliderTriggering : MonoBehaviour
    {
        public void StartDestroy(Item rasengan) => StartCoroutine(OnRasenganStop(rasengan));
        public void ChangeGraph(VisualEffect vfx, float maxRotations, float maxTime, float speed, string rasenganType) {


            if (rasenganType == "Rasengan")
            {
                Debug.Log("Outer blue attraction" + vfx.GetFloat("attractionSpeedOuterBlue"));

                vfx.SetFloat("attractionSpeedOuterBlue", (((float)maxRotations * (100 / maxTime))) * (0.01f) * 1);
                vfx.SetFloat("attractionForceOuterBlue", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * 1f);
                vfx.SetFloat("stickForceOuterBlue", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * 3.5f);
                vfx.SetFloat("intensityOuterBlue", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * 1f);
                vfx.SetFloat("frequencyOuterBlue", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * 1f);

                vfx.SetFloat("intensityInnerWhite", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * -1f);
                vfx.SetFloat("frequencyInnerWhite", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * -1f);
                vfx.SetFloat("attractionSpeedInnerWhite", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * 1f);
                vfx.SetFloat("attractionForceInnerWhite", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * -0.5f);
                vfx.SetFloat("stickForceInnerWhite", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * 1f);

                vfx.SetFloat("attractionSpeedOuterWhite", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * 1f);
                vfx.SetFloat("attractionForceOuterWhite", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * 3f);
                vfx.SetFloat("stickForceOuterWhite", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * 5f);


            }

            else {

                vfx.SetFloat("attractionSpeedOuterBlue", (((float)maxRotations * (100 / maxTime))) * (0.01f) * 1);
                vfx.SetFloat("attractionForceOuterBlue", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * 1f);
                vfx.SetFloat("stickForceOuterBlue", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * 4f);
                vfx.SetFloat("intensityOuterBlue", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * -1f);
                vfx.SetFloat("frequencyOuterBlue", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * -1f);

                vfx.SetFloat("intensityInnerWhite", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * 1f);
                vfx.SetFloat("frequencyInnerWhite", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * 1f);
                vfx.SetFloat("attractionSpeedInnerWhite", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * 1f);
                vfx.SetFloat("attractionForceInnerWhite", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * 1f);
                vfx.SetFloat("stickForceInnerWhite", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * 100f);

                vfx.SetFloat("attractionSpeedOuterWhite", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * 1f);
                vfx.SetFloat("attractionForceOuterWhite", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * 3f);
                vfx.SetFloat("stickForceOuterWhite", (((float)maxRotations * (100 / maxTime)) * (0.01f)) * 5f);


            }
            
        }

        
        public void SetAlphaVeanishing(VisualEffect vfx) {

            vfx.SetFloat("alphaInside", 0f);
            vfx.SetFloat("alphaOutside",0f);
        
        }


        public void ChangeGraphDestroy(VisualEffect vfx)
        {

            Debug.Log("Outer blue attraction" + vfx.GetFloat("attractionSpeedOuterBlue"));

            vfx.SetFloat("attractionSpeedOuterBlue", 0f);
            vfx.SetFloat("attractionForceOuterBlue", 0f);
            vfx.SetFloat("stickForceOuterBlue", 0f);
            vfx.SetFloat("intensityOuterBlue", 0f);
            vfx.SetFloat("frequencyOuterBlue", 0f);

            vfx.SetFloat("intensityInnerWhite", 0f);
            vfx.SetFloat("frequencyInnerWhite", 0f);
            vfx.SetFloat("attractionSpeedInnerWhite", 0f);
            vfx.SetFloat("attractionForceInnerWhite", 0f);
            vfx.SetFloat("stickForceInnerWhite", 0f);

            vfx.SetFloat("attractionSpeedOuterWhite", 0f);
            vfx.SetFloat("attractionForceOuterWhite", 0f);
            vfx.SetFloat("stickForceOuterWhite", 0f);

        }


        public void ChangeToZero(VisualEffect vfx) {

            vfx.SetFloat("attractionSpeedInnerWhite", 5000f);
            vfx.SetFloat("attractionForceInnerWhite", 300f);
            vfx.SetFloat("stickForceInnerWhite", -1f);

        }


        IEnumerator OnRasenganStop(Item rasengan)
        {

            yield return new WaitForSeconds(3);
            rasengan.Despawn();
        }



    }

    
}
