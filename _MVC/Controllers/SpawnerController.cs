using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WG_Game
{
    public class SpawnerController : Controller, IExecutable
    {
        private List<SpawnPointView> _spawnPoints;
        private Dictionary<Elements, GameObject> _elementalsPull;
        private ElementalController _elementalController;

        public List<SpawnPointView> SpawnPoints { get => _spawnPoints; }

        public SpawnerController(SpawnPointView[] spawnPoints) : base()
        {
            _executeType = ExecuteType.None;
            _spawnPoints = new List<SpawnPointView>();
            _spawnPoints.AddRange(spawnPoints);
            for(int i = 0; i < _spawnPoints.Count; i++)
            {
                _spawnPoints[i].ID = i;
                _spawnPoints[i].OnTrigger += OnTrigger;
                _spawnPoints[i].OnInteraction += OnInteraction;
            }
            InitiateElementalsPull();
        }

        private void OnTrigger(int id, SceneObjectView sender)
        {
            Debug.Log($"Trigger set by {sender.ViewTransform.name} at {_spawnPoints[id].name}");
        }

        private void OnInteraction(int id, SceneObjectView sender, Action<InteractionResult> sendResult)
        {
            _elementalController = new ElementalController(_elementalsPull[_spawnPoints[id].Element].GetComponent<ElementalView>(), OnDestroyElemental);
            _elementalController?.Activate(_spawnPoints[id].ViewTransform.position, sender, flyHeight: _spawnPoints[id].ViewTransform.localScale.y);
        }

        private void InitiateElementalsPull()
        {
            _elementalsPull = new Dictionary<Elements, GameObject>();
            for (int i = 0; i < 6; i++)
            {
                _elementalsPull.Add((Elements)i, GameObject.Instantiate(Resources.Load<GameObject>($"Prefabs/Elementals/{(Elements)i}Elemental")));
                _elementalsPull[(Elements)i].SetActive(false);
            }
        }

        private void OnDestroyElemental()
        {
            _elementalController = null;
        }
    }
}
