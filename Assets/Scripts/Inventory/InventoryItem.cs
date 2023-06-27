using Infrastructure.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private InventoryItemSO _itemData;
        [SerializeField]
        private Image _image;

        private Transform _parentAfterDrag;
        private InputSystem _inputSystem;
        private Transform _inventoryRoot;

        private void Start()
        {
            //InitializeItem(_itemData);
            _inputSystem = AllServices.Container.Single<InputSystem>();
        }
        public void InitializeItem(InventoryItemSO itemData, Transform rootTransform)
        {
            _itemData = itemData;
            _image.sprite = itemData.image;
            _inventoryRoot = rootTransform;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            _image.raycastTarget = false;
            _parentAfterDrag = transform.parent;
            transform.SetParent(_inventoryRoot);

            Debug.Log("Drag begin");
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = _inputSystem.MousePosition;
            //transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _image.raycastTarget = true;
            transform.SetParent(_parentAfterDrag);

            Debug.Log("End drag");
        }
        
        public void SetParent(Transform parent)
        {
            _parentAfterDrag = parent;
        }
    }
}