using UnityEngine;

namespace WG_Game
{
    public abstract class SceneObjectView : MonoBehaviour
    {
        private Transform _transform;
        private Collider _collider;
        private Renderer _renderer;

        public Transform ViewTransform { get => _transform; set { if (_transform == null) _transform = value; } }
        public Collider ViewCollider { get => _collider; set { if (_collider == null) _collider = value; } }
        public Renderer ViewRenderer { get => _renderer; set { if (_renderer == null) _renderer = value; } }

        public void SetGameObject(GameObject obj)
        {
            _transform = obj.transform;
            obj.TryGetComponent<Collider>(out _collider);
            obj.TryGetComponent<Renderer>(out _renderer);
        }

        public virtual void Awake()
        {
            _transform = this.transform;
            TryGetComponent<Collider>(out _collider);
            TryGetComponent<Renderer>(out _renderer);
        }
    }
}
