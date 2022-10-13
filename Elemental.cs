using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WG_Game
{
    public class Elemental : MonoBehaviour, IInteractable, IDamagable
    {
        [SerializeField] private SO_ElementalsStats baseStats;

        [SerializeField] protected Elements type;
        Animator animator;
        protected float health;
        protected int level;
        protected bool responded;
        protected bool isAttacking;

        protected Transform target;
        public bool IsRespondable { get => !responded; }

        public float Health { get => health; }
        public int Level { get => level; set => level = value > 1 ? value : 1; }
        public Elements Type { get => type; }

        float elapsedTime = 0;
        float rndTime = 3.0f;

        public void Initiate(Elements element, int level = 1, float flyHeight = 1.0f)
        {
            animator = transform.GetComponentInChildren<Animator>();

            type = element;
            responded = true;
            health = baseStats.Health * (1 + UnityEngine.Random.Range(level / 10 - 0.05f, level / 10 + 0.1f));
            transform.localScale = Vector3.one * 0.5f + Vector3.one * level / 10;
            target = GameObject.FindGameObjectWithTag("Player").transform;
            StartCoroutine(FlyOut(1.0f, flyHeight));
            animator.SetBool("Active", true);
        }


        void FixedUpdate()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > rndTime)
            {
                elapsedTime -= rndTime;
                rndTime = UnityEngine.Random.Range(1.0f, 5.0f);
                animator.SetTrigger("Attack");
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position, Vector3.up), Time.deltaTime);
        }

        public void GetDamage(float damage)
        {

        }

        public virtual void RequestResponse(Action<InteractionResult> SendResult)
        {
            if (!responded)
            {
                //target.GetComponent<PlayerController>().GetDamage(110);
                StartCoroutine(Kill(2.0f));
                animator.SetBool("Active", false);
                responded = true;
                //SendResult(new InteractionResult($"{type} elemental responded", InteractionResult.ActionType.indirect));
            }
        }

        private IEnumerator FlyOut(float t, float height)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = transform.position + Vector3.up * height;
            Quaternion startingRotation = transform.localRotation;
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
            float elapsedTime = 0;

            while (elapsedTime < t)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, endPos, 1.5f * elapsedTime / t);

                targetRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
                transform.localRotation = Quaternion.Slerp(startingRotation, targetRotation, elapsedTime / t);

                yield return new WaitForFixedUpdate();
            }
            transform.localRotation = targetRotation;
            responded = false;
        }

        private IEnumerator Kill(float t)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = transform.position - Vector3.up * 2.0f;
            float elapsedTime = 0;

            while (elapsedTime < t)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / t);
                yield return new WaitForFixedUpdate();
            }
            Destroy(gameObject);
        }
    }
}
