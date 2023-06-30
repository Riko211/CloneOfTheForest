using Infrastructure.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Inventory
{
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private ItemDataSO _itemData;
        [SerializeField]
        private Image _image;
        [SerializeField]
        private TextMeshProUGUI _countTXT;
        [SerializeField]
        private int _count = 1;

        private int _maxStackSize;
        private Transform _parentAfterDrag;
        private Transform _parentBeforeDrag;
        private InputSystem _inputSystem;
        private Transform _inventoryRoot;

        private void Start()
        {
            //InitializeItem(_itemData);
            _inputSystem = AllServices.Container.Single<InputSystem>();
        }
        public void InitializeItem(ItemDataSO itemData, Transform rootTransform)
        {
            _itemData = itemData;
            _image.sprite = itemData.image;
            _inventoryRoot = rootTransform;
            _maxStackSize = itemData.maxStackSize;
            RefreshCount();
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            _image.raycastTarget = false;
            _parentBeforeDrag = transform.parent;
            _parentAfterDrag = transform.parent;
            transform.SetParent(_inventoryRoot);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = _inputSystem.MousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _image.raycastTarget = true;
            if (_parentBeforeDrag == _parentAfterDrag) transform.SetParent(_parentAfterDrag);
        }
        
        public void SetSlot(Transform parent)
        {
            _parentAfterDrag = parent;
            transform.SetParent(_parentAfterDrag);
        }
        public Transform GetSlot()
        {
            return _parentAfterDrag;
        }
        public ItemDataSO GetItemData()
        {
            return _itemData;
        }
        public int GetItemCount()
        {
            return _count;
        }
        public void AddItem()
        {
            _count++;
            RefreshCount();
        }
        public void RemoveItem()
        {
            _count--;
            RefreshCount();
            if (_count <= 0) Destroy(gameObject);
        }
        private void RefreshCount()
        {
            if (_count > 1) _countTXT.text = _count.ToString();
            else _countTXT.text = "";
        }
    }
}