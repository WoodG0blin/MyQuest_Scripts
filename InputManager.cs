using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ClipperLib;

namespace WG_Game
{
    public class InputManager : IExecutable
    {
        private ExecuteType _executeType = ExecuteType.Graphics;
        public ExecuteType ExType { get => _executeType; }

        public struct InputData
        {
            public Vector2 InputDirection;
            public float RotateDirectionY;
            public float LookDirectionY;
            public bool Accelerating;
            public bool Jump;
            public bool Interact;
            public float LookDist;
        }

        private List<IGetInput> _receivers;

        public InputManager (params IGetInput[] receivers)
        {
            _receivers = new List<IGetInput> ();
            _receivers.AddRange(receivers);
        }

        void Update()
        {
            InputData _input = new InputData();

            _input.RotateDirectionY = Input.GetAxis("Mouse X");

            _input.InputDirection.x = Input.GetAxis("Horizontal");
            _input.InputDirection.y = Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.LeftShift)) _input.Accelerating = true;
            //if (Input.GetKeyUp(KeyCode.LeftShift)) _input.Accelerating = false;

            if (Input.GetKeyDown(KeyCode.Space)) _input.Jump = true;

            if (Input.GetKeyDown(KeyCode.F)) _input.Interact = true;

            _input.LookDirectionY = Input.GetAxis("Mouse Y");
            _input.LookDist = Input.GetAxis("Mouse ScrollWheel");

            foreach(IGetInput receiver in _receivers) receiver.SetInput(_input);
        }

        public Action GetExecute(ExecuteType t) { return Update; }
    }
}
