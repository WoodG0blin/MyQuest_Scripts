using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WG_Game
{
    public class PlayerIKAnimation : MonoBehaviour
    {
        private PlayerController _controller;
        private Animator _animator;
        private float senseRadius;
        private Transform interactTarget;
        private Vector3 interactTargetPosition;
        [SerializeField] private float weight;

        void Start()
        {
            _animator = GetComponent<Animator>();
            _controller = GetComponentInParent<PlayerController>();
            senseRadius = _controller.SenseRadius;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (interactTarget != _controller.InteractTarget)
            {
                interactTarget = _controller.InteractTarget;
                StartCoroutine(ResetTarget(0.5f, interactTarget.position));
            }

            if (interactTarget && Vector3.Dot(interactTarget.position - transform.position, transform.forward) > 0.1f)
            {
                weight = SetWeight(1 - Vector3.Distance(transform.position, interactTargetPosition) / senseRadius);

                _animator.SetLookAtWeight(weight);
                _animator.SetLookAtPosition(interactTargetPosition);

                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, weight);
                _animator.SetIKPosition(AvatarIKGoal.RightHand, interactTargetPosition);
                _animator.SetIKRotation(AvatarIKGoal.RightHand, transform.rotation);
            }
            else
            {
                weight = SetWeight(0);

                _animator.SetLookAtWeight(weight);
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, weight);
                _animator.SetIKPosition(AvatarIKGoal.RightHand, interactTargetPosition);
                _animator.SetIKRotation(AvatarIKGoal.RightHand, transform.rotation);
            }
        }

        private float SetWeight(float targetWeight)
        {
            return Mathf.Lerp(weight, targetWeight, Time.deltaTime * 3.0f);
        }

        private IEnumerator ResetTarget(float t, Vector3 newTargetposition)
        {
            yield return new WaitForSeconds(t);
            interactTargetPosition = newTargetposition;
        }
    }
}
