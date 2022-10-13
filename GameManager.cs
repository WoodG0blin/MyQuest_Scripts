using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WG_Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] Transform _player;
        [SerializeField] Transform _camera;
        [SerializeField] SpawnPointView[] _spawnPoints;
        [SerializeField] Transform _canvas;

        private ListExecutable _executables;
        private EventManager _eventManager;
        private SpawnerController _spawnerController;
        private ResultController _resultController;
        private UIController _UIcontroller;
        public Controller _playerController;
        public Controller _cameraController;

        public EventManager Events { get => _eventManager; }
        public ResultController Results { get => _resultController; }
        public UIController DispayUI { get => _UIcontroller; }

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _UIcontroller = new UIController(_canvas);

            if (_camera == null) _camera = Camera.main.transform;
            _cameraController = new CameraController(_camera, _player);

            if (_player == null) _player = GameObject.Find("Player").transform;
            _playerController = new PlayerController(_player);

            _eventManager = new EventManager();

            _spawnerController = new SpawnerController(_spawnPoints);
            _resultController = new ResultController();

            _executables = new ListExecutable();
            _executables.Add(new InputManager(_playerController as IGetInput));
            _executables.Add(_playerController);
            _executables.Add(_cameraController);
            _executables.Add(_eventManager);
            _executables.Add(_spawnerController);
        }

        private void Update()
        {
            _executables.Execute(ExecuteType.Graphics);
        }

        private void FixedUpdate()
        {
            _executables.Execute(ExecuteType.Physics);
        }

        public void AddExecutable(IExecutable ex) { _executables.Add(ex); }
        public void RemoveExecutable(IExecutable ex) { _executables.Remove(ex); }

        public void RestartGame() { SceneManager.LoadScene(0); }
    }
}

