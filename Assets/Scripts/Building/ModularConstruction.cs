using Infrastructure.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Building
{
    public class ModularConstruction : MonoBehaviour, IConstruction
    {
        [SerializeField]
        private float _raycastLength = 5;
        [SerializeField]
        private LayerMask _terrainLayer;
        [SerializeField]
        private LayerMask _connectionPointLayer;
        [SerializeField]
        private ItemDataSO.ConstructionType _constructionType;
        [SerializeField]
        private ItemDataSO _constructionData;
        [SerializeField]
        private float _rotationSpeed = 100f;

        private GameObject _blueprint;
        private GameObject _construction;
        private Collider _priorityPoint;

        private Camera _mainCamera;
        private InputSystem _inputSystem;
        private EventManager _eventManager;
        private void Start()
        {
            _inputSystem = AllServices.Container.Single<InputSystem>();
            _eventManager = AllServices.Container.Single<EventManager>();

            _blueprint = Instantiate(_constructionData.blueprint);

            _mainCamera = Camera.main;
            _inputSystem.LMBClickAction += Construct;
        }
        private void OnDestroy()
        {
            _inputSystem.LMBClickAction -= Construct;

            Destroy(_blueprint.gameObject);
        }
        private void Update()
        {
            MoveBlueprint();
        }
        public void Construct()
        {
            _construction = Instantiate(_constructionData.construction, _blueprint.transform.position, _blueprint.transform.rotation);
            if (_priorityPoint != null)
            {
                Collider[] targets = Physics.OverlapSphere(_priorityPoint.transform.position, 0.3f, _connectionPointLayer);
                foreach (Collider target in targets) Destroy(target.gameObject);
            }
            _eventManager.SelectedItemUseAction?.Invoke();
        }

        public void MoveBlueprint()
        {
            var ray = _mainCamera.ViewportPointToRay(Vector3.one * 0.5f);
            RaycastHit hit;

            Vector3 checkPoint = _mainCamera.transform.position + _mainCamera.transform.forward * 5;
            Collider[] targetPoints = Physics.OverlapCapsule(_mainCamera.transform.position, checkPoint, 3f, _connectionPointLayer);

            if (targetPoints.Length != 0)
            {
                List<Collider> targets = new List<Collider>();
                foreach (Collider targetPoint in targetPoints) if (targetPoint.GetComponent<ConnectionPoint>().GetPointType() == _constructionType) targets.Add(targetPoint);

                if (targets.Count != 0)
                {
                    Collider priorityTarget = targets[0];

                    foreach (Collider target in targets)
                    {
                        Vector3 cameraPos = _mainCamera.transform.position;

                        float angleToTarget = Vector3.Angle((cameraPos + _mainCamera.transform.forward) - cameraPos, target.transform.position - cameraPos);
                        float angleToPrevious = Vector3.Angle((cameraPos + _mainCamera.transform.forward) - cameraPos, priorityTarget.transform.position - cameraPos);

                        if (angleToTarget < angleToPrevious) priorityTarget = target;
                    }
                    _priorityPoint = priorityTarget;
                    _blueprint.transform.position = priorityTarget.transform.position;
                    _blueprint.transform.rotation = priorityTarget.transform.rotation;
                }
            }
            else
            {
                if (Physics.Raycast(ray, out hit, _raycastLength, _terrainLayer))
                {
                    _blueprint.transform.position = hit.point;

                    if (!_constructionData.verticalConstruction)
                    {
                        Vector3 newRotation;


                        newRotation = (Quaternion.FromToRotation(_blueprint.transform.up, hit.normal) * _blueprint.transform.rotation).eulerAngles;
                        newRotation.y = _blueprint.transform.rotation.eulerAngles.y;

                        _blueprint.transform.rotation = Quaternion.Euler(newRotation);
                    }
                }
            }
        }
        public bool CanBeConstructed()
        {
            return true;
        }
    }
}
