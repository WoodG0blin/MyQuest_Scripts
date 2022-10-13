using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WG_Game
{
    public class Item : MonoBehaviour, IInteractable
    {
        protected Spawner spawner;
        protected bool responded;
        protected GameObject elemental;
        [SerializeField] protected Elements type;
        protected float cooldown = 15.0f;

        public bool IsRespondable { get => !responded; }

        protected virtual void Start()
        {
            if (spawner == null) spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
            responded = false;
        }

        public virtual void RequestResponse(Action<InteractionResult> SendResult)
        {
            if (!responded)
            {
                responded = true;
                elemental = spawner.Call(transform, type, flyHeight: Mathf.Clamp(transform.localScale.z, 1.5f, 3.0f));
            }
        }

        protected virtual void OnElementalDestroy()
        {
            StartCoroutine(RenewResponse(cooldown));
        }

        protected IEnumerator RenewResponse(float t)
        {
            yield return new WaitForSeconds(t);
            responded = false;
        }
    }
}
