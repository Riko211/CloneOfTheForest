using Infrastructure.Services;
using System.Collections;
using UnityEngine;

namespace Inventory
{
    public class Construction : MonoBehaviour
    {
        [SerializeField]
        private float _raycastLength = 5;
        [SerializeField]
        private LayerMask _terrainLayer;
        [SerializeField] 
        private ConstructionSO _constructionData;
        [SerializeField]
        private float _rotationSpeed = 100f;

        private GameObject _blueprint;
        private GameObject _construction;

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

            _inputSystem.StartRotateConstructionAction += StartRotateBlueprint;
            _inputSystem.StopRotateConstructionAction += StopRotateBlueprint;
        }
        private void OnDestroy()
        {
            _inputSystem.LMBClickAction -= Construct;

            _inputSystem.StartRotateConstructionAction -= StartRotateBlueprint;
            _inputSystem.StopRotateConstructionAction -= StopRotateBlueprint;

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
        private void Construct()
        {
            _construction = Instantiate(_constructionData.construction, _blueprint.transform.position, _blueprint.transform.rotation);
            _eventManager.SelectedItemUseAction?.Invoke();
        }
        private void StartRotateBlueprint()
        {
            StartCoroutine(RotateBlueprint());
        }
        private void StopRotateBlueprint()
        {
            StopAllCoroutines();
        }

        private IEnumerator RotateBlueprint()
        {
            Vector3 curRotation;
            while (true)
            {
                curRotation = _blueprint.transform.rotation.eulerAngles;
                curRotation.y += _rotationSpeed * Time.deltaTime;
                Debug.Log(curRotation);
                _blueprint.transform.rotation = Quaternion.Euler(curRotation);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}