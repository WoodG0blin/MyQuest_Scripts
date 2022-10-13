using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WG_Game
{
    public class LightPole : MonoBehaviour, ILightable
    {
        private Light _light;
        private bool flickering;
        private bool active;

        void Start()
        {
            _light = transform.Find("Light").GetComponentInChildren<Light>();
            flickering = false;
            active = true;
            //TurnLights(false);
        }

        private void FixedUpdate()
        {
            if (active && !flickering) StartCoroutine(Flicker(Random.Range(4.0f, 10.0f)));
        }
        private IEnumerator Flicker(float delay)
        {
            flickering = true;
            yield return new WaitForSecondsRealtime(delay);
            _light.enabled = false;
            yield return new WaitForSecondsRealtime(Random.Range(0.02f, 0.5f));
            _light.enabled = true;
            flickering = false;
        }

        public void TurnLights(bool on)
        {
            StopAllCoroutines();
            _light.enabled = on;
            active = on;
        }
    }
}
