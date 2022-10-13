using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WG_Game
{
    public class SpawnController : Controller, IInteractable
    {
        private Item spawner;
        public SpawnController(Transform transform) : base(transform) { }

        public bool IsRespondable { get => spawner.IsRespondable; }

        protected override void Initiate()
        {
            base.Initiate();
            _executeType = ExecuteType.None;
            if (_transform != null) spawner = _transform.GetComponent<Item>();
        }

        public void RequestResponse(Action<InteractionResult> SendResult)
        {
            Initiate();
            spawner.RequestResponse(SendResult);
            //SendResult(null);
        }
    }
}
