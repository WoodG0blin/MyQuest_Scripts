using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace WG_Game
{
    public class AnimatedController : Controller, IInteractable
    {
        public AnimatedController(Transform transform) : base(transform) { }

        protected Animator _animator;
        public bool IsRespondable { get => _animator.GetBool("IsRespondable"); }

        protected override void Initiate()
        {
            base.Initiate();
            _executeType = ExecuteType.None;
            if(_transform != null)
                if(!_transform.TryGetComponent<Animator>(out _animator)) _animator = _transform.GetComponentInParent<Animator>();
        }

        public void RequestResponse(Action<InteractionResult> SendResult)
        {
            Initiate();
            if (IsRespondable)
            {
                _animator.SetTrigger("Activate");
                _animator.SetBool("IsRespondable", false);
                (_gameController as MonoBehaviour).StartCoroutine(ResetResponse(2.0f, SendResult));
            }
        }

        private IEnumerator ResetResponse(float t, Action<InteractionResult> sendResult)
        {
            yield return new WaitForSeconds(t);
            _animator.SetBool("IsRespondable", true);
            //sendResult(new InteractionResult("AnimatedControllerResult"));
        }
    }
}
