using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WG_Game
{
    public class House : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerController>().IsInside = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerController>().IsInside = false;
            }
        }
    }
}
