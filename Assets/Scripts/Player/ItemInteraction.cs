using Infrastructure.Services;
using Inventory;
using UnityEngine;

namespace Player.Items
{
    public class ItemInteraction : MonoBehaviour
    {
        [SerializeField]
        private InventoryManager _inventoryManager;

        [SerializeField]
        private float _raycastLength = 2.5f;
        [SerializeField]
        private LayerMask _interactableItemLayer;

        private Camera _mainCamera;
        private InputSystem _inputSystem;

        private void Start()
        {
            _mainCamera = Camera.main;
            _inputSystem = AllServices.Container.Single<InputSystem>();

            if (_mainCamera == null) Debug.Log("Main camera = potik bachok");

            _inputSystem.ItemPickUpAction += PickUpItem;
        }

        private void OnDestroy()
        {
            _inputSystem.ItemPickUpAction -= PickUpItem;
        }

        private void PickUpItem()
        {
            var ray = _mainCamera.ViewportPointToRay(Vector3.one * 0.5f);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _raycastLength, _interactableItemLayer))
            {
                ICollectable collectable = hit.transform.GetComponent<ICollectable>();
                if (collectable != null)
                {
                    bool isCollected = _inventoryManager.AddItemToInventory(collectable.CollectItem());
                    if (isCollected) collectable.DestroyItem();
                }

            }
        }
    }
}