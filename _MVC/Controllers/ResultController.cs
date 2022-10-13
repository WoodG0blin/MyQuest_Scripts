using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WG_Game
{
    public class ResultController : Controller
    {
        private InteractionResult _result;
        private ResultView _resultView;

        private Dictionary<ResultType, GameObject> _bonusInstances;

        public ResultController() : base()
        {
            _executeType = ExecuteType.None;

            _bonusInstances = new Dictionary<ResultType, GameObject>();
            _bonusInstances.Add(ResultType.Good, GameObject.Instantiate(Resources.Load<GameObject>($"Prefabs/GoodBonus")));
            _bonusInstances.Add(ResultType.Bad, GameObject.Instantiate(Resources.Load<GameObject>($"Prefabs/BadBonus")));

            _bonusInstances[ResultType.Bad].SetActive(false);
            _bonusInstances[ResultType.Good].SetActive(false);
        }

        public void CreateNewResult(InteractionResult result, SceneObjectView source, SceneObjectView target, Action<InteractionResult> sendResult)
        {
            _result = result;
            _resultView = _bonusInstances[_result.rType].GetComponent<ResultView>();

            //TODO - make flying out
            _resultView.ViewTransform.position = Vector3.Lerp(source.ViewTransform.position, target.ViewTransform.position, 0.5f);
            _resultView.ViewTransform.gameObject.SetActive(true);

            _resultView.OnCollect += (Action<InteractionResult> res) => { DestroyCurrentResult(); res.Invoke(_result); };
        }

        private void DestroyCurrentResult()
        {
            _resultView?.ViewTransform.gameObject.SetActive(false);
            _resultView.OnCollect -= (Action<InteractionResult> res) => { DestroyCurrentResult(); res.Invoke(_result); };
            _resultView = null;
        }
    }
}
