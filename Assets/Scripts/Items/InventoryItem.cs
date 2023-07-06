using Infrastructure.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Inventory
{
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
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
        private InventoryManager _inventoryManager;

        private void Start()
        {
            //InitializeItem(_itemData);
            _inputSystem = AllServices.Container.Single<InputSystem>();
        }
        public void InitializeItem(ItemDataSO itemData, Transform rootTransform, InventoryManager inventoryManager)
        {
            _itemData = itemData;
            _image.sprite = itemData.image;
            _inventoryRoot = rootTransform;
            _maxStackSize = itemData.maxStackSize;
            _inventoryManager = inventoryManager;
            RefreshCount();
        }
        public void InitializeItem(ItemDataSO itemData, Transform rootTransform, int count, InventoryManager inventoryManager)
        {
            _itemData = itemData;
            _image.sprite = itemData.image;
            _inventoryRoot = rootTransform;
            _maxStackSize = itemData.maxStackSize;
            _inventoryManager = inventoryManager;
            _count = count;
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
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (_count > 1)
                {
                    int itemsCountToAdd = _count / 2;
                    bool isItemAdded = _inventoryManager.AddItemsToInventory(_itemData, itemsCountToAdd);
                    if (isItemAdded)
                    {
                        _count -= itemsCountToAdd;
                        RefreshCount();
                    }
                }
            }
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
        public bool IsItemTool()
        {
            if (_itemData.type == ItemDataSO.ItemType.Tool) return true;
            else return false;
        }
        private void RefreshCount()
        {
            if (_count > 1) _countTXT.text = _count.ToString();
            else _countTXT.text = "";
        }


    }
}