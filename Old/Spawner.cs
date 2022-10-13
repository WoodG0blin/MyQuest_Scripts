using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WG_Game
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> elementals;

        public GameObject Call(Transform origin, Elements element, int level = 1, float flyHeight = 1.0f)
        {
            GameObject el;
            for (int i = 0; i < elementals.Count; i++)
            {
                if (elementals[i].GetComponent<Elemental>().Type == element)
                {
                    el = GameObject.Instantiate(elementals[i], origin.position, origin.rotation);
                    el.GetComponent<Elemental>().Initiate(element, level, flyHeight);
                    return el;
                }
            }
            return null;
        }
    }
}
