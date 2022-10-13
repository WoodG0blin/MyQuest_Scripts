using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace WG_Game
{
    public class PlayerController : Controller, IGetInput, IDamagable
    {
#region FIELDS
        private CharacterController controller;
        private Animator animator;
        private Vector3 _inputDirection;
        private Vector3 prevMoveDirection;
        private Vector3 moveDirection;
        private Vector3 rotateDirection;
        private Vector3 lookDirection;
        private float gravityForce = 9.81f;
        private float verticalMove;

        Camera _camera;
        CameraController camera_Controller;
        private float cameraDistance;
        private Quaternion lookRotation;

        private Volume postEffects;
        private DepthOfField focus;
        private MinFloatParameter basicFocusDistance = new MinFloatParameter(30.0f, 0.0f);
        private MinFloatParameter newFocusDistance = new MinFloatParameter(30.0f, 0.0f);

        private bool isActive = true;
        private Transform look;
        private float acceleration = 1;

        private float playerMoveSpeed = 3;
        private float currentSpeed = 0;
        private float animSpeed = 0;
        private float playerRotateSpeed = 50f;
        private float accelerationCoeff = 3.0f;
        private float inertia = 5.0f;
        private float jumpForce = 5.0f;

        private bool jump = false;
        private bool InAir = false;
        private bool OnMove = false;

        private bool isInside = false;

        float senseRadius = 5.0f;
        private LayerMask mask;
        private bool gotInteraction;
        private Transform interactTarget;
        private bool interact;

        private GameObject ragdoll;

        private float concentration = 100.0f;
        #endregion

        #region PROPERTIES
        public bool IsInside
        {
            get => isInside;
            set
            {
                isInside = value;
                camera_Controller.IsInside = value;
            }
        }
        public float SenseRadius { get => senseRadius; }
        public Transform InteractTarget { get => gotInteraction ? interactTarget : null; }
        public float Concentration { get => concentration; }
        public float Health { get => concentration; }
        private float camDist { get => cameraDistance; set => cameraDistance = Mathf.Clamp(value, camera_Controller.MinDistance, camera_Controller.MaxDistance); }

        public event Action<string> OnDamage;
        public event Action OnDeath;
        public event Action<string> OnInfo;
        #endregion

        public PlayerController(Transform transform) : base(transform)
        {
            _executeType = ExecuteType.Both;

            controller = _transform.GetComponent<CharacterController>();
            animator = _transform.GetComponentInChildren<Animator>();
            look = _transform.GetChild(0);

            (_sceneObject as PlayerView).OnCollect += GetInteractionResult;

            _inputDirection = Vector3.zero;
            rotateDirection = Vector3.zero;
            lookDirection = _transform.forward;

            _camera = Camera.main;
            camera_Controller = _gameController._cameraController as CameraController;

            if (!postEffects) postEffects = _transform.GetComponentInChildren<Volume>();
            postEffects.sharedProfile.TryGet<DepthOfField>(out focus);
            focus.focusDistance.SetValue(basicFocusDistance);

            mask = LayerMask.GetMask("Interactable");
            ragdoll = _transform.Find("Ragdoll").gameObject;
            ragdoll.SetActive(false);

            OnDamage += (string val) => { _gameController.DispayUI.Display(UIController.DisplayParameter.Concentration, val); };
            OnDeath += _gameController.DispayUI.GameOver;
            OnInfo += (string val) => { _gameController.DispayUI.Display(UIController.DisplayParameter.Message, val); };
        }

        #region PUBLIC METHODS
        public void GetDamage(float damage)
        {
            concentration -= damage;
            OnDamage?.Invoke($"{concentration:F0}");
            if (concentration < 0) Faint();
        }

        public void SetInput(InputManager.InputData inputData)
        {
            rotateDirection.y = inputData.RotateDirectionY;

            _inputDirection = inputData.InputDirection.y * _transform.forward + inputData.InputDirection.x * _transform.right;
            OnMove = _inputDirection.magnitude > 0;

            if (inputData.Accelerating) acceleration = accelerationCoeff;
            else acceleration = 1;

            if (currentSpeed != playerMoveSpeed * acceleration * (OnMove ? 1 : 0))
            {
                currentSpeed = currentSpeed + Mathf.Sign(playerMoveSpeed * acceleration * (OnMove ? 1 : 0) - currentSpeed) * Time.deltaTime * inertia;
                currentSpeed = Mathf.Clamp(currentSpeed, 0, currentSpeed);
            }

            if (inputData.Jump && controller.isGrounded)
            {
                jump = true;
                animator.SetTrigger("Jump");
            }

            if (inputData.Interact && interactTarget) interact = true;

            lookDirection.y += (Mathf.Clamp(inputData.LookDirectionY, -1.0f, 1.0f) * Time.deltaTime);
            camDist -= inputData.LookDist;

        }

         public Transform GetCameraLook() { return look; }
        #endregion

        #region PRIVATE METHODS

        protected override void Update1()
        {
            if (isActive)
            {
                //ReadInput();
                SetPlayerPosition();
                SetLook();
            }
        }

        protected override void FixedUpdate1()
        {
            if (isActive)
            {
                ReadEnvironment();
                controller.Move(moveDirection * Time.deltaTime);
                animator.SetFloat("Speed", animSpeed);
                TakeActions();
            }
        }

        private void SetPlayerPosition()
        {
            _transform.Rotate(rotateDirection * playerRotateSpeed * Time.deltaTime);

            if (jump && !InAir)
            {
                verticalMove = jumpForce;
                (_gameController as MonoBehaviour).StartCoroutine(AwaitLanding());
                jump = false;
            }

            verticalMove -= gravityForce * Time.deltaTime;
            if (verticalMove < -gravityForce) verticalMove = -gravityForce;

            moveDirection = (OnMove ? _inputDirection : prevMoveDirection);
            prevMoveDirection = moveDirection;

            animSpeed = Vector3.Dot(_transform.forward, moveDirection.normalized) < -0.2f ? -currentSpeed / 2 : currentSpeed;

            moveDirection = moveDirection.normalized * Mathf.Abs(animSpeed);
            moveDirection.y += verticalMove;
        }

        private void SetLook()
        {
            lookDirection.x = _transform.forward.x;
            lookDirection.z = _transform.forward.z;
            lookRotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
            look.rotation = lookRotation;

            camera_Controller.Distance = camDist;
        }


        private void ReadEnvironment()
        {
            List<Collider> hits = new List<Collider>();
            hits.AddRange(Physics.OverlapSphere(look.transform.position, senseRadius, mask));

            if (hits.Count > 0)
            {
                interactTarget = GetNearestInteractable(hits);
                if (interactTarget)
                {
                    //newFocusDistance.value = Vector3.Distance(interactTarget.position, _camera.transform.position);
                    //focus.focusDistance.SetValue(newFocusDistance);

                    //MonoBehaviour m = TryFindInteractable(interactTarget.GetComponents<MonoBehaviour>());
                    //if (m is IInteractable && (m as IInteractable).IsRespondable)
                    //{
                        gotInteraction = true;
                        OnInfo.Invoke($"Press F to interact with {interactTarget.name}");
                        return;
                    //}
                }
            }
            interactTarget = null;
            gotInteraction = false;
            OnInfo.Invoke("");
            //focus.focusDistance.SetValue(basicFocusDistance);
        }

        private void TakeActions()
        {
            if (interact && interactTarget)
            {
                //(TryFindInteractable(interactTarget.GetComponents<MonoBehaviour>()) as IInteractable).RequestResponse();
                //_gameController.Events.RegisterEvent(new GameEvent(GameEvent.EventType.Interaction, this, InteractTarget));
                interactTarget.GetComponent<IInteract>().RequestInteraction(_transform.GetComponent<SceneObjectView>(), GetInteractionResult);
                interact = false;
            }
        }

        private void Faint()
        {
            _transform.Find("Controller").gameObject.SetActive(false);
            isActive = false;
            _transform.GetComponent<CharacterController>().enabled = false;
            animator.enabled = false;
            ragdoll.SetActive(true);
            ragdoll.transform.position = _transform.position - Vector3.up;
            ragdoll.transform.rotation = _transform.rotation;
            look.transform.LookAt(ragdoll.transform);

            OnDeath.Invoke();
        }

        private void GetInteractionResult(InteractionResult result)
        {
            Debug.Log(result.Message);
            GetDamage(-result.EnergyImpact);
        }

        //private void OnGUI()
        //{
        //    if (gotInteraction && InteractTarget != null)
        //    {
        //        Vector2 targ = _camera.WorldToScreenPoint(interactTarget.position);

        //        MonoBehaviour m = interactTarget.GetComponent<MonoBehaviour>();

        //        string message = $"Press F to interact with {interactTarget.name}";
        //        if (m is IDamagable) message += $"\n health: {(m as IDamagable).Health:F2}";

        //        GUI.Label(new Rect(targ.x - 150, Screen.height - targ.y - 110, 300, 50), message);
        //    }
        //}

        private Transform GetNearestInteractable(List<Collider> list)
        {
            float euristic = senseRadius;
            float minDistance = senseRadius;
            int index = -1;

            for (int i = 0; i < list.Count; i++)
            {
                euristic = InteractableDistEuristic(Vector3.Distance(list[i].transform.position, look.transform.position), InSight(list[i].transform));
                if (euristic < minDistance)
                {
                    minDistance = euristic;
                    index = i;
                }
            }
            return index > -1 ? list[index].transform : null;
        }

        private float InteractableDistEuristic(float dist, float sight)
        {
            float lookcoeff = 2.0f;
            return dist - sight * lookcoeff;
        }

        private float InSight(Transform target)
        {
            return Vector3.Dot(look.transform.forward, (target.position - _transform.position).normalized);
        }

        private MonoBehaviour TryFindInteractable(MonoBehaviour[] arr)
        {
            for (int i = 0; i < arr.Length; i++) if (arr[i] is IInteractable) return arr[i];
            return arr[0];
        }
        #endregion

        IEnumerator AwaitLanding()
        {
            InAir = true;
            yield return new WaitForFixedUpdate();
            while (!controller.isGrounded) yield return null;
            InAir = false;
            animator.SetTrigger("Land");
        }

    }
}
