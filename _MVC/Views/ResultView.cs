using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WG_Game
{
    public class ResultView : SceneObjectView
    {
        public event Action<Action<InteractionResult>> OnCollect;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent<SceneObjectView>(out SceneObjectView obj))
                if (obj is ICollect c) OnCollect?.Invoke(c.Collect);
        }
    }
}
