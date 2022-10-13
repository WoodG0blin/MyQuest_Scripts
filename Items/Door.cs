using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WG_Game
{
    public class Door : MonoBehaviour, IInteractable
    {
        private bool isOpen;
        private Animator animator;
        private bool isRespondable;
        public bool IsRespondable { get => isRespondable; }

        void Start()
        {
            isOpen = false;
            isRespondable = true;
            animator = GetComponentInParent<Animator>();
        }

        public void RequestResponse(Action<InteractionResult> SendResult)
        {
            if (isRespondable)
            {
                isOpen = !isOpen;
                animator.SetBool("IsOpen", isOpen);
                isRespondable = false;
                StartCoroutine(ResetResponse(2.0f));
            }
        }

        private IEnumerator ResetResponse(float t)
        {
            yield return new WaitForSeconds(t);
            isRespondable = true;
        }
    }
}
