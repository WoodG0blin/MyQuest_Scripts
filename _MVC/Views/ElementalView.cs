using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WG_Game
{
    public class ElementalView : InteractableView
    {
        [SerializeField] private Elements _element;
        private Animator _animator;
        public Elements Element { get => _element; }
        public Animator ViewAnimator { get => _animator; }

        public event Action<SceneObjectView, Action<InteractionResult>> OnInteraction;

        public override void Awake()
        {
            base.Awake();
            ViewTransform.GetChild(0).TryGetComponent<Animator>(out _animator);
        }

        public override void RequestInteraction(SceneObjectView sender, Action<InteractionResult> sendResult)
        {
            if(IsRespondable) OnInteraction?.Invoke(sender, sendResult);
            base.RequestInteraction(sender, sendResult);
        }

        public void SetActive(bool b) { _animator?.SetBool("Active", b); }
        public void SetAttack() { _animator?.SetTrigger("Attack"); }

        public void ResetResponse(bool b) { _isRespondable = b; } 
    }
}