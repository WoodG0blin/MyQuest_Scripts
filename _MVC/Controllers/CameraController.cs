using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WG_Game
{
    public class CameraController : Controller
    {
        private Transform _player;
        Transform look;
        private float adjustSpeed = 100;
        bool isInMainMenu;
        private float distance;
        private Vector3 offset;
        private bool isInside = false;
        private bool isAdapting = false;

        private float minDistance = 0.5f;
        private float maxDistance = 5.0f;

        public CameraController(Transform transform, Transform player) : base(transform)
        {
            _player = player;
            look = _player.Find("Look");
            isInMainMenu = false;
            _executeType = ExecuteType.Physics;
        }

        public float MinDistance { get => minDistance; }
        public float MaxDistance { get => maxDistance; }

        public bool IsInside
        {
            set
            {
                isInside = value;
                if (isInside) (_gameController as MonoBehaviour).StartCoroutine(SmoothDistance(Distance, 0f, 0.5f));
                else (_gameController as MonoBehaviour).StartCoroutine(SmoothDistance(Distance, minDistance, 0.5f));
            }
        }


        public float Distance
        {
            get => distance;
            set
            {
                float val = Mathf.Clamp(value, minDistance, maxDistance);

                if (!isInside && !isAdapting)
                {
                    if (Mathf.Abs(distance - val) > minDistance) (_gameController as MonoBehaviour).StartCoroutine(SmoothDistance(distance, val, Mathf.Abs(distance - val)));
                    else distance = val;
                }
            }
        }
        public Vector3 Forward { get => _transform.forward; }

        protected override void FixedUpdate1()
        {
            if (isInMainMenu)
            {
                _transform.RotateAround(look.position, look.up, Time.fixedDeltaTime * 5.0f);
            }
            else if (look != null)
            {
                offset = (Vector3.up / 3 - look.forward) * distance;
                _transform.position = Vector3.Lerp(_transform.position, look.position + offset, adjustSpeed * Time.deltaTime);
                _transform.rotation = Quaternion.Slerp(_transform.rotation, look.rotation, adjustSpeed * Time.deltaTime);
            }
        }

        public void SetCamera(Transform target, bool mainMenu)
        {
            look = target;
            isInMainMenu = mainMenu;
            if (isInMainMenu)
            {
                _transform.position = look.position + new Vector3(0, 1, -6);
                _transform.rotation = Quaternion.identity;
            }
        }

        private IEnumerator SmoothDistance(float from, float to, float t)
        {
            isAdapting = true;
            float _t = t;
            while (_t > 0)
            {
                _t -= Time.deltaTime;
                distance = Mathf.Lerp(from, to, 1 - _t / t);
                yield return null;
            }
            distance = to;
            isAdapting = false;
        }
    }
}
