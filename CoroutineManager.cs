using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using System.Collections;

namespace Rasengan
{
    class CoroutineManager : MonoBehaviour
    {
        public void StartGripCheck(Rasengan rasengan)
        {
            StartCoroutine(StartTimerForGripChange(rasengan));
        }

        public IEnumerator StartTimerForGripChange(Rasengan rasengan)
        {

            yield return new WaitForSeconds(1.5f);

            rasengan.gripCheck = true;

        }
    }
}
