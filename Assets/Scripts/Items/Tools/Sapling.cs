using Infrastructure.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Items
{
    public class Sapling : MonoBehaviour
    {
        [SerializeField]
        private ItemDataSO _saplingData;

        [SerializeField]
        private float _raycastLength = 5f;

        [SerializeField]
        private LayerMask _terrainLayer;

        private InputSystem _inputSystem;
        private EventManager _eventManager;
        private Camera _mainCamera;
        private GameObject _blueprint;

        private void Start()
        {
            _inputSystem = AllServices.Container.Single<InputSystem>();
            _eventManager = AllServices.Container.Single<EventManager>();

            _blueprint = Instantiate(_saplingData.blueprint);

            _mainCamera = Camera.main;
            _inputSystem.LMBClickAction += PlantTree;
        }
        private void OnDestroy()
        {
            _inputSystem.LMBClickAction -= PlantTree;
            Destroy(_blueprint.gameObject);
        }

        private void Update()
        {
            MoveBlueprint();
        }
        private void MoveBlueprint()
        {
            var ray = _mainCamera.ViewportPointToRay(Vector3.one * 0.5f);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _raycastLength, _terrainLayer))
            {
                _blueprint.transform.position = hit.point;
            }

        }
        private void PlantTree()
        {
            Instantiate(_saplingData.construction, _blueprint.transform.position, Quaternion.identity);
            _eventManager.SelectedItemUseAction?.Invoke();
            Destroy(gameObject);
        }
      
    }
}