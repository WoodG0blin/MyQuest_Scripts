using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WG_Game
{
    public class SpawnPointView : InteractableView
    {
        private int _id;
        [SerializeField] private Elements _element;
        public int ID { get => _id; set => _id = value; }
        public Elements Element { get => _element; }

        public event Action<int, SceneObjectView, Action<InteractionResult>> OnInteraction;
        public event Action<int, SceneObjectView> OnTrigger;

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.TryGetComponent<SceneObjectView>(out SceneObjectView temp)) OnTrigger?.Invoke(_id, temp);
        }

        public override void RequestInteraction(SceneObjectView sender, Action<InteractionResult> sendResult)
        {
            if(IsRespondable) OnInteraction?.Invoke(_id, sender, sendResult);
            base.RequestInteraction(sender, sendResult);
        }
    }
}
