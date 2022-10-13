using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WG_Game
{
    public abstract class InteractableView : SceneObjectView, IInteract
    {
        protected bool _isRespondable = true;
        public bool IsRespondable { get => _isRespondable; }

        public virtual void RequestInteraction(SceneObjectView sender, Action<InteractionResult> sendResult)
        {
            _isRespondable = false;
            StartCoroutine(ResetResponse(5.0f));
        }

        private IEnumerator ResetResponse(float time)
        {
            yield return new WaitForSeconds(time);
            _isRespondable = true;
        }
    }
}
