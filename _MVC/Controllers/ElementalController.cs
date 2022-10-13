using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WG_Game
{
    public class ElementalController : Controller
    {
        private ElementalView _elemental;
        private SceneObjectView _target;

        Action<InteractionResult> _sendResult;

        protected float health;
        protected int level;
        protected bool responded;
        protected bool isAttacking;

        float elapsedTime = 0;
        float rndTime = 3.0f;

        public event Action OnDestroy;

        public ElementalController(ElementalView elementalView, Action onDestroy) : base(elementalView.ViewTransform)
        {
            _executeType = ExecuteType.Physics;
            _gameController.AddExecutable(this);

            _elemental = elementalView;
            
            OnDestroy += onDestroy;
        }

        public void Activate(Vector3 initPosition, SceneObjectView target, int level = 1, float flyHeight = 1.0f)
        {
            _elemental.ViewTransform.position = initPosition;
            _elemental.ViewTransform.gameObject.SetActive(true);

            _target = target;

            this.level = level;
            _elemental.ViewTransform.localScale = Vector3.one * 0.5f + Vector3.one * level / 10;
            health = 100.0f * (1 + UnityEngine.Random.Range(level / 10 - 0.05f, level / 10 + 0.1f));

            _elemental.OnInteraction += OnInteraction;

            _elemental.ResetResponse(true);
            _elemental.StartCoroutine(FlyOut(1.0f, flyHeight));
            _elemental.SetActive(true);
        }

        protected override void FixedUpdate1()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > rndTime)
            {
                elapsedTime -= rndTime;
                rndTime = UnityEngine.Random.Range(1.0f, 5.0f);
                _elemental.SetAttack();
            }
            transform.rotation = Quaternion.Slerp(_elemental.ViewTransform.rotation, Quaternion.LookRotation(_target.ViewTransform.position - _elemental.ViewTransform.position, Vector3.up), Time.deltaTime);
        }

        private void OnInteraction(SceneObjectView sender, Action<InteractionResult> sendResult)
        {
            _target = sender;
            _sendResult = sendResult;
            _elemental.StartCoroutine(Kill(2.0f, FormInteractionResults));
            _elemental.SetActive(false);
            _elemental.ResetResponse(false);
        }

        private void FormInteractionResults()
        {
            (InteractionResult?, InteractionResult?) results = ResultsModel.CalculateResults($"{_elemental.ViewTransform.name} throws interaction result");
            if (results.Item2 != null) { _gameController.Results.CreateNewResult(results.Item2.Value, _elemental, _target, _sendResult); }
            if (results.Item1 != null) { _sendResult(results.Item1.Value); }
        }

        private IEnumerator FlyOut(float t, float height)
        {
            Vector3 startPos = _elemental.ViewTransform.position;
            Vector3 endPos = _elemental.ViewTransform.position + Vector3.up * height;
            Quaternion startingRotation = _elemental.ViewTransform.localRotation;
            Quaternion targetRotation = Quaternion.LookRotation(_target.ViewTransform.position - _elemental.ViewTransform.position, Vector3.up);
            float elapsedTime = 0;

            while (elapsedTime < t)
            {
                elapsedTime += Time.deltaTime;
                _elemental.ViewTransform.position = Vector3.Lerp(startPos, endPos, 1.5f * elapsedTime / t);

                targetRotation = Quaternion.LookRotation(_target.ViewTransform.position - _elemental.ViewTransform.position, Vector3.up);
                _elemental.ViewTransform.localRotation = Quaternion.Slerp(startingRotation, targetRotation, elapsedTime / t);

                yield return new WaitForFixedUpdate();
            }
            _elemental.ViewTransform.localRotation = targetRotation;
            _elemental.ResetResponse(true);
        }

        private IEnumerator Kill(float t, Action pushResults)
        {
            //TODO: results will be formed after interaction level
            pushResults();

            Vector3 startPos = _elemental.ViewTransform.position;
            Vector3 endPos = _elemental.ViewTransform.position - Vector3.up * 2.0f;
            float elapsedTime = 0;

            while (elapsedTime < t)
            {
                elapsedTime += Time.deltaTime;
                _elemental.ViewTransform.position = Vector3.Lerp(startPos, endPos, elapsedTime / t);
                yield return new WaitForFixedUpdate();
            }

            Destroy();
        }

        private void Destroy()
        {
            _gameController.RemoveExecutable(this);
            _elemental.ViewTransform.gameObject.SetActive(false);
            _elemental.OnInteraction -= OnInteraction;
            OnDestroy?.Invoke();
        }
    }
}
