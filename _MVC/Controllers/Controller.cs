using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace WG_Game
{
    public abstract class Controller : IExecutable
    {
        protected Transform _transform;
        protected SceneObjectView _sceneObject;
        protected GameManager _gameController;
        protected ExecuteType _executeType;
        public  ExecuteType ExType { get => _executeType; }
        public Transform transform { get => _transform; set {if(_transform == null) _transform = value; } }

        public Controller()
        {
            _gameController = GameObject.Find("GameController").GetComponent<GameManager>();
            _executeType = ExecuteType.Graphics;
        }
        public Controller(Transform transform) : this()
        {
            _transform = transform;
            _transform.TryGetComponent<SceneObjectView>(out _sceneObject);
        }


        protected virtual void Initiate() { }

        protected virtual void Update1()
        {

        }

        protected virtual void FixedUpdate1()
        {

        }

        public Action GetExecute(ExecuteType t)
        {
            return t == 0 ? () => Update1() : () => FixedUpdate1();
        }
    }
}
