using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WG_Game
{
    public class StreetLights : MonoBehaviour, ILightable
    {
        [SerializeField] private List<ILightable> lights;
        [SerializeField] private DayTime daytime;

        private void Start()
        {
            daytime = GameObject.Find("SUN").GetComponent<DayTime>();
            if (daytime) daytime.SetInformant(this);
            lights = new List<ILightable>();
            lights.AddRange(GetComponentsInChildren<ILightable>());
            TurnLights(false);
        }

        public void TurnLights(bool on)
        {
            for (int i = 1; i < lights.Count; i++) { lights[i].TurnLights(on); }
        }
    }
}
