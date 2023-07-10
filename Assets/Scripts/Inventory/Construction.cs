using Infrastructure.Services;
using UnityEngine;

namespace Inventory
{
    public class Construction : MonoBehaviour
    {
        [SerializeField]
        private InventoryManager _inventoryManager;
        [SerializeField]
        private float _raycastLength = 5;
        [SerializeField]
        private LayerMask _terrainLayer;
        [SerializeField] 
        private ConstructionSO _constructionData;

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
            Instantiate(_constructionData.construction, _blueprint.transform.position, _blueprint.transform.rotation);
            _eventManager.SelectedItemUseAction?.Invoke();
            Destroy(gameObject);
        }
    }
}